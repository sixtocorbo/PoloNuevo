Imports System.IO
Imports System.Data
Imports System.Drawing

Public Class frmNuevoIngreso

    ' Variables de Estado
    Private _idDocumentoEditar As Integer = 0
    Private _listaCompletaReclusos As New List(Of ReclusoItem)
    Private _archivoBytes As Byte() = Nothing
    Private _archivoNombre As String = ""
    Private _archivoExt As String = ""
    Private _idPadreVerificado As Integer = 0

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
        grpRelacion.Enabled = False
    End Sub

    ' =========================================================
    ' CARGA INICIAL
    ' =========================================================
    Private Sub frmNuevoIngreso_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CargarListas()
        ' Ocultamos el grupo de Salida Inmediata si existiera en el diseño, para no confundir
        If Me.Controls.ContainsKey("grpDestino") Then Me.Controls("grpDestino").Visible = False

        If _idDocumentoEditar > 0 Then CargarDatosEdicion()
    End Sub
    ' =========================================================
    ' CARGA DE DATOS (EDICIÓN) - AGREGAR ESTE BLOQUE
    ' =========================================================
    Private Sub CargarDatosEdicion()
        Try
            Using db As New PoloNuevoEntities()
                Dim doc = db.Documentos.Find(_idDocumentoEditar)
                If doc IsNot Nothing Then
                    txtNumero.Text = doc.ReferenciaExterna
                    txtAsunto.Text = doc.Descripcion

                    If IsNumeric(doc.TipoDocumentoId) Then cmbTipo.SelectedValue = doc.TipoDocumentoId

                    ' Cargar Recluso
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

                    ' Cargar Origen Original (buscando el primer movimiento)
                    Dim primerMov = doc.MovimientosDocumentos.OrderBy(Function(m) m.FechaMovimiento).FirstOrDefault()
                    If primerMov IsNot Nothing Then
                        cmbOrigen.Text = primerMov.Origen
                    End If
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("Error al cargar datos: " & ex.Message)
        End Try
    End Sub
    Private Sub CargarListas()
        Using db As New PoloNuevoEntities()
            ' Tipos
            cmbTipo.DataSource = db.TiposDocumento.Where(Function(t) t.Nombre <> "ARCHIVO").OrderBy(Function(t) t.Nombre).ToList()
            cmbTipo.DisplayMember = "Nombre" : cmbTipo.ValueMember = "Id"

            ' Reclusos
            _listaCompletaReclusos = db.Reclusos.Select(Function(r) New ReclusoItem With {.Id = r.Id, .Texto = r.Nombre & " (" & r.Cedula & ")"}).OrderBy(Function(r) r.Texto).ToList()
            ActualizarListaReclusos(_listaCompletaReclusos)

            ' Orígenes (De dónde viene este papel)
            Dim origenes = db.MovimientosDocumentos.Where(Function(m) m.Origen <> "" And m.Origen <> "SISTEMA").Select(Function(m) m.Origen).Distinct().ToList()
            Dim defaults As String() = {"JUZGADO LETRADO", "MINISTERIO DEL INTERIOR", "FISCALÍA", "DEFENSORÍA", "DIRECCIÓN", "JEFATURA DE SERVICIO", "OGLAST"}
            For Each def In defaults
                If Not origenes.Contains(def) Then origenes.Add(def)
            Next
            origenes.Sort()
            cmbOrigen.DataSource = origenes
            cmbOrigen.SelectedIndex = -1
        End Using
    End Sub

    Private Sub ActualizarListaReclusos(items As List(Of ReclusoItem), Optional selectedId As Integer? = Nothing)
        lstReclusos.DataSource = items
        lstReclusos.DisplayMember = "Texto" : lstReclusos.ValueMember = "Id"
        lstReclusos.SelectedValue = If(selectedId, -1)
    End Sub

    ' =========================================================
    ' LÓGICA DE VINCULACIÓN (EL PUENTE)
    ' =========================================================
    Private Sub chkEsRespuesta_CheckedChanged(sender As Object, e As EventArgs) Handles chkEsRespuesta.CheckedChanged
        txtIdPadre.Enabled = chkEsRespuesta.Checked
        btnVerificarPadre.Enabled = chkEsRespuesta.Checked
        btnBuscarPadre.Enabled = chkEsRespuesta.Checked
        If Not chkEsRespuesta.Checked Then
            txtIdPadre.Text = "" : lblInfoPadre.Text = "..." : _idPadreVerificado = 0
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
                    lblInfoPadre.Text = "Documento no encontrado." : lblInfoPadre.ForeColor = Color.Red : _idPadreVerificado = 0
                End If
            End Using
        End If
    End Sub

    ' Buscador Rápido (?)
    Private Sub btnBuscarPadre_Click(sender As Object, e As EventArgs) Handles btnBuscarPadre.Click
        Dim fBuscar As New Form() With {.Text = "Buscar Documento Padre", .Size = New Size(800, 500), .StartPosition = FormStartPosition.CenterScreen}
        Dim txtFiltro As New TextBox() With {.Top = 10, .Left = 10, .Width = 600}
        Dim btnB As New Button() With {.Text = "Buscar", .Top = 8, .Left = 620}
        Dim dgv As New DataGridView() With {.Top = 40, .Left = 10, .Width = 760, .Height = 400, .ReadOnly = True, .SelectionMode = DataGridViewSelectionMode.FullRowSelect}
        fBuscar.Controls.AddRange({txtFiltro, btnB, dgv})

        Dim Act = Sub()
                      Using db As New PoloNuevoEntities()
                          Dim q = db.Documentos.AsNoTracking().OrderByDescending(Function(d) d.Id).Take(50).Select(Function(d) New With {.ID = d.Id, .Ref = d.ReferenciaExterna, .Asunto = d.Descripcion}).ToList()
                          dgv.DataSource = q
                      End Using
                  End Sub
        AddHandler btnB.Click, Sub(s, ev) Act()
        AddHandler dgv.CellDoubleClick, Sub(s, ev)
                                            txtIdPadre.Text = dgv.Rows(ev.RowIndex).Cells("ID").Value.ToString()
                                            btnVerificarPadre.PerformClick()
                                            fBuscar.Close()
                                        End Sub
        Act()
        fBuscar.ShowDialog()
    End Sub

    ' =========================================================
    ' GUARDADO CORRECTO: REGRESO A MESA DE ENTRADA
    ' =========================================================
    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If txtNumero.Text = "" Or txtAsunto.Text = "" Or cmbOrigen.Text = "" Then
            MessageBox.Show("Faltan datos obligatorios.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Using db As New PoloNuevoEntities()
                ' 1. Registrar el NUEVO Documento (El Memo de Respuesta)
                Dim doc As New Documentos()
                If _idDocumentoEditar > 0 Then doc = db.Documentos.Find(_idDocumentoEditar)

                doc.FechaCarga = DateTime.Now
                doc.TipoDocumentoId = Convert.ToInt32(cmbTipo.SelectedValue)
                doc.ReferenciaExterna = txtNumero.Text.Trim()
                ' Referencia visual en el asunto
                doc.Descripcion = txtAsunto.Text.Trim() & If(_idPadreVerificado > 0, $" [RESPONDE A ID #{_idPadreVerificado}]", "")

                If _archivoBytes IsNot Nothing Then
                    doc.Contenido = _archivoBytes : doc.NombreArchivo = _archivoNombre : doc.Extension = _archivoExt
                ElseIf _idDocumentoEditar = 0 Then
                    doc.Contenido = New Byte() {0} : doc.NombreArchivo = "Fisico" : doc.Extension = ".phy"
                End If

                If _idDocumentoEditar = 0 Then db.Documentos.Add(doc)
                db.SaveChanges()

                ' SOLO SI ES NUEVO HACEMOS LOS MOVIMIENTOS
                If _idDocumentoEditar = 0 Then

                    ' A. MOVIMIENTO DEL HIJO (EL MEMO)
                    ' Entra desde el Jefe hacia tu Mesa. Queda en tu poder.
                    Dim movHijo As New MovimientosDocumentos()
                    movHijo.DocumentoId = doc.Id
                    movHijo.FechaMovimiento = DateTime.Now
                    movHijo.Origen = cmbOrigen.Text.Trim().ToUpper() ' ej: JEFATURA DE SERVICIO
                    movHijo.Destino = "MESA DE ENTRADA"              ' LLEGA A TI
                    movHijo.EsSalida = False                         ' NO SALE TODAVÍA
                    movHijo.Observaciones = "Recepción de respuesta/informe."
                    db.MovimientosDocumentos.Add(movHijo)

                    ' B. MOVIMIENTO DEL PADRE (EL OFICIO ORIGINAL)
                    ' Aquí está la clave: El Padre TAMBIÉN regresa a tu Mesa.
                    If _idPadreVerificado > 0 Then
                        Dim movPadre As New MovimientosDocumentos()
                        movPadre.DocumentoId = _idPadreVerificado
                        movPadre.FechaMovimiento = DateTime.Now.AddSeconds(1)

                        ' Viene del mismo lugar de donde viene el Memo
                        movPadre.Origen = cmbOrigen.Text.Trim().ToUpper()

                        ' Y regresa a tu control
                        movPadre.Destino = "MESA DE ENTRADA"
                        movPadre.EsSalida = False
                        movPadre.Observaciones = $"REGRESA ADJUNTO A: {doc.ReferenciaExterna} (ID:{doc.Id})"

                        db.MovimientosDocumentos.Add(movPadre)
                    End If

                    db.SaveChanges()
                End If

                MessageBox.Show("Documentos ingresados a Mesa de Entrada (El nuevo y el original).", "Proceso Completado")
                Me.DialogResult = DialogResult.OK
                Me.Close()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub

    ' Eventos visuales (mantener igual)
    Private Sub btnAdjuntar_Click(sender As Object, e As EventArgs) Handles btnAdjuntar.Click
        Using ofd As New OpenFileDialog()
            If ofd.ShowDialog() = DialogResult.OK Then
                _archivoBytes = File.ReadAllBytes(ofd.FileName) : _archivoNombre = Path.GetFileName(ofd.FileName) : _archivoExt = Path.GetExtension(ofd.FileName)
                lblArchivoNombre.Text = _archivoNombre : lblArchivoNombre.ForeColor = Color.Green
            End If
        End Using
    End Sub
    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Me.Close()
    End Sub
    Private Sub txtBuscarRecluso_TextChanged(sender As Object, e As EventArgs) Handles txtBuscarRecluso.TextChanged
        ' Tu código de búsqueda de reclusos...
    End Sub
    Private Sub chkVincular_CheckedChanged(sender As Object, e As EventArgs) Handles chkVincular.CheckedChanged
        txtBuscarRecluso.Enabled = chkVincular.Checked : lstReclusos.Enabled = chkVincular.Checked
    End Sub
    Private Sub chkVencimiento_CheckedChanged(sender As Object, e As EventArgs) Handles chkVencimiento.CheckedChanged
        dtpVencimiento.Visible = chkVencimiento.Checked : dtpVencimiento.Enabled = chkVencimiento.Checked
    End Sub
End Class