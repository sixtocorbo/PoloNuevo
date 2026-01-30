Imports System.IO
Imports System.Data
Imports System.Data.Entity

Public Class frmGenerarDocumento

    ' =========================================================
    ' VARIABLES GLOBALES
    ' =========================================================
    Private _idDocExterno As Integer = 0
    Private _archivoBytes As Byte() = Nothing
    Private _archivoNombre As String = ""
    Private _archivoExt As String = ""

    ' Variables para control de Rangos (Numeración Automática)
    Private _tieneRangoActivo As Boolean = False
    Private _minRango As Integer = 0
    Private _maxRango As Integer = 0
    Private _nombreRango As String = ""

    ' =========================================================
    ' CONSTRUCTOR
    ' =========================================================
    Public Sub New(idDocExterno As Integer)
        InitializeComponent()
        _idDocExterno = idDocExterno
    End Sub

    ' =========================================================
    ' CARGA INICIAL
    ' =========================================================
    Private Sub frmGenerarDocumento_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CargarDatosIniciales()

        ' Ocultar controles que no apliquen en este modo
        Dim controls = Me.Controls.Find("chkMoverOriginal", True)
        If controls.Length > 0 Then controls(0).Visible = False
    End Sub

    Private Sub CargarDatosIniciales()
        Try
            Using db As New PoloNuevoEntities()
                ' 1. Cargar Tipos (Solo producción propia, excluyendo ARCHIVO)
                cmbTipo.DataSource = db.TiposDocumento.Where(Function(t) t.Nombre <> "ARCHIVO").OrderBy(Function(t) t.Nombre).ToList()
                cmbTipo.DisplayMember = "Nombre"
                cmbTipo.ValueMember = "Id"

                ' 2. Cargar Destinos (Basado en el historial para autocompletar)
                Dim destinos = db.MovimientosDocumentos _
                                 .Where(Function(m) m.Destino IsNot Nothing And m.Destino <> "") _
                                 .Select(Function(m) m.Destino) _
                                 .Distinct().ToList()

                cmbDestino.DataSource = destinos
                cmbDestino.SelectedIndex = -1

                ' 3. Información del Padre (Referencia Visual)
                If _idDocExterno > 0 Then
                    Dim docExt = db.Documentos.Find(_idDocExterno)
                    If docExt IsNot Nothing Then
                        lblRefExterna.Text = $"RESPONDIENDO A: {docExt.TiposDocumento.Nombre} {docExt.ReferenciaExterna}"
                        lblRefExterna.ForeColor = Color.Blue

                        ' Sugerir Asunto prellenado
                        txtAsunto.Text = $"REF {docExt.ReferenciaExterna} // "
                        txtAsunto.SelectionStart = txtAsunto.Text.Length

                        ' Intentar preseleccionar el destino sugerido (Origen del padre)
                        ' Buscamos el último movimiento para saber de dónde vino
                        Dim ultimoMov = docExt.MovimientosDocumentos.OrderByDescending(Function(m) m.FechaMovimiento).FirstOrDefault()
                        If ultimoMov IsNot Nothing AndAlso Not String.IsNullOrEmpty(ultimoMov.Origen) Then
                            cmbDestino.Text = ultimoMov.Origen ' Sugerimos devolverlo a quien lo mandó
                        End If
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
    ' LÓGICA DE RANGOS Y NUMERACIÓN AUTOMÁTICA
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
    ' GUARDADO PRINCIPAL (GENERAR ACTUACIÓN Y MOVER TODO)
    ' =========================================================
    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        ' 1. Validaciones básicas
        If txtNumero.Text.Trim = "" Or txtAsunto.Text.Trim = "" Or cmbDestino.Text.Trim = "" Then
            MessageBox.Show("Faltan datos obligatorios (Número, Asunto o Destino).", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Me.Cursor = Cursors.WaitCursor
            Using db As New PoloNuevoEntities()

                ' Validar Rango si aplica
                Dim numVal As Integer
                If _tieneRangoActivo AndAlso Integer.TryParse(txtNumero.Text, numVal) Then
                    If numVal < _minRango Or numVal > _maxRango Then
                        If MessageBox.Show("El número está FUERA del rango oficial. ¿Guardar igual?", "Alerta", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.No Then
                            Me.Cursor = Cursors.Default
                            Return
                        End If
                    End If
                End If

                ' ---------------------------------------------------------
                ' PASO 1: CREAR EL NUEVO DOCUMENTO (TU RESPUESTA/ACTUACIÓN)
                ' ---------------------------------------------------------
                Dim docRespuesta As New Documentos()
                docRespuesta.FechaCarga = DateTime.Now
                docRespuesta.TipoDocumentoId = Convert.ToInt32(cmbTipo.SelectedValue)
                docRespuesta.ReferenciaExterna = txtNumero.Text.Trim()
                docRespuesta.Descripcion = txtAsunto.Text.Trim()

                ' NOTA: Si en el futuro agregas la columna DocumentoPadreId a la BD, descomenta esto:
                ' If _idDocExterno > 0 Then docRespuesta.DocumentoPadreId = _idDocExterno

                ' Gestión de Archivo Adjunto
                If _archivoBytes IsNot Nothing Then
                    docRespuesta.Contenido = _archivoBytes
                    docRespuesta.NombreArchivo = _archivoNombre
                    docRespuesta.Extension = _archivoExt
                Else
                    ' Archivo dummy si es papel físico o no se cargó nada
                    docRespuesta.Contenido = New Byte() {0}
                    docRespuesta.NombreArchivo = "Generado Interno"
                    docRespuesta.Extension = ".phy"
                End If

                ' Actualizar el contador del Rango si corresponde
                If _tieneRangoActivo AndAlso Integer.TryParse(txtNumero.Text, numVal) Then
                    Dim rango = db.NumeracionRangos.FirstOrDefault(Function(r) r.TipoDocumentoId = docRespuesta.TipoDocumentoId And r.Activo = True)
                    If rango IsNot Nothing AndAlso numVal > rango.UltimoUtilizado Then
                        rango.UltimoUtilizado = numVal
                    End If
                End If

                db.Documentos.Add(docRespuesta)
                db.SaveChanges() ' Obtenemos el ID del nuevo documento

                ' ---------------------------------------------------------
                ' PASO 2: EL NACIMIENTO (Entrada Administrativa)
                ' ---------------------------------------------------------
                ' Creamos un movimiento inicial para que conste que se creó aquí.
                Dim movCreacion As New MovimientosDocumentos()
                movCreacion.DocumentoId = docRespuesta.Id
                movCreacion.FechaMovimiento = DateTime.Now
                movCreacion.Origen = "SISTEMA"
                movCreacion.Destino = "MESA DE ENTRADA" ' O "MI OFICINA"
                movCreacion.EsSalida = False
                movCreacion.Observaciones = "Generación de Documento"
                db.MovimientosDocumentos.Add(movCreacion)

                ' ---------------------------------------------------------
                ' PASO 3: LA SALIDA (ESTO EVITA QUE SE QUEDE EN BANDEJA)
                ' ---------------------------------------------------------
                Dim destinoFinal As String = cmbDestino.Text.Trim().ToUpper()

                Dim movSalida As New MovimientosDocumentos()
                movSalida.DocumentoId = docRespuesta.Id
                movSalida.FechaMovimiento = DateTime.Now.AddSeconds(1) ' +1 seg para que sea posterior
                movSalida.Origen = "MESA DE ENTRADA"
                movSalida.Destino = destinoFinal
                movSalida.EsSalida = True ' <--- CLAVE: Esto lo saca de "Pendientes"
                movSalida.Observaciones = "Pase / Respuesta enviada"
                db.MovimientosDocumentos.Add(movSalida)

                ' ---------------------------------------------------------
                ' PASO 4: ARRASTRE AUTOMÁTICO DEL DOCUMENTO ORIGINAL (PADRE)
                ' ---------------------------------------------------------
                ' Si estamos respondiendo a un Oficio, ese Oficio también se va con la respuesta.
                If _idDocExterno > 0 Then
                    Dim movOriginal As New MovimientosDocumentos()
                    movOriginal.DocumentoId = _idDocExterno
                    movOriginal.FechaMovimiento = DateTime.Now.AddSeconds(1)
                    movOriginal.Origen = "MESA DE ENTRADA"
                    movOriginal.Destino = destinoFinal
                    movOriginal.EsSalida = True ' <--- CLAVE: El padre también se va

                    ' Referencia cruzada en la observación
                    movOriginal.Observaciones = $"ADJUNTO A NUEVO: {docRespuesta.ReferenciaExterna}"

                    db.MovimientosDocumentos.Add(movOriginal)
                End If

                db.SaveChanges()

                MessageBox.Show("Documento generado y enviado correctamente." & vbCrLf &
                                "El documento original también ha sido movido al destino.",
                                "Proceso Completo", MessageBoxButtons.OK, MessageBoxIcon.Information)
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
    ' EVENTOS DE INTERFAZ
    ' =========================================================
    Private Sub btnAdjuntar_Click(sender As Object, e As EventArgs) Handles btnAdjuntar.Click
        Using ofd As New OpenFileDialog()
            ofd.Filter = "Archivos|*.pdf;*.jpg;*.jpeg;*.png;*.doc;*.docx"
            ofd.Title = "Seleccionar adjunto digital"
            If ofd.ShowDialog() = DialogResult.OK Then
                _archivoBytes = File.ReadAllBytes(ofd.FileName)
                _archivoNombre = Path.GetFileName(ofd.FileName)
                _archivoExt = Path.GetExtension(ofd.FileName).ToLower()
                lblArchivo.Text = _archivoNombre
                lblArchivo.ForeColor = Color.Green
            End If
        End Using
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Me.Close()
    End Sub

End Class