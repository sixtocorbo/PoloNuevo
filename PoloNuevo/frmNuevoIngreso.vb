Imports System.IO
Imports System.Data
Imports System.Drawing
Imports System.Linq ' Necesario para las consultas LINQ

Public Class frmNuevoIngreso

    ' =========================================================
    ' VARIABLES DE ESTADO
    ' =========================================================
    Private _idDocumentoEditar As Integer = 0
    Private _listaCompletaReclusos As New List(Of ReclusoItem)

    ' NUEVA VARIABLE: Caché para la búsqueda rápida de orígenes
    Private _todosOrigenes As New List(Of String)

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

        ' Ajustes visuales para la lista de sugerencias
        lstSugerenciasOrigen.Visible = False
        lstSugerenciasOrigen.BringToFront()

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

            ' 3. Cargar Orígenes (AHORA EN UNA LISTA EN MEMORIA, NO EN COMBO)
            Dim listaTemp = db.MovimientosDocumentos.Where(Function(m) m.Origen <> "" And m.Origen <> "SISTEMA") _
                                                    .Select(Function(m) m.Origen).Distinct().ToList()

            Dim defaults As String() = {"JUZGADO LETRADO", "MINISTERIO DEL INTERIOR", "FISCALÍA", "DEFENSORÍA", "DIRECCIÓN", "JEFATURA DE SERVICIO", "OGLAST"}

            For Each def In defaults
                If Not listaTemp.Contains(def) Then listaTemp.Add(def)
            Next

            ' Ordenamos y guardamos en la variable global para búsqueda rápida
            listaTemp.Sort()
            _todosOrigenes = listaTemp
        End Using
    End Sub

    ' =========================================================
    ' LÓGICA DE BÚSQUEDA INTELIGENTE (ORIGEN)
    ' =========================================================
    Private Sub txtOrigen_TextChanged(sender As Object, e As EventArgs) Handles txtOrigen.TextChanged
        Dim texto As String = txtOrigen.Text.Trim().ToLower()

        ' Si está vacío, ocultamos la lista
        If String.IsNullOrEmpty(texto) Then
            lstSugerenciasOrigen.Visible = False
            Return
        End If

        If _todosOrigenes Is Nothing Then Return

        ' Algoritmo: Divide lo que escribes por espacios
        Dim palabras As String() = texto.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)

        ' === CORRECCIÓN AQUÍ ===
        ' Usamos una lambda de una sola línea (más estable en VB.NET)
        ' Busca elementos donde TODAS (.All) las palabras escritas estén contenidas
        Dim resultados = _todosOrigenes.Where(Function(o) palabras.All(Function(p) o.ToLower().Contains(p))) _
                                       .Take(10) _
                                       .ToList()
        ' =======================

        If resultados.Count > 0 Then
            lstSugerenciasOrigen.DataSource = resultados
            lstSugerenciasOrigen.Visible = True
            ' Posicionamos la lista justo debajo del TextBox
            lstSugerenciasOrigen.Top = txtOrigen.Bottom + 2
            lstSugerenciasOrigen.Left = txtOrigen.Left
            lstSugerenciasOrigen.Width = txtOrigen.Width
            lstSugerenciasOrigen.BringToFront()
        Else
            lstSugerenciasOrigen.Visible = False
        End If
    End Sub
    ' =========================================================
    ' NAVEGACIÓN POR TECLADO EN EL BUSCADOR
    ' =========================================================
    Private Sub txtOrigen_KeyDown(sender As Object, e As KeyEventArgs) Handles txtOrigen.KeyDown
        ' Solo actuamos si la lista de sugerencias está visible
        If lstSugerenciasOrigen.Visible AndAlso lstSugerenciasOrigen.Items.Count > 0 Then
            Select Case e.KeyCode
                Case Keys.Down
                    ' Flecha ABAJO: Mover selección hacia abajo
                    e.Handled = True ' Evita que el cursor se mueva en el TextBox
                    Dim index As Integer = lstSugerenciasOrigen.SelectedIndex
                    If index < lstSugerenciasOrigen.Items.Count - 1 Then
                        lstSugerenciasOrigen.SelectedIndex += 1
                    ElseIf index = -1 Then
                        ' Si no había nada seleccionado, seleccionar el primero
                        lstSugerenciasOrigen.SelectedIndex = 0
                    End If

                Case Keys.Up
                    ' Flecha ARRIBA: Mover selección hacia arriba
                    e.Handled = True
                    Dim index As Integer = lstSugerenciasOrigen.SelectedIndex
                    If index > 0 Then
                        lstSugerenciasOrigen.SelectedIndex -= 1
                    End If

                Case Keys.Enter
                    ' ENTER: Confirmar selección
                    e.SuppressKeyPress = True ' Evita el sonido "Ding" y el salto de línea
                    If lstSugerenciasOrigen.SelectedItem IsNot Nothing Then
                        txtOrigen.Text = lstSugerenciasOrigen.SelectedItem.ToString()
                        lstSugerenciasOrigen.Visible = False
                        txtOrigen.SelectionStart = txtOrigen.Text.Length ' Poner cursor al final
                        txtOrigen.Focus()
                    End If

                Case Keys.Escape
                    ' ESCAPE: Cerrar la lista si el usuario se arrepiente
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
        ' Pequeño hack: Si el foco se fue a la lista (porque hice clic), no la ocultes todavía
        If Not lstSugerenciasOrigen.Focused Then
            lstSugerenciasOrigen.Visible = False
        End If
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
                        ' CAMBIO: Ahora asignamos al TextBox
                        txtOrigen.Text = primerMov.Origen
                    End If

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

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        ' 1. Validaciones
        If String.IsNullOrWhiteSpace(txtAsunto.Text) Then
            MessageBox.Show("Debe ingresar el Asunto o Descripción.", "Faltan datos", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' CAMBIO: Validamos txtOrigen en lugar del combo
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
                db.SaveChanges() ' Guardamos para tener el ID del nuevo documento

                ' ---------------------------------------------------------
                ' PASO 2: LÓGICA DE MOVIMIENTOS E VINCULACIÓN
                ' ---------------------------------------------------------
                If _idDocumentoEditar = 0 Then

                    ' A) Analizar Dónde está el Padre
                    If _idPadreVerificado > 0 Then
                        Dim padre = db.Documentos.Find(_idPadreVerificado)
                        If padre IsNot Nothing Then
                            referenciaPadre = padre.ReferenciaExterna
                            Dim ultimoMov = padre.MovimientosDocumentos _
                                                 .OrderByDescending(Function(m) m.FechaMovimiento) _
                                                 .FirstOrDefault()

                            If ultimoMov IsNot Nothing AndAlso Not String.IsNullOrEmpty(ultimoMov.Destino) Then
                                destinoDelPadre = ultimoMov.Destino
                                If destinoDelPadre <> "MESA DE ENTRADA" Then padreEstaEnMesa = False
                            End If

                            ' =========================================================
                            ' === [NUEVO] AQUÍ GUARDAMOS EL VÍNCULO SEGURO POR ID ===
                            ' =========================================================
                            Dim vinculo As New DocumentoVinculos()
                            vinculo.IdDocumentoPadre = _idPadreVerificado ' El ID del Padre encontrado
                            vinculo.IdDocumentoHijo = nuevoDoc.Id         ' El ID del Nuevo que acabamos de crear
                            vinculo.TipoRelacion = "INGRESO RESPUESTA"
                            vinculo.FechaVinculo = DateTime.Now

                            db.DocumentoVinculos.Add(vinculo)
                            ' =========================================================
                        End If
                    End If

                    ' B) Registrar la ENTRADA (Siempre debe constar que entró)
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

                    ' C) RUTEO AUTOMÁTICO
                    If _idPadreVerificado > 0 Then
                        If padreEstaEnMesa Then
                            ' CASO 1: Padre sigue en Mesa (Pendiente) -> Se quedan juntos
                            Dim movUpdate As New MovimientosDocumentos()
                            movUpdate.DocumentoId = _idPadreVerificado
                            movUpdate.FechaMovimiento = DateTime.Now.AddSeconds(1)
                            movUpdate.Origen = txtOrigen.Text.Trim().ToUpper()
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