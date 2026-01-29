Imports System.IO
Imports System.Drawing.Printing

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

                ' 1. CONSULTA BASE
                Dim query = db.Documentos.Where(Function(d) d.TiposDocumento.Nombre <> "ARCHIVO")

                ' 2. Filtro Pendientes
                If chkPendientes.Checked Then
                    query = query.Where(Function(d) d.MovimientosDocumentos.Count() = 1)
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
                Dim lista = query.OrderByDescending(Function(d) d.FechaCarga) _
                                 .Select(Function(d) New With {
                                     .Id = d.Id,
                                     .Fecha = d.FechaCarga,
                                     .Referencia = d.TiposDocumento.Nombre & " " & d.ReferenciaExterna,
                                     .Asunto = d.Descripcion,
                                     .Recluso = If(d.Reclusos IsNot Nothing, d.Reclusos.Nombre, "Sin Vincular"),
                                     .Estado = If(d.MovimientosDocumentos.Count() = 1, "PENDIENTE", "MOVIDO"),
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
    ' BOTONES DE ACCIÓN (LO QUE PEDISTE)
    ' =========================================================

    ' 1. NUEVO INGRESO (Lo de ingreso)
    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        Dim frm As New frmNuevoIngreso()
        If frm.ShowDialog() = DialogResult.OK Then CargarMesa()
    End Sub

    ' 2. REGISTRAR PASE (Lo de pases)
    Private Sub btnPase_Click(sender As Object, e As EventArgs) Handles btnPase.Click
        If dgvMesa.SelectedRows.Count = 0 Then
            MessageBox.Show("Seleccione un documento.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If
        Dim idDoc As Integer = Convert.ToInt32(dgvMesa.SelectedRows(0).Cells("Id").Value)

        ' Abre frmNuevoPase (que tiene la lógica de arrastre de adjuntos)
        Dim frm As New frmNuevoPase(idDoc)
        If frm.ShowDialog() = DialogResult.OK Then
            CargarMesa()
            CargarHistorial(idDoc)
        End If
    End Sub

    ' 3. ACTUAR / RESPONDER (Lo de actuar)
    Private Sub btnActuar_Click(sender As Object, e As EventArgs) Handles btnActuar.Click
        If dgvMesa.SelectedRows.Count = 0 Then
            MessageBox.Show("Seleccione un documento para responder o actuar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If
        Dim idDoc As Integer = Convert.ToInt32(dgvMesa.SelectedRows(0).Cells("Id").Value)

        ' Abre frmGenerarDocumento (que crea un documento nuevo vinculado al padre)
        Dim frm As New frmGenerarDocumento(idDoc)
        If frm.ShowDialog() = DialogResult.OK Then
            CargarMesa()
            CargarHistorial(idDoc)
        End If
    End Sub

    ' 4. EDITAR / DETALLE
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

    ' 5. ELIMINAR
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

                        For Each mov In doc.MovimientosDocumentos
                            Dim audMov As New AuditoriaMovimientos()
                            audMov.AuditoriaDocId = auditoria.Id
                            audMov.FechaMovimientoOriginal = mov.FechaMovimiento
                            audMov.Origen = mov.Origen
                            audMov.Destino = mov.Destino
                            audMov.Observaciones = mov.Observaciones
                            audMov.TipoMovimiento = If(mov.EsSalida, "SALIDA", "ENTRADA")
                            db.AuditoriaMovimientos.Add(audMov)
                        Next

                        db.MovimientosDocumentos.RemoveRange(doc.MovimientosDocumentos)
                        db.Documentos.Remove(doc)
                        db.SaveChanges()

                        MessageBox.Show("Eliminación completa. El historial ha sido resguardado.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        CargarMesa()
                        dgvMovimientos.DataSource = Nothing
                    End If
                End Using
            Catch ex As Exception
                MessageBox.Show("Error crítico: " & ex.Message)
            End Try
        End If
    End Sub

    ' =========================================================
    ' EVENTOS DE INTERFAZ AUXILIARES
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
            Dim historial = db.MovimientosDocumentos _
                              .Where(Function(m) m.DocumentoId = idDoc) _
                              .OrderBy(Function(m) m.FechaMovimiento) _
                              .Select(Function(m) New With {
                                  .IdMov = m.Id,
                                  .Origen = m.Origen,
                                  .Tipo = If(m.EsSalida,
                                             "SALIDA (" & If(m.Observaciones Is Nothing, "S/D", m.Observaciones) & ")",
                                             "ENTRADA / PASE"),
                                  .Destino = If(m.Destino Is Nothing OrElse m.Destino = "", "SIN DESTINO", m.Destino),
                                  .Fecha = m.FechaMovimiento
                              }) _
                              .ToList()

            dgvMovimientos.DataSource = historial

            If dgvMovimientos.Columns("IdMov") IsNot Nothing Then dgvMovimientos.Columns("IdMov").Visible = False
            If dgvMovimientos.Columns("Tipo") IsNot Nothing Then dgvMovimientos.Columns("Tipo").Width = 300
            If dgvMovimientos.Columns("Destino") IsNot Nothing Then
                dgvMovimientos.Columns("Destino").HeaderText = "Destino"
                dgvMovimientos.Columns("Destino").Width = 180
            End If
            If dgvMovimientos.Columns("Fecha") IsNot Nothing Then
                dgvMovimientos.Columns("Fecha").HeaderText = "Fecha mov."
                dgvMovimientos.Columns("Fecha").Width = 120
                dgvMovimientos.Columns("Fecha").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            End If
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
            Dim rowMov As DataGridViewRow
            If dgvMovimientos.SelectedRows.Count > 0 Then
                rowMov = dgvMovimientos.SelectedRows(0)
            Else
                rowMov = dgvMovimientos.Rows(dgvMovimientos.Rows.Count - 1)
            End If
            _impFecha = rowMov.Cells("Fecha").Value.ToString()
            _impOrigen = rowMov.Cells("Origen").Value.ToString()
            _impDestino = rowMov.Cells("Destino").Value.ToString()
        Else
            _impFecha = DateTime.Now.ToString()
            _impOrigen = "SIN DATOS"
            _impDestino = "SIN DATOS"
        End If
        If PrintDialog1.ShowDialog() = DialogResult.OK Then
            PrintDocument1.Print()
        End If
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

End Class