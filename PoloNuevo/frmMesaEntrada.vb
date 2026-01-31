Imports System.IO
Imports System.Drawing.Printing
Imports System.Data.Entity ' Necesario para Include y consultas avanzadas

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

    ' =========================================================
    ' CLASE AUXILIAR INTERNA PARA ORDENAR EL TIMELINE 360°
    ' =========================================================
    Private Class EventoTimeline
        Public Property Fecha As DateTime
        Public Property Icono As String
        Public Property TextoPrincipal As String
        Public Property Detalle As String
        Public Property EsHijo As Boolean ' Para saber si es un evento externo
    End Class

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

                ' 1. CONSULTA BASE
                Dim query = db.Documentos.Where(Function(d) d.TiposDocumento.Nombre <> "ARCHIVO")

                ' 2. Filtro Pendientes (LÓGICA CORREGIDA: Verifica ubicación actual, no cantidad de viajes)
                If chkPendientes.Checked Then
                    query = query.Where(Function(d) d.MovimientosDocumentos _
                                            .OrderByDescending(Function(m) m.FechaMovimiento) _
                                            .FirstOrDefault().Destino = "MESA DE ENTRADA")
                End If

                ' 3. Buscador Inteligente
                Dim texto = txtBuscar.Text.Trim().ToLower()
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

                ' 4. Filtro Fechas
                If chkFechas.Checked Then
                    Dim fDesde = dtpDesde.Value.Date
                    Dim fHasta = dtpHasta.Value.Date.AddDays(1).AddSeconds(-1)
                    query = query.Where(Function(d) d.FechaCarga >= fDesde And d.FechaCarga <= fHasta)
                End If

                ' 5. Ejecución y Proyección
                ' NOTA: Usamos lógica segura para determinar el estado
                Dim lista = query.OrderByDescending(Function(d) d.FechaCarga) _
                                 .Select(Function(d) New With {
                                     .Id = d.Id,
                                     .Fecha = d.FechaCarga,
                                     .Referencia = d.TiposDocumento.Nombre & " " & d.ReferenciaExterna,
                                     .Asunto = d.Descripcion,
                                     .Recluso = If(d.Reclusos IsNot Nothing, d.Reclusos.Nombre, "Sin Vincular"),
                                     .Estado = If(d.MovimientosDocumentos.OrderByDescending(Function(m) m.FechaMovimiento) _
                                                                        .Select(Function(m) m.Destino) _
                                                                        .FirstOrDefault() = "MESA DE ENTRADA",
                                                  "PENDIENTE", "MOVIDO"),
                                     .Digital = If(d.Extension <> ".phy", "SI", "NO"),
                                     .Vencimiento = d.FechaVencimiento
                                 }) _
                                 .Take(200) _
                                 .ToList()

                ' 6. Asignación a la Grilla
                dgvMesa.DataSource = lista

                ' 7. Configuración Visual
                If dgvMesa.Columns("Id") IsNot Nothing Then dgvMesa.Columns("Id").Visible = False
                If dgvMesa.Columns("Vencimiento") IsNot Nothing Then dgvMesa.Columns("Vencimiento").Visible = False

                ConfigurarColumnas()
                ColorearPendientes()

                ' Limpiar detalles si la lista quedó vacía
                If lista.Count = 0 Then dgvMovimientos.DataSource = Nothing
            End Using
        Catch ex As Exception
            MessageBox.Show("Error al cargar datos: " & ex.Message)
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
        Dim frm As New frmNuevoPase(idDoc)
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
            Dim docActual = db.Documentos.Find(idDoc)
            Dim referenciaDoc As String = ""
            If docActual IsNot Nothing Then referenciaDoc = docActual.ReferenciaExterna

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
            If Not String.IsNullOrEmpty(referenciaDoc) Then
                Dim textoBusqueda As String = "VINCULADO A: " & referenciaDoc
                Dim qVinculos = db.MovimientosDocumentos _
                                  .Include("Documentos").Include("Documentos.TiposDocumento") _
                                  .Where(Function(m) m.DocumentoId <> idDoc AndAlso m.Observaciones.Contains(textoBusqueda)) _
                                  .Select(Function(m) New With {
                                      .IdMov = m.Id,
                                      .Fecha = m.FechaMovimiento,
                                      .Origen = "MESA DE ENTRADA",
                                      .Destino = "MESA DE ENTRADA",
                                      .EsSalida = False,
                                      .Observaciones = "RESPUESTA/VÍNCULO RECIBIDO",
                                      .EsVinculoExterno = True,
                                      .RefHijo = m.Documentos.TiposDocumento.Nombre & " " & m.Documentos.ReferenciaExterna
                                  }).ToList()

                For Each item In qVinculos
                    listaPropios.Add(New With {
                        .IdMov = item.IdMov,
                        .Fecha = item.Fecha,
                        .Origen = item.Origen,
                        .Destino = item.Destino,
                        .EsSalida = item.EsSalida,
                        .Observaciones = item.Observaciones,
                        .EsVinculoExterno = True,
                        .RefHijo = item.RefHijo
                    })
                Next
            End If

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



    ' =========================================================
    ' VISUALIZADOR DE HISTORIAL 360° (CORREGIDO Y ESTABILIZADO)
    ' =========================================================
    Private Sub btnVerHistorial_Click(sender As Object, e As EventArgs) Handles btnVerHistorial.Click
        ' 1. Validamos selección de forma segura
        If dgvMesa.SelectedRows.Count = 0 Then
            MessageBox.Show("Por favor, seleccione un documento primero.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Try
            ' Usamos Convert.ToString para evitar error si la celda es nula
            Dim idDocInicial As Integer = Convert.ToInt32(dgvMesa.SelectedRows(0).Cells("Id").Value)
            Dim refInicial As String = Convert.ToString(dgvMesa.SelectedRows(0).Cells("Referencia").Value)

            ' 2. Formulario visual
            Dim fHistorial As New Form() With {
                .Text = "Historia Clínica del Documento: " & refInicial,
                .Size = New Size(950, 550),
                .StartPosition = FormStartPosition.CenterScreen,
                .BackColor = Color.White
            }

            Dim dgvH As New DataGridView() With {
                .Dock = DockStyle.Fill,
                .ReadOnly = True,
                .RowHeadersVisible = False,
                .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                .BackgroundColor = Color.WhiteSmoke,
                .BorderStyle = BorderStyle.None,
                .CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                .AllowUserToAddRows = False,
                .AutoGenerateColumns = True ' Forzamos generación automática
            }
            dgvH.EnableHeadersVisualStyles = False
            dgvH.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue
            dgvH.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
            dgvH.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 10, FontStyle.Bold)
            dgvH.DefaultCellStyle.Font = New Font("Segoe UI", 9)
            fHistorial.Controls.Add(dgvH)

            ' 3. Lógica de Datos
            Using db As New PoloNuevoEntities()
                ' A. ENCONTRAR LA FAMILIA (ÁRBOL DE VÍNCULOS)
                Dim familiaIds As New List(Of Integer) From {idDocInicial}
                Dim nuevosEncontrados As Boolean = True

                ' Límite de seguridad para evitar bucles infinitos
                Dim iteraciones As Integer = 0
                While nuevosEncontrados AndAlso iteraciones < 50
                    nuevosEncontrados = False
                    iteraciones += 1
                    Dim listaActual = familiaIds.ToList()
                    Dim vinculos = db.DocumentoVinculos _
                                     .Where(Function(v) listaActual.Contains(v.IdDocumentoPadre) Or listaActual.Contains(v.IdDocumentoHijo)) _
                                     .ToList()
                    For Each v In vinculos
                        If Not familiaIds.Contains(v.IdDocumentoPadre) Then
                            familiaIds.Add(v.IdDocumentoPadre)
                            nuevosEncontrados = True
                        End If
                        If Not familiaIds.Contains(v.IdDocumentoHijo) Then
                            familiaIds.Add(v.IdDocumentoHijo)
                            nuevosEncontrados = True
                        End If
                    Next
                End While

                ' B. TRAER MOVIMIENTOS (Raw Data)
                Dim rawData = db.MovimientosDocumentos _
                                .Include("Documentos").Include("Documentos.TiposDocumento") _
                                .Where(Function(m) familiaIds.Contains(m.DocumentoId)) _
                                .OrderBy(Function(m) m.FechaMovimiento) _
                                .ToList()

                ' C. PROYECCIÓN SEGURA (SOLUCIÓN: SIN TILDE EN PROPIEDAD)
                Dim listaMovimientos = rawData.Select(Function(m)
                                                          Dim accionHumana As String = "Trámite"
                                                          Dim obs As String = If(m.Observaciones, "").ToUpper()

                                                          If m.Origen = "SISTEMA" Then
                                                              accionHumana = "✨ GENERACIÓN INICIAL"
                                                          ElseIf obs.Contains("VINCULADO") Or obs.Contains("ADJUNTO") Then
                                                              accionHumana = "🔗 VINCULACIÓN"
                                                          ElseIf m.EsSalida Then
                                                              accionHumana = "📤 PASE / ENVÍO"
                                                          Else
                                                              accionHumana = "📥 RECEPCIÓN"
                                                          End If

                                                          ' Protección de referencias nulas
                                                          Dim docRef As String = "Desconocido"
                                                          If m.Documentos IsNot Nothing Then
                                                              Dim tipo As String = If(m.Documentos.TiposDocumento IsNot Nothing, m.Documentos.TiposDocumento.Nombre, "DOC")
                                                              Dim ref As String = If(m.Documentos.ReferenciaExterna, "S/N")
                                                              docRef = tipo & " " & ref
                                                          End If

                                                          If m.DocumentoId = idDocInicial Then docRef = "➤ ESTE DOCUMENTO"

                                                          ' AQUÍ LA CLAVE: Propiedades sin acentos
                                                          Return New With {
                                                 .Fecha = m.FechaMovimiento.ToString("dd/MM HH:mm"),
                                                 .Documento = docRef,
                                                 .Accion = accionHumana,
                                                 .Origen = If(m.Origen, "S/D"),
                                                 .Destino = If(m.Destino, "S/D"),
                                                 .Nota = obs
                                             }
                                                      End Function).ToList()

                ' D. ASIGNACIÓN A LA GRILLA
                If listaMovimientos.Count > 0 Then
                    dgvH.DataSource = listaMovimientos

                    ' === CONFIGURACIÓN VISUAL BLINDADA ===
                    With dgvH
                        ' Verificamos existencia antes de tocar
                        If .Columns.Contains("Fecha") Then
                            .Columns("Fecha").Width = 110
                            .Columns("Fecha").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                        End If

                        If .Columns.Contains("Documento") Then
                            .Columns("Documento").Width = 200
                        End If

                        If .Columns.Contains("Accion") Then
                            .Columns("Accion").Width = 150
                            .Columns("Accion").HeaderText = "Acción" ' Visualmente ponemos la tilde aquí
                        End If

                        If .Columns.Contains("Nota") Then
                            .Columns("Nota").Visible = False
                        End If
                    End With

                    fHistorial.ShowDialog()
                Else
                    MessageBox.Show("Este documento no tiene movimientos registrados.", "Historial Vacío", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End Using

        Catch ex As Exception
            MessageBox.Show("Error al generar historial: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

End Class
'Imports System.IO
'Imports System.Drawing.Printing
'Imports System.Data.Entity ' Necesario para Include y consultas avanzadas

'Public Class frmMesaEntrada

'    ' =========================================================
'    ' VARIABLES GLOBALES PARA IMPRESIÓN
'    ' =========================================================
'    Private _impReferencia As String
'    Private _impAsunto As String
'    Private _impOrigen As String
'    Private _impDestino As String
'    Private _impFecha As String
'    Private _impUsuario As String = "Operador de Mesa"

'    Private Sub frmMesaEntrada_Load(sender As Object, e As EventArgs) Handles MyBase.Load
'        ConfigurarGrillaMovimientos()
'        CargarMesa()
'    End Sub

'    Private Sub ConfigurarGrillaMovimientos()
'        dgvMovimientos.BackgroundColor = Color.WhiteSmoke
'        dgvMovimientos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
'    End Sub

'    ' =========================================================
'    ' LÓGICA PRINCIPAL: CARGA Y FILTROS
'    ' =========================================================
'    Private Sub CargarMesa()
'        Try
'            Using db As New PoloNuevoEntities()

'                ' 1. CONSULTA BASE
'                Dim query = db.Documentos.Where(Function(d) d.TiposDocumento.Nombre <> "ARCHIVO")

'                ' 2. Filtro Pendientes (LÓGICA CORREGIDA: Verifica ubicación actual, no cantidad de viajes)
'                If chkPendientes.Checked Then
'                    query = query.Where(Function(d) d.MovimientosDocumentos _
'                                            .OrderByDescending(Function(m) m.FechaMovimiento) _
'                                            .FirstOrDefault().Destino = "MESA DE ENTRADA")
'                End If

'                ' 3. Buscador Inteligente
'                Dim texto = txtBuscar.Text.Trim().ToLower()
'                If Not String.IsNullOrEmpty(texto) Then
'                    Dim palabras As String() = texto.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)
'                    For Each palabra In palabras
'                        Dim termino = palabra
'                        query = query.Where(Function(d) d.ReferenciaExterna.Contains(termino) Or
'                                                        d.TiposDocumento.Nombre.Contains(termino) Or
'                                                        d.Descripcion.Contains(termino) Or
'                                                        (d.Reclusos IsNot Nothing AndAlso d.Reclusos.Nombre.Contains(termino)))
'                    Next
'                End If

'                ' 4. Filtro Fechas
'                If chkFechas.Checked Then
'                    Dim fDesde = dtpDesde.Value.Date
'                    Dim fHasta = dtpHasta.Value.Date.AddDays(1).AddSeconds(-1)
'                    query = query.Where(Function(d) d.FechaCarga >= fDesde And d.FechaCarga <= fHasta)
'                End If

'                ' 5. Ejecución y Proyección
'                ' NOTA: Usamos Select(m => m.Destino).FirstOrDefault() para evitar errores si no hay movimientos
'                Dim lista = query.OrderByDescending(Function(d) d.FechaCarga) _
'                                 .Select(Function(d) New With {
'                                     .Id = d.Id,
'                                     .Fecha = d.FechaCarga,
'                                     .Referencia = d.TiposDocumento.Nombre & " " & d.ReferenciaExterna,
'                                     .Asunto = d.Descripcion,
'                                     .Recluso = If(d.Reclusos IsNot Nothing, d.Reclusos.Nombre, "Sin Vincular"),
'                                     .Estado = If(d.MovimientosDocumentos.OrderByDescending(Function(m) m.FechaMovimiento) _
'                                                                        .Select(Function(m) m.Destino) _
'                                                                        .FirstOrDefault() = "MESA DE ENTRADA",
'                                                  "PENDIENTE", "MOVIDO"),
'                                     .Digital = If(d.Extension <> ".phy", "SI", "NO"),
'                                     .Vencimiento = d.FechaVencimiento
'                                 }) _
'                                 .Take(200) _
'                                 .ToList()

'                ' 6. Asignación a la Grilla
'                dgvMesa.DataSource = lista

'                ' 7. Configuración Visual
'                If dgvMesa.Columns("Id") IsNot Nothing Then dgvMesa.Columns("Id").Visible = False
'                If dgvMesa.Columns("Vencimiento") IsNot Nothing Then dgvMesa.Columns("Vencimiento").Visible = False

'                ConfigurarColumnas()
'                ColorearPendientes()

'                ' Limpiar detalles si la lista quedó vacía
'                If lista.Count = 0 Then dgvMovimientos.DataSource = Nothing
'            End Using
'        Catch ex As Exception
'            MessageBox.Show("Error al cargar datos: " & ex.Message)
'        End Try
'    End Sub

'    Private Sub ConfigurarColumnas()
'        If dgvMesa.Columns.Count = 0 Then Return

'        With dgvMesa
'            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
'            If .Columns("Fecha") IsNot Nothing Then
'                .Columns("Fecha").Width = 85
'                .Columns("Fecha").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
'            End If
'            If .Columns("Referencia") IsNot Nothing Then
'                .Columns("Referencia").Width = 140
'                .Columns("Referencia").HeaderText = "Tipo y Nro."
'            End If
'            If .Columns("Recluso") IsNot Nothing Then
'                .Columns("Recluso").Width = 180
'            End If
'            If .Columns("Digital") IsNot Nothing Then
'                .Columns("Digital").Width = 50
'                .Columns("Digital").HeaderText = "Dig."
'                .Columns("Digital").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
'            End If
'            If .Columns("Estado") IsNot Nothing Then
'                .Columns("Estado").Width = 90
'                .Columns("Estado").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
'                .Columns("Estado").DefaultCellStyle.Font = New Font("Segoe UI", 8, FontStyle.Bold)
'            End If
'            If .Columns("Asunto") IsNot Nothing Then
'                .Columns("Asunto").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
'            End If
'        End With
'    End Sub

'    Private Sub ColorearPendientes()
'        If dgvMesa.Columns("Estado") Is Nothing Then Return

'        Dim fechaHoy As Date = DateTime.Now.Date

'        For Each row As DataGridViewRow In dgvMesa.Rows
'            Dim tieneVencimiento As Boolean = False
'            If dgvMesa.Columns("Vencimiento") IsNot Nothing AndAlso row.Cells("Vencimiento").Value IsNot Nothing Then
'                Dim fechaVenc As Date = Convert.ToDateTime(row.Cells("Vencimiento").Value)
'                tieneVencimiento = True
'                Dim diasRestantes As Integer = (fechaVenc - fechaHoy).TotalDays

'                If diasRestantes < 0 Then
'                    row.DefaultCellStyle.BackColor = Color.Firebrick
'                    row.DefaultCellStyle.ForeColor = Color.White
'                    row.Cells("Referencia").Value = "(VENCIDO) " & row.Cells("Referencia").Value.ToString()
'                ElseIf diasRestantes <= 5 Then
'                    row.DefaultCellStyle.BackColor = Color.Orange
'                    row.DefaultCellStyle.ForeColor = Color.Black
'                End If
'            End If

'            If Not tieneVencimiento AndAlso row.Cells("Estado").Value.ToString() = "PENDIENTE" Then
'                If row.DefaultCellStyle.BackColor = dgvMesa.DefaultCellStyle.BackColor Then
'                    row.DefaultCellStyle.ForeColor = Color.DarkRed
'                    row.DefaultCellStyle.BackColor = Color.MistyRose
'                End If
'            End If

'            If dgvMesa.Columns("Digital") IsNot Nothing AndAlso row.Cells("Digital").Value.ToString() = "SI" Then
'                row.Cells("Digital").Style.ForeColor = Color.Blue
'                row.Cells("Digital").Style.Font = New Font("Segoe UI", 8, FontStyle.Bold)
'                If row.DefaultCellStyle.BackColor = Color.Firebrick Then
'                    row.Cells("Digital").Style.ForeColor = Color.Yellow
'                End If
'            End If
'        Next
'    End Sub

'    ' =========================================================
'    ' BOTONES DE ACCIÓN
'    ' =========================================================

'    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
'        Dim idCandidatoPadre As Integer = 0
'        If dgvMesa.SelectedRows.Count > 0 Then
'            idCandidatoPadre = Convert.ToInt32(dgvMesa.SelectedRows(0).Cells("Id").Value)
'        End If
'        Dim frm As New frmNuevoIngreso(idCandidatoPadre, esVinculacion:=True)
'        If frm.ShowDialog() = DialogResult.OK Then CargarMesa()
'    End Sub

'    Private Sub btnActuar_Click(sender As Object, e As EventArgs) Handles btnActuar.Click
'        If dgvMesa.SelectedRows.Count = 0 Then
'            MessageBox.Show("Seleccione un documento para responder o actuar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information)
'            Return
'        End If
'        Dim idDoc As Integer = Convert.ToInt32(dgvMesa.SelectedRows(0).Cells("Id").Value)
'        Dim frm As New frmNuevoPase(idDoc)
'        If frm.ShowDialog() = DialogResult.OK Then
'            CargarMesa()
'            CargarHistorial(idDoc)
'        End If
'    End Sub

'    Private Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
'        If dgvMesa.SelectedRows.Count = 0 Then
'            MessageBox.Show("Seleccione un documento de la lista para ver su detalle o editar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information)
'            Return
'        End If
'        Dim idDoc As Integer = Convert.ToInt32(dgvMesa.SelectedRows(0).Cells("Id").Value)
'        Dim frm As New frmNuevoIngreso(idDoc)
'        If frm.ShowDialog() = DialogResult.OK Then
'            CargarMesa()
'            CargarHistorial(idDoc)
'        End If
'    End Sub

'    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
'        If dgvMesa.SelectedRows.Count = 0 Then
'            MessageBox.Show("Por favor, seleccione el documento que desea eliminar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information)
'            Return
'        End If

'        Dim motivo As String = InputBox("Ingrese el MOTIVO de la eliminación:" & vbCrLf & "(Ej: Error de carga, Duplicado, Orden Judicial, etc.)", "Auditoría de Eliminación")
'        If String.IsNullOrWhiteSpace(motivo) Then Return

'        If MessageBox.Show("¿Está seguro? Se eliminará el documento y se archivará TODO su historial en auditoría.", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
'            Try
'                Dim idDoc As Integer = Convert.ToInt32(dgvMesa.SelectedRows(0).Cells("Id").Value)
'                Using db As New PoloNuevoEntities()
'                    Dim doc = db.Documentos.Find(idDoc)
'                    If doc IsNot Nothing Then
'                        ' 1. AUDITORÍA
'                        Dim auditoria As New AuditoriaDocumentos()
'                        auditoria.FechaEliminacion = DateTime.Now
'                        auditoria.UsuarioResponsable = "Operador de Mesa"
'                        auditoria.MotivoEliminacion = motivo
'                        auditoria.DocIdOriginal = doc.Id
'                        auditoria.DocReferencia = doc.ReferenciaExterna
'                        auditoria.DocAsunto = doc.Descripcion
'                        auditoria.DocFechaCarga = doc.FechaCarga
'                        db.AuditoriaDocumentos.Add(auditoria)
'                        db.SaveChanges()

'                        For Each mov In doc.MovimientosDocumentos.ToList()
'                            Dim audMov As New AuditoriaMovimientos()
'                            audMov.AuditoriaDocId = auditoria.Id
'                            audMov.FechaMovimientoOriginal = mov.FechaMovimiento
'                            audMov.Origen = mov.Origen
'                            audMov.Destino = mov.Destino
'                            audMov.Observaciones = mov.Observaciones
'                            audMov.TipoMovimiento = If(mov.EsSalida, "SALIDA", "ENTRADA")
'                            db.AuditoriaMovimientos.Add(audMov)
'                        Next

'                        ' 2. LIMPIEZA DE VÍNCULOS
'                        Dim vinculos = db.DocumentoVinculos _
'                                         .Where(Function(v) v.IdDocumentoPadre = idDoc Or v.IdDocumentoHijo = idDoc) _
'                                         .ToList()
'                        If vinculos.Count > 0 Then db.DocumentoVinculos.RemoveRange(vinculos)

'                        ' 3. ELIMINACIÓN
'                        db.MovimientosDocumentos.RemoveRange(doc.MovimientosDocumentos)
'                        db.Documentos.Remove(doc)
'                        db.SaveChanges()

'                        MessageBox.Show("Eliminación completa. El historial ha sido resguardado.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
'                        CargarMesa()
'                        dgvMovimientos.DataSource = Nothing
'                    End If
'                End Using
'            Catch ex As Exception
'                MessageBox.Show("Error crítico al eliminar: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
'            End Try
'        End If
'    End Sub

'    ' =========================================================
'    ' EVENTOS DE INTERFAZ
'    ' =========================================================
'    Private Sub txtBuscar_TextChanged(sender As Object, e As EventArgs) Handles txtBuscar.TextChanged
'        CargarMesa()
'    End Sub
'    Private Sub chkPendientes_CheckedChanged(sender As Object, e As EventArgs) Handles chkPendientes.CheckedChanged
'        CargarMesa()
'    End Sub
'    Private Sub chkFechas_CheckedChanged(sender As Object, e As EventArgs) Handles chkFechas.CheckedChanged
'        dtpDesde.Enabled = chkFechas.Checked
'        dtpHasta.Enabled = chkFechas.Checked
'        CargarMesa()
'    End Sub
'    Private Sub dtpDesde_ValueChanged(sender As Object, e As EventArgs) Handles dtpDesde.ValueChanged
'        If chkFechas.Checked Then CargarMesa()
'    End Sub
'    Private Sub dtpHasta_ValueChanged(sender As Object, e As EventArgs) Handles dtpHasta.ValueChanged
'        If chkFechas.Checked Then CargarMesa()
'    End Sub
'    Private Sub btnLimpiar_Click(sender As Object, e As EventArgs) Handles btnLimpiar.Click
'        txtBuscar.Text = ""
'        chkFechas.Checked = False
'        chkPendientes.Checked = False
'        dtpDesde.Value = DateTime.Now
'        dtpHasta.Value = DateTime.Now
'    End Sub

'    ' =========================================================
'    ' HISTORIAL (GRILLA INFERIOR)
'    ' =========================================================
'    Private Sub dgvMesa_SelectionChanged(sender As Object, e As EventArgs) Handles dgvMesa.SelectionChanged
'        If dgvMesa.SelectedRows.Count > 0 Then
'            Dim idDoc As Integer = Convert.ToInt32(dgvMesa.SelectedRows(0).Cells("Id").Value)
'            CargarHistorial(idDoc)
'        Else
'            dgvMovimientos.DataSource = Nothing
'        End If
'    End Sub

'    Private Sub CargarHistorial(idDoc As Integer)
'        Using db As New PoloNuevoEntities()
'            Dim docActual = db.Documentos.Find(idDoc)
'            Dim referenciaDoc As String = ""
'            If docActual IsNot Nothing Then referenciaDoc = docActual.ReferenciaExterna

'            ' A) MOVIMIENTOS PROPIOS
'            Dim listaPropios = db.MovimientosDocumentos _
'                                 .Where(Function(m) m.DocumentoId = idDoc) _
'                                 .Select(Function(m) New With {
'                                     .IdMov = m.Id,
'                                     .Fecha = m.FechaMovimiento,
'                                     .Origen = m.Origen,
'                                     .Destino = If(m.Destino Is Nothing Or m.Destino = "", "SIN DESTINO", m.Destino),
'                                     .EsSalida = m.EsSalida,
'                                     .Observaciones = m.Observaciones,
'                                     .EsVinculoExterno = False,
'                                     .RefHijo = ""
'                                 }).ToList()

'            ' B) VINCULACIONES EXTERNAS
'            If Not String.IsNullOrEmpty(referenciaDoc) Then
'                Dim textoBusqueda As String = "VINCULADO A: " & referenciaDoc
'                Dim qVinculos = db.MovimientosDocumentos _
'                                  .Include("Documentos").Include("Documentos.TiposDocumento") _
'                                  .Where(Function(m) m.DocumentoId <> idDoc AndAlso m.Observaciones.Contains(textoBusqueda)) _
'                                  .Select(Function(m) New With {
'                                      .IdMov = m.Id,
'                                      .Fecha = m.FechaMovimiento,
'                                      .Origen = "MESA DE ENTRADA",
'                                      .Destino = "MESA DE ENTRADA",
'                                      .EsSalida = False,
'                                      .Observaciones = "RESPUESTA/VÍNCULO RECIBIDO",
'                                      .EsVinculoExterno = True,
'                                      .RefHijo = m.Documentos.TiposDocumento.Nombre & " " & m.Documentos.ReferenciaExterna
'                                  }).ToList()

'                For Each item In qVinculos
'                    listaPropios.Add(New With {
'                        .IdMov = item.IdMov,
'                        .Fecha = item.Fecha,
'                        .Origen = item.Origen,
'                        .Destino = item.Destino,
'                        .EsSalida = item.EsSalida,
'                        .Observaciones = item.Observaciones,
'                        .EsVinculoExterno = True,
'                        .RefHijo = item.RefHijo
'                    })
'                Next
'            End If

'            ' C) UNIFICACIÓN
'            Dim historialFinal = listaPropios.OrderBy(Function(x) x.Fecha).Select(Function(x) New With {
'                .IdMov = x.IdMov,
'                .Origen = x.Origen,
'                .Tipo = If(x.EsVinculoExterno, "🔗 SE VINCULÓ: " & x.RefHijo, If(x.EsSalida, "SALIDA (" & If(x.Observaciones Is Nothing, "S/D", x.Observaciones) & ")", "ENTRADA (" & If(x.Observaciones Is Nothing, "Pase", x.Observaciones) & ")")),
'                .Destino = x.Destino,
'                .Fecha = x.Fecha
'            }).ToList()

'            dgvMovimientos.DataSource = historialFinal

'            ' Ajustes visuales
'            If dgvMovimientos.Columns("IdMov") IsNot Nothing Then dgvMovimientos.Columns("IdMov").Visible = False
'            If dgvMovimientos.Columns("Tipo") IsNot Nothing Then
'                dgvMovimientos.Columns("Tipo").Width = 350
'                dgvMovimientos.Columns("Tipo").HeaderText = "Movimiento / Evento"
'            End If
'            If dgvMovimientos.Columns("Destino") IsNot Nothing Then
'                dgvMovimientos.Columns("Destino").HeaderText = "Ubicación"
'                dgvMovimientos.Columns("Destino").Width = 180
'            End If
'            If dgvMovimientos.Columns("Fecha") IsNot Nothing Then
'                dgvMovimientos.Columns("Fecha").HeaderText = "Fecha"
'                dgvMovimientos.Columns("Fecha").Width = 120
'                dgvMovimientos.Columns("Fecha").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
'            End If

'            For Each row As DataGridViewRow In dgvMovimientos.Rows
'                If row.Cells("Tipo").Value.ToString().Contains("🔗") Then
'                    row.DefaultCellStyle.ForeColor = Color.Blue
'                    row.DefaultCellStyle.Font = New Font("Segoe UI", 8, FontStyle.Bold)
'                End If
'            Next
'        End Using
'    End Sub

'    Private Sub btnVerDigital_Click(sender As Object, e As EventArgs) Handles btnVerDigital.Click
'        If dgvMesa.SelectedRows.Count = 0 Then
'            MessageBox.Show("Seleccione un documento.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information)
'            Return
'        End If
'        Dim idDoc As Integer = Convert.ToInt32(dgvMesa.SelectedRows(0).Cells("Id").Value)
'        Try
'            Using db As New PoloNuevoEntities()
'                Dim doc = db.Documentos.Find(idDoc)
'                If doc IsNot Nothing AndAlso doc.Contenido IsNot Nothing AndAlso doc.Contenido.Length > 0 Then
'                    Dim extension As String = If(String.IsNullOrEmpty(doc.Extension), ".dat", doc.Extension)
'                    Dim tempPath As String = Path.Combine(Path.GetTempPath(), "Doc_" & doc.Id & extension)
'                    File.WriteAllBytes(tempPath, doc.Contenido)
'                    Process.Start(tempPath)
'                Else
'                    MessageBox.Show("Este es un registro físico histórico. No tiene archivo digital adjunto.", "Sin Adjunto", MessageBoxButtons.OK, MessageBoxIcon.Information)
'                End If
'            End Using
'        Catch ex As Exception
'            MessageBox.Show("No se pudo abrir el archivo: " & ex.Message)
'        End Try
'    End Sub

'    ' =========================================================
'    ' EDICIÓN E IMPRESIÓN
'    ' =========================================================
'    Private Sub EditarMovimiento()
'        If dgvMovimientos.SelectedRows.Count = 0 Then
'            MessageBox.Show("Seleccione una línea del historial abajo.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information)
'            Return
'        End If
'        Dim idMov As Integer = Convert.ToInt32(dgvMovimientos.SelectedRows(0).Cells("IdMov").Value)
'        Dim idDoc As Integer = Convert.ToInt32(dgvMesa.SelectedRows(0).Cells("Id").Value)
'        Dim frm As New frmNuevoPase(idDoc, idMov)
'        If frm.ShowDialog() = DialogResult.OK Then
'            CargarMesa()
'            CargarHistorial(idDoc)
'        End If
'    End Sub

'    Private Sub btnCorregirMov_Click(sender As Object, e As EventArgs) Handles btnCorregirMov.Click
'        EditarMovimiento()
'    End Sub
'    Private Sub dgvMovimientos_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvMovimientos.CellDoubleClick
'        If e.RowIndex >= 0 Then EditarMovimiento()
'    End Sub

'    Private Sub btnImprimirRecibo_Click(sender As Object, e As EventArgs) Handles btnImprimirRecibo.Click
'        If dgvMesa.SelectedRows.Count = 0 Then
'            MessageBox.Show("Seleccione un documento.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information)
'            Return
'        End If
'        Dim row = dgvMesa.SelectedRows(0)
'        _impReferencia = row.Cells("Referencia").Value.ToString()
'        _impAsunto = row.Cells("Asunto").Value.ToString()
'        If dgvMovimientos.Rows.Count > 0 Then
'            Dim rowMov As DataGridViewRow = If(dgvMovimientos.SelectedRows.Count > 0, dgvMovimientos.SelectedRows(0), dgvMovimientos.Rows(dgvMovimientos.Rows.Count - 1))
'            _impFecha = rowMov.Cells("Fecha").Value.ToString()
'            _impOrigen = rowMov.Cells("Origen").Value.ToString()
'            _impDestino = rowMov.Cells("Destino").Value.ToString()
'        Else
'            _impFecha = DateTime.Now.ToString()
'            _impOrigen = "SIN DATOS"
'            _impDestino = "SIN DATOS"
'        End If
'        If PrintDialog1.ShowDialog() = DialogResult.OK Then PrintDocument1.Print()
'    End Sub

'    Private Sub PrintDocument1_PrintPage(sender As Object, e As PrintPageEventArgs) Handles PrintDocument1.PrintPage
'        Dim g As Graphics = e.Graphics
'        Dim fuenteTitulo As New Font("Arial", 16, FontStyle.Bold)
'        Dim fuenteSub As New Font("Arial", 12, FontStyle.Bold)
'        Dim fuenteNormal As New Font("Arial", 10, FontStyle.Regular)
'        Dim fuenteChica As New Font("Arial", 8, FontStyle.Regular)
'        Dim pincel As New SolidBrush(Color.Black)
'        Dim margenIzq As Integer = 50
'        Dim y As Integer = 50

'        g.DrawString("UNIDAD N° 4 - SANTIAGO VÁZQUEZ", fuenteTitulo, pincel, margenIzq, y)
'        y += 30
'        g.DrawString("CONSTANCIA DE PASE / MOVIMIENTO", fuenteSub, pincel, margenIzq, y)
'        y += 10
'        g.DrawLine(Pens.Black, margenIzq, y + 20, 750, y + 20)
'        y += 40
'        g.DrawString("DOCUMENTO: " & _impReferencia, fuenteSub, pincel, margenIzq, y)
'        y += 25
'        g.DrawString("ASUNTO: ", fuenteNormal, pincel, margenIzq, y)
'        Dim rectAsunto As New RectangleF(margenIzq + 70, y, 600, 60)
'        g.DrawString(_impAsunto, fuenteNormal, pincel, rectAsunto)
'        y += 70
'        g.DrawLine(Pens.Gray, margenIzq, y, 750, y)
'        y += 20
'        g.DrawString("FECHA MOVIMIENTO: " & _impFecha, fuenteNormal, pincel, margenIzq, y)
'        y += 30
'        g.DrawString("ORIGEN: " & _impOrigen, fuenteNormal, pincel, margenIzq, y)
'        y += 30
'        g.DrawString("DESTINO: " & _impDestino, fuenteSub, pincel, margenIzq, y)
'        y += 50
'        Dim rectFirma As New Rectangle(margenIzq, y, 700, 150)
'        g.DrawRectangle(Pens.Black, rectFirma)
'        g.DrawString("RECIBIDO POR:", fuenteNormal, pincel, margenIzq + 10, y + 10)
'        g.DrawString("FIRMA Y ACLARACIÓN:", fuenteNormal, pincel, margenIzq + 10, y + 100)
'        g.DrawString("FECHA: ______ / ______ / ___________", fuenteNormal, pincel, margenIzq + 400, y + 100)
'        y += 170
'        g.DrawString("Emitido por Sistema POLO el " & DateTime.Now.ToString(), fuenteChica, Brushes.Gray, margenIzq, y)
'        g.DrawString("Operador: " & _impUsuario, fuenteChica, Brushes.Gray, margenIzq, y + 15)
'        e.HasMorePages = False
'    End Sub

'    Private Sub dgvMesa_CellToolTipTextNeeded(sender As Object, e As DataGridViewCellToolTipTextNeededEventArgs) Handles dgvMesa.CellToolTipTextNeeded
'        If e.RowIndex >= 0 Then
'            Dim colName As String = dgvMesa.Columns(e.ColumnIndex).Name
'            If colName = "Referencia" Or colName = "Asunto" Then
'                Dim idDoc As Integer = Convert.ToInt32(dgvMesa.Rows(e.RowIndex).Cells("Id").Value)
'                e.ToolTipText = GenerarTimelineSimple(idDoc)
'            End If
'        End If
'    End Sub

'    Private Function GenerarTimelineSimple(idDoc As Integer) As String
'        Dim sb As New System.Text.StringBuilder()
'        Try
'            Using db As New PoloNuevoEntities()
'                Dim movimientos = db.MovimientosDocumentos.Where(Function(m) m.DocumentoId = idDoc).OrderBy(Function(m) m.FechaMovimiento).ToList()
'                If movimientos.Count = 0 Then Return "Sin movimientos."
'                sb.AppendLine("📋 HISTORIAL RÁPIDO")
'                sb.AppendLine(New String("-"c, 30))
'                For i As Integer = 0 To movimientos.Count - 1
'                    Dim m = movimientos(i)
'                    Dim icono As String = If(m.EsSalida, "📤", "📥")
'                    sb.AppendLine($"{icono} {m.FechaMovimiento:dd/MM HH:mm} | {If(m.EsSalida, m.Destino, m.Origen)}")
'                    If i < movimientos.Count - 1 Then sb.AppendLine("      ⬇")
'                Next
'            End Using
'        Catch ex As Exception
'            Return "..."
'        End Try
'        Return sb.ToString()
'    End Function

'    ' =========================================================
'    ' VISUALIZADOR DE HISTORIAL 360° (CORREGIDO EL ERROR)
'    ' =========================================================
'    Private Sub btnVerHistorial_Click(sender As Object, e As EventArgs) Handles btnVerHistorial.Click
'        ' 1. Validamos selección
'        If dgvMesa.SelectedRows.Count = 0 Then
'            MessageBox.Show("Por favor, seleccione un documento primero.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information)
'            Return
'        End If

'        Dim idDocInicial As Integer = Convert.ToInt32(dgvMesa.SelectedRows(0).Cells("Id").Value)
'        Dim refInicial As String = dgvMesa.SelectedRows(0).Cells("Referencia").Value.ToString()

'        ' 2. Formulario visual
'        Dim fHistorial As New Form() With {
'            .Text = "Historia Clínica del Documento: " & refInicial,
'            .Size = New Size(900, 500),
'            .StartPosition = FormStartPosition.CenterScreen,
'            .BackColor = Color.White
'        }

'        Dim dgvH As New DataGridView() With {
'            .Dock = DockStyle.Fill,
'            .ReadOnly = True,
'            .RowHeadersVisible = False,
'            .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
'            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
'            .BackgroundColor = Color.WhiteSmoke,
'            .BorderStyle = BorderStyle.None,
'            .CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
'            .AllowUserToAddRows = False
'        }
'        dgvH.EnableHeadersVisualStyles = False
'        dgvH.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue
'        dgvH.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
'        dgvH.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 10, FontStyle.Bold)
'        dgvH.DefaultCellStyle.Font = New Font("Segoe UI", 9)
'        fHistorial.Controls.Add(dgvH)

'        ' 3. Lógica de Datos
'        Try
'            Using db As New PoloNuevoEntities()
'                ' A. ENCONTRAR LA FAMILIA
'                Dim familiaIds As New List(Of Integer) From {idDocInicial}
'                Dim nuevosEncontrados As Boolean = True
'                While nuevosEncontrados
'                    nuevosEncontrados = False
'                    Dim listaActual = familiaIds.ToList()
'                    Dim vinculos = db.DocumentoVinculos _
'                                     .Where(Function(v) listaActual.Contains(v.IdDocumentoPadre) Or listaActual.Contains(v.IdDocumentoHijo)) _
'                                     .ToList()
'                    For Each v In vinculos
'                        If Not familiaIds.Contains(v.IdDocumentoPadre) Then
'                            familiaIds.Add(v.IdDocumentoPadre)
'                            nuevosEncontrados = True
'                        End If
'                        If Not familiaIds.Contains(v.IdDocumentoHijo) Then
'                            familiaIds.Add(v.IdDocumentoHijo)
'                            nuevosEncontrados = True
'                        End If
'                    Next
'                End While

'                ' B. TRAER MOVIMIENTOS
'                Dim listaMovimientos = db.MovimientosDocumentos _
'                                         .Include("Documentos").Include("Documentos.TiposDocumento") _
'                                         .Where(Function(m) familiaIds.Contains(m.DocumentoId)) _
'                                         .OrderBy(Function(m) m.FechaMovimiento) _
'                                         .ToList() _
'                                         .Select(Function(m)
'                                                     Dim accionHumana As String = "Trámite"
'                                                     If m.Origen = "SISTEMA" Then
'                                                         accionHumana = "✨ GENERACIÓN INICIAL"
'                                                     ElseIf Not String.IsNullOrEmpty(m.Observaciones) AndAlso (m.Observaciones.Contains("VINCULADO") Or m.Observaciones.Contains("ADJUNTO")) Then
'                                                         accionHumana = "🔗 VINCULACIÓN"
'                                                     ElseIf m.EsSalida Then
'                                                         accionHumana = "📤 PASE / ENVÍO"
'                                                     Else
'                                                         accionHumana = "📥 RECEPCIÓN"
'                                                     End If
'                                                     Dim docNombre As String = m.Documentos.TiposDocumento.Nombre & " " & m.Documentos.ReferenciaExterna
'                                                     If m.DocumentoId = idDocInicial Then docNombre = "➤ ESTE DOCUMENTO"
'                                                     Return New With {
'                                                         .Fecha = m.FechaMovimiento.ToString("dd/MM HH:mm"),
'                                                         .Documento = docNombre,
'                                                         .Acción = accionHumana,
'                                                         .Origen = m.Origen,
'                                                         .Destino = m.Destino,
'                                                         .Nota = m.Observaciones
'                                                     }
'                                                 End Function).ToList()

'                ' =========================================================================
'                ' CORRECCIÓN DEL ERROR DE REFERENCIA NULA
'                ' =========================================================================
'                ' Validamos que haya datos antes de asignarlos y formatear columnas
'                If listaMovimientos.Count > 0 Then
'                    dgvH.DataSource = listaMovimientos

'                    ' Solo tocamos las columnas si existen
'                    If dgvH.Columns.Contains("Fecha") Then dgvH.Columns("Fecha").Width = 110
'                    If dgvH.Columns.Contains("Documento") Then dgvH.Columns("Documento").Width = 150
'                    If dgvH.Columns.Contains("Acción") Then dgvH.Columns("Acción").Width = 140
'                    If dgvH.Columns.Contains("Nota") Then dgvH.Columns("Nota").Visible = False

'                    fHistorial.ShowDialog()
'                Else
'                    MessageBox.Show("Este documento no tiene movimientos registrados para mostrar en el historial.", "Historial Vacío", MessageBoxButtons.OK, MessageBoxIcon.Information)
'                End If
'            End Using
'        Catch ex As Exception
'            MessageBox.Show("Error al generar historial: " & ex.Message)
'        End Try
'    End Sub

'End Class