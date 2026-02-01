Imports System.IO
Imports System.Drawing.Printing
Imports System.Data.Entity

Public Class frmMesaEntrada

    ' =========================================================
    ' VARIABLES GLOBALES PARA IMPRESIÓN
    ' =========================================================
    Private _impReferencia As String
    Private _impAsunto As String
    Private _impOrigen As String
    Private _impDestino As String
    Private _impFecha As String
    Private _impUsuario As String = "Operador de Mesa"

    Private Sub frmMesaEntrada_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ConfigurarGrillaMovimientos()
        CargarMesa()
    End Sub

    Private Sub ConfigurarGrillaMovimientos()
        dgvMovimientos.BackgroundColor = Color.WhiteSmoke
        dgvMovimientos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    End Sub

    ' =========================================================
    ' LÓGICA PRINCIPAL: CARGA Y FILTROS
    ' =========================================================
    Private Sub CargarMesa()
        Try
            Using db As New PoloNuevoEntities()
                ' =========================================================
                ' 1. CONSULTA BASE (Traemos todo menos lo archivado)
                ' =========================================================
                Dim query = db.Documentos.Include("MovimientosDocumentos") _
                                         .Where(Function(d) d.TiposDocumento.Nombre <> "ARCHIVO")

                ' =========================================================
                ' 2. APLICACIÓN DE FILTROS (Buscador y Fechas)
                ' =========================================================
                ' A) Buscador Inteligente
                Dim texto = txtBuscar.Text.Trim()
                If Not String.IsNullOrEmpty(texto) Then
                    Dim palabras As String() = texto.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)
                    For Each palabra In palabras
                        Dim termino = palabra
                        query = query.Where(Function(d) d.ReferenciaExterna.Contains(termino) Or
                                                        d.TiposDocumento.Nombre.Contains(termino) Or
                                                        d.Descripcion.Contains(termino) Or
                                                        (d.Reclusos IsNot Nothing AndAlso d.Reclusos.Nombre.Contains(termino)))
                    Next
                End If

                ' B) Filtro de Fechas
                If chkFechas.Checked Then
                    Dim fDesde = dtpDesde.Value.Date
                    Dim fHasta = dtpHasta.Value.Date.AddDays(1).AddSeconds(-1)
                    query = query.Where(Function(d) d.FechaCarga >= fDesde And d.FechaCarga <= fHasta)
                End If

                ' =========================================================
                ' 3. EJECUCIÓN Y PROYECCIÓN EN MEMORIA (CORREGIDO)
                ' =========================================================
                ' Ejecutamos la consulta base
                Dim rawList = query.OrderByDescending(Function(d) d.FechaCarga).ToList()

                ' Obtenemos los últimos movimientos para saber dónde está cada documento
                Dim ultimosMovimientos = MesaEntradaInteligencia.ObtenerUltimosMovimientos(rawList)

                ' Proyectamos los datos para la grilla
                Dim displayList = rawList.Select(Function(d)
                                                     Dim ultimoMov As MovimientosDocumentos = Nothing
                                                     ultimosMovimientos.TryGetValue(d.Id, ultimoMov)

                                                     ' -------------------------------------------------------
                                                     ' CORRECCIÓN CRÍTICA: IGUALAR LÓGICA CON DASHBOARD
                                                     ' -------------------------------------------------------
                                                     ' Si no hay movimiento (es nuevo) o el destino está vacío,
                                                     ' ASUMIMOS que está físicamente en la Mesa de Entrada.
                                                     Dim destino As String = "MESA DE ENTRADA"

                                                     If ultimoMov IsNot Nothing AndAlso Not String.IsNullOrEmpty(ultimoMov.Destino) Then
                                                         destino = ultimoMov.Destino
                                                     End If
                                                     ' -------------------------------------------------------

                                                     Dim enMesa As Boolean = destino.Trim().ToUpper() = "MESA DE ENTRADA"

                                                     ' Determinamos el estado para la columna visual
                                                     Dim estadoFinal As String = "DERIVADO"
                                                     If enMesa Then estadoFinal = "PENDIENTE"

                                                     Return New With {
                                                         .Id = d.Id,
                                                         .Fecha = d.FechaCarga,
                                                         .Referencia = d.TiposDocumento.Nombre & " " & d.ReferenciaExterna,
                                                         .Asunto = d.Descripcion,
                                                         .Recluso = If(d.Reclusos IsNot Nothing, d.Reclusos.Nombre, "Sin Vincular"),
                                                         .Estado = estadoFinal,
                                                         .Digital = If(d.Extension <> ".phy", "SI", "NO"),
                                                         .Vencimiento = d.FechaVencimiento
                                                     }
                                                 End Function).ToList()

                ' =========================================================
                ' 4. ASIGNACIÓN A LA GRILLA
                ' =========================================================
                If chkPendientes.Checked Then
                    ' Si el check está activo, solo mostramos lo que calculamos como PENDIENTE
                    dgvMesa.DataSource = displayList.Where(Function(d) d.Estado = "PENDIENTE").ToList()
                Else
                    ' Si no, mostramos todo
                    dgvMesa.DataSource = displayList
                End If

                ' =========================================================
                ' 5. CONFIGURACIÓN VISUAL
                ' =========================================================
                If dgvMesa.Columns("Id") IsNot Nothing Then dgvMesa.Columns("Id").Visible = False
                If dgvMesa.Columns("Vencimiento") IsNot Nothing Then dgvMesa.Columns("Vencimiento").Visible = False

                ConfigurarColumnas()
                ColorearPendientes()

                ' Limpiamos la grilla de historial si no hay documentos listados
                If displayList.Count = 0 Then dgvMovimientos.DataSource = Nothing

            End Using
        Catch ex As Exception
            MessageBox.Show("Error al cargar datos: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub ConfigurarColumnas()
        If dgvMesa.Columns.Count = 0 Then Return

        With dgvMesa
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
            If .Columns("Fecha") IsNot Nothing Then
                .Columns("Fecha").Width = 85
                .Columns("Fecha").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            End If
            If .Columns("Referencia") IsNot Nothing Then
                .Columns("Referencia").Width = 140
                .Columns("Referencia").HeaderText = "Tipo y Nro."
            End If
            If .Columns("Recluso") IsNot Nothing Then
                .Columns("Recluso").Width = 180
            End If
            If .Columns("Digital") IsNot Nothing Then
                .Columns("Digital").Width = 50
                .Columns("Digital").HeaderText = "Dig."
                .Columns("Digital").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            End If
            If .Columns("Estado") IsNot Nothing Then
                .Columns("Estado").Width = 90
                .Columns("Estado").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                .Columns("Estado").DefaultCellStyle.Font = New Font("Segoe UI", 8, FontStyle.Bold)
            End If
            If .Columns("Asunto") IsNot Nothing Then
                .Columns("Asunto").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            End If
        End With
    End Sub

    Private Sub ColorearPendientes()
        If dgvMesa.Columns("Estado") Is Nothing Then Return

        Dim fechaHoy As Date = DateTime.Now.Date

        For Each row As DataGridViewRow In dgvMesa.Rows
            Dim tieneVencimiento As Boolean = False
            If dgvMesa.Columns("Vencimiento") IsNot Nothing AndAlso row.Cells("Vencimiento").Value IsNot Nothing Then
                Dim fechaVenc As Date = Convert.ToDateTime(row.Cells("Vencimiento").Value)
                tieneVencimiento = True
                Dim diasRestantes As Integer = (fechaVenc - fechaHoy).TotalDays

                If diasRestantes < 0 Then
                    row.DefaultCellStyle.BackColor = Color.Firebrick
                    row.DefaultCellStyle.ForeColor = Color.White
                    row.Cells("Referencia").Value = "(VENCIDO) " & row.Cells("Referencia").Value.ToString()
                ElseIf diasRestantes <= 5 Then
                    row.DefaultCellStyle.BackColor = Color.Orange
                    row.DefaultCellStyle.ForeColor = Color.Black
                End If
            End If

            If Not tieneVencimiento AndAlso row.Cells("Estado").Value.ToString() = "PENDIENTE" Then
                If row.DefaultCellStyle.BackColor = dgvMesa.DefaultCellStyle.BackColor Then
                    row.DefaultCellStyle.ForeColor = Color.DarkRed
                    row.DefaultCellStyle.BackColor = Color.MistyRose
                End If
            End If

            If dgvMesa.Columns("Digital") IsNot Nothing AndAlso row.Cells("Digital").Value.ToString() = "SI" Then
                row.Cells("Digital").Style.ForeColor = Color.Blue
                row.Cells("Digital").Style.Font = New Font("Segoe UI", 8, FontStyle.Bold)
                If row.DefaultCellStyle.BackColor = Color.Firebrick Then
                    row.Cells("Digital").Style.ForeColor = Color.Yellow
                End If
            End If
        Next
    End Sub

    ' =========================================================
    ' BOTONES DE ACCIÓN
    ' =========================================================

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        Dim idCandidatoPadre As Integer = 0
        If dgvMesa.SelectedRows.Count > 0 Then
            idCandidatoPadre = Convert.ToInt32(dgvMesa.SelectedRows(0).Cells("Id").Value)
        End If
        Dim frm As New frmNuevoIngreso(idCandidatoPadre, esVinculacion:=True)
        If frm.ShowDialog() = DialogResult.OK Then CargarMesa()
    End Sub

    Private Sub btnActuar_Click(sender As Object, e As EventArgs) Handles btnActuar.Click
        If dgvMesa.SelectedRows.Count = 0 Then
            MessageBox.Show("Seleccione un documento para responder o actuar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If
        Dim idDoc As Integer = Convert.ToInt32(dgvMesa.SelectedRows(0).Cells("Id").Value)
        Dim idDocPase As Integer = ObtenerDocumentoPase(idDoc)
        Dim frm As New frmNuevoPase(idDocPase)
        If frm.ShowDialog() = DialogResult.OK Then
            CargarMesa()
            CargarHistorial(idDoc)
        End If
    End Sub

    Private Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        If dgvMesa.SelectedRows.Count = 0 Then
            MessageBox.Show("Seleccione un documento de la lista para ver su detalle o editar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If
        Dim idDoc As Integer = Convert.ToInt32(dgvMesa.SelectedRows(0).Cells("Id").Value)
        Dim frm As New frmNuevoIngreso(idDoc)
        If frm.ShowDialog() = DialogResult.OK Then
            CargarMesa()
            CargarHistorial(idDoc)
        End If
    End Sub

    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        If dgvMesa.SelectedRows.Count = 0 Then
            MessageBox.Show("Por favor, seleccione el documento que desea eliminar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim motivo As String = InputBox("Ingrese el MOTIVO de la eliminación:" & vbCrLf & "(Ej: Error de carga, Duplicado, Orden Judicial, etc.)", "Auditoría de Eliminación")
        If String.IsNullOrWhiteSpace(motivo) Then Return

        If MessageBox.Show("¿Está seguro? Se eliminará el documento y se archivará TODO su historial en auditoría.", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
            Try
                Dim idDoc As Integer = Convert.ToInt32(dgvMesa.SelectedRows(0).Cells("Id").Value)
                Using db As New PoloNuevoEntities()
                    Dim doc = db.Documentos.Find(idDoc)
                    If doc IsNot Nothing Then
                        ' 1. AUDITORÍA
                        Dim auditoria As New AuditoriaDocumentos()
                        auditoria.FechaEliminacion = DateTime.Now
                        auditoria.UsuarioResponsable = "Operador de Mesa"
                        auditoria.MotivoEliminacion = motivo
                        auditoria.DocIdOriginal = doc.Id
                        auditoria.DocReferencia = doc.ReferenciaExterna
                        auditoria.DocAsunto = doc.Descripcion
                        auditoria.DocFechaCarga = doc.FechaCarga
                        db.AuditoriaDocumentos.Add(auditoria)
                        db.SaveChanges()

                        ' 2. AUDITORÍA DE MOVIMIENTOS
                        For Each mov In doc.MovimientosDocumentos.ToList()
                            Dim audMov As New AuditoriaMovimientos()
                            audMov.AuditoriaDocId = auditoria.Id
                            audMov.FechaMovimientoOriginal = mov.FechaMovimiento
                            audMov.Origen = mov.Origen
                            audMov.Destino = mov.Destino
                            audMov.Observaciones = mov.Observaciones
                            audMov.TipoMovimiento = If(mov.EsSalida, "SALIDA", "ENTRADA")
                            db.AuditoriaMovimientos.Add(audMov)
                        Next

                        ' 3. LIMPIEZA DE VÍNCULOS
                        Dim vinculos = db.DocumentoVinculos _
                                         .Where(Function(v) v.IdDocumentoPadre = idDoc Or v.IdDocumentoHijo = idDoc) _
                                         .ToList()
                        If vinculos.Count > 0 Then db.DocumentoVinculos.RemoveRange(vinculos)

                        ' 4. ELIMINACIÓN FÍSICA
                        db.MovimientosDocumentos.RemoveRange(doc.MovimientosDocumentos)
                        db.Documentos.Remove(doc)
                        db.SaveChanges()

                        MessageBox.Show("Eliminación completa. El historial ha sido resguardado.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        CargarMesa()
                        dgvMovimientos.DataSource = Nothing
                    End If
                End Using
            Catch ex As Exception
                MessageBox.Show("Error crítico al eliminar: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    ' =========================================================
    ' EVENTOS DE INTERFAZ
    ' =========================================================
    Private Sub txtBuscar_TextChanged(sender As Object, e As EventArgs) Handles txtBuscar.TextChanged
        CargarMesa()
    End Sub
    Private Sub chkPendientes_CheckedChanged(sender As Object, e As EventArgs) Handles chkPendientes.CheckedChanged
        CargarMesa()
    End Sub
    Private Sub chkFechas_CheckedChanged(sender As Object, e As EventArgs) Handles chkFechas.CheckedChanged
        dtpDesde.Enabled = chkFechas.Checked
        dtpHasta.Enabled = chkFechas.Checked
        CargarMesa()
    End Sub
    Private Sub dtpDesde_ValueChanged(sender As Object, e As EventArgs) Handles dtpDesde.ValueChanged
        If chkFechas.Checked Then CargarMesa()
    End Sub
    Private Sub dtpHasta_ValueChanged(sender As Object, e As EventArgs) Handles dtpHasta.ValueChanged
        If chkFechas.Checked Then CargarMesa()
    End Sub
    Private Sub btnLimpiar_Click(sender As Object, e As EventArgs) Handles btnLimpiar.Click
        txtBuscar.Text = ""
        chkFechas.Checked = False
        chkPendientes.Checked = False
        dtpDesde.Value = DateTime.Now
        dtpHasta.Value = DateTime.Now
    End Sub

    ' =========================================================
    ' HISTORIAL (GRILLA INFERIOR)
    ' =========================================================
    Private Sub dgvMesa_SelectionChanged(sender As Object, e As EventArgs) Handles dgvMesa.SelectionChanged
        If dgvMesa.SelectedRows.Count > 0 Then
            Dim idDoc As Integer = Convert.ToInt32(dgvMesa.SelectedRows(0).Cells("Id").Value)
            CargarHistorial(idDoc)
        Else
            dgvMovimientos.DataSource = Nothing
        End If
    End Sub

    Private Sub CargarHistorial(idDoc As Integer)
        Using db As New PoloNuevoEntities()
            ' A) MOVIMIENTOS PROPIOS
            Dim listaPropios = db.MovimientosDocumentos _
                                 .Where(Function(m) m.DocumentoId = idDoc) _
                                 .Select(Function(m) New With {
                                     .IdMov = m.Id,
                                     .Fecha = m.FechaMovimiento,
                                     .Origen = m.Origen,
                                     .Destino = If(m.Destino Is Nothing Or m.Destino = "", "SIN DESTINO", m.Destino),
                                     .EsSalida = m.EsSalida,
                                     .Observaciones = m.Observaciones,
                                     .EsVinculoExterno = False,
                                     .RefHijo = ""
                                 }).ToList()

            ' B) VINCULACIONES EXTERNAS
            Dim hijos = db.DocumentoVinculos _
                .Include("Documentos") _
                .Include("Documentos.TiposDocumento") _
                .Where(Function(v) v.IdDocumentoPadre = idDoc) _
                .ToList()

            For Each v In hijos
                Dim hijo = v.Documentos
                Dim fechaEvento As DateTime = If(v.FechaVinculo.HasValue, v.FechaVinculo.Value, If(hijo IsNot Nothing AndAlso hijo.FechaCarga.HasValue, hijo.FechaCarga.Value, DateTime.Now))
                Dim tipoHijo As String = If(hijo IsNot Nothing AndAlso hijo.TiposDocumento IsNot Nothing, hijo.TiposDocumento.Nombre, "DOCUMENTO")
                Dim referenciaHijo As String = If(hijo IsNot Nothing, hijo.ReferenciaExterna, "SIN REFERENCIA")

                listaPropios.Add(New With {
                    .IdMov = 0,
                    .Fecha = fechaEvento,
                    .Origen = "MESA DE ENTRADA",
                    .Destino = "MESA DE ENTRADA",
                    .EsSalida = False,
                    .Observaciones = "RESPUESTA/VÍNCULO RECIBIDO",
                    .EsVinculoExterno = True,
                    .RefHijo = tipoHijo & " " & referenciaHijo
                })
            Next

            ' C) UNIFICACIÓN
            Dim historialFinal = listaPropios.OrderBy(Function(x) x.Fecha).Select(Function(x) New With {
                .IdMov = x.IdMov,
                .Origen = x.Origen,
                .Tipo = If(x.EsVinculoExterno, "🔗 SE VINCULÓ: " & x.RefHijo, If(x.EsSalida, "SALIDA (" & If(x.Observaciones Is Nothing, "S/D", x.Observaciones) & ")", "ENTRADA (" & If(x.Observaciones Is Nothing, "Pase", x.Observaciones) & ")")),
                .Destino = x.Destino,
                .Fecha = x.Fecha
            }).ToList()

            dgvMovimientos.DataSource = historialFinal

            ' Ajustes visuales
            If dgvMovimientos.Columns("IdMov") IsNot Nothing Then dgvMovimientos.Columns("IdMov").Visible = False
            If dgvMovimientos.Columns("Tipo") IsNot Nothing Then
                dgvMovimientos.Columns("Tipo").Width = 350
                dgvMovimientos.Columns("Tipo").HeaderText = "Movimiento / Evento"
            End If
            If dgvMovimientos.Columns("Destino") IsNot Nothing Then
                dgvMovimientos.Columns("Destino").HeaderText = "Ubicación"
                dgvMovimientos.Columns("Destino").Width = 180
            End If
            If dgvMovimientos.Columns("Fecha") IsNot Nothing Then
                dgvMovimientos.Columns("Fecha").HeaderText = "Fecha"
                dgvMovimientos.Columns("Fecha").Width = 120
                dgvMovimientos.Columns("Fecha").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            End If

            For Each row As DataGridViewRow In dgvMovimientos.Rows
                If row.Cells("Tipo").Value.ToString().Contains("🔗") Then
                    row.DefaultCellStyle.ForeColor = Color.Blue
                    row.DefaultCellStyle.Font = New Font("Segoe UI", 8, FontStyle.Bold)
                End If
            Next
        End Using
    End Sub

    Private Function ObtenerDocumentoPase(idDoc As Integer) As Integer
        Using db As New PoloNuevoEntities()
            Dim ultimoVinculo = db.DocumentoVinculos _
                .Include("Documentos") _
                .Where(Function(v) v.IdDocumentoPadre = idDoc) _
                .OrderByDescending(Function(v) If(v.FechaVinculo.HasValue,
                                                 v.FechaVinculo.Value,
                                                 If(v.Documentos IsNot Nothing AndAlso v.Documentos.FechaCarga.HasValue,
                                                    v.Documentos.FechaCarga.Value,
                                                    DateTime.MinValue))) _
                .FirstOrDefault()

            If ultimoVinculo IsNot Nothing Then
                Return ultimoVinculo.IdDocumentoHijo
            End If
        End Using

        Return idDoc
    End Function

    Private Sub btnVerDigital_Click(sender As Object, e As EventArgs) Handles btnVerDigital.Click
        If dgvMesa.SelectedRows.Count = 0 Then
            MessageBox.Show("Seleccione un documento.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If
        Dim idDoc As Integer = Convert.ToInt32(dgvMesa.SelectedRows(0).Cells("Id").Value)
        Try
            Using db As New PoloNuevoEntities()
                Dim doc = db.Documentos.Find(idDoc)
                If doc IsNot Nothing AndAlso doc.Contenido IsNot Nothing AndAlso doc.Contenido.Length > 0 Then
                    Dim extension As String = If(String.IsNullOrEmpty(doc.Extension), ".dat", doc.Extension)
                    Dim tempPath As String = Path.Combine(Path.GetTempPath(), "Doc_" & doc.Id & extension)
                    File.WriteAllBytes(tempPath, doc.Contenido)
                    Process.Start(tempPath)
                Else
                    MessageBox.Show("Este es un registro físico histórico. No tiene archivo digital adjunto.", "Sin Adjunto", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("No se pudo abrir el archivo: " & ex.Message)
        End Try
    End Sub

    ' =========================================================
    ' EDICIÓN E IMPRESIÓN
    ' =========================================================
    Private Sub EditarMovimiento()
        If dgvMovimientos.SelectedRows.Count = 0 Then
            MessageBox.Show("Seleccione una línea del historial abajo.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If
        Dim idMov As Integer = Convert.ToInt32(dgvMovimientos.SelectedRows(0).Cells("IdMov").Value)
        Dim idDoc As Integer = Convert.ToInt32(dgvMesa.SelectedRows(0).Cells("Id").Value)
        Dim frm As New frmNuevoPase(idDoc, idMov)
        If frm.ShowDialog() = DialogResult.OK Then
            CargarMesa()
            CargarHistorial(idDoc)
        End If
    End Sub

    Private Sub btnCorregirMov_Click(sender As Object, e As EventArgs) Handles btnCorregirMov.Click
        EditarMovimiento()
    End Sub
    Private Sub dgvMovimientos_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvMovimientos.CellDoubleClick
        If e.RowIndex >= 0 Then EditarMovimiento()
    End Sub

    Private Sub btnImprimirRecibo_Click(sender As Object, e As EventArgs) Handles btnImprimirRecibo.Click
        If dgvMesa.SelectedRows.Count = 0 Then
            MessageBox.Show("Seleccione un documento.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If
        Dim row = dgvMesa.SelectedRows(0)
        _impReferencia = row.Cells("Referencia").Value.ToString()
        _impAsunto = row.Cells("Asunto").Value.ToString()
        If dgvMovimientos.Rows.Count > 0 Then
            Dim rowMov As DataGridViewRow = If(dgvMovimientos.SelectedRows.Count > 0, dgvMovimientos.SelectedRows(0), dgvMovimientos.Rows(dgvMovimientos.Rows.Count - 1))
            _impFecha = rowMov.Cells("Fecha").Value.ToString()
            _impOrigen = rowMov.Cells("Origen").Value.ToString()
            _impDestino = rowMov.Cells("Destino").Value.ToString()
        Else
            _impFecha = DateTime.Now.ToString()
            _impOrigen = "SIN DATOS"
            _impDestino = "SIN DATOS"
        End If
        If PrintDialog1.ShowDialog() = DialogResult.OK Then PrintDocument1.Print()
    End Sub

    Private Sub PrintDocument1_PrintPage(sender As Object, e As PrintPageEventArgs) Handles PrintDocument1.PrintPage
        Dim g As Graphics = e.Graphics
        Dim fuenteTitulo As New Font("Arial", 16, FontStyle.Bold)
        Dim fuenteSub As New Font("Arial", 12, FontStyle.Bold)
        Dim fuenteNormal As New Font("Arial", 10, FontStyle.Regular)
        Dim fuenteChica As New Font("Arial", 8, FontStyle.Regular)
        Dim pincel As New SolidBrush(Color.Black)
        Dim margenIzq As Integer = 50
        Dim y As Integer = 50

        g.DrawString("UNIDAD N° 4 - SANTIAGO VÁZQUEZ", fuenteTitulo, pincel, margenIzq, y)
        y += 30
        g.DrawString("CONSTANCIA DE PASE / MOVIMIENTO", fuenteSub, pincel, margenIzq, y)
        y += 10
        g.DrawLine(Pens.Black, margenIzq, y + 20, 750, y + 20)
        y += 40
        g.DrawString("DOCUMENTO: " & _impReferencia, fuenteSub, pincel, margenIzq, y)
        y += 25
        g.DrawString("ASUNTO: ", fuenteNormal, pincel, margenIzq, y)
        Dim rectAsunto As New RectangleF(margenIzq + 70, y, 600, 60)
        g.DrawString(_impAsunto, fuenteNormal, pincel, rectAsunto)
        y += 70
        g.DrawLine(Pens.Gray, margenIzq, y, 750, y)
        y += 20
        g.DrawString("FECHA MOVIMIENTO: " & _impFecha, fuenteNormal, pincel, margenIzq, y)
        y += 30
        g.DrawString("ORIGEN: " & _impOrigen, fuenteNormal, pincel, margenIzq, y)
        y += 30
        g.DrawString("DESTINO: " & _impDestino, fuenteSub, pincel, margenIzq, y)
        y += 50
        Dim rectFirma As New Rectangle(margenIzq, y, 700, 150)
        g.DrawRectangle(Pens.Black, rectFirma)
        g.DrawString("RECIBIDO POR:", fuenteNormal, pincel, margenIzq + 10, y + 10)
        g.DrawString("FIRMA Y ACLARACIÓN:", fuenteNormal, pincel, margenIzq + 10, y + 100)
        g.DrawString("FECHA: ______ / ______ / ___________", fuenteNormal, pincel, margenIzq + 400, y + 100)
        y += 170
        g.DrawString("Emitido por Sistema POLO el " & DateTime.Now.ToString(), fuenteChica, Brushes.Gray, margenIzq, y)
        g.DrawString("Operador: " & _impUsuario, fuenteChica, Brushes.Gray, margenIzq, y + 15)
        e.HasMorePages = False
    End Sub

    Private Sub dgvMesa_CellToolTipTextNeeded(sender As Object, e As DataGridViewCellToolTipTextNeededEventArgs) Handles dgvMesa.CellToolTipTextNeeded
        If e.RowIndex >= 0 Then
            Dim colName As String = dgvMesa.Columns(e.ColumnIndex).Name
            If colName = "Referencia" Or colName = "Asunto" Then
                Dim idDoc As Integer = Convert.ToInt32(dgvMesa.Rows(e.RowIndex).Cells("Id").Value)
                e.ToolTipText = GenerarTimelineSimple(idDoc)
            End If
        End If
    End Sub

    Private Function GenerarTimelineSimple(idDoc As Integer) As String
        Dim sb As New System.Text.StringBuilder()
        Try
            Using db As New PoloNuevoEntities()
                Dim movimientos = db.MovimientosDocumentos.Where(Function(m) m.DocumentoId = idDoc).OrderBy(Function(m) m.FechaMovimiento).ToList()
                If movimientos.Count = 0 Then Return "Sin movimientos."
                sb.AppendLine("📋 HISTORIAL RÁPIDO")
                sb.AppendLine(New String("-"c, 30))
                For i As Integer = 0 To movimientos.Count - 1
                    Dim m = movimientos(i)
                    Dim icono As String = If(m.EsSalida, "📤", "📥")
                    sb.AppendLine($"{icono} {m.FechaMovimiento:dd/MM HH:mm} | {If(m.EsSalida, m.Destino, m.Origen)}")
                    If i < movimientos.Count - 1 Then sb.AppendLine("      ⬇")
                Next
            End Using
        Catch ex As Exception
            Return "..."
        End Try
        Return sb.ToString()
    End Function
    Private Sub btnVerHistorial_Click(sender As Object, e As EventArgs) Handles btnVerHistorial.Click
        ' 1. Validamos selección
        If dgvMesa.SelectedRows.Count = 0 Then
            MessageBox.Show("Por favor, seleccione un documento primero.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Try
            ' 2. Obtenemos datos
            Dim idDoc As Integer = Convert.ToInt32(dgvMesa.SelectedRows(0).Cells("Id").Value)
            Dim ref As String = Convert.ToString(dgvMesa.SelectedRows(0).Cells("Referencia").Value)

            ' 3. Llamamos al formulario nuevo
            ' Ya no hay código complejo aquí, solo abrir la ventana
            Dim frm As New frmHistorial(idDoc, ref)
            frm.ShowDialog()

        Catch ex As Exception
            MessageBox.Show("Error al abrir historial: " & ex.Message)
        End Try
    End Sub

End Class
