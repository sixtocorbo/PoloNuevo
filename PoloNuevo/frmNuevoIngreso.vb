Imports System.IO
Imports System.Data
Imports System.Drawing

Public Class frmNuevoIngreso

    ' =========================================================
    ' VARIABLES DE ESTADO
    ' =========================================================
    Private _idDocumentoEditar As Integer = 0
    Private _listaCompletaReclusos As New List(Of ReclusoItem)
    Private _archivoBytes As Byte() = Nothing
    Private _archivoNombre As String = ""
    Private _archivoExt As String = ""

    ' Variable clave para la trazabilidad
    Private _idPadreVerificado As Integer = 0

    ' Clase auxiliar para el combo de reclusos
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
        Me.Text = "Nuevo Ingreso / Respuesta"
        lblNumero.Text = "Ref. Externa / Nro. Memorando:"
    End Sub

    Public Sub New(idDocumento As Integer)
        InitializeComponent()
        _idDocumentoEditar = idDocumento
        Me.Text = "Editar Documento"
        btnGuardar.Text = "GUARDAR CAMBIOS"
        ' En edición no permitimos cambiar la relación padre para evitar inconsistencias
        grpRelacion.Enabled = False
    End Sub

    ' =========================================================
    ' CARGA INICIAL
    ' =========================================================
    Private Sub frmNuevoIngreso_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CargarListas()

        ' Si hubiera controles heredados no deseados, los ocultamos
        If Me.Controls.ContainsKey("grpDestino") Then Me.Controls("grpDestino").Visible = False

        If _idDocumentoEditar > 0 Then CargarDatosEdicion()
    End Sub

    Private Sub CargarListas()
        Using db As New PoloNuevoEntities()
            ' 1. Cargar Tipos de Documento
            cmbTipo.DataSource = db.TiposDocumento.Where(Function(t) t.Nombre <> "ARCHIVO").OrderBy(Function(t) t.Nombre).ToList()
            cmbTipo.DisplayMember = "Nombre"
            cmbTipo.ValueMember = "Id"

            ' 2. Cargar Reclusos para vinculación
            _listaCompletaReclusos = db.Reclusos.Select(Function(r) New ReclusoItem With {
                .Id = r.Id,
                .Texto = r.Nombre & " (" & r.Cedula & ")"
            }).OrderBy(Function(r) r.Texto).ToList()

            ActualizarListaReclusos(_listaCompletaReclusos)

            ' 3. Cargar Orígenes (Autocompletado inteligente basado en historial)
            Dim origenes = db.MovimientosDocumentos.Where(Function(m) m.Origen <> "" And m.Origen <> "SISTEMA") _
                                                   .Select(Function(m) m.Origen).Distinct().ToList()

            Dim defaults As String() = {"JUZGADO LETRADO", "MINISTERIO DEL INTERIOR", "FISCALÍA", "DEFENSORÍA", "DIRECCIÓN", "JEFATURA DE SERVICIO", "OGLAST"}
            For Each def In defaults
                If Not origenes.Contains(def) Then origenes.Add(def)
            Next
            origenes.Sort()
            cmbOrigen.DataSource = origenes
            cmbOrigen.SelectedIndex = -1
        End Using
    End Sub

    Private Sub CargarDatosEdicion()
        Try
            Using db As New PoloNuevoEntities()
                Dim doc = db.Documentos.Find(_idDocumentoEditar)
                If doc IsNot Nothing Then
                    txtNumero.Text = doc.ReferenciaExterna
                    txtAsunto.Text = doc.Descripcion

                    If IsNumeric(doc.TipoDocumentoId) Then cmbTipo.SelectedValue = doc.TipoDocumentoId

                    ' Cargar Vinculación con Recluso
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

                    ' Cargar Vencimiento
                    If doc.FechaVencimiento.HasValue Then
                        chkVencimiento.Checked = True
                        dtpVencimiento.Value = doc.FechaVencimiento.Value
                        dtpVencimiento.Visible = True
                    Else
                        chkVencimiento.Checked = False
                        dtpVencimiento.Visible = False
                    End If

                    ' Cargar Archivo
                    If doc.Extension <> ".phy" Then
                        lblArchivoNombre.Text = "Archivo actual: " & doc.NombreArchivo
                        lblArchivoNombre.ForeColor = Color.Blue
                        btnAdjuntar.Text = "Reemplazar..."
                    Else
                        lblArchivoNombre.Text = "Registro Físico (Sin digitalizar)"
                    End If

                    ' Intentar cargar Origen buscando el primer movimiento
                    Dim primerMov = doc.MovimientosDocumentos.OrderBy(Function(m) m.FechaMovimiento).FirstOrDefault()
                    If primerMov IsNot Nothing Then
                        cmbOrigen.Text = primerMov.Origen
                    End If

                    ' Si tiene padre, mostrarlo (Solo lectura en edición)
                    ' NOTA: Asegúrate de haber actualizado el EDMX para que reconozca DocumentoPadreId
                    ' If doc.DocumentoPadreId.HasValue Then
                    '    chkEsRespuesta.Checked = True
                    '    txtIdPadre.Text = doc.DocumentoPadreId.ToString()
                    '    _idPadreVerificado = doc.DocumentoPadreId.Value
                    ' End If
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("Error al cargar datos: " & ex.Message)
        End Try
    End Sub

    Private Sub ActualizarListaReclusos(items As List(Of ReclusoItem), Optional selectedId As Integer? = Nothing)
        lstReclusos.DataSource = items
        lstReclusos.DisplayMember = "Texto"
        lstReclusos.ValueMember = "Id"
        lstReclusos.SelectedValue = If(selectedId, -1)
    End Sub

    ' =========================================================
    ' LÓGICA DE TRAZABILIDAD (EL PUENTE)
    ' =========================================================
    Private Sub chkEsRespuesta_CheckedChanged(sender As Object, e As EventArgs) Handles chkEsRespuesta.CheckedChanged
        txtIdPadre.Enabled = chkEsRespuesta.Checked
        btnVerificarPadre.Enabled = chkEsRespuesta.Checked
        btnBuscarPadre.Enabled = chkEsRespuesta.Checked

        If Not chkEsRespuesta.Checked Then
            txtIdPadre.Text = ""
            lblInfoPadre.Text = "..."
            _idPadreVerificado = 0
            lblInfoPadre.ForeColor = Color.Black
        End If
    End Sub

    Private Sub btnVerificarPadre_Click(sender As Object, e As EventArgs) Handles btnVerificarPadre.Click
        Dim idBusqueda As Integer
        If Integer.TryParse(txtIdPadre.Text, idBusqueda) AndAlso idBusqueda > 0 Then
            Using db As New PoloNuevoEntities()
                Dim padre = db.Documentos.Find(idBusqueda)
                If padre IsNot Nothing Then
                    lblInfoPadre.Text = $"Confirmado: {padre.TiposDocumento.Nombre} {padre.ReferenciaExterna}"
                    lblInfoPadre.ForeColor = Color.Green
                    _idPadreVerificado = padre.Id
                Else
                    lblInfoPadre.Text = "Documento no encontrado."
                    lblInfoPadre.ForeColor = Color.Red
                    _idPadreVerificado = 0
                End If
            End Using
        Else
            lblInfoPadre.Text = "ID Inválido."
            lblInfoPadre.ForeColor = Color.Red
        End If
    End Sub

    ' Buscador Rápido de Documentos
    Private Sub btnBuscarPadre_Click(sender As Object, e As EventArgs) Handles btnBuscarPadre.Click
        Dim fBuscar As New Form() With {
            .Text = "Buscar Documento Padre",
            .Size = New Size(800, 500),
            .StartPosition = FormStartPosition.CenterScreen
        }

        Dim txtFiltro As New TextBox() With {.Top = 10, .Left = 10, .Width = 600}
        Dim btnB As New Button() With {.Text = "Buscar", .Top = 8, .Left = 620}
        Dim dgv As New DataGridView() With {
            .Top = 40, .Left = 10, .Width = 760, .Height = 400,
            .ReadOnly = True, .SelectionMode = DataGridViewSelectionMode.FullRowSelect, .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        }

        fBuscar.Controls.AddRange({txtFiltro, btnB, dgv})

        Dim RealizarBusqueda = Sub()
                                   Using db As New PoloNuevoEntities()
                                       Dim term = txtFiltro.Text.ToLower()
                                       Dim q = db.Documentos.AsNoTracking() _
                                                 .Where(Function(d) d.Descripcion.Contains(term) Or d.ReferenciaExterna.Contains(term)) _
                                                 .OrderByDescending(Function(d) d.Id) _
                                                 .Take(50) _
                                                 .Select(Function(d) New With {.ID = d.Id, .Ref = d.ReferenciaExterna, .Asunto = d.Descripcion, .Fecha = d.FechaCarga}) _
                                                 .ToList()
                                       dgv.DataSource = q
                                   End Using
                               End Sub

        AddHandler btnB.Click, Sub(s, ev) RealizarBusqueda()
        AddHandler txtFiltro.KeyDown, Sub(s, k)
                                          If k.KeyCode = Keys.Enter Then RealizarBusqueda()
                                      End Sub

        AddHandler dgv.CellDoubleClick, Sub(s, ev)
                                            If ev.RowIndex >= 0 Then
                                                txtIdPadre.Text = dgv.Rows(ev.RowIndex).Cells("ID").Value.ToString()
                                                btnVerificarPadre.PerformClick()
                                                fBuscar.Close()
                                            End If
                                        End Sub
        ' Carga inicial
        RealizarBusqueda()
        fBuscar.ShowDialog()
    End Sub


    ' =========================================================
    ' GUARDADO CON VINCULACIÓN CRUZADA (PADRE <-> HIJO)
    ' =========================================================
    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        ' Validaciones Básicas
        If txtNumero.Text = "" Or txtAsunto.Text = "" Or cmbOrigen.Text = "" Then
            MessageBox.Show("Faltan datos obligatorios (Número, Asunto u Origen).", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Using db As New PoloNuevoEntities()
                ' 1. Crear o Buscar el objeto Documento (El HIJO, ej: Memorando 21)
                Dim doc As New Documentos()
                If _idDocumentoEditar > 0 Then doc = db.Documentos.Find(_idDocumentoEditar)

                ' 2. Llenar propiedades
                doc.TipoDocumentoId = Convert.ToInt32(cmbTipo.SelectedValue)
                doc.ReferenciaExterna = txtNumero.Text.Trim()
                doc.Descripcion = txtAsunto.Text.Trim()

                If _idDocumentoEditar = 0 Then doc.FechaCarga = DateTime.Now

                ' 3. VINCULACIÓN FUERTE EN BASE DE DATOS
                If _idPadreVerificado > 0 Then
                    doc.DocumentoPadreId = _idPadreVerificado
                End If

                ' 4. Manejo de Archivos y otros datos (Reclusos, Vencimientos, etc...)
                If _archivoBytes IsNot Nothing Then
                    doc.Contenido = _archivoBytes
                    doc.NombreArchivo = _archivoNombre
                    doc.Extension = _archivoExt
                ElseIf _idDocumentoEditar = 0 Then
                    doc.Contenido = New Byte() {0}
                    doc.NombreArchivo = "Fisico"
                    doc.Extension = ".phy"
                End If

                If chkVincular.Checked AndAlso lstReclusos.SelectedValue IsNot Nothing Then
                    doc.ReclusoId = Convert.ToInt32(lstReclusos.SelectedValue)
                Else
                    doc.ReclusoId = Nothing
                End If

                If chkVencimiento.Checked Then
                    doc.FechaVencimiento = dtpVencimiento.Value
                Else
                    doc.FechaVencimiento = Nothing
                End If

                ' Guardar el Documento Nuevo
                If _idDocumentoEditar = 0 Then db.Documentos.Add(doc)
                db.SaveChanges()

                ' =========================================================
                ' GESTIÓN DE MOVIMIENTOS Y VINCULACIÓN
                ' =========================================================
                If _idDocumentoEditar = 0 Then

                    ' Variables para los textos de referencia
                    Dim obsHijo As String = "Ingreso al sistema."
                    Dim obsPadre As String = ""
                    Dim nombrePadre As String = ""

                    ' Si hay padre, preparamos la información
                    If _idPadreVerificado > 0 Then
                        Dim padre = db.Documentos.Find(_idPadreVerificado)
                        If padre IsNot Nothing Then
                            nombrePadre = padre.TiposDocumento.Nombre & " " & padre.ReferenciaExterna

                            ' AQUI ESTÁ EL CAMBIO QUE PEDISTE:
                            ' Definimos explícitamente que el hijo se adjunta al padre.
                            obsHijo = $"INGRESO VINCULADO / SE ADJUNTA A: {nombrePadre}"
                            obsPadre = $"SE VINCULA CON RESPUESTA: {doc.ReferenciaExterna}"
                        End If
                    End If

                    ' A. MOVIMIENTO DEL DOCUMENTO NUEVO (EJ: MEMO 21)
                    Dim movHijo As New MovimientosDocumentos()
                    movHijo.DocumentoId = doc.Id
                    movHijo.FechaMovimiento = DateTime.Now
                    movHijo.Origen = cmbOrigen.Text.Trim().ToUpper()
                    movHijo.Destino = "MESA DE ENTRADA"
                    movHijo.EsSalida = False
                    movHijo.Observaciones = obsHijo ' <--- "SE ADJUNTA A..."
                    db.MovimientosDocumentos.Add(movHijo)

                    ' B. MOVIMIENTO DEL DOCUMENTO PADRE (EJ: OFICIO 100)
                    ' Lo traemos a Mesa de Entrada para que estén juntos y no quede pendiente.
                    If _idPadreVerificado > 0 Then
                        Dim movPadre As New MovimientosDocumentos()
                        movPadre.DocumentoId = _idPadreVerificado

                        ' Le sumamos un segundo para que en el historial aparezca justo encima
                        movPadre.FechaMovimiento = DateTime.Now.AddSeconds(1)

                        ' Viene del mismo origen del que trae la respuesta
                        movPadre.Origen = cmbOrigen.Text.Trim().ToUpper()
                        movPadre.Destino = "MESA DE ENTRADA"
                        movPadre.EsSalida = False
                        movPadre.Observaciones = obsPadre ' <--- "SE VINCULA CON..."

                        db.MovimientosDocumentos.Add(movPadre)
                    End If

                    db.SaveChanges()
                End If

                MessageBox.Show("Documento ingresado y vinculado correctamente.", "Éxito")
                Me.DialogResult = DialogResult.OK
                Me.Close()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error al guardar: " & ex.Message)
        End Try
    End Sub

    ' =========================================================
    ' EVENTOS DE INTERFAZ
    ' =========================================================
    Private Sub btnAdjuntar_Click(sender As Object, e As EventArgs) Handles btnAdjuntar.Click
        Using ofd As New OpenFileDialog()
            If ofd.ShowDialog() = DialogResult.OK Then
                _archivoBytes = File.ReadAllBytes(ofd.FileName)
                _archivoNombre = Path.GetFileName(ofd.FileName)
                _archivoExt = Path.GetExtension(ofd.FileName)
                lblArchivoNombre.Text = _archivoNombre
                lblArchivoNombre.ForeColor = Color.Green
            End If
        End Using
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Me.Close()
    End Sub

    Private Sub txtBuscarRecluso_TextChanged(sender As Object, e As EventArgs) Handles txtBuscarRecluso.TextChanged
        Dim filtro = txtBuscarRecluso.Text.ToLower()
        If _listaCompletaReclusos IsNot Nothing Then
            Dim filtrada = _listaCompletaReclusos.Where(Function(r) r.Texto.ToLower().Contains(filtro)).ToList()
            ActualizarListaReclusos(filtrada)
        End If
    End Sub

    Private Sub chkVincular_CheckedChanged(sender As Object, e As EventArgs) Handles chkVincular.CheckedChanged
        txtBuscarRecluso.Enabled = chkVincular.Checked
        lstReclusos.Enabled = chkVincular.Checked
    End Sub

    Private Sub chkVencimiento_CheckedChanged(sender As Object, e As EventArgs) Handles chkVencimiento.CheckedChanged
        dtpVencimiento.Visible = chkVencimiento.Checked
        dtpVencimiento.Enabled = chkVencimiento.Checked
    End Sub

End Class