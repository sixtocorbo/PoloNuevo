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



    ' EN frmNuevoIngreso.vb - Evento btnGuardar_Click

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        ' 1. Validaciones
        If String.IsNullOrWhiteSpace(txtAsunto.Text) Then
            MessageBox.Show("Debe ingresar el Asunto o Descripción.", "Faltan datos", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Me.Cursor = Cursors.WaitCursor
            Using db As New PoloNuevoEntities()

                ' =============================================================================
                ' CORRECCIÓN: Declaramos las variables AQUÍ para que sean accesibles en todo el bloque
                ' =============================================================================
                Dim padreEstaEnMesa As Boolean = True
                Dim destinoDelPadre As String = "MESA DE ENTRADA"
                Dim referenciaPadre As String = ""
                ' =============================================================================

                ' ---------------------------------------------------------
                ' PASO 1: GUARDAR DOCUMENTO (Datos Básicos)
                ' ---------------------------------------------------------
                Dim nuevoDoc As New Documentos()
                If _idDocumentoEditar > 0 Then nuevoDoc = db.Documentos.Find(_idDocumentoEditar)

                nuevoDoc.FechaCarga = If(_idDocumentoEditar = 0, DateTime.Now, nuevoDoc.FechaCarga)
                nuevoDoc.Descripcion = txtAsunto.Text.Trim()
                nuevoDoc.ReferenciaExterna = txtNumero.Text.Trim()

                If cmbTipo.SelectedValue IsNot Nothing Then
                    nuevoDoc.TipoDocumentoId = Convert.ToInt32(cmbTipo.SelectedValue)
                End If

                ' NOTA: Solo descomentar si tienes la columna DocumentoPadreId en la BD
                ' If _idPadreVerificado > 0 Then nuevoDoc.DocumentoPadreId = _idPadreVerificado

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

                ' ---------------------------------------------------------
                ' PASO 2: LÓGICA DE MOVIMIENTOS INTELIGENTE
                ' ---------------------------------------------------------
                If _idDocumentoEditar = 0 Then

                    ' A) Analizar Dónde está el Padre
                    If _idPadreVerificado > 0 Then
                        Dim padre = db.Documentos.Find(_idPadreVerificado)
                        If padre IsNot Nothing Then
                            referenciaPadre = padre.ReferenciaExterna

                            ' Buscamos el último movimiento para ver su ubicación real
                            Dim ultimoMov = padre.MovimientosDocumentos _
                                                 .OrderByDescending(Function(m) m.FechaMovimiento) _
                                                 .FirstOrDefault()

                            If ultimoMov IsNot Nothing AndAlso Not String.IsNullOrEmpty(ultimoMov.Destino) Then
                                destinoDelPadre = ultimoMov.Destino
                                ' Si su último destino NO es Mesa de Entrada, es que ya se fue.
                                If destinoDelPadre <> "MESA DE ENTRADA" Then padreEstaEnMesa = False
                            End If
                        End If
                    End If

                    ' B) Registrar la ENTRADA (Siempre debe constar que entró)
                    Dim movEntrada As New MovimientosDocumentos()
                    movEntrada.DocumentoId = nuevoDoc.Id
                    movEntrada.FechaMovimiento = DateTime.Now
                    movEntrada.Origen = cmbOrigen.Text
                    movEntrada.Destino = "MESA DE ENTRADA"
                    movEntrada.EsSalida = False

                    If _idPadreVerificado > 0 Then
                        movEntrada.Observaciones = "VINCULADO A: " & referenciaPadre
                    Else
                        movEntrada.Observaciones = "Ingreso Estándar"
                    End If

                    db.MovimientosDocumentos.Add(movEntrada)
                    db.SaveChanges() ' Guardamos para asegurar el orden cronológico

                    ' C) RUTEO AUTOMÁTICO (Aquí está la magia)
                    If _idPadreVerificado > 0 Then

                        If padreEstaEnMesa Then
                            ' CASO 1: Padre sigue en Mesa (Pendiente) -> Se quedan juntos
                            Dim movUpdate As New MovimientosDocumentos()
                            movUpdate.DocumentoId = _idPadreVerificado
                            movUpdate.FechaMovimiento = DateTime.Now.AddSeconds(1)
                            movUpdate.Origen = cmbOrigen.Text
                            movUpdate.Destino = "MESA DE ENTRADA"
                            movUpdate.EsSalida = False
                            movUpdate.Observaciones = "SE ADJUNTA INFORME/DOC: " & nuevoDoc.ReferenciaExterna
                            db.MovimientosDocumentos.Add(movUpdate)

                        Else
                            ' CASO 2: Padre está lejos -> Salida Automática
                            Dim movSalidaAuto As New MovimientosDocumentos()
                            movSalidaAuto.DocumentoId = nuevoDoc.Id
                            movSalidaAuto.FechaMovimiento = DateTime.Now.AddSeconds(2)
                            movSalidaAuto.Origen = "MESA DE ENTRADA"
                            movSalidaAuto.Destino = destinoDelPadre
                            movSalidaAuto.EsSalida = True
                            movSalidaAuto.Observaciones = "PASE AUTOMÁTICO (Alcanza al Exp. Principal)"
                            db.MovimientosDocumentos.Add(movSalidaAuto)

                            MessageBox.Show($"El documento ha sido registrado y enviado automáticamente a {destinoDelPadre} para unirse al principal.", "Ruteo Automático", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If

                        db.SaveChanges()
                    End If
                End If

                ' Mensaje final estándar si no hubo ruteo automático
                If _idDocumentoEditar > 0 Then
                    MessageBox.Show("Cambios guardados correctamente.", "Éxito")
                ElseIf _idPadreVerificado = 0 OrElse (padreEstaEnMesa And _idPadreVerificado > 0) Then
                    MessageBox.Show("Ingreso registrado correctamente.", "Éxito")
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