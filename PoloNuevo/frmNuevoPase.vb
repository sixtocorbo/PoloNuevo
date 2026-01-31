Imports System.IO
Imports System.Data
Imports System.Data.Entity

Public Class frmNuevoPase

    ' VARIABLES GLOBALES
    Private _idDocumento As Integer
    Private _idMovimientoEditar As Integer = 0
    Private _todosDestinos As New List(Of String) ' Caché de destinos para búsqueda rápida

    ' Variables para Actuación
    Private _archivoBytes As Byte() = Nothing
    Private _archivoNombre As String = ""
    Private _archivoExt As String = ""
    Private _tieneRangoActivo As Boolean = False
    Private _rangoId As Integer = 0

    ' --- CONSTRUCTORES (Igual que antes) ---
    Public Sub New(idDocumento As Integer)
        InitializeComponent()
        _idDocumento = idDocumento
        _idMovimientoEditar = 0
    End Sub

    Public Sub New(idDocumento As Integer, idMovimiento As Integer)
        InitializeComponent()
        _idDocumento = idDocumento
        _idMovimientoEditar = idMovimiento
    End Sub

    Public Sub New()
        InitializeComponent()
        _idDocumento = 0
        _idMovimientoEditar = 0
    End Sub

    ' --- CARGA INICIAL ---
    Private Sub frmNuevoPase_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CargarListas()

        If _idMovimientoEditar > 0 Then
            Me.Text = "Corregir Movimiento"
            btnGuardar.Text = "GUARDAR CAMBIOS"
            chkGenerarActuacion.Visible = False
            grpActuacion.Visible = False
            CargarDatosEdicion()
        Else
            If _idDocumento = 0 Then
                Me.Text = "Nuevo Documento Independiente"
                chkGenerarActuacion.Checked = True
                chkGenerarActuacion.Enabled = False
                chkGenerarActuacion.Text = "Generando Nuevo Documento..."
            Else
                Me.Text = "Registrar Pase / Respuesta"
                chkGenerarActuacion.Checked = False
                dtpFecha.Value = DateTime.Now
            End If
        End If
    End Sub

    Private Sub CargarListas()
        Using db As New PoloNuevoEntities()
            ' 1. Cargar Destinos: FILTRO BLINDADO CONTRA NULOS
            _todosDestinos = db.MovimientosDocumentos _
                               .Where(Function(m) m.Destino IsNot Nothing AndAlso m.Destino <> "") _
                               .Select(Function(m) m.Destino) _
                               .Distinct() _
                               .OrderBy(Function(d) d) _
                               .ToList()

            ' 2. Cargar Tipos (Igual que antes)
            cmbTipo.DataSource = db.TiposDocumento.Where(Function(t) t.Nombre <> "ARCHIVO").OrderBy(Function(t) t.Nombre).ToList()
            cmbTipo.DisplayMember = "Nombre"
            cmbTipo.ValueMember = "Id"
            cmbTipo.SelectedIndex = -1
        End Using
    End Sub

    Private Sub CargarDatosEdicion()
        Using db As New PoloNuevoEntities()
            Dim mov = db.MovimientosDocumentos.Find(_idMovimientoEditar)
            If mov IsNot Nothing Then
                txtDestino.Text = mov.Destino ' Asignamos al TextBox
                dtpFecha.Value = mov.FechaMovimiento
            End If
        End Using
    End Sub


    ' =========================================================
    ' BUSCADOR INTELIGENTE DE DESTINOS
    ' =========================================================
    Private Sub txtDestino_TextChanged(sender As Object, e As EventArgs) Handles txtDestino.TextChanged
        Dim texto As String = txtDestino.Text.Trim().ToLower()

        ' Si está vacío, ocultamos la lista
        If String.IsNullOrEmpty(texto) Then
            lstSugerencias.Visible = False
            Return
        End If

        ' PROTECCIÓN: Si la lista aún no cargó
        If _todosDestinos Is Nothing Then Return

        ' Algoritmo: Divide lo que escribes por espacios
        Dim palabras As String() = texto.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)

        ' === LÓGICA MEJORADA ===
        ' Busca destinos donde TODAS las palabras escritas aparezcan, sin importar el orden.
        Dim resultados = _todosDestinos.Where(Function(d) d IsNot Nothing AndAlso palabras.All(Function(p) d.ToLower().Contains(p))) _
                                       .Take(10) _
                                       .ToList()
        ' =======================

        If resultados.Count > 0 Then
            lstSugerencias.DataSource = resultados
            lstSugerencias.Visible = True
            lstSugerencias.Top = txtDestino.Bottom + 2
            lstSugerencias.Left = txtDestino.Left
            lstSugerencias.Width = txtDestino.Width
            lstSugerencias.BringToFront()
        Else
            lstSugerencias.Visible = False
        End If
    End Sub
    ' =========================================================
    ' NAVEGACIÓN POR TECLADO (Flechas y Enter)
    ' =========================================================
    Private Sub txtDestino_KeyDown(sender As Object, e As KeyEventArgs) Handles txtDestino.KeyDown
        If lstSugerencias.Visible AndAlso lstSugerencias.Items.Count > 0 Then
            Select Case e.KeyCode
                Case Keys.Down
                    ' Mover selección ABAJO
                    e.Handled = True
                    Dim index As Integer = lstSugerencias.SelectedIndex
                    If index < lstSugerencias.Items.Count - 1 Then
                        lstSugerencias.SelectedIndex += 1
                    ElseIf index = -1 Then
                        lstSugerencias.SelectedIndex = 0
                    End If

                Case Keys.Up
                    ' Mover selección ARRIBA
                    e.Handled = True
                    Dim index As Integer = lstSugerencias.SelectedIndex
                    If index > 0 Then
                        lstSugerencias.SelectedIndex -= 1
                    End If

                Case Keys.Enter
                    ' ENTER para confirmar
                    e.SuppressKeyPress = True ' Evita el sonido "Ding"
                    If lstSugerencias.SelectedItem IsNot Nothing Then
                        txtDestino.Text = lstSugerencias.SelectedItem.ToString()
                        lstSugerencias.Visible = False
                        txtDestino.SelectionStart = txtDestino.Text.Length
                        txtDestino.Focus()
                    End If

                Case Keys.Escape
                    ' ESCAPE para cerrar sugerencias
                    e.Handled = True
                    lstSugerencias.Visible = False
            End Select
        End If
    End Sub
    Private Sub lstSugerencias_Click(sender As Object, e As EventArgs) Handles lstSugerencias.Click
        If lstSugerencias.SelectedItem IsNot Nothing Then
            txtDestino.Text = lstSugerencias.SelectedItem.ToString()
            lstSugerencias.Visible = False
            txtDestino.SelectionStart = txtDestino.Text.Length ' Cursor al final
            txtDestino.Focus()
        End If
    End Sub

    ' Para ocultar la lista si hacemos clic fuera o salimos
    Private Sub txtDestino_Leave(sender As Object, e As EventArgs) Handles txtDestino.Leave
        ' Pequeño retardo para permitir el clic en la lista antes de que desaparezca
        If Not lstSugerencias.Focused Then
            lstSugerencias.Visible = False
        End If
    End Sub

    ' =========================================================
    ' LÓGICA DE INTERFAZ Y GUARDADO (Igual que antes pero usando txtDestino)
    ' =========================================================
    Private Sub chkGenerarActuacion_CheckedChanged(sender As Object, e As EventArgs) Handles chkGenerarActuacion.CheckedChanged
        grpActuacion.Visible = chkGenerarActuacion.Checked
        If chkGenerarActuacion.Checked Then
            btnGuardar.Text = "GENERAR Y ENVIAR"
            btnGuardar.BackColor = Color.ForestGreen
            If _idDocumento > 0 AndAlso String.IsNullOrEmpty(txtAsunto.Text) Then
                Using db As New PoloNuevoEntities()
                    Dim doc = db.Documentos.Include("TiposDocumento").FirstOrDefault(Function(d) d.Id = _idDocumento)
                    If doc IsNot Nothing Then
                        txtAsunto.Text = $"REF {doc.TiposDocumento.Nombre} {doc.ReferenciaExterna} // "
                        txtAsunto.SelectionStart = txtAsunto.Text.Length
                    End If
                End Using
            End If
        Else
            btnGuardar.Text = "CONFIRMAR PASE"
            btnGuardar.BackColor = Color.SteelBlue
        End If
    End Sub

    Private Sub btnAdjuntar_Click(sender As Object, e As EventArgs) Handles btnAdjuntar.Click
        Using ofd As New OpenFileDialog()
            ofd.Filter = "Documentos|*.pdf;*.doc;*.docx;*.jpg;*.png"
            If ofd.ShowDialog() = DialogResult.OK Then
                _archivoBytes = File.ReadAllBytes(ofd.FileName)
                _archivoNombre = Path.GetFileName(ofd.FileName)
                _archivoExt = Path.GetExtension(ofd.FileName).ToLower()
                lblArchivo.Text = _archivoNombre
                lblArchivo.ForeColor = Color.Green
            End If
        End Using
    End Sub

    Private Sub cmbTipo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbTipo.SelectedIndexChanged
        If cmbTipo.SelectedValue IsNot Nothing AndAlso IsNumeric(cmbTipo.SelectedValue) Then
            CalcularNumeroEstricto(Convert.ToInt32(cmbTipo.SelectedValue))
        End If
    End Sub

    Private Sub CalcularNumeroEstricto(idTipo As Integer)
        Using db As New PoloNuevoEntities()
            Dim rango = db.NumeracionRangos.FirstOrDefault(Function(r) r.TipoDocumentoId = idTipo And r.Activo = True And r.UltimoUtilizado < r.NumeroFin)
            If rango IsNot Nothing Then
                _tieneRangoActivo = True
                _rangoId = rango.Id
                txtNumero.Text = (rango.UltimoUtilizado + 1).ToString()
                txtNumero.BackColor = Color.LightYellow
                lblInfoRango.Text = $"Rango Activo: {rango.NombreRango} (Disp: {rango.NumeroFin - rango.UltimoUtilizado})"
                lblInfoRango.ForeColor = Color.Green
            Else
                _tieneRangoActivo = False
                txtNumero.Text = "SIN RANGO"
                txtNumero.BackColor = Color.MistyRose
                lblInfoRango.Text = "No existe rango disponible."
                lblInfoRango.ForeColor = Color.Red
            End If
        End Using
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        ' Validación: Ahora usamos txtDestino en lugar del combo
        If String.IsNullOrWhiteSpace(txtDestino.Text) Then
            MessageBox.Show("Debe indicar el Destino u Oficina.", "Falta Destino", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtDestino.Focus()
            Return
        End If

        Try
            Using db As New PoloNuevoEntities()
                Dim destinoFinal As String = txtDestino.Text.Trim().ToUpper()

                If chkGenerarActuacion.Checked Then
                    If Not _tieneRangoActivo Then
                        MessageBox.Show("ERROR BLOQUEANTE: Sin rango no se puede generar documento.", "Seguridad", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                        Return
                    End If
                    If String.IsNullOrWhiteSpace(txtAsunto.Text) Then
                        MessageBox.Show("Falta Asunto.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Return
                    End If

                    Dim docNuevo As New Documentos()
                    docNuevo.FechaCarga = DateTime.Now
                    docNuevo.TipoDocumentoId = Convert.ToInt32(cmbTipo.SelectedValue)
                    docNuevo.ReferenciaExterna = txtNumero.Text
                    docNuevo.Descripcion = txtAsunto.Text.Trim()

                    If _archivoBytes IsNot Nothing Then
                        docNuevo.Contenido = _archivoBytes
                        docNuevo.NombreArchivo = _archivoNombre
                        docNuevo.Extension = _archivoExt
                    Else
                        docNuevo.Contenido = New Byte() {0}
                        docNuevo.NombreArchivo = "Interno"
                        docNuevo.Extension = ".phy"
                    End If

                    Dim rango = db.NumeracionRangos.Find(_rangoId)
                    If rango IsNot Nothing Then
                        Dim numUsar As Integer = Convert.ToInt32(docNuevo.ReferenciaExterna)
                        If numUsar <= rango.UltimoUtilizado Then
                            MessageBox.Show("Número ocupado. Reintentando...", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                            CalcularNumeroEstricto(docNuevo.TipoDocumentoId)
                            Return
                        End If
                        rango.UltimoUtilizado = numUsar
                    End If

                    db.Documentos.Add(docNuevo)
                    db.SaveChanges()

                    Dim movNacer As New MovimientosDocumentos() With {
                        .DocumentoId = docNuevo.Id, .FechaMovimiento = dtpFecha.Value,
                        .Origen = "SISTEMA", .Destino = "MESA DE ENTRADA", .EsSalida = False, .Observaciones = "Generación Inicial"
                    }
                    db.MovimientosDocumentos.Add(movNacer)

                    Dim movSalir As New MovimientosDocumentos() With {
                        .DocumentoId = docNuevo.Id, .FechaMovimiento = dtpFecha.Value.AddSeconds(1),
                        .Origen = "MESA DE ENTRADA", .Destino = destinoFinal, .EsSalida = True, .Observaciones = "Enviado / Producido"
                    }
                    db.MovimientosDocumentos.Add(movSalir)

                    If _idDocumento > 0 Then
                        Dim movPadre As New MovimientosDocumentos() With {
    .DocumentoId = _idDocumento, .FechaMovimiento = dtpFecha.Value.AddSeconds(1),
    .Origen = "MESA DE ENTRADA", .Destino = destinoFinal, .EsSalida = True,    ' CORRECCIÓN: Agregamos cmbTipo.Text para que sea dinámico
                        .Observaciones = $"ADJUNTO A NUEVO: {cmbTipo.Text} {docNuevo.ReferenciaExterna}"
}
                        db.MovimientosDocumentos.Add(movPadre)
                    End If

                    db.SaveChanges()
                    MessageBox.Show($"Documento Nº {docNuevo.ReferenciaExterna} generado y enviado a {destinoFinal}.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)

                Else
                    ' PASE SIMPLE
                    If _idDocumento = 0 Then
                        MessageBox.Show("Error interno.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return
                    End If

                    If _idMovimientoEditar = 0 Then
                        Dim mov As New MovimientosDocumentos() With {
                            .DocumentoId = _idDocumento, .FechaMovimiento = dtpFecha.Value,
                            .Origen = "MESA DE ENTRADA", .Destino = destinoFinal, .EsSalida = True
                        }
                        Dim doc = db.Documentos.Find(_idDocumento)
                        If doc IsNot Nothing Then mov.Observaciones = $"PASE: {doc.ReferenciaExterna}"

                        db.MovimientosDocumentos.Add(mov)
                        db.SaveChanges()
                        MessageBox.Show($"Pase registrado a {destinoFinal}.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Else
                        Dim mov = db.MovimientosDocumentos.Find(_idMovimientoEditar)
                        If mov IsNot Nothing Then
                            mov.Destino = destinoFinal
                            mov.FechaMovimiento = dtpFecha.Value
                            db.SaveChanges()
                            MessageBox.Show("Movimiento actualizado.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If
                    End If
                End If

                Me.DialogResult = DialogResult.OK
                Me.Close()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Me.Close()
    End Sub
End Class