Imports System.IO
Imports System.Data

Public Class frmNuevoIngreso

    ' Variables de Estado
    Private _idDocumentoEditar As Integer = 0
    Private _listaCompletaReclusos As New List(Of ReclusoItem)
    Private _archivoBytes As Byte() = Nothing
    Private _archivoNombre As String = ""
    Private _archivoExt As String = ""

    Public Class ReclusoItem
        Public Property Id As Integer
        Public Property Texto As String
    End Class

    ' =========================================================
    ' CONSTRUCTORES
    ' =========================================================
    Public Sub New()
        InitializeComponent()
        _idDocumentoEditar = 0
        Me.Text = "Nuevo Ingreso Documental (Externo)"
        ' Cambiamos la etiqueta para que sea claro
        lblNumero.Text = "Ref. Externa / Nro. Origen:"
    End Sub

    Public Sub New(idDocumento As Integer)
        InitializeComponent()
        _idDocumentoEditar = idDocumento
        Me.Text = "Editar / Detalle de Documento"
        btnGuardar.Text = "GUARDAR CAMBIOS"
        btnGuardar.BackColor = Color.SlateGray
    End Sub

    ' =========================================================
    ' CARGA INICIAL
    ' =========================================================
    Private Sub frmNuevoIngreso_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CargarListas()
        If _idDocumentoEditar > 0 Then
            CargarDatosEdicion()
        End If
    End Sub

    Private Sub CargarListas()
        Using db As New PoloNuevoEntities()
            ' Tipos de documento
            cmbTipo.DataSource = db.TiposDocumento.Where(Function(t) t.Nombre <> "ARCHIVO").OrderBy(Function(t) t.Nombre).ToList()
            cmbTipo.DisplayMember = "Nombre"
            cmbTipo.ValueMember = "Id"

            ' Lista de reclusos
            _listaCompletaReclusos = db.Reclusos _
                                        .Select(Function(r) New ReclusoItem With {
                                            .Id = r.Id,
                                            .Texto = r.Nombre & " (" & r.Cedula & ")"
                                        }) _
                                        .OrderBy(Function(r) r.Texto) _
                                        .ToList()

            ActualizarListaReclusos(_listaCompletaReclusos)
        End Using
    End Sub

    Private Sub ActualizarListaReclusos(items As List(Of ReclusoItem), Optional selectedId As Integer? = Nothing)
        lstReclusos.DataSource = Nothing
        lstReclusos.DataSource = items
        lstReclusos.DisplayMember = "Texto"
        lstReclusos.ValueMember = "Id"
        If selectedId.HasValue Then
            lstReclusos.SelectedValue = selectedId.Value
        Else
            lstReclusos.SelectedIndex = -1
        End If
    End Sub

    ' =========================================================
    ' EVENTOS DE INTERFAZ (LIMPIOS DE VALIDACIONES DE RANGO)
    ' =========================================================
    Private Sub txtBuscarRecluso_TextChanged(sender As Object, e As EventArgs) Handles txtBuscarRecluso.TextChanged
        Dim textoBusqueda As String = txtBuscarRecluso.Text.Trim().ToLower()
        Dim selectedId As Integer? = Nothing
        If lstReclusos.SelectedValue IsNot Nothing Then selectedId = Convert.ToInt32(lstReclusos.SelectedValue)

        If String.IsNullOrWhiteSpace(textoBusqueda) Then
            ActualizarListaReclusos(_listaCompletaReclusos, selectedId)
        Else
            Dim palabras = textoBusqueda.Split({" "c}, StringSplitOptions.RemoveEmptyEntries)
            Dim filtrados = _listaCompletaReclusos.Where(Function(r) palabras.All(Function(p) r.Texto.ToLower().Contains(p))).ToList()
            ActualizarListaReclusos(filtrados, selectedId)
        End If
    End Sub

    Private Sub chkVencimiento_CheckedChanged(sender As Object, e As EventArgs) Handles chkVencimiento.CheckedChanged
        dtpVencimiento.Visible = chkVencimiento.Checked
        dtpVencimiento.Enabled = chkVencimiento.Checked
    End Sub

    Private Sub chkVincular_CheckedChanged(sender As Object, e As EventArgs) Handles chkVincular.CheckedChanged
        txtBuscarRecluso.Enabled = chkVincular.Checked
        lstReclusos.Enabled = chkVincular.Checked
        If Not chkVincular.Checked Then
            txtBuscarRecluso.Text = ""
            ActualizarListaReclusos(_listaCompletaReclusos)
        End If
    End Sub

    Private Sub btnAdjuntar_Click(sender As Object, e As EventArgs) Handles btnAdjuntar.Click
        Using ofd As New OpenFileDialog()
            ofd.Filter = "Archivos|*.pdf;*.jpg;*.jpeg;*.png;*.doc;*.docx"
            If ofd.ShowDialog() = DialogResult.OK Then
                _archivoBytes = File.ReadAllBytes(ofd.FileName)
                _archivoNombre = Path.GetFileName(ofd.FileName)
                _archivoExt = Path.GetExtension(ofd.FileName)
                lblArchivoNombre.Text = _archivoNombre
                lblArchivoNombre.ForeColor = Color.Green
            End If
        End Using
    End Sub

    ' =========================================================
    ' CARGA DE DATOS (EDICIÓN)
    ' =========================================================
    Private Sub CargarDatosEdicion()
        Try
            Using db As New PoloNuevoEntities()
                Dim doc = db.Documentos.Find(_idDocumentoEditar)
                If doc IsNot Nothing Then
                    txtNumero.Text = doc.ReferenciaExterna
                    txtAsunto.Text = doc.Descripcion

                    If IsNumeric(doc.TipoDocumentoId) Then cmbTipo.SelectedValue = doc.TipoDocumentoId

                    If doc.ReclusoId.HasValue Then
                        chkVincular.Checked = True
                        txtBuscarRecluso.Enabled = True
                        lstReclusos.Enabled = True
                        ActualizarListaReclusos(_listaCompletaReclusos, doc.ReclusoId.Value)
                    Else
                        chkVincular.Checked = False
                        txtBuscarRecluso.Enabled = False
                        lstReclusos.Enabled = False
                    End If

                    If doc.FechaVencimiento.HasValue Then
                        chkVencimiento.Checked = True
                        dtpVencimiento.Value = doc.FechaVencimiento.Value
                        dtpVencimiento.Visible = True
                    Else
                        chkVencimiento.Checked = False
                        dtpVencimiento.Visible = False
                    End If

                    If doc.Extension <> ".phy" Then
                        lblArchivoNombre.Text = "Archivo actual: " & doc.NombreArchivo
                        lblArchivoNombre.ForeColor = Color.Blue
                        btnAdjuntar.Text = "Reemplazar..."
                    Else
                        lblArchivoNombre.Text = "Registro Físico (Sin digitalizar)"
                    End If
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("Error al cargar datos: " & ex.Message)
        End Try
    End Sub

    ' =========================================================
    ' GUARDADO SIMPLE (SIN RANGOS, SOLO REGISTRO)
    ' =========================================================
    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If txtNumero.Text.Trim = "" Or txtAsunto.Text.Trim = "" Then
            MessageBox.Show("Faltan datos obligatorios (Número de Referencia o Asunto).", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Using db As New PoloNuevoEntities()
                Dim doc As Documentos
                If _idDocumentoEditar = 0 Then
                    doc = New Documentos()
                    doc.FechaCarga = DateTime.Now
                    db.Documentos.Add(doc)
                Else
                    doc = db.Documentos.Find(_idDocumentoEditar)
                End If

                ' Tipo
                If cmbTipo.SelectedValue IsNot Nothing AndAlso IsNumeric(cmbTipo.SelectedValue) Then
                    doc.TipoDocumentoId = Convert.ToInt32(cmbTipo.SelectedValue)
                Else
                    MessageBox.Show("Seleccione un Tipo válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If

                ' Datos básicos
                doc.ReferenciaExterna = txtNumero.Text.Trim() ' Aquí guardamos lo que venga de afuera (ej: "OF. 32/2025")
                doc.Descripcion = txtAsunto.Text.Trim()

                ' Vinculación
                If chkVincular.Checked And lstReclusos.SelectedValue IsNot Nothing Then
                    doc.ReclusoId = Convert.ToInt32(lstReclusos.SelectedValue)
                Else
                    doc.ReclusoId = Nothing
                End If

                ' Vencimiento
                If chkVencimiento.Checked Then
                    doc.FechaVencimiento = dtpVencimiento.Value.Date
                Else
                    doc.FechaVencimiento = Nothing
                End If

                ' Archivo
                If _archivoBytes IsNot Nothing Then
                    doc.Contenido = _archivoBytes
                    doc.NombreArchivo = _archivoNombre
                    doc.Extension = _archivoExt
                ElseIf _idDocumentoEditar = 0 Then
                    doc.Contenido = New Byte() {0}
                    doc.NombreArchivo = "Registro Fisico"
                    doc.Extension = ".phy"
                End If

                ' NOTA: Aquí quitamos toda la lógica de NumeracionRangos porque esto es ENTRADA EXTERNA.
                ' No incrementamos contadores propios porque el número no es nuestro.

                db.SaveChanges()

                ' Movimiento Inicial
                If _idDocumentoEditar = 0 Then
                    Dim mov As New MovimientosDocumentos()
                    mov.DocumentoId = doc.Id
                    mov.FechaMovimiento = DateTime.Now
                    mov.Origen = "EXTERNO / RECEPCIÓN"
                    mov.Destino = "MESA DE ENTRADA"
                    mov.Observaciones = "Ingreso inicial por Mesa de Entrada"
                    db.MovimientosDocumentos.Add(mov)
                    db.SaveChanges()
                End If

                MessageBox.Show("Documento externo registrado correctamente.", "Éxito")
                Me.DialogResult = DialogResult.OK
                Me.Close()
            End Using

        Catch valEx As System.Data.Entity.Validation.DbEntityValidationException
            Dim mensajeError As String = ""
            For Each entidadError In valEx.EntityValidationErrors
                For Each errorDetalle In entidadError.ValidationErrors
                    mensajeError &= "- " & errorDetalle.PropertyName & ": " & errorDetalle.ErrorMessage & vbCrLf
                Next
            Next
            MessageBox.Show("Error de validación:" & vbCrLf & mensajeError, "Datos Incorrectos", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show("Error general: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Me.Close()
    End Sub

End Class