Imports System.IO
Imports System.Data
Imports System.Data.Entity
Imports System.Linq

Public Class frmNuevoPase

    ' VARIABLES GLOBALES
    Private _idDocumento As Integer
    Private _idMovimientoEditar As Integer = 0
    Private _todosDestinos As New List(Of String)
    Private _origenCalculado As String = "MESA DE ENTRADA"
    Private _observacionCalculada As String = ""

    ' Variables para Actuación
    Private _archivoBytes As Byte() = Nothing
    Private _archivoNombre As String = ""
    Private _archivoExt As String = ""
    Private _tieneRangoActivo As Boolean = False
    Private _rangoId As Integer = 0

    ' --- CONSTRUCTORES ---
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
                PrecalcularInfoDocumento()
            End If
        End If
    End Sub

    Private Sub CargarListas()
        Using db As New PoloNuevoEntities()
            _todosDestinos = db.MovimientosDocumentos _
                               .Where(Function(m) m.Destino IsNot Nothing AndAlso m.Destino <> "") _
                               .Select(Function(m) m.Destino) _
                               .Distinct() _
                               .OrderBy(Function(d) d) _
                               .ToList()

            cmbTipo.DataSource = db.TiposDocumento.Where(Function(t) t.Nombre <> "ARCHIVO").OrderBy(Function(t) t.Nombre).ToList()
            cmbTipo.DisplayMember = "Nombre"
            cmbTipo.ValueMember = "Id"
            cmbTipo.SelectedIndex = -1
        End Using
    End Sub

    ' =========================================================
    ' LÓGICA INTELIGENTE 
    ' =========================================================
    Private Sub PrecalcularInfoDocumento()
        Try
            Using db As New PoloNuevoEntities()
                Dim doc = db.Documentos.Find(_idDocumento)
                If doc IsNot Nothing Then
                    Dim ultimoMov = doc.MovimientosDocumentos _
                                       .OrderByDescending(Function(m) m.FechaMovimiento) _
                                       .ThenByDescending(Function(m) m.Id) _
                                       .FirstOrDefault()

                    _origenCalculado = "MESA DE ENTRADA"

                    If ultimoMov IsNot Nothing Then
                        Dim ultimoLugar As String = If(ultimoMov.Destino, "").Trim().ToUpper()
                        Dim ultimoOrigen As String = If(ultimoMov.Origen, "").Trim().ToUpper()

                        If ultimoLugar = "MESA DE ENTRADA" Then
                            If ultimoOrigen <> "SISTEMA" And ultimoOrigen <> "MESA DE ENTRADA" Then
                                txtDestino.Text = ultimoOrigen
                                _observacionCalculada = "Pase de retorno / Respuesta"
                            End If
                        Else
                            _origenCalculado = ultimoLugar
                        End If
                    End If
                End If
            End Using
        Catch ex As Exception
        End Try
    End Sub

    Private Sub CargarDatosEdicion()
        Using db As New PoloNuevoEntities()
            Dim mov = db.MovimientosDocumentos.Find(_idMovimientoEditar)
            If mov IsNot Nothing Then
                txtDestino.Text = mov.Destino
                dtpFecha.Value = mov.FechaMovimiento
                _origenCalculado = mov.Origen
                _observacionCalculada = mov.Observaciones
            End If
        End Using
    End Sub

    ' =========================================================
    ' INTERFAZ
    ' =========================================================
    Private Sub txtDestino_TextChanged(sender As Object, e As EventArgs) Handles txtDestino.TextChanged
        Dim texto As String = txtDestino.Text.Trim().ToLower()
        If String.IsNullOrEmpty(texto) Then
            lstSugerencias.Visible = False
            Return
        End If
        If _todosDestinos Is Nothing Then Return

        Dim palabras As String() = texto.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)
        Dim resultados = _todosDestinos.Where(Function(d) d IsNot Nothing AndAlso palabras.All(Function(p) d.ToLower().Contains(p))).Take(10).ToList()

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

    Private Sub txtDestino_KeyDown(sender As Object, e As KeyEventArgs) Handles txtDestino.KeyDown
        If lstSugerencias.Visible AndAlso lstSugerencias.Items.Count > 0 Then
            Select Case e.KeyCode
                Case Keys.Down, Keys.Up
                    e.Handled = True
                    Dim diff As Integer = If(e.KeyCode = Keys.Down, 1, -1)
                    Dim newIdx As Integer = lstSugerencias.SelectedIndex + diff
                    If newIdx >= 0 And newIdx < lstSugerencias.Items.Count Then lstSugerencias.SelectedIndex = newIdx
                Case Keys.Enter
                    e.SuppressKeyPress = True
                    If lstSugerencias.SelectedItem IsNot Nothing Then
                        txtDestino.Text = lstSugerencias.SelectedItem.ToString()
                        lstSugerencias.Visible = False
                        txtDestino.SelectionStart = txtDestino.Text.Length
                        txtDestino.Focus()
                    End If
                Case Keys.Escape
                    e.Handled = True
                    lstSugerencias.Visible = False
            End Select
        End If
    End Sub

    Private Sub lstSugerencias_Click(sender As Object, e As EventArgs) Handles lstSugerencias.Click
        If lstSugerencias.SelectedItem IsNot Nothing Then
            txtDestino.Text = lstSugerencias.SelectedItem.ToString()
            lstSugerencias.Visible = False
            txtDestino.SelectionStart = txtDestino.Text.Length
            txtDestino.Focus()
        End If
    End Sub

    Private Sub txtDestino_Leave(sender As Object, e As EventArgs) Handles txtDestino.Leave
        If Not lstSugerencias.Focused Then lstSugerencias.Visible = False
    End Sub

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
                lblInfoRango.Text = $"Rango: {rango.NombreRango} (Libres: {rango.NumeroFin - rango.UltimoUtilizado})"
                lblInfoRango.ForeColor = Color.Green
            Else
                _tieneRangoActivo = False
                txtNumero.Text = "S/R"
                txtNumero.BackColor = Color.MistyRose
                lblInfoRango.Text = "Sin rango."
                lblInfoRango.ForeColor = Color.Red
            End If
        End Using
    End Sub

    ' =========================================================
    ' MÉTODO RECURSIVO: MUDANZA DE FAMILIA
    ' =========================================================
    ' =========================================================
    ' MÉTODO DE ARRASTRE TOTAL: Mueve a toda la familia junta
    ' =========================================================
    Private Sub MoverFamiliaCompleta(db As PoloNuevoEntities, idPadreSupremo As Integer, destino As String, fecha As Date, idExcluir As Integer)
        ' 1. Obtenemos TODOS los IDs que pertenecen a este expediente (Padre, hijos, nietos...)
        Dim familiaCompletaIds As New List(Of Integer)
        familiaCompletaIds.Add(idPadreSupremo)
        ObtenerDescendenciaRecursiva(db, idPadreSupremo, familiaCompletaIds)

        ' 2. Recorremos cada miembro de la familia
        For Each idMiembro In familiaCompletaIds
            ' Saltamos el documento que acabamos de crear/mover manualmente para no duplicar
            If idMiembro = idExcluir Then Continue For

            ' 3. Verificamos su ubicación actual con la lógica de desempate (Fecha + ID)
            Dim ultMov = db.MovimientosDocumentos.Where(Function(m) m.DocumentoId = idMiembro) _
                                                 .OrderByDescending(Function(m) m.FechaMovimiento) _
                                                 .ThenByDescending(Function(m) m.Id) _
                                                 .FirstOrDefault()

            Dim lugarActual As String = "MESA DE ENTRADA"
            If ultMov IsNot Nothing AndAlso ultMov.Destino IsNot Nothing Then
                lugarActual = ultMov.Destino.Trim().ToUpper()
            End If

            ' 4. SI EL HIJO/HERMANO ESTÁ EN MESA, LO MOVEMOS AL DESTINO
            If lugarActual = "MESA DE ENTRADA" Then
                Dim movArrastre As New MovimientosDocumentos With {
                    .DocumentoId = idMiembro,
                    .FechaMovimiento = fecha, ' Misma hora exacta que el principal
                    .Origen = "MESA DE ENTRADA",
                    .Destino = destino,
                    .EsSalida = True,
                    .Observaciones = "SALIDA AUTOMÁTICA (Arrastre de Expediente)"
                }
                db.MovimientosDocumentos.Add(movArrastre)
            End If
        Next
    End Sub

    ' Función de apoyo para encontrar a todos los parientes sin importar el nivel
    Private Sub ObtenerDescendenciaRecursiva(db As PoloNuevoEntities, idPadre As Integer, ByRef listaIds As List(Of Integer))
        Dim hijos = db.DocumentoVinculos.Where(Function(v) v.IdDocumentoPadre = idPadre).Select(Function(v) v.IdDocumentoHijo).ToList()
        For Each idHijo In hijos
            If Not listaIds.Contains(idHijo) Then
                listaIds.Add(idHijo)
                ObtenerDescendenciaRecursiva(db, idHijo, listaIds) ' Sigue bajando a nietos, bisnietos...
            End If
        Next
    End Sub

    ' =========================================================
    ' GUARDAR MAESTRO (CON CORRECCIÓN DE TIEMPOS)
    ' =========================================================
    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If String.IsNullOrWhiteSpace(txtDestino.Text) Then
            MessageBox.Show("Falta el Destino.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtDestino.Focus()
            Return
        End If

        Try
            Using db As New PoloNuevoEntities()
                Dim destinoFinal As String = txtDestino.Text.Trim().ToUpper()
                Dim idPadreSupremo As Integer = _idDocumento
                Dim idNuevoDoc As Integer = 0

                ' 1. BUSCAR PADRE SUPREMO
                If _idMovimientoEditar = 0 And _idDocumento > 0 Then
                    Dim idRastro As Integer = _idDocumento
                    Dim encontrado As Boolean = True
                    Dim iteraciones As Integer = 0
                    While encontrado AndAlso iteraciones < 50
                        iteraciones += 1
                        Dim idActual = idRastro
                        Dim vinculo = db.DocumentoVinculos.FirstOrDefault(Function(v) v.IdDocumentoHijo = idActual)
                        If vinculo IsNot Nothing Then
                            idRastro = vinculo.IdDocumentoPadre
                            idPadreSupremo = idRastro
                        Else
                            encontrado = False
                        End If
                    End While
                End If

                ' ------------------------------------------------------------
                ' ¡NUEVO! LÓGICA ANTI-COLISIÓN DE TIEMPO
                ' ------------------------------------------------------------
                ' Buscamos la fecha del último movimiento registrado en el expediente padre
                Dim fechaBase As Date = dtpFecha.Value

                If _idMovimientoEditar = 0 Then
                    Dim ultimoMovPadre = db.MovimientosDocumentos _
                                           .Where(Function(m) m.DocumentoId = idPadreSupremo) _
                                           .OrderByDescending(Function(m) m.FechaMovimiento) _
                                           .FirstOrDefault()

                    If ultimoMovPadre IsNot Nothing Then
                        ' Si la fecha elegida es IGUAL o ANTERIOR a lo que ya existe...
                        If fechaBase <= ultimoMovPadre.FechaMovimiento Then
                            ' ...forzamos que sea 1 segundo después para que aparezca ARRIBA
                            fechaBase = ultimoMovPadre.FechaMovimiento.AddSeconds(1)
                        End If
                    End If
                End If
                ' ------------------------------------------------------------

                ' --- ESCENARIO A: GENERAR NUEVA ACTUACIÓN ---
                If chkGenerarActuacion.Checked Then
                    If Not _tieneRangoActivo Then
                        MessageBox.Show("Sin rango no se puede crear documento.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                        Return
                    End If

                    Dim docNuevo As New Documentos With {
                        .FechaCarga = DateTime.Now,
                        .TipoDocumentoId = Convert.ToInt32(cmbTipo.SelectedValue),
                        .ReferenciaExterna = txtNumero.Text,
                        .Descripcion = txtAsunto.Text.Trim(),
                        .Contenido = If(_archivoBytes, New Byte() {0}),
                        .NombreArchivo = If(_archivoBytes IsNot Nothing, _archivoNombre, "Interno"),
                        .Extension = If(_archivoBytes IsNot Nothing, _archivoExt, ".phy")
                    }

                    Dim rango = db.NumeracionRangos.Find(_rangoId)
                    If rango IsNot Nothing Then rango.UltimoUtilizado = Convert.ToInt32(docNuevo.ReferenciaExterna)

                    db.Documentos.Add(docNuevo)
                    db.SaveChanges()
                    idNuevoDoc = docNuevo.Id

                    ' Movimiento Único: Generación y Salida
                    db.MovimientosDocumentos.Add(New MovimientosDocumentos With {
                        .DocumentoId = idNuevoDoc,
                        .FechaMovimiento = fechaBase, ' Usamos la fecha corregida
                        .Origen = "MESA DE ENTRADA",
                        .Destino = destinoFinal,
                        .EsSalida = True,
                        .Observaciones = "GENERACIÓN Y ENVÍO"
                    })

                    ' Movimiento del Padre
                    If _idDocumento > 0 Then
                        db.MovimientosDocumentos.Add(New MovimientosDocumentos With {
                            .DocumentoId = idPadreSupremo, .FechaMovimiento = fechaBase, ' Misma fecha corregida
                            .Origen = "MESA DE ENTRADA", .Destino = destinoFinal, .EsSalida = True,
                            .Observaciones = $"TRÁMITE: Con {cmbTipo.Text} {docNuevo.ReferenciaExterna}"
                        })

                        db.DocumentoVinculos.Add(New DocumentoVinculos With {
                            .IdDocumentoPadre = idPadreSupremo, .IdDocumentoHijo = idNuevoDoc,
                            .TipoRelacion = "ACTUACION", .FechaVinculo = DateTime.Now
                        })
                    End If

                    ' --- ESCENARIO B: PASE SIMPLE ---
                Else
                    If _idDocumento = 0 Then Return

                    Dim nuevoMov As New MovimientosDocumentos()
                    If _idMovimientoEditar > 0 Then
                        nuevoMov = db.MovimientosDocumentos.Find(_idMovimientoEditar)
                    End If

                    nuevoMov.DocumentoId = idPadreSupremo
                    nuevoMov.FechaMovimiento = fechaBase ' Usamos la fecha corregida
                    nuevoMov.Origen = If(_idMovimientoEditar > 0, _origenCalculado, "MESA DE ENTRADA")
                    nuevoMov.Destino = destinoFinal
                    nuevoMov.EsSalida = (destinoFinal <> "MESA DE ENTRADA")

                    Dim obs As String = If(_observacionCalculada <> "", _observacionCalculada, "PASE DE EXPEDIENTE")
                    nuevoMov.Observaciones = obs

                    If _idMovimientoEditar = 0 Then db.MovimientosDocumentos.Add(nuevoMov)
                End If

                ' --- ARRASTRE DE FAMILIA ---
                If _idMovimientoEditar = 0 And _idDocumento > 0 Then
                    MoverFamiliaCompleta(db, idPadreSupremo, destinoFinal, fechaBase, idNuevoDoc)
                End If

                db.SaveChanges()
                Me.DialogResult = DialogResult.OK
                Me.Close()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Me.Close()
    End Sub
End Class