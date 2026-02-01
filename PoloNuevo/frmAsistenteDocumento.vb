Imports System.IO
Imports System.Data.Entity
Imports System.Linq

Public Class frmAsistenteDocumento

    ' =========================================================
    ' ESTRUCTURA PARA LA INTELIGENCIA DE VINCULACIÓN
    ' =========================================================
    Private Class InfoVinculacion
        Public EsPosibleVincular As Boolean = False
        Public MensajeEstado As String = ""
        Public ColorEstado As Color = Color.DimGray

        Public IdPadreRaiz As Integer = 0
        Public ReferenciaVisual As String = ""

        ' Datos para autocompletar (Predictivos)
        Public SugerenciaOrigen As String = ""
        Public SugerenciaAsunto As String = ""
    End Class

    ' =========================================================
    ' VARIABLES
    ' =========================================================
    Private currentStep As Integer = 0
    Private totalSteps As Integer = 4

    Private _idCandidato As Integer = 0      ' ID del documento seleccionado en la grilla
    Private _info As New InfoVinculacion()   ' Resultado del análisis

    Private _idPadreFinal As Integer = 0     ' El ID que usaremos para guardar (0 = Nuevo)

    Private _todosOrigenes As New List(Of String)
    Private _archivoBytes As Byte() = Nothing
    Private _archivoNombre As String = ""
    Private _archivoExt As String = ""

    ' =========================================================
    ' CONSTRUCTOR
    ' =========================================================
    Public Sub New(Optional idCandidato As Integer = 0)
        InitializeComponent()
        _idCandidato = idCandidato
    End Sub

    ' =========================================================
    ' CARGA INICIAL
    ' =========================================================
    Private Sub frmAsistenteDocumento_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ConfigurarDiseño()
        CargarListas()

        ' 1. ANALIZAR EL DOCUMENTO SELECCIONADO (Si existe)
        If _idCandidato > 0 Then
            AnalizarCandidato(_idCandidato)
        Else
            ' Caso: No seleccionó nada en la grilla y dio click a Nuevo
            _info.EsPosibleVincular = False
            _info.MensajeEstado = "No se seleccionó ningún documento base."
            ConfigurarOpcionesVinculacion()
        End If

        ActualizarVista()
    End Sub

    Private Sub ConfigurarDiseño()
        tabWizard.Appearance = TabAppearance.FlatButtons
        tabWizard.ItemSize = New Size(0, 1)
        tabWizard.SizeMode = TabSizeMode.Fixed
        btnAnterior.Enabled = False

        If lstSugerenciasOrigen IsNot Nothing Then
            lstSugerenciasOrigen.Visible = False
            lstSugerenciasOrigen.BringToFront()
        End If
    End Sub

    Private Sub CargarListas()
        Try
            Using db As New PoloNuevoEntities()
                cmbTipoDoc.DataSource = db.TiposDocumento.Where(Function(t) t.Nombre <> "ARCHIVO").OrderBy(Function(t) t.Nombre).ToList()
                cmbTipoDoc.DisplayMember = "Nombre"
                cmbTipoDoc.ValueMember = "Id"
                cmbTipoDoc.SelectedIndex = -1

                Dim listaTemp = db.MovimientosDocumentos _
                                  .Where(Function(m) m.Origen <> "" And m.Origen <> "SISTEMA") _
                                  .Select(Function(m) m.Origen) _
                                  .Distinct().ToList()

                Dim defaults As String() = {"JUZGADO LETRADO", "MINISTERIO DEL INTERIOR", "FISCALÍA", "DEFENSORÍA", "DIRECCIÓN"}
                For Each def In defaults
                    If Not listaTemp.Contains(def) Then listaTemp.Add(def)
                Next
                listaTemp.Sort()
                _todosOrigenes = listaTemp
            End Using
        Catch ex As Exception
            ' Fallo silencioso en listas secundarias
        End Try
    End Sub

    ' =========================================================
    ' CEREBRO: ANALIZAR SI SE PUEDE VINCULAR
    ' =========================================================
    ' =========================================================
    ' CEREBRO: ANALIZAR SI SE PUEDE VINCULAR
    ' =========================================================
    Private Sub AnalizarCandidato(idDoc As Integer)
        Using db As New PoloNuevoEntities()
            _info = New InfoVinculacion()

            ' 1. Encontrar al "Padre Supremo" (Raíz del Expediente)
            Dim idRastro As Integer = idDoc
            Dim encontrado As Boolean = True
            Dim iteraciones As Integer = 0

            While encontrado AndAlso iteraciones < 50
                iteraciones += 1
                Dim idActual = idRastro
                ' Buscamos si "idActual" es hijo de alguien
                Dim vinculo = db.DocumentoVinculos.FirstOrDefault(Function(v) v.IdDocumentoHijo = idActual)
                If vinculo IsNot Nothing Then
                    idRastro = vinculo.IdDocumentoPadre ' Seguimos subiendo
                Else
                    encontrado = False ' Ya no tiene padres, idRastro es la raíz
                End If
            End While

            ' 2. Verificar DÓNDE está ese expediente raíz
            Dim raiz = db.Documentos.Include("MovimientosDocumentos").Include("TiposDocumento").FirstOrDefault(Function(d) d.Id = idRastro)

            If raiz Is Nothing Then
                _info.EsPosibleVincular = False
                _info.MensajeEstado = "Error: No se encontró el expediente raíz."
                ConfigurarOpcionesVinculacion()
                Return
            End If

            ' Obtenemos el último movimiento de la RAÍZ
            Dim ultimoMov = raiz.MovimientosDocumentos _
                                .OrderByDescending(Function(m) m.FechaMovimiento) _
                                .ThenByDescending(Function(m) m.Id) _
                                .FirstOrDefault()

            Dim ubicacionActual As String = "MESA DE ENTRADA" ' Default
            If ultimoMov IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(ultimoMov.Destino) Then
                ubicacionActual = ultimoMov.Destino.Trim().ToUpper()
            End If

            ' =========================================================================
            ' 3. REGLA DE NEGOCIO CORREGIDA:
            ' Permitir vincular SOLO si el expediente NO está en MESA DE ENTRADA.
            ' =========================================================================
            If ubicacionActual <> "MESA DE ENTRADA" Then

                ' CASO: El expediente está fuera (Fiscalía, Juzgado, etc.) -> PERMITIR VINCULAR
                _info.EsPosibleVincular = True
                _info.IdPadreRaiz = raiz.Id
                _info.ReferenciaVisual = $"{raiz.TiposDocumento.Nombre} {raiz.ReferenciaExterna}"
                _info.MensajeEstado = $"UBICACIÓN: {ubicacionActual} -> Se generará retorno automático."
                _info.ColorEstado = Color.Green

                ' Inteligencia Predictiva
                If ultimoMov IsNot Nothing Then
                    ' Si está en Fiscalía, sugerimos que viene de Fiscalía
                    _info.SugerenciaOrigen = ubicacionActual
                End If
                _info.SugerenciaAsunto = "Ref: " & raiz.Descripcion

            Else

                ' CASO: El expediente YA está en Mesa de Entrada -> BLOQUEAR VINCULACIÓN DE RETORNO
                _info.EsPosibleVincular = False
                _info.MensajeEstado = $"El expediente {raiz.ReferenciaExterna} ya se encuentra físico en MESA DE ENTRADA."
                _info.ColorEstado = Color.DimGray

            End If

            ConfigurarOpcionesVinculacion()
        End Using
    End Sub

    Private Sub ConfigurarOpcionesVinculacion()
        lblDetalleVinculo.Text = _info.MensajeEstado
        lblDetalleVinculo.ForeColor = _info.ColorEstado

        If _info.EsPosibleVincular Then
            ' Habilitamos la opción y la seleccionamos por defecto si el usuario quiso "Nuevo" sobre un doc
            optVincular.Enabled = True
            optVincular.Text = "Vincular a: " & _info.ReferenciaVisual

            ' Estrategia: Dejar chequeado "Nuevo" por defecto para evitar errores, 
            ' pero el usuario VE que puede vincular.
            optNuevo.Checked = True
        Else
            ' Bloqueamos
            optVincular.Enabled = False
            optVincular.Text = "Vincular (No disponible)"
            optNuevo.Checked = True
        End If
    End Sub

    ' =========================================================
    ' NAVEGACIÓN Y LOGICA "SIGUIENTE"
    ' =========================================================
    Private Sub btnSiguiente_Click(sender As Object, e As EventArgs) Handles btnSiguiente.Click
        If ValidarPasoActual() Then

            ' TRANSICIÓN PASO 0 -> PASO 1 (Inteligencia de Autocompletado)
            If currentStep = 0 Then
                If optVincular.Checked Then
                    _idPadreFinal = _info.IdPadreRaiz

                    ' AUTOCOMPLETAR SI ESTÁ VACÍO
                    If String.IsNullOrWhiteSpace(txtOrigen.Text) Then
                        txtOrigen.Text = _info.SugerenciaOrigen
                    End If
                    If String.IsNullOrWhiteSpace(txtAsunto.Text) Then
                        txtAsunto.Text = _info.SugerenciaAsunto
                    End If
                Else
                    _idPadreFinal = 0 ' Modo Independiente
                End If
            End If

            If currentStep < totalSteps - 1 Then
                currentStep += 1
                tabWizard.SelectedIndex = currentStep
                ActualizarVista()
            Else
                GuardarEnBaseDeDatos()
            End If
        End If
    End Sub

    Private Sub btnAnterior_Click(sender As Object, e As EventArgs) Handles btnAnterior.Click
        If currentStep > 0 Then
            currentStep -= 1
            tabWizard.SelectedIndex = currentStep
            ActualizarVista()
        End If
    End Sub

    Private Sub ActualizarVista()
        Select Case currentStep
            Case 0 ' Tipo
                lblTituloPaso.Text = "Paso 1: Clasificación del Documento"
                btnAnterior.Enabled = False
                btnSiguiente.Text = "Siguiente >"
                lblEstado.Text = "Seleccione si es un documento nuevo o parte de un expediente."

            Case 1 ' Datos
                lblTituloPaso.Text = "Paso 2: Datos Principales"
                btnAnterior.Enabled = True
                lblEstado.Text = "Complete los detalles del documento."

            Case 2 ' Adjunto
                lblTituloPaso.Text = "Paso 3: Archivo Digital"
                lblEstado.Text = "Opcional: Adjunte el PDF escaneado."

            Case 3 ' Final
                lblTituloPaso.Text = "Paso 4: Confirmar Ingreso"
                btnSiguiente.Text = "FINALIZAR"
                lblEstado.Text = "Verifique los datos antes de guardar."
                CargarResumen()
        End Select
    End Sub

    ' =========================================================
    ' VALIDACIONES Y AUTOCOMPLETADO (UI)
    ' =========================================================
    Private Sub txtOrigen_TextChanged(sender As Object, e As EventArgs) Handles txtOrigen.TextChanged
        Dim texto As String = txtOrigen.Text.Trim().ToLower()
        If String.IsNullOrEmpty(texto) Then
            lstSugerenciasOrigen.Visible = False
            Return
        End If

        If _todosOrigenes Is Nothing Then Return

        Dim palabras As String() = texto.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)
        Dim resultados = _todosOrigenes.Where(Function(o) palabras.All(Function(p) o.ToLower().Contains(p))).Take(8).ToList()

        If resultados.Count > 0 Then
            lstSugerenciasOrigen.DataSource = resultados
            lstSugerenciasOrigen.Visible = True
            lstSugerenciasOrigen.BringToFront()
        Else
            lstSugerenciasOrigen.Visible = False
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

    ' Soporte Teclado para lista
    Private Sub txtOrigen_KeyDown(sender As Object, e As KeyEventArgs) Handles txtOrigen.KeyDown
        If lstSugerenciasOrigen.Visible Then
            If e.KeyCode = Keys.Down Then
                If lstSugerenciasOrigen.SelectedIndex < lstSugerenciasOrigen.Items.Count - 1 Then lstSugerenciasOrigen.SelectedIndex += 1
                e.Handled = True
            ElseIf e.KeyCode = Keys.Up Then
                If lstSugerenciasOrigen.SelectedIndex > 0 Then lstSugerenciasOrigen.SelectedIndex -= 1
                e.Handled = True
            ElseIf e.KeyCode = Keys.Enter Then
                lstSugerenciasOrigen_Click(Nothing, Nothing)
                e.SuppressKeyPress = True
            ElseIf e.KeyCode = Keys.Escape Then
                lstSugerenciasOrigen.Visible = False
                e.Handled = True
            End If
        End If
    End Sub

    Private Function ValidarPasoActual() As Boolean
        Select Case currentStep
            Case 0 ' Tipo
                If cmbTipoDoc.SelectedIndex = -1 Then
                    MessageBox.Show("Por favor seleccione qué tipo de documento está ingresando.", "Faltan datos")
                    Return False
                End If
                Return True

            Case 1 ' Datos
                If String.IsNullOrWhiteSpace(txtAsunto.Text) Then
                    MessageBox.Show("El Asunto es obligatorio.", "Faltan datos")
                    txtAsunto.Focus()
                    Return False
                End If
                If String.IsNullOrWhiteSpace(txtOrigen.Text) Then
                    MessageBox.Show("El Origen es obligatorio.", "Faltan datos")
                    txtOrigen.Focus()
                    Return False
                End If
                Return True

            Case Else
                Return True
        End Select
    End Function

    ' =========================================================
    ' GUARDADO EN BASE DE DATOS
    ' =========================================================
    Private Sub GuardarEnBaseDeDatos()
        Try
            Me.Cursor = Cursors.WaitCursor
            Using db As New PoloNuevoEntities()

                ' 1. Crear el Documento
                Dim nuevoDoc As New Documentos()
                nuevoDoc.FechaCarga = DateTime.Now
                nuevoDoc.Descripcion = txtAsunto.Text.Trim()
                nuevoDoc.ReferenciaExterna = If(txtNumero IsNot Nothing, txtNumero.Text.Trim(), "")
                nuevoDoc.TipoDocumentoId = Convert.ToInt32(cmbTipoDoc.SelectedValue)

                If _archivoBytes IsNot Nothing Then
                    nuevoDoc.Contenido = _archivoBytes
                    nuevoDoc.NombreArchivo = _archivoNombre
                    nuevoDoc.Extension = _archivoExt
                Else
                    nuevoDoc.Contenido = New Byte() {0}
                    nuevoDoc.NombreArchivo = "S/D"
                    nuevoDoc.Extension = ".phy"
                End If

                db.Documentos.Add(nuevoDoc)
                db.SaveChanges()

                ' 2. Movimiento Inicial
                Dim movEntrada As New MovimientosDocumentos()
                movEntrada.DocumentoId = nuevoDoc.Id
                movEntrada.FechaMovimiento = nuevoDoc.FechaCarga
                movEntrada.Origen = txtOrigen.Text.Trim().ToUpper()
                movEntrada.Destino = "MESA DE ENTRADA"
                movEntrada.EsSalida = False

                If _idPadreFinal > 0 Then
                    movEntrada.Observaciones = "VINCULADO AL EXPEDIENTE"
                Else
                    movEntrada.Observaciones = "INGRESO INDEPENDIENTE"
                End If

                db.MovimientosDocumentos.Add(movEntrada)

                ' 3. Generar Vínculo (Si aplica)
                If _idPadreFinal > 0 Then
                    Dim vinculo As New DocumentoVinculos()
                    vinculo.IdDocumentoPadre = _idPadreFinal
                    vinculo.IdDocumentoHijo = nuevoDoc.Id
                    vinculo.TipoRelacion = "ADJUNTO"
                    vinculo.FechaVinculo = nuevoDoc.FechaCarga
                    db.DocumentoVinculos.Add(vinculo)

                    ' TRAER FAMILIA (Retorno automático del expediente a Mesa)
                    TraerFamiliaCompleta(db, _idPadreFinal, txtOrigen.Text.Trim(), nuevoDoc.FechaCarga)
                End If

                db.SaveChanges()

                MessageBox.Show("Documento ingresado con éxito.", "Operación Completada", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.DialogResult = DialogResult.OK
                Me.Close()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error crítico al guardar: " & ex.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ' Método recursivo para traer a todos los hijos que estén paseando por otras oficinas
    Private Sub TraerFamiliaCompleta(db As PoloNuevoEntities, idPadre As Integer, origen As String, fecha As Date)
        Dim hijos = db.DocumentoVinculos.Where(Function(v) v.IdDocumentoPadre = idPadre).ToList()

        For Each h In hijos
            Dim idHijo = h.IdDocumentoHijo

            Dim ultMov = db.MovimientosDocumentos.Where(Function(m) m.DocumentoId = idHijo) _
                           .OrderByDescending(Function(m) m.FechaMovimiento) _
                           .ThenByDescending(Function(m) m.Id).FirstOrDefault()

            Dim destinoActual As String = "MESA DE ENTRADA"
            If ultMov IsNot Nothing AndAlso ultMov.Destino IsNot Nothing Then
                destinoActual = ultMov.Destino.Trim().ToUpper()
            End If

            ' Si el hijo NO está en Mesa, creamos un movimiento de retorno
            If destinoActual <> "MESA DE ENTRADA" Then
                Dim retHijo As New MovimientosDocumentos With {
                    .DocumentoId = idHijo,
                    .FechaMovimiento = fecha, ' Misma fecha de hoy
                    .Origen = destinoActual,
                    .Destino = "MESA DE ENTRADA",
                    .EsSalida = False,
                    .Observaciones = "RETORNO AUTOMÁTICO (Unificación de Expediente)"
                }
                db.MovimientosDocumentos.Add(retHijo)
            End If

            ' Recursividad para nietos
            TraerFamiliaCompleta(db, idHijo, origen, fecha)
        Next
    End Sub

    Private Sub btnExaminar_Click(sender As Object, e As EventArgs) Handles btnExaminar.Click
        Using ofd As New OpenFileDialog()
            ofd.Filter = "Documentos PDF|*.pdf|Todos los archivos|*.*"
            If ofd.ShowDialog() = DialogResult.OK Then
                _archivoBytes = File.ReadAllBytes(ofd.FileName)
                _archivoNombre = Path.GetFileName(ofd.FileName)
                _archivoExt = Path.GetExtension(ofd.FileName)
                txtRutaArchivo.Text = _archivoNombre
            End If
        End Using
    End Sub

    Private Sub CargarResumen()
        lblResumenTipo.Text = cmbTipoDoc.Text
        lblResumenAsunto.Text = txtAsunto.Text
        lblResumenOrigen.Text = txtOrigen.Text
        lblResumenAdjunto.Text = If(String.IsNullOrEmpty(_archivoNombre), "NO", "SI - " & _archivoNombre)

        If _idPadreFinal > 0 Then
            lblEstado.Text = "Modo: VINCULACIÓN a Exp. ID " & _idPadreFinal
        Else
            lblEstado.Text = "Modo: NUEVO INGRESO (Independiente)"
        End If
    End Sub

End Class