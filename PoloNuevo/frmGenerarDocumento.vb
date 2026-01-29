Imports System.IO
Imports System.Data
Imports System.Data.Entity

Public Class frmGenerarDocumento

    ' Variables Principales
    Private _idDocExterno As Integer = 0
    Private _archivoBytes As Byte() = Nothing
    Private _archivoNombre As String = ""
    Private _archivoExt As String = ""

    ' Variables para control de Rangos
    Private _tieneRangoActivo As Boolean = False
    Private _minRango As Integer = 0
    Private _maxRango As Integer = 0
    Private _nombreRango As String = ""

    ' Constructor Obligatorio con ID del padre
    Public Sub New(idDocExterno As Integer)
        InitializeComponent()
        _idDocExterno = idDocExterno
    End Sub

    Private Sub frmGenerarDocumento_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CargarDatosIniciales()

        ' Seguridad: Si el usuario olvidó borrar el checkbox del diseño, lo ocultamos por código
        ' para que no confunda. (Si ya lo borraste, puedes quitar estas líneas).
        Dim controls = Me.Controls.Find("chkMoverOriginal", True)
        If controls.Length > 0 Then controls(0).Visible = False
    End Sub

    Private Sub CargarDatosIniciales()
        Try
            Using db As New PoloNuevoEntities()
                ' 1. Cargar Tipos (Solo producción propia)
                cmbTipo.DataSource = db.TiposDocumento.Where(Function(t) t.Nombre <> "ARCHIVO").OrderBy(Function(t) t.Nombre).ToList()
                cmbTipo.DisplayMember = "Nombre"
                cmbTipo.ValueMember = "Id"

                ' 2. Cargar Destinos (Histórico)
                Dim destinos = db.MovimientosDocumentos.Where(Function(m) m.Destino <> "").Select(Function(m) m.Destino).Distinct().ToList()
                cmbDestino.DataSource = destinos
                cmbDestino.SelectedIndex = -1

                ' 3. Información del Padre (Solo visual)
                If _idDocExterno > 0 Then
                    Dim docExt = db.Documentos.Find(_idDocExterno)
                    If docExt IsNot Nothing Then
                        lblRefExterna.Text = $"RESPONDIENDO AL EXPEDIENTE: {docExt.TiposDocumento.Nombre} {docExt.ReferenciaExterna}"
                        lblRefExterna.ForeColor = Color.Blue

                        ' Autocompletar asunto para mantener hilo
                        txtAsunto.Text = $"REF {docExt.ReferenciaExterna} // "
                        txtAsunto.SelectionStart = txtAsunto.Text.Length
                    End If
                Else
                    lblRefExterna.Text = "Nuevo Documento Independiente"
                    lblRefExterna.ForeColor = Color.Black
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("Error cargando datos iniciales: " & ex.Message)
        End Try
    End Sub

    ' =========================================================
    ' LÓGICA DE RANGOS
    ' =========================================================
    Private Sub cmbTipo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbTipo.SelectedIndexChanged
        If cmbTipo.SelectedValue IsNot Nothing AndAlso IsNumeric(cmbTipo.SelectedValue) Then
            Dim idTipo As Integer = Convert.ToInt32(cmbTipo.SelectedValue)
            CalcularProximoNumero(idTipo)
        End If
    End Sub

    Private Sub CalcularProximoNumero(idTipo As Integer)
        Try
            Using db As New PoloNuevoEntities()
                Dim rango = db.NumeracionRangos.FirstOrDefault(Function(r) r.TipoDocumentoId = idTipo And r.Activo = True And r.UltimoUtilizado < r.NumeroFin)

                If rango IsNot Nothing Then
                    _tieneRangoActivo = True
                    _minRango = rango.NumeroInicio
                    _maxRango = rango.NumeroFin
                    _nombreRango = rango.NombreRango

                    Dim siguiente As Integer = rango.UltimoUtilizado + 1
                    txtNumero.Text = siguiente.ToString()
                    txtNumero.BackColor = Color.LightYellow
                    ToolTip1.SetToolTip(txtNumero, $"Rango Oficial: {_nombreRango}")
                Else
                    _tieneRangoActivo = False
                    txtNumero.Text = ""
                    txtNumero.BackColor = Color.White
                End If
            End Using
        Catch ex As Exception
            txtNumero.Text = ""
        End Try
    End Sub

    ' =========================================================
    ' GUARDADO AUTOMATIZADO (NUEVO + ORIGINAL)
    ' =========================================================
    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        ' 1. Validaciones básicas
        If txtNumero.Text.Trim = "" Or txtAsunto.Text.Trim = "" Or cmbDestino.Text.Trim = "" Then
            MessageBox.Show("Faltan datos obligatorios.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Using db As New PoloNuevoEntities()
                ' Validar Rango
                Dim numVal As Integer
                If _tieneRangoActivo AndAlso Integer.TryParse(txtNumero.Text, numVal) Then
                    If numVal < _minRango Or numVal > _maxRango Then
                        If MessageBox.Show("El número está FUERA del rango oficial. ¿Guardar igual?", "Alerta", MessageBoxButtons.YesNo) = DialogResult.No Then Return
                    End If
                End If

                ' ---------------------------------------------------------
                ' PASO 1: CREAR EL NUEVO DOCUMENTO (HIJO)
                ' ---------------------------------------------------------
                Dim docRespuesta As New Documentos()
                docRespuesta.FechaCarga = DateTime.Now
                docRespuesta.TipoDocumentoId = Convert.ToInt32(cmbTipo.SelectedValue)
                docRespuesta.ReferenciaExterna = txtNumero.Text.Trim()
                docRespuesta.Descripcion = txtAsunto.Text.Trim()

                If _archivoBytes IsNot Nothing Then
                    docRespuesta.Contenido = _archivoBytes
                    docRespuesta.NombreArchivo = _archivoNombre
                    docRespuesta.Extension = _archivoExt
                Else
                    docRespuesta.Contenido = New Byte() {0}
                    docRespuesta.NombreArchivo = "Generado Interno"
                    docRespuesta.Extension = ".phy"
                End If

                ' Actualizar Rango
                If _tieneRangoActivo AndAlso Integer.TryParse(txtNumero.Text, numVal) Then
                    Dim rango = db.NumeracionRangos.FirstOrDefault(Function(r) r.TipoDocumentoId = docRespuesta.TipoDocumentoId And r.Activo = True)
                    If rango IsNot Nothing AndAlso numVal > rango.UltimoUtilizado Then
                        rango.UltimoUtilizado = numVal
                    End If
                End If

                db.Documentos.Add(docRespuesta)
                db.SaveChanges() ' ID Generado

                ' ---------------------------------------------------------
                ' PASO 2: CREACIÓN (NACIMIENTO)
                ' ---------------------------------------------------------
                Dim movCreacion As New MovimientosDocumentos()
                movCreacion.DocumentoId = docRespuesta.Id
                movCreacion.FechaMovimiento = DateTime.Now
                movCreacion.Origen = "SISTEMA"
                movCreacion.Destino = "SECRETARÍA (MI OFICINA)"
                movCreacion.EsSalida = False
                movCreacion.Observaciones = "Generado automáticamente"
                db.MovimientosDocumentos.Add(movCreacion)

                ' ---------------------------------------------------------
                ' PASO 3: SALIDA DEL NUEVO DOCUMENTO
                ' ---------------------------------------------------------
                Dim destinoFinal As String = cmbDestino.Text.Trim().ToUpper()

                Dim movSalida As New MovimientosDocumentos()
                movSalida.DocumentoId = docRespuesta.Id
                movSalida.FechaMovimiento = DateTime.Now.AddSeconds(1)
                movSalida.Origen = "SECRETARÍA (MI OFICINA)"
                movSalida.Destino = destinoFinal
                movSalida.EsSalida = True
                movSalida.Observaciones = "Enviado como respuesta/actuación."
                db.MovimientosDocumentos.Add(movSalida)

                ' ---------------------------------------------------------
                ' PASO 4: ARRASTRE DEL DOCUMENTO ORIGINAL (AUTOMÁTICO)
                ' ---------------------------------------------------------
                ' AQUÍ ESTÁ LA MAGIA: Si hay padre, viaja con el hijo.
                If _idDocExterno > 0 Then
                    Dim movOriginal As New MovimientosDocumentos()
                    movOriginal.DocumentoId = _idDocExterno
                    movOriginal.FechaMovimiento = DateTime.Now.AddSeconds(1)
                    movOriginal.Origen = "MESA DE ENTRADA / SECRETARÍA"
                    movOriginal.Destino = destinoFinal
                    movOriginal.EsSalida = True

                    ' Dejamos constancia en el historial de por qué se movió
                    movOriginal.Observaciones = $"Se adjunta al nuevo {cmbTipo.Text} N° {docRespuesta.ReferenciaExterna}"

                    db.MovimientosDocumentos.Add(movOriginal)
                End If

                db.SaveChanges()

                MessageBox.Show("Documento generado y enviado correctamente (Expediente actualizado).", "Proceso Completo")
                Me.DialogResult = DialogResult.OK
                Me.Close()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub

    Private Sub btnAdjuntar_Click(sender As Object, e As EventArgs) Handles btnAdjuntar.Click
        Using ofd As New OpenFileDialog()
            ofd.Filter = "Archivos|*.pdf;*.jpg;*.jpeg;*.png;*.doc;*.docx"
            If ofd.ShowDialog() = DialogResult.OK Then
                _archivoBytes = File.ReadAllBytes(ofd.FileName)
                _archivoNombre = Path.GetFileName(ofd.FileName)
                _archivoExt = Path.GetExtension(ofd.FileName)
                lblArchivo.Text = _archivoNombre
                lblArchivo.ForeColor = Color.Green
            End If
        End Using
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Me.Close()
    End Sub
End Class