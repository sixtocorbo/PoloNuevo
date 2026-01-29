Imports System.Data.Entity
Imports System.Data.Entity.SqlServer
Imports System.IO
Imports System.Text

Public Class frmReportesDocumentos

    ' =========================================================
    ' CARGA INICIAL
    ' =========================================================
    Private Sub frmReportesDocumentos_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Llenar el combo si no está lleno por diseño
        If cmbTipoReporte.Items.Count = 0 Then
            cmbTipoReporte.Items.AddRange({"Libro de Entradas", "Libro de Salidas", "Pendientes / En Cartera", "Estadísticas"})
        End If

        cmbTipoReporte.SelectedIndex = 0 ' Seleccionar el primero por defecto
        dtpDesde.Value = DateTime.Now.AddDays(-30) ' Último mes por defecto
    End Sub

    ' =========================================================
    ' BOTÓN GENERAR
    ' =========================================================
    Private Sub btnGenerar_Click(sender As Object, e As EventArgs) Handles btnGenerar.Click
        Dim fDesde = dtpDesde.Value.Date
        Dim fHasta = dtpHasta.Value.Date.AddDays(1).AddSeconds(-1) ' Hasta el final del día

        ' Limpiamos la grilla antes de cargar
        dgvResultados.DataSource = Nothing

        Try
            Select Case cmbTipoReporte.SelectedIndex
                Case 0 : ReporteEntradas(fDesde, fHasta)
                Case 1 : ReporteSalidas(fDesde, fHasta)
                Case 2 : ReportePendientes()
                Case 3 : ReporteEstadisticas(fDesde, fHasta)
            End Select
        Catch ex As Exception
            MessageBox.Show("Error al generar reporte: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' =========================================================
    ' 1. LIBRO DE ENTRADAS (Todo lo que ingresó físicamente)
    ' =========================================================
    Private Sub ReporteEntradas(desde As DateTime, hasta As DateTime)
        Using db As New PoloNuevoEntities()
            Dim lista = db.Documentos _
                          .Where(Function(d) d.Extension = ".phy" And d.FechaCarga >= desde And d.FechaCarga <= hasta) _
                          .OrderBy(Function(d) d.FechaCarga) _
                          .ToList() _
                          .Select(Function(d) New With {
                              .ID = d.Id,
                              .Fecha = d.FechaCarga.ToString("dd/MM/yyyy HH:mm"),
                              .Tipo = d.TiposDocumento.Nombre,
                              .Numero = d.ReferenciaExterna,
                              .Asunto = d.Descripcion,
                              .Origen = If(d.MovimientosDocumentos.FirstOrDefault() IsNot Nothing, d.MovimientosDocumentos.FirstOrDefault().Origen, "S/D")
                          }) _
                          .ToList()

            dgvResultados.DataSource = lista
        End Using
        ConfigurarColumnas("Asunto")
    End Sub

    ' =========================================================
    ' 2. LIBRO DE SALIDAS (Movimientos con EsSalida = True)
    ' =========================================================
    Private Sub ReporteSalidas(desde As DateTime, hasta As DateTime)
        Using db As New PoloNuevoEntities()
            Dim lista = db.MovimientosDocumentos _
                          .Where(Function(m) m.EsSalida = True And m.FechaMovimiento >= desde And m.FechaMovimiento <= hasta) _
                          .OrderBy(Function(m) m.FechaMovimiento) _
                          .ToList() _
                          .Select(Function(m) New With {
                              .Fecha_Salida = m.FechaMovimiento.ToString("dd/MM/yyyy HH:mm"),
                              .Tipo = m.Documentos.TiposDocumento.Nombre,
                              .Numero = m.Documentos.ReferenciaExterna,
                              .Destino = m.Destino,
                              .Detalle_Salida = If(m.Observaciones Is Nothing, "", m.Observaciones)
                          }) _
                          .ToList()

            dgvResultados.DataSource = lista
        End Using
        ConfigurarColumnas("Detalle_Salida")
    End Sub


    ' =========================================================
    ' 3. PENDIENTES (CORREGIDO: Error de TimeSpan?)
    ' =========================================================
    Private Sub ReportePendientes()
        Using db As New PoloNuevoEntities()
            ' PASO 1: Traemos los documentos a memoria
            Dim docs = db.Documentos _
                         .Where(Function(d) d.Extension = ".phy") _
                         .Include("MovimientosDocumentos") _
                         .Include("TiposDocumento") _
                         .ToList()

            ' PASO 2: Filtramos y Proyectamos
            Dim pendientes = docs.Select(Function(d) New With {
                                    .Doc = d,
                                    .UltimoMov = d.MovimientosDocumentos.OrderByDescending(Function(m) m.FechaMovimiento).FirstOrDefault()
                                 }) _
                                 .Where(Function(x) x.UltimoMov IsNot Nothing AndAlso x.UltimoMov.EsSalida = False) _
                                 .Select(Function(x) New With {
                                    .Dias_Espera = (DateTime.Now - x.Doc.FechaCarga.GetValueOrDefault(DateTime.Now)).Days,
                                    .Fecha_Ingreso = x.Doc.FechaCarga.GetValueOrDefault(DateTime.Now).ToString("dd/MM/yyyy"),
                                    .Ubicacion_Actual = x.UltimoMov.Destino,
                                    .Tipo = x.Doc.TiposDocumento.Nombre,
                                    .Numero = x.Doc.ReferenciaExterna,
                                    .Asunto = x.Doc.Descripcion
                                 }) _
                                 .OrderByDescending(Function(x) x.Dias_Espera) _
                                 .ToList()

            dgvResultados.DataSource = pendientes
        End Using
        ConfigurarColumnas("Asunto")
    End Sub

    ' =========================================================
    ' 4. ESTADÍSTICAS (Conteo por Tipo)
    ' =========================================================
    Private Sub ReporteEstadisticas(desde As DateTime, hasta As DateTime)
        Using db As New PoloNuevoEntities()
            Dim lista = db.Documentos _
                          .Where(Function(d) d.Extension = ".phy" And d.FechaCarga >= desde And d.FechaCarga <= hasta) _
                          .GroupBy(Function(d) d.TiposDocumento.Nombre) _
                          .Select(Function(g) New With {
                              .Tipo_Documento = g.Key,
                              .Cantidad = g.Count()
                          }) _
                          .OrderByDescending(Function(x) x.Cantidad) _
                          .ToList()

            dgvResultados.DataSource = lista
        End Using
        dgvResultados.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    End Sub

    ' =========================================================
    ' MÉTODOS AUXILIARES
    ' =========================================================
    Private Sub ConfigurarColumnas(columnaAncha As String)
        If dgvResultados.Columns.Count = 0 Then Return

        dgvResultados.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        For Each col As DataGridViewColumn In dgvResultados.Columns
            If col.Name = columnaAncha Then
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            Else
                col.Width = 130
            End If
        Next
    End Sub

    ' =========================================================
    ' EXPORTAR A EXCEL (CSV)
    ' =========================================================
    Private Sub btnExportar_Click(sender As Object, e As EventArgs) Handles btnExportar.Click
        If dgvResultados.Rows.Count = 0 Then
            MessageBox.Show("No hay datos para exportar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Using sfd As New SaveFileDialog()
            sfd.Filter = "Archivo CSV (Excel)|*.csv"
            sfd.FileName = "Reporte_" & DateTime.Now.ToString("yyyyMMdd_HHmm") & ".csv"

            If sfd.ShowDialog() = DialogResult.OK Then
                Try
                    Dim sb As New StringBuilder()

                    ' 1. Encabezados
                    Dim columnas As New List(Of String)
                    For Each col As DataGridViewColumn In dgvResultados.Columns
                        columnas.Add(col.Name)
                    Next
                    sb.AppendLine(String.Join(";", columnas))

                    ' 2. Filas de Datos
                    For Each row As DataGridViewRow In dgvResultados.Rows
                        Dim celdas As New List(Of String)
                        For Each cell As DataGridViewCell In row.Cells
                            Dim valor As String = If(cell.Value Is Nothing, "", cell.Value.ToString())
                            ' Limpieza crítica para no romper el CSV
                            valor = valor.Replace(";", ",").Replace(vbCr, " ").Replace(vbLf, " ")
                            celdas.Add(valor)
                        Next
                        sb.AppendLine(String.Join(";", celdas))
                    Next

                    ' 3. Guardar con Encoding UTF8 para tildes
                    File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8)

                    MessageBox.Show("Exportado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Catch ex As Exception
                    MessageBox.Show("Error al exportar: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        End Using
    End Sub

End Class