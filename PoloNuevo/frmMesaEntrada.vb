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

    ' =========================================================
    ' CARGA INICIAL
    ' =========================================================
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
                ' Traemos TODO lo que NO sea una foto de perfil ("ARCHIVO")
                ' Esto incluye los registros físicos (.phy) y los nuevos digitales (.pdf, .jpg)
                Dim query = db.Documentos.Where(Function(d) d.TiposDocumento.Nombre <> "ARCHIVO")

                ' 2. Filtro Pendientes (Documentos con Entrada pero sin Salida)
                If chkPendientes.Checked Then
                    query = query.Where(Function(d) d.MovimientosDocumentos.Count() = 1)
                End If

                ' 3. Buscador Inteligente
                Dim texto = txtBuscar.Text.Trim().ToLower()
                If Not String.IsNullOrEmpty(texto) Then
                    Dim palabras As String() = texto.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)
                    For Each palabra In palabras
                        Dim termino = palabra
                        ' Busca en Referencia, Tipo, Descripción (Asunto) y Nombre del Recluso
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
                                     .Digital = If(d.Extension <> ".phy", "SI", "NO") ' Para saber si tiene adjunto
                                 }) _
                                 .Take(200) _
                                 .ToList()

                ' 6. Asignación a la Grilla
                dgvMesa.DataSource = lista

                ' 7. Configuración Visual
                If dgvMesa.Columns("Id") IsNot Nothing Then dgvMesa.Columns("Id").Visible = False
                ConfigurarColumnas()
                ColorearPendientes()

                ' Limpiar detalles si la lista quedó vacía
                If lista.Count = 0 Then dgvMovimientos.DataSource = Nothing
            End Using
        Catch ex As Exception
            MessageBox.Show("Error al cargar datos: " & ex.Message)
        End Try
    End Sub

    ' =========================================================
    ' DISEÑO DE COLUMNAS
    ' =========================================================
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

        For Each row As DataGridViewRow In dgvMesa.Rows
            ' PENDIENTES EN ROJO
            If row.Cells("Estado").Value.ToString() = "PENDIENTE" Then
                row.DefaultCellStyle.ForeColor = Color.DarkRed
                row.DefaultCellStyle.BackColor = Color.MistyRose
            End If
            ' DIGITALES EN AZULITO (Opcional, para destacar)
            If dgvMesa.Columns("Digital") IsNot Nothing AndAlso row.Cells("Digital").Value.ToString() = "SI" Then
                row.Cells("Digital").Style.ForeColor = Color.Blue
                row.Cells("Digital").Style.Font = New Font("Segoe UI", 8, FontStyle.Bold)
            End If
        Next
    End Sub

    ' =========================================================
    ' EVENTOS DE FILTROS
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
                                  .FechaMovimiento = m.FechaMovimiento,
                                  .Origen = m.Origen,
                                  .MovidoA = m.Destino,
                                  .Tipo = If(m.EsSalida,
                                             "SALIDA (" & If(m.Observaciones Is Nothing, "S/D", m.Observaciones) & ")",
                                             "ENTRADA / PASE")
                              }) _
                              .ToList()

            dgvMovimientos.DataSource = historial

            If dgvMovimientos.Columns("IdMov") IsNot Nothing Then dgvMovimientos.Columns("IdMov").Visible = False
            If dgvMovimientos.Columns("FechaMovimiento") IsNot Nothing Then
                dgvMovimientos.Columns("FechaMovimiento").HeaderText = "Fecha mov."
                dgvMovimientos.Columns("FechaMovimiento").Width = 120
                dgvMovimientos.Columns("FechaMovimiento").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            End If
            If dgvMovimientos.Columns("MovidoA") IsNot Nothing Then
                dgvMovimientos.Columns("MovidoA").HeaderText = "Movido a"
                dgvMovimientos.Columns("MovidoA").Width = 180
            End If
            If dgvMovimientos.Columns("Tipo") IsNot Nothing Then dgvMovimientos.Columns("Tipo").Width = 300
        End Using
    End Sub

    ' =========================================================
    ' BOTONES DE ACCIÓN (NUEVO, PASE, EDITAR)
    ' =========================================================
    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        Dim frm As New frmNuevoIngreso()
        If frm.ShowDialog() = DialogResult.OK Then CargarMesa()
    End Sub

    Private Sub btnPase_Click(sender As Object, e As EventArgs) Handles btnPase.Click
        If dgvMesa.SelectedRows.Count = 0 Then
            MessageBox.Show("Seleccione un documento.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If
        Dim idDoc As Integer = Convert.ToInt32(dgvMesa.SelectedRows(0).Cells("Id").Value)

        Dim frm As New frmNuevoPase(idDoc)
        If frm.ShowDialog() = DialogResult.OK Then
            CargarMesa()
            CargarHistorial(idDoc)
        End If
    End Sub

    ' =========================================================
    ' BOTÓN EDITAR / VER DETALLE
    ' =========================================================
    Private Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        ' 1. Validar que haya algo seleccionado
        If dgvMesa.SelectedRows.Count = 0 Then
            MessageBox.Show("Seleccione un documento de la lista para ver su detalle o editar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        ' 2. Obtener el ID oculto de la grilla
        Dim idDoc As Integer = Convert.ToInt32(dgvMesa.SelectedRows(0).Cells("Id").Value)

        ' 3. Abrir el formulario en MODO EDICIÓN (Usando el constructor con ID)
        ' Al pasarle idDoc, el formulario sabe que debe buscar los datos, el archivo adjunto y el recluso vinculado.
        Dim frm As New frmNuevoIngreso(idDoc)

        If frm.ShowDialog() = DialogResult.OK Then
            ' Si el usuario guardó cambios, refrescamos todo
            CargarMesa()
            CargarHistorial(idDoc)
        End If
    End Sub

    ' =========================================================
    ' DIGITALIZACIÓN (VER DOCUMENTO)
    ' =========================================================
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

                    ' 1. Crear ruta temporal segura
                    Dim extension As String = If(String.IsNullOrEmpty(doc.Extension), ".dat", doc.Extension)
                    Dim tempPath As String = Path.Combine(Path.GetTempPath(), "Doc_" & doc.Id & extension)

                    ' 2. Escribir el archivo
                    File.WriteAllBytes(tempPath, doc.Contenido)

                    ' 3. Abrir con programa predeterminado
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
    ' CORRECCIÓN DE MOVIMIENTOS
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

    ' =========================================================
    ' IMPRESIÓN (RECIBO DE PASE)
    ' =========================================================
    Private Sub btnImprimirRecibo_Click(sender As Object, e As EventArgs) Handles btnImprimirRecibo.Click
        If dgvMesa.SelectedRows.Count = 0 Then
            MessageBox.Show("Seleccione un documento.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        ' Capturar Datos Doc
        Dim row = dgvMesa.SelectedRows(0)
        _impReferencia = row.Cells("Referencia").Value.ToString()
        _impAsunto = row.Cells("Asunto").Value.ToString()

        ' Capturar Datos Movimiento (Seleccionado o Último)
        If dgvMovimientos.Rows.Count > 0 Then
            Dim rowMov As DataGridViewRow
            If dgvMovimientos.SelectedRows.Count > 0 Then
                rowMov = dgvMovimientos.SelectedRows(0)
            Else
                rowMov = dgvMovimientos.Rows(dgvMovimientos.Rows.Count - 1)
            End If
            _impFecha = rowMov.Cells("FechaMovimiento").Value.ToString()
            _impOrigen = rowMov.Cells("Origen").Value.ToString()
            _impDestino = rowMov.Cells("MovidoA").Value.ToString()
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

        ' ENCABEZADO
        g.DrawString("UNIDAD N° 4 - SANTIAGO VÁZQUEZ", fuenteTitulo, pincel, margenIzq, y)
        y += 30
        g.DrawString("CONSTANCIA DE PASE / MOVIMIENTO", fuenteSub, pincel, margenIzq, y)
        y += 10
        g.DrawLine(Pens.Black, margenIzq, y + 20, 750, y + 20)
        y += 40

        ' DATOS DOC
        g.DrawString("DOCUMENTO: " & _impReferencia, fuenteSub, pincel, margenIzq, y)
        y += 25
        g.DrawString("ASUNTO: ", fuenteNormal, pincel, margenIzq, y)

        Dim rectAsunto As New RectangleF(margenIzq + 70, y, 600, 60)
        g.DrawString(_impAsunto, fuenteNormal, pincel, rectAsunto)
        y += 70

        g.DrawLine(Pens.Gray, margenIzq, y, 750, y)
        y += 20

        ' DATOS MOV
        g.DrawString("FECHA MOVIMIENTO: " & _impFecha, fuenteNormal, pincel, margenIzq, y)
        y += 30
        g.DrawString("ORIGEN: " & _impOrigen, fuenteNormal, pincel, margenIzq, y)
        y += 30
        g.DrawString("DESTINO: " & _impDestino, fuenteSub, pincel, margenIzq, y)
        y += 50

        ' FIRMA
        Dim rectFirma As New Rectangle(margenIzq, y, 700, 150)
        g.DrawRectangle(Pens.Black, rectFirma)
        g.DrawString("RECIBIDO POR:", fuenteNormal, pincel, margenIzq + 10, y + 10)
        g.DrawString("FIRMA Y ACLARACIÓN:", fuenteNormal, pincel, margenIzq + 10, y + 100)
        g.DrawString("FECHA: ______ / ______ / ___________", fuenteNormal, pincel, margenIzq + 400, y + 100)

        ' PIE
        y += 170
        g.DrawString("Emitido por Sistema POLO el " & DateTime.Now.ToString(), fuenteChica, Brushes.Gray, margenIzq, y)
        g.DrawString("Operador: " & _impUsuario, fuenteChica, Brushes.Gray, margenIzq, y + 15)

        e.HasMorePages = False
    End Sub

End Class
