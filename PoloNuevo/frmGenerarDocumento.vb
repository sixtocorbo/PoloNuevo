Imports System.IO
Imports System.Data

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
    End Sub

    Private Sub CargarDatosIniciales()
        Try
            Using db As New PoloNuevoEntities()
                ' 1. Cargar Tipos (Solo producción propia, excluye 'Archivo' o 'Externo')
                cmbTipo.DataSource = db.TiposDocumento.Where(Function(t) t.Nombre <> "ARCHIVO").OrderBy(Function(t) t.Nombre).ToList()
                cmbTipo.DisplayMember = "Nombre"
                cmbTipo.ValueMember = "Id"

                ' 2. Cargar Destinos (Histórico)
                Dim destinos = db.MovimientosDocumentos.Where(Function(m) m.Destino <> "").Select(Function(m) m.Destino).Distinct().ToList()
                cmbDestino.DataSource = destinos
                cmbDestino.SelectedIndex = -1

                ' 3. Cargar Datos del Documento Padre
                Dim docExt = db.Documentos.Find(_idDocExterno)
                If docExt IsNot Nothing Then
                    lblRefExterna.Text = $"Respondiendo a: {docExt.TiposDocumento.Nombre} {docExt.ReferenciaExterna}"

                    ' Sugerimos el asunto
                    txtAsunto.Text = $"RESPUESTA A: {docExt.TiposDocumento.Nombre} {docExt.ReferenciaExterna} - "
                    txtAsunto.SelectionStart = txtAsunto.Text.Length

                    ' Personalizamos el texto del check
                    chkMoverOriginal.Text = $"Adjuntar y enviar el {docExt.TiposDocumento.Nombre} original a este destino"
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("Error cargando datos iniciales: " & ex.Message)
        End Try
    End Sub

    ' =========================================================
    ' LÓGICA DE RANGOS (Cálculo Automático)
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
                ' Buscamos un rango ACTIVO y con ESPACIO
                Dim rango = db.NumeracionRangos.FirstOrDefault(Function(r) r.TipoDocumentoId = idTipo And r.Activo = True And r.UltimoUtilizado < r.NumeroFin)

                If rango IsNot Nothing Then
                    ' --- TENEMOS RANGO ---
                    _tieneRangoActivo = True
                    _minRango = rango.NumeroInicio
                    _maxRango = rango.NumeroFin
                    _nombreRango = rango.NombreRango

                    Dim siguiente As Integer = rango.UltimoUtilizado + 1
                    txtNumero.Text = siguiente.ToString()
                    txtNumero.BackColor = Color.LightYellow
                    ToolTip1.SetToolTip(txtNumero, $"Rango Oficial: {_nombreRango}")
                Else
                    ' --- RANGO AGOTADO O INEXISTENTE ---
                    _tieneRangoActivo = False
                    txtNumero.Text = ""
                    txtNumero.BackColor = Color.White

                    ' Verificamos si hay rangos llenos para avisar
                    Dim hayLlenos = db.NumeracionRangos.Any(Function(r) r.TipoDocumentoId = idTipo And r.Activo = True)

                    If hayLlenos Then
                        txtNumero.BackColor = Color.MistyRose
                        Dim r = MessageBox.Show("El Rango Oficial para este documento se ha AGOTADO." & vbCrLf &
                                                "¿Desea configurar un nuevo rango ahora?", "Alerta de Numeración", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                        If r = DialogResult.Yes Then
                            Dim frm As New frmGestionRangos()
                            frm.ShowDialog()
                            CalcularProximoNumero(idTipo) ' Reintentamos al volver
                        End If
                    End If
                End If
            End Using
        Catch ex As Exception
            txtNumero.Text = ""
        End Try
    End Sub

    ' Validación visual mientras escribe
    Private Sub txtNumero_TextChanged(sender As Object, e As EventArgs) Handles txtNumero.TextChanged
        If Not _tieneRangoActivo Then Return
        Dim n As Integer
        If Integer.TryParse(txtNumero.Text, n) Then
            If n >= _minRango And n <= _maxRango Then
                txtNumero.BackColor = Color.LightYellow
            Else
                txtNumero.BackColor = Color.MistyRose ' Alerta Visual
            End If
        End If
    End Sub

    ' =========================================================
    ' GUARDADO INTELIGENTE (DOBLE MOVIMIENTO)
    ' =========================================================
    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        ' 1. Validaciones
        If txtNumero.Text.Trim = "" Or txtAsunto.Text.Trim = "" Or cmbDestino.Text.Trim = "" Then
            MessageBox.Show("Faltan datos obligatorios.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Using db As New PoloNuevoEntities()
                ' Valida Rango si aplica
                Dim numVal As Integer
                If _tieneRangoActivo AndAlso Integer.TryParse(txtNumero.Text, numVal) Then
                    If numVal < _minRango Or numVal > _maxRango Then
                        If MessageBox.Show("El número está FUERA del rango oficial. ¿Guardar igual?", "Alerta", MessageBoxButtons.YesNo) = DialogResult.No Then Return
                    End If
                End If

                ' ---------------------------------------------------------
                ' PASO 1: CREAR EL DOCUMENTO (FISICAMENTE EN BD)
                ' ---------------------------------------------------------
                Dim docRespuesta As New Documentos()
                docRespuesta.FechaCarga = DateTime.Now
                docRespuesta.TipoDocumentoId = Convert.ToInt32(cmbTipo.SelectedValue)
                docRespuesta.ReferenciaExterna = txtNumero.Text.Trim()
                docRespuesta.Descripcion = txtAsunto.Text.Trim()

                ' Archivo adjunto
                If _archivoBytes IsNot Nothing Then
                    docRespuesta.Contenido = _archivoBytes
                    docRespuesta.NombreArchivo = _archivoNombre
                    docRespuesta.Extension = _archivoExt
                Else
                    docRespuesta.Contenido = New Byte() {0}
                    docRespuesta.NombreArchivo = "Generado Interno"
                    docRespuesta.Extension = ".phy"
                End If

                ' Actualizar Rango (Contador)
                If _tieneRangoActivo AndAlso Integer.TryParse(txtNumero.Text, numVal) Then
                    Dim rango = db.NumeracionRangos.FirstOrDefault(Function(r) r.TipoDocumentoId = docRespuesta.TipoDocumentoId And r.Activo = True)
                    If rango IsNot Nothing AndAlso numVal > rango.UltimoUtilizado AndAlso numVal <= rango.NumeroFin Then
                        rango.UltimoUtilizado = numVal
                    End If
                End If

                db.Documentos.Add(docRespuesta)
                db.SaveChanges() ' Guardar para obtener ID

                ' ---------------------------------------------------------
                ' PASO 2: REGISTRAR EL "NACIMIENTO" (CREACIÓN)
                ' ---------------------------------------------------------
                Dim movCreacion As New MovimientosDocumentos()
                movCreacion.DocumentoId = docRespuesta.Id
                movCreacion.FechaMovimiento = DateTime.Now
                movCreacion.Origen = "SISTEMA / USUARIO"
                movCreacion.Destino = "SECRETARÍA (MI OFICINA)"
                movCreacion.EsSalida = False
                movCreacion.Observaciones = "Documento creado / generado en el sistema."

                db.MovimientosDocumentos.Add(movCreacion)

                ' ---------------------------------------------------------
                ' PASO 3: REGISTRAR LA "SALIDA" (EL PASE)
                ' ---------------------------------------------------------
                Dim destinoFinal As String = cmbDestino.Text.Trim().ToUpper()

                Dim movSalida As New MovimientosDocumentos()
                movSalida.DocumentoId = docRespuesta.Id
                movSalida.FechaMovimiento = DateTime.Now.AddSeconds(1)
                movSalida.Origen = "SECRETARÍA (MI OFICINA)"
                movSalida.Destino = destinoFinal
                movSalida.EsSalida = True
                movSalida.Observaciones = "Enviado en respuesta al Doc ID: " & _idDocExterno

                db.MovimientosDocumentos.Add(movSalida)

                ' ---------------------------------------------------------
                ' PASO 4: MOVER EL ORIGINAL (SI CORRESPONDE)
                ' ---------------------------------------------------------
                If chkMoverOriginal.Checked And _idDocExterno > 0 Then
                    Dim movOriginal As New MovimientosDocumentos()
                    movOriginal.DocumentoId = _idDocExterno
                    movOriginal.FechaMovimiento = DateTime.Now.AddSeconds(1)
                    movOriginal.Origen = "MESA DE ENTRADA"
                    movOriginal.Destino = destinoFinal
                    movOriginal.EsSalida = True

                    ' --- CORRECCIÓN AQUÍ: Usamos cmbTipo.Text ---
                    movOriginal.Observaciones = $"Se adjunta al nuevo {cmbTipo.Text} N° {docRespuesta.ReferenciaExterna}"

                    db.MovimientosDocumentos.Add(movOriginal)
                End If

                db.SaveChanges()

                MessageBox.Show("Documento generado y enviado correctamente.", "Éxito")
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