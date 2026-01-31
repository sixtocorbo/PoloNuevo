Imports System.IO
Imports System.Data
Imports System.Drawing
Imports System.Linq

Public Class frmNuevoIngreso

    ' =========================================================
    ' VARIABLES DE ESTADO
    ' =========================================================
    Private _idDocumentoEditar As Integer = 0
    Private _idPadrePreseleccionado As Integer = 0 ' El ID que viene de la Mesa de Entrada

    Private _listaCompletaReclusos As New List(Of ReclusoItem)
    Private _todosOrigenes As New List(Of String)

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

    ' 1. Constructor Nuevo (Limpio)
    Public Sub New()
        InitializeComponent()
        _idDocumentoEditar = 0
        Me.Text = "Nuevo Ingreso / Respuesta"
        lblNumero.Text = "Ref. Externa / Nro. Memorando:"
        grpRelacion.Enabled = False ' Si no hay ID, no se puede vincular nada
        lblInfoPadre.Text = "No se seleccionó ningún documento en la Mesa de Entrada."
        lblInfoPadre.ForeColor = Color.Gray
    End Sub

    ' 2. Constructor Editar (Existente)
    Public Sub New(idDocumento As Integer)
        InitializeComponent()
        _idDocumentoEditar = idDocumento
        Me.Text = "Editar Documento"
        btnGuardar.Text = "GUARDAR CAMBIOS"
        grpRelacion.Enabled = False ' En edición no cambiamos padres para mantener integridad
    End Sub

    ' 3. Constructor Vinculado (Desde Mesa de Entrada)
    Public Sub New(idPadre As Integer, esVinculacion As Boolean)
        InitializeComponent()
        _idDocumentoEditar = 0
        _idPadrePreseleccionado = idPadre
        Me.Text = "Nuevo Ingreso Vinculado"
        lblNumero.Text = "Ref. Externa / Nro. Memorando:"
    End Sub

    ' =========================================================
    ' CARGA INICIAL
    ' =========================================================
    Private Sub frmNuevoIngreso_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CargarListas()

        ' Ajustes visuales
        lstSugerenciasOrigen.Visible = False
        lstSugerenciasOrigen.BringToFront()
        If Me.Controls.ContainsKey("grpDestino") Then Me.Controls("grpDestino").Visible = False

        ' Preparar interfaz de vinculación
        If _idPadrePreseleccionado > 0 Then
            grpRelacion.Enabled = True
            chkEsRespuesta.Checked = False ' Empezamos desmarcado por seguridad
            lblInfoPadre.Text = "Marque la casilla para vincular con el documento seleccionado."
            lblInfoPadre.ForeColor = Color.DimGray
        End If

        If _idDocumentoEditar > 0 Then CargarDatosEdicion()
    End Sub

    Private Sub CargarListas()
        Using db As New PoloNuevoEntities()
            cmbTipo.DataSource = db.TiposDocumento.Where(Function(t) t.Nombre <> "ARCHIVO").OrderBy(Function(t) t.Nombre).ToList()
            cmbTipo.DisplayMember = "Nombre"
            cmbTipo.ValueMember = "Id"

            _listaCompletaReclusos = db.Reclusos.Select(Function(r) New ReclusoItem With {.Id = r.Id, .Texto = r.Nombre & " (" & r.Cedula & ")"}).OrderBy(Function(r) r.Texto).ToList()
            ActualizarListaReclusos(_listaCompletaReclusos)

            Dim listaTemp = db.MovimientosDocumentos.Where(Function(m) m.Origen <> "" And m.Origen <> "SISTEMA").Select(Function(m) m.Origen).Distinct().ToList()
            Dim defaults As String() = {"JUZGADO LETRADO", "MINISTERIO DEL INTERIOR", "FISCALÍA", "DEFENSORÍA", "DIRECCIÓN", "JEFATURA DE SERVICIO", "OGLAST"}
            For Each def In defaults
                If Not listaTemp.Contains(def) Then listaTemp.Add(def)
            Next
            listaTemp.Sort()
            _todosOrigenes = listaTemp
        End Using
    End Sub

    ' =========================================================
    ' LÓGICA DE TRAZABILIDAD (AUTOMÁTICA)
    ' =========================================================
    Private Sub chkEsRespuesta_CheckedChanged(sender As Object, e As EventArgs) Handles chkEsRespuesta.CheckedChanged
        If chkEsRespuesta.Checked Then
            ' Si marca la casilla, verificamos automáticamente el ID que trajimos de memoria
            If _idPadrePreseleccionado > 0 Then
                VerificarPadreLogica(_idPadrePreseleccionado)
            End If
        Else
            ' Si desmarca, limpiamos la vinculación
            lblInfoPadre.Text = "Marque la casilla para vincular con el documento seleccionado."
            lblInfoPadre.ForeColor = Color.DimGray
            _idPadreVerificado = 0
        End If
    End Sub

    Private Sub VerificarPadreLogica(idDoc As Integer)
        Using db As New PoloNuevoEntities()
            Dim padre = db.Documentos.Find(idDoc)
            If padre IsNot Nothing Then
                ' Aquí mostramos al usuario exactamente con qué se está vinculando
                lblInfoPadre.Text = $"SE VINCULA CON: {padre.TiposDocumento.Nombre} {padre.ReferenciaExterna}"
                lblInfoPadre.ForeColor = Color.Green
                _idPadreVerificado = padre.Id
            Else
                lblInfoPadre.Text = "Error: El documento seleccionado no existe."
                lblInfoPadre.ForeColor = Color.Red
                _idPadreVerificado = 0
            End If
        End Using
    End Sub

    ' =========================================================
    ' GUARDAR Y PROCESAR
    ' =========================================================
    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If String.IsNullOrWhiteSpace(txtAsunto.Text) Then
            MessageBox.Show("Debe ingresar el Asunto o Descripción.", "Faltan datos", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        If String.IsNullOrWhiteSpace(txtOrigen.Text) Then
            MessageBox.Show("Debe indicar el Organismo de Origen.", "Faltan datos", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Me.Cursor = Cursors.WaitCursor
            Using db As New PoloNuevoEntities()
                Dim padreEstaEnMesa As Boolean = True
                Dim destinoDelPadre As String = "MESA DE ENTRADA"
                Dim referenciaPadre As String = ""

                ' 1. Guardar Doc
                Dim nuevoDoc As New Documentos()
                If _idDocumentoEditar > 0 Then nuevoDoc = db.Documentos.Find(_idDocumentoEditar)

                nuevoDoc.FechaCarga = If(_idDocumentoEditar = 0, DateTime.Now, nuevoDoc.FechaCarga)
                nuevoDoc.Descripcion = txtAsunto.Text.Trim()
                nuevoDoc.ReferenciaExterna = txtNumero.Text.Trim()
                If cmbTipo.SelectedValue IsNot Nothing Then nuevoDoc.TipoDocumentoId = Convert.ToInt32(cmbTipo.SelectedValue)

                If _archivoBytes IsNot Nothing Then
                    nuevoDoc.Contenido = _archivoBytes
                    nuevoDoc.NombreArchivo = _archivoNombre
                    nuevoDoc.Extension = _archivoExt
                ElseIf _idDocumentoEditar = 0 Then
                    nuevoDoc.Contenido = New Byte() {0}
                    nuevoDoc.NombreArchivo = "S/D"
                    nuevoDoc.Extension = ".phy"
                End If

                If _idDocumentoEditar = 0 Then db.Documentos.Add(nuevoDoc)
                db.SaveChanges()

                ' 2. Movimientos y Vínculos
                If _idDocumentoEditar = 0 Then

                    ' A) Vínculo Padre (Solo si se verificó correctamente)
                    If _idPadreVerificado > 0 Then
                        Dim padre = db.Documentos.Find(_idPadreVerificado)
                        If padre IsNot Nothing Then
                            referenciaPadre = padre.ReferenciaExterna
                            Dim ultimoMov = padre.MovimientosDocumentos.OrderByDescending(Function(m) m.FechaMovimiento).FirstOrDefault()
                            If ultimoMov IsNot Nothing AndAlso Not String.IsNullOrEmpty(ultimoMov.Destino) Then
                                destinoDelPadre = ultimoMov.Destino
                                If destinoDelPadre <> "MESA DE ENTRADA" Then padreEstaEnMesa = False
                            End If

                            ' GUARDAR VÍNCULO EN BD
                            Dim vinculo As New DocumentoVinculos()
                            vinculo.IdDocumentoPadre = _idPadreVerificado
                            vinculo.IdDocumentoHijo = nuevoDoc.Id
                            vinculo.TipoRelacion = "INGRESO RESPUESTA"
                            vinculo.FechaVinculo = DateTime.Now
                            db.DocumentoVinculos.Add(vinculo)
                        End If
                    End If

                    ' B) Movimiento Entrada
                    Dim movEntrada As New MovimientosDocumentos()
                    movEntrada.DocumentoId = nuevoDoc.Id
                    movEntrada.FechaMovimiento = DateTime.Now
                    movEntrada.Origen = txtOrigen.Text.Trim().ToUpper()
                    movEntrada.Destino = "MESA DE ENTRADA"
                    movEntrada.EsSalida = False
                    If _idPadreVerificado > 0 Then
                        movEntrada.Observaciones = "VINCULADO A: " & referenciaPadre
                    Else
                        movEntrada.Observaciones = "Ingreso Estándar"
                    End If
                    db.MovimientosDocumentos.Add(movEntrada)
                    db.SaveChanges()

                    ' C) Ruteo Automático (Solo si hay padre)
                    If _idPadreVerificado > 0 Then
                        If padreEstaEnMesa Then
                            Dim movUpdate As New MovimientosDocumentos()
                            movUpdate.DocumentoId = _idPadreVerificado
                            movUpdate.FechaMovimiento = DateTime.Now.AddSeconds(1)
                            movUpdate.Origen = txtOrigen.Text.Trim().ToUpper()
                            movUpdate.Destino = "MESA DE ENTRADA"
                            movUpdate.EsSalida = False
                            movUpdate.Observaciones = "SE ADJUNTA INFORME/DOC: " & nuevoDoc.ReferenciaExterna
                            db.MovimientosDocumentos.Add(movUpdate)
                        Else
                            Dim movSalidaAuto As New MovimientosDocumentos()
                            movSalidaAuto.DocumentoId = nuevoDoc.Id
                            movSalidaAuto.FechaMovimiento = DateTime.Now.AddSeconds(2)
                            movSalidaAuto.Origen = "MESA DE ENTRADA"
                            movSalidaAuto.Destino = destinoDelPadre
                            movSalidaAuto.EsSalida = True
                            movSalidaAuto.Observaciones = "PASE AUTOMÁTICO (Alcanza al Exp. Principal)"
                            db.MovimientosDocumentos.Add(movSalidaAuto)
                            MessageBox.Show($"El documento ha sido enviado automáticamente a {destinoDelPadre} para unirse al principal.", "Ruteo Automático", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If
                        db.SaveChanges()
                    End If
                End If

                If _idDocumentoEditar > 0 Then
                    MessageBox.Show("Cambios guardados.", "Éxito")
                ElseIf _idPadreVerificado = 0 OrElse (padreEstaEnMesa And _idPadreVerificado > 0) Then
                    MessageBox.Show("Ingreso registrado.", "Éxito")
                End If

                Me.DialogResult = DialogResult.OK
                Me.Close()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ' =========================================================
    ' EVENTOS DE INTERFAZ AUXILIARES
    ' =========================================================
    Private Sub txtOrigen_TextChanged(sender As Object, e As EventArgs) Handles txtOrigen.TextChanged
        Dim texto As String = txtOrigen.Text.Trim().ToLower()
        If String.IsNullOrEmpty(texto) Then
            lstSugerenciasOrigen.Visible = False
            Return
        End If
        If _todosOrigenes Is Nothing Then Return
        Dim palabras As String() = texto.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)
        Dim resultados = _todosOrigenes.Where(Function(o) palabras.All(Function(p) o.ToLower().Contains(p))).Take(10).ToList()
        If resultados.Count > 0 Then
            lstSugerenciasOrigen.DataSource = resultados
            lstSugerenciasOrigen.Visible = True
            lstSugerenciasOrigen.Top = txtOrigen.Bottom + 2
            lstSugerenciasOrigen.Left = txtOrigen.Left
            lstSugerenciasOrigen.Width = txtOrigen.Width
            lstSugerenciasOrigen.BringToFront()
        Else
            lstSugerenciasOrigen.Visible = False
        End If
    End Sub

    Private Sub txtOrigen_KeyDown(sender As Object, e As KeyEventArgs) Handles txtOrigen.KeyDown
        If lstSugerenciasOrigen.Visible AndAlso lstSugerenciasOrigen.Items.Count > 0 Then
            Select Case e.KeyCode
                Case Keys.Down
                    e.Handled = True
                    ' BLOQUE IF CORREGIDO
                    If lstSugerenciasOrigen.SelectedIndex < lstSugerenciasOrigen.Items.Count - 1 Then
                        lstSugerenciasOrigen.SelectedIndex += 1
                    ElseIf lstSugerenciasOrigen.SelectedIndex = -1 Then
                        lstSugerenciasOrigen.SelectedIndex = 0
                    End If

                Case Keys.Up
                    e.Handled = True
                    If lstSugerenciasOrigen.SelectedIndex > 0 Then
                        lstSugerenciasOrigen.SelectedIndex -= 1
                    End If

                Case Keys.Enter
                    e.SuppressKeyPress = True
                    If lstSugerenciasOrigen.SelectedItem IsNot Nothing Then
                        txtOrigen.Text = lstSugerenciasOrigen.SelectedItem.ToString()
                        lstSugerenciasOrigen.Visible = False
                        txtOrigen.SelectionStart = txtOrigen.Text.Length
                        txtOrigen.Focus()
                    End If

                Case Keys.Escape
                    e.Handled = True
                    lstSugerenciasOrigen.Visible = False
            End Select
        End If
    End Sub

    Private Sub lstSugerenciasOrigen_Click(sender As Object, e As EventArgs) Handles lstSugerenciasOrigen.Click
        If lstSugerenciasOrigen.SelectedItem IsNot Nothing Then
            txtOrigen.Text = lstSugerenciasOrigen.SelectedItem.ToString()
            lstSugerenciasOrigen.Visible = False
            txtOrigen.SelectionStart = txtOrigen.Text.Length
            txtOrigen.Focus()
        End If
    End Sub

    Private Sub txtOrigen_Leave(sender As Object, e As EventArgs) Handles txtOrigen.Leave
        If Not lstSugerenciasOrigen.Focused Then lstSugerenciasOrigen.Visible = False
    End Sub

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
                        ActualizarListaReclusos(_listaCompletaReclusos, doc.ReclusoId.Value)
                    Else
                        chkVincular.Checked = False
                    End If
                    If doc.FechaVencimiento.HasValue Then
                        chkVencimiento.Checked = True
                        dtpVencimiento.Value = doc.FechaVencimiento.Value
                    Else
                        chkVencimiento.Checked = False
                    End If
                    If doc.Extension <> ".phy" Then
                        lblArchivoNombre.Text = "Archivo actual: " & doc.NombreArchivo
                        lblArchivoNombre.ForeColor = Color.Blue
                        btnAdjuntar.Text = "Reemplazar..."
                    Else
                        lblArchivoNombre.Text = "Registro Físico (Sin digitalizar)"
                    End If
                    Dim primerMov = doc.MovimientosDocumentos.OrderBy(Function(m) m.FechaMovimiento).FirstOrDefault()
                    If primerMov IsNot Nothing Then txtOrigen.Text = primerMov.Origen
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