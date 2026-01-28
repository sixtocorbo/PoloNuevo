Imports System.Data.Entity.SqlServer
Imports System.IO
Imports System.Text

Public Class frmReportesDocumentos

    Private Sub frmReportesDocumentos_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cmbTipoReporte.SelectedIndex = 0 ' Seleccionar el primero por defecto
        dtpDesde.Value = DateTime.Now.AddDays(-30) ' Último mes por defecto
    End Sub

    Private Sub btnGenerar_Click(sender As Object, e As EventArgs) Handles btnGenerar.Click
        Dim fDesde = dtpDesde.Value.Date
        Dim fHasta = dtpHasta.Value.Date.AddDays(1).AddSeconds(-1)

        Select Case cmbTipoReporte.SelectedIndex
            Case 0 : ReporteEntradas(fDesde, fHasta)
            Case 1 : ReporteSalidas(fDesde, fHasta)
            Case 2 : ReportePendientes()
            Case 3 : ReporteEstadisticas(fDesde, fHasta)
        End Select
    End Sub

    ' 1. LIBRO DE ENTRADAS (Todo lo que se cargó)
    Private Sub ReporteEntradas(desde As DateTime, hasta As DateTime)
        Using db As New PoloNuevoEntities()
            Dim lista = db.Documentos _
                          .Where(Function(d) d.Extension = ".phy" And d.FechaCarga >= desde And d.FechaCarga <= hasta) _
                          .OrderBy(Function(d) d.FechaCarga) _
                          .Select(Function(d) New With {
                              .ID = d.Id,
                              .Fecha = d.FechaCarga,
                              .Tipo = d.TiposDocumento.Nombre,
                              .Numero = d.ReferenciaExterna,
                              .Asunto = d.Descripcion,
                              .Origen = d.MovimientosDocumentos.FirstOrDefault().Origen
                          }) _
                          .ToList()
            dgvResultados.DataSource = lista
        End Using
        ConfigurarColumnas("Asunto")
    End Sub

    ' 2. LIBRO DE SALIDAS (Usamos la columna OBSERVACIONES recuperada)
    Private Sub ReporteSalidas(desde As DateTime, hasta As DateTime)
        Using db As New PoloNuevoEntities()
            ' Buscamos movimientos que sean SALIDA (EsSalida = True)
            Dim lista = db.MovimientosDocumentos _
                          .Where(Function(m) m.EsSalida = True And m.FechaMovimiento >= desde And m.FechaMovimiento <= hasta) _
                          .OrderBy(Function(m) m.FechaMovimiento) _
                          .Select(Function(m) New With {
                              .Fecha = m.FechaMovimiento,
                              .Doc_Original = m.Documentos.TiposDocumento.Nombre & " " & m.Documentos.ReferenciaExterna,
                              .Destino = m.Destino,
                              .Salio_Con = If(m.Observaciones Is Nothing, "S/D", m.Observaciones) ' <--- DATO RECUPERADO
                          }) _
                          .ToList()
            dgvResultados.DataSource = lista
        End Using
        ConfigurarColumnas("Destino")
    End Sub

    ' 3. PENDIENTES (Entró y no salió)
    Private Sub ReportePendientes()
        Using db As New PoloNuevoEntities()
            ' Pendiente = Solo tiene 1 movimiento (la entrada)
            Dim lista = db.Documentos _
                          .Where(Function(d) d.Extension = ".phy" And d.MovimientosDocumentos.Count() = 1) _
                          .OrderBy(Function(d) d.FechaCarga) _
                          .Select(Function(d) New With {
                              .Dias_Espera = SqlFunctions.DateDiff("day", d.FechaCarga, DateTime.Now),
                              .Fecha = d.FechaCarga,
                              .Tipo = d.TiposDocumento.Nombre & " " & d.ReferenciaExterna,
                              .Asunto = d.Descripcion,
                              .Origen = d.MovimientosDocumentos.FirstOrDefault().Origen
                          }) _
                          .ToList()
            dgvResultados.DataSource = lista
        End Using
        ConfigurarColumnas("Asunto")
    End Sub

    ' 4. ESTADÍSTICAS (Conteo rápido)
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

    Private Sub ConfigurarColumnas(columnaAncha As String)
        dgvResultados.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        For Each col As DataGridViewColumn In dgvResultados.Columns
            If col.Name = columnaAncha Then
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            Else
                col.Width = 120
            End If
        Next
    End Sub

    ' EXPORTAR A EXCEL (CSV)
    Private Sub btnExportar_Click(sender As Object, e As EventArgs) Handles btnExportar.Click
        If dgvResultados.Rows.Count = 0 Then Return

        Using sfd As New SaveFileDialog()
            sfd.Filter = "Archivo CSV|*.csv"
            sfd.FileName = "Reporte_" & DateTime.Now.ToString("yyyyMMdd") & ".csv"
            If sfd.ShowDialog() = DialogResult.OK Then
                Try
                    Dim sb As New StringBuilder()

                    ' Encabezados
                    Dim columnas As New List(Of String)
                    For Each col As DataGridViewColumn In dgvResultados.Columns
                        columnas.Add(col.Name)
                    Next
                    sb.AppendLine(String.Join(";", columnas))

                    ' Filas
                    For Each row As DataGridViewRow In dgvResultados.Rows
                        Dim celdas As New List(Of String)
                        For Each cell As DataGridViewCell In row.Cells
                            Dim valor = If(cell.Value Is Nothing, "", cell.Value.ToString().Replace(";", ",").Replace(vbCr, " ").Replace(vbLf, ""))
                            celdas.Add(valor)
                        Next
                        sb.AppendLine(String.Join(";", celdas))
                    Next

                    File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.Default)
                    MessageBox.Show("Exportado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Catch ex As Exception
                    MessageBox.Show("Error al exportar: " & ex.Message)
                End Try
            End If
        End Using
    End Sub

End Class