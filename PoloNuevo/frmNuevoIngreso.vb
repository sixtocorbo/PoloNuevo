Imports System.IO
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Data.Entity

Public Class frmNuevoIngreso

    ' =========================================================
    ' VARIABLES DE ESTADO
    ' =========================================================
    Private _idDocumentoEditar As Integer = 0
    Private _idPadrePreseleccionado As Integer = 0
    Private _idPadreVerificado As Integer = 0
    Private _padreEnMesa As Boolean = True
    Private _ubicacionPadre As String = "MESA DE ENTRADA"
    Private _ajustandoVinculacion As Boolean = False

    Private _listaCompletaReclusos As New List(Of ReclusoItem)
    Private _todosOrigenes As New List(Of String)

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
        Me.Text = "Nuevo Ingreso / Respuesta"
        lblNumero.Text = "Ref. Externa / Nro. Memorando:"
        grpRelacion.Enabled = False
        lblInfoPadre.Text = "No se seleccionó ningún documento en la Mesa de Entrada."
        lblInfoPadre.ForeColor = Color.Gray
    End Sub

    Public Sub New(idDocumento As Integer)
        InitializeComponent()
        _idDocumentoEditar = idDocumento
        Me.Text = "Editar Documento"
        btnGuardar.Text = "GUARDAR CAMBIOS"
        grpRelacion.Enabled = False
    End Sub

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

        lstSugerenciasOrigen.Visible = False
        lstSugerenciasOrigen.BringToFront()
        If Me.Controls.ContainsKey("grpDestino") Then Me.Controls("grpDestino").Visible = False

        If _idPadrePreseleccionado > 0 Then
            grpRelacion.Enabled = True
            chkEsRespuesta.Checked = False
            lblInfoPadre.Text = "Marque la casilla para vincular con el documento seleccionado."
            lblInfoPadre.ForeColor = Color.DimGray
            PrepararEstadoVinculacion(_idPadrePreseleccionado)
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
    ' LÓGICA DE VINCULACIÓN INTELIGENTE
    ' =========================================================
    Private Sub chkEsRespuesta_CheckedChanged(sender As Object, e As EventArgs) Handles chkEsRespuesta.CheckedChanged
        If _ajustandoVinculacion Then Return
        If chkEsRespuesta.Checked Then
            If _idPadrePreseleccionado > 0 Then
                VerificarPadreLogica(_idPadrePreseleccionado)
            End If
        Else
            lblInfoPadre.Text = "Marque la casilla para vincular con el documento seleccionado."
            lblInfoPadre.ForeColor = Color.DimGray
            _idPadreVerificado = 0
            _padreEnMesa = True
            _ubicacionPadre = "MESA DE ENTRADA"
        End If
    End Sub

    Private Sub VerificarPadreLogica(idDoc As Integer)
        Using db As New PoloNuevoEntities()
            Dim idRastro As Integer = idDoc
            Dim idPadreFinal As Integer = idDoc
            Dim encontrado As Boolean = True
            Dim iteraciones As Integer = 0

            While encontrado AndAlso iteraciones < 50
                iteraciones += 1
                Dim idActual = idRastro
                Dim vinculo = db.DocumentoVinculos.FirstOrDefault(Function(v) v.IdDocumentoHijo = idActual)
                If vinculo IsNot Nothing Then
                    idRastro = vinculo.IdDocumentoPadre
                    idPadreFinal = idRastro
                Else
                    encontrado = False
                End If
            End While

            Dim padre = db.Documentos.Include("MovimientosDocumentos").Include("TiposDocumento").FirstOrDefault(Function(d) d.Id = idPadreFinal)
            If padre IsNot Nothing Then
                Dim ultimoMov = padre.MovimientosDocumentos _
                    .OrderByDescending(Function(m) m.FechaMovimiento) _
                    .ThenByDescending(Function(m) m.Id) _
                    .FirstOrDefault()

                _ubicacionPadre = "MESA DE ENTRADA"
                If ultimoMov IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(ultimoMov.Destino) Then
                    _ubicacionPadre = ultimoMov.Destino.Trim()
                End If

                _padreEnMesa = _ubicacionPadre.Trim().ToUpper() = "MESA DE ENTRADA"
                If Not _padreEnMesa Then
                    _idPadreVerificado = 0
                    lblInfoPadre.Text = $"No se puede vincular: el expediente principal está en {_ubicacionPadre}."
                    lblInfoPadre.ForeColor = Color.Red
                    _ajustandoVinculacion = True
                    chkEsRespuesta.Checked = False
                    _ajustandoVinculacion = False
                    Return
                End If

                _idPadreVerificado = padre.Id
                If idPadreFinal <> idDoc Then
                    lblInfoPadre.Text = $"SE VINCULA AL PRINCIPAL: {padre.TiposDocumento.Nombre} {padre.ReferenciaExterna}"
                    lblInfoPadre.ForeColor = Color.Blue
                Else
                    lblInfoPadre.Text = $"SE VINCULA CON: {padre.TiposDocumento.Nombre} {padre.ReferenciaExterna}"
                    lblInfoPadre.ForeColor = Color.Green
                End If
            Else
                lblInfoPadre.Text = "Error: No se pudo identificar el documento raíz."
                lblInfoPadre.ForeColor = Color.Red
                _idPadreVerificado = 0
                _padreEnMesa = True
                _ubicacionPadre = "MESA DE ENTRADA"
            End If
        End Using
    End Sub

    Private Sub PrepararEstadoVinculacion(idDoc As Integer)
        Dim ubicacion As String = ""
        _padreEnMesa = PadreEstaEnMesaDeEntrada(idDoc, ubicacion)
        _ubicacionPadre = If(String.IsNullOrWhiteSpace(ubicacion), "MESA DE ENTRADA", ubicacion)

        If Not _padreEnMesa Then
            lblInfoPadre.Text = $"No se puede vincular: el expediente principal está en {_ubicacionPadre}."
            lblInfoPadre.ForeColor = Color.Red
            chkEsRespuesta.Enabled = False
            _idPadreVerificado = 0
        Else
            chkEsRespuesta.Enabled = True
        End If
    End Sub

    Private Function PadreEstaEnMesaDeEntrada(idDoc As Integer, ByRef ubicacionPadre As String) As Boolean
        Using db As New PoloNuevoEntities()
            Dim idRastro As Integer = idDoc
            Dim encontrado As Boolean = True
            Dim iteraciones As Integer = 0

            While encontrado AndAlso iteraciones < 50
                iteraciones += 1
                Dim idActual = idRastro
                Dim vinculo = db.DocumentoVinculos.FirstOrDefault(Function(v) v.IdDocumentoHijo = idActual)
                If vinculo IsNot Nothing Then
                    idRastro = vinculo.IdDocumentoPadre
                Else
                    encontrado = False
                End If
            End While

            Dim docPadre = db.Documentos.Include("MovimientosDocumentos").FirstOrDefault(Function(d) d.Id = idRastro)
            If docPadre Is Nothing Then
                ubicacionPadre = "MESA DE ENTRADA"
                Return True
            End If

            Dim ultimoMov = docPadre.MovimientosDocumentos _
                .OrderByDescending(Function(m) m.FechaMovimiento) _
                .ThenByDescending(Function(m) m.Id) _
                .FirstOrDefault()

            ubicacionPadre = "MESA DE ENTRADA"
            If ultimoMov IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(ultimoMov.Destino) Then
                ubicacionPadre = ultimoMov.Destino.Trim()
            End If

            Return ubicacionPadre.Trim().ToUpper() = "MESA DE ENTRADA"
        End Using
    End Function

    ' =========================================================
    ' GUARDAR (CON RETORNO FAMILIAR COMPLETO)
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

                Dim referenciaPadre As String = ""

                If _idPadreVerificado > 0 And _idDocumentoEditar = 0 Then
                    Dim docPadre = db.Documentos.Include("MovimientosDocumentos").FirstOrDefault(Function(d) d.Id = _idPadreVerificado)

                    If docPadre IsNot Nothing Then
                        referenciaPadre = docPadre.TiposDocumento.Nombre & " " & docPadre.ReferenciaExterna
                    End If
                End If

                If _idPadreVerificado > 0 AndAlso Not _padreEnMesa Then
                    MessageBox.Show($"No se puede vincular porque el expediente principal está en {_ubicacionPadre}.", "Vinculación no disponible", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return
                End If

                ' B) CREACIÓN O EDICIÓN DEL DOCUMENTO
                Dim nuevoDoc As New Documentos()
                If _idDocumentoEditar > 0 Then nuevoDoc = db.Documentos.Find(_idDocumentoEditar)

                ' Fecha segura para el hijo (siempre después del retorno)
                nuevoDoc.FechaCarga = If(_idDocumentoEditar = 0, DateTime.Now.AddSeconds(2), nuevoDoc.FechaCarga)

                nuevoDoc.Descripcion = txtAsunto.Text.Trim()
                nuevoDoc.ReferenciaExterna = txtNumero.Text.Trim()
                If cmbTipo.SelectedValue IsNot Nothing Then nuevoDoc.TipoDocumentoId = Convert.ToInt32(cmbTipo.SelectedValue)

                If chkVincular.Checked AndAlso lstReclusos.SelectedValue IsNot Nothing AndAlso lstReclusos.SelectedValue > 0 Then
                    nuevoDoc.ReclusoId = Convert.ToInt32(lstReclusos.SelectedValue)
                Else
                    nuevoDoc.ReclusoId = Nothing
                End If

                If chkVencimiento.Checked Then
                    nuevoDoc.FechaVencimiento = dtpVencimiento.Value
                Else
                    nuevoDoc.FechaVencimiento = Nothing
                End If

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

                ' C) MOVIMIENTOS Y VÍNCULOS
                If _idDocumentoEditar = 0 Then

                    Dim movEntrada As New MovimientosDocumentos()
                    movEntrada.DocumentoId = nuevoDoc.Id
                    movEntrada.FechaMovimiento = nuevoDoc.FechaCarga
                    movEntrada.Origen = txtOrigen.Text.Trim().ToUpper()
                    movEntrada.Destino = "MESA DE ENTRADA"
                    movEntrada.EsSalida = False

                    If _idPadreVerificado > 0 Then
                        movEntrada.Observaciones = "VINCULADO A: " & referenciaPadre
                    Else
                        movEntrada.Observaciones = "INGRESO / CREACIÓN"
                    End If

                    db.MovimientosDocumentos.Add(movEntrada)

                    If _idPadreVerificado > 0 Then
                        Dim vinculo As New DocumentoVinculos()
                        vinculo.IdDocumentoPadre = _idPadreVerificado
                        vinculo.IdDocumentoHijo = nuevoDoc.Id
                        vinculo.TipoRelacion = "ADJUNTO"
                        vinculo.FechaVinculo = nuevoDoc.FechaCarga
                        db.DocumentoVinculos.Add(vinculo)
                    End If

                    db.SaveChanges()

                    If _idPadreVerificado > 0 Then
                        MessageBox.Show("Documento ingresado y vinculado." & vbCrLf & "Se ha retornado todo el expediente a Mesa de Entrada.", "Gestión Completa", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Else
                        MessageBox.Show("Documento ingresado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                Else
                    MessageBox.Show("Cambios guardados.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If

                Me.DialogResult = DialogResult.OK
                Me.Close()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error al guardar: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ' =========================================================
    ' MÉTODO RECURSIVO PARA TRAER A TODA LA FAMILIA
    ' =========================================================
    Private Sub TraerFamiliaCompleta(db As PoloNuevoEntities, idPadre As Integer, origen As String, fecha As Date)
        ' 1. Buscamos todos los hijos directos de este padre
        Dim hijos = db.DocumentoVinculos.Where(Function(v) v.IdDocumentoPadre = idPadre).ToList()

        For Each h In hijos
            Dim idHijo = h.IdDocumentoHijo

            ' A. Verificamos dónde está el hijo actualmente
            ' Solo lo traemos si NO está ya en Mesa (para no duplicar movimientos innecesariamente)
            Dim ultMov = db.MovimientosDocumentos.Where(Function(m) m.DocumentoId = idHijo) _
                                                 .OrderByDescending(Function(m) m.FechaMovimiento) _
                                                 .ThenByDescending(Function(m) m.Id) _
                                                 .FirstOrDefault()

            Dim destinoActual As String = "MESA DE ENTRADA"
            If ultMov IsNot Nothing AndAlso ultMov.Destino IsNot Nothing Then
                destinoActual = ultMov.Destino.Trim().ToUpper()
            End If

            If destinoActual <> "MESA DE ENTRADA" Then
                Dim retHijo As New MovimientosDocumentos With {
                    .DocumentoId = idHijo,
                    .FechaMovimiento = fecha, ' Usamos la misma fecha del padre para que viajen juntos
                    .Origen = destinoActual,  ' Viene de donde estaba
                    .Destino = "MESA DE ENTRADA",
                    .EsSalida = False,
                    .Observaciones = "RETORNO AUTOMÁTICO (Arrastre de Expediente)"
                }
                db.MovimientosDocumentos.Add(retHijo)
            End If

            ' B. Recursividad: Buscamos si este hijo tiene sus propios hijos (nietos del original)
            TraerFamiliaCompleta(db, idHijo, origen, fecha)
        Next
    End Sub

    ' =========================================================
    ' EVENTOS DE INTERFAZ
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
