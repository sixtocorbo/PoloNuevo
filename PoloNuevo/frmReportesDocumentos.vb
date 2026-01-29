Imports System.Data.Entity
Imports System.Data.Entity.SqlServer
Imports System.IO
Imports System.Text

Public Class frmReportesDocumentos

    ' =========================================================
    ' CARGA INICIAL
    ' =========================================================
    Private Sub frmReportesDocumentos_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If cmbTipoReporte.Items.Count = 0 Then
            cmbTipoReporte.Items.AddRange({"Libro de Entradas", "Libro de Salidas", "Pendientes / En Cartera", "Estadísticas"})
        End If

        cmbTipoReporte.SelectedIndex = 0
        dtpDesde.Value = DateTime.Now.AddDays(-30)
    End Sub

    ' =========================================================
    ' BOTÓN GENERAR
    ' =========================================================
    Private Sub btnGenerar_Click(sender As Object, e As EventArgs) Handles btnGenerar.Click
        Dim fDesde = dtpDesde.Value.Date
        Dim fHasta = dtpHasta.Value.Date.AddDays(1).AddSeconds(-1) ' Todo el día final

        dgvResultados.DataSource = Nothing

        Try
            Select Case cmbTipoReporte.SelectedIndex
                Case 0 : ReporteEntradas(fDesde, fHasta)
                Case 1 : ReporteSalidas(fDesde, fHasta)
                Case 2 : ReportePendientes(fDesde, fHasta)
                Case 3 : ReporteEstadisticas(fDesde, fHasta)
            End Select

            If dgvResultados.Rows.Count = 0 AndAlso dgvResultados.DataSource IsNot Nothing Then
                MessageBox.Show("No se encontraron registros en el período seleccionado.", "Sin Resultados", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

        Catch ex As Exception
            MessageBox.Show("Error al generar reporte: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' =========================================================
    ' 1. LIBRO DE ENTRADAS (CORREGIDO: ToString fuera de LINQ)
    ' =========================================================
    Private Sub ReporteEntradas(desde As DateTime, hasta As DateTime)
        Using db As New PoloNuevoEntities()
            ' ETAPA 1: Traer datos crudos de la BD (SQL)
            ' NOTA: No usamos ToString() aquí adentro para evitar el error de Entity Framework
            Dim datosCrudos = db.Documentos _
                          .Where(Function(d) d.Extension = ".phy" And d.FechaCarga >= desde And d.FechaCarga <= hasta) _
                          .OrderBy(Function(d) d.FechaCarga) _
                          .Select(Function(d) New With {
                              .ID = d.Id,
                              .FechaRaw = d.FechaCarga,
                              .Tipo = d.TiposDocumento.Nombre,
                              .Numero = d.ReferenciaExterna,
                              .Asunto = d.Descripcion,
                              .Origen = If(d.MovimientosDocumentos.OrderBy(Function(m) m.FechaMovimiento).FirstOrDefault() IsNot Nothing,
                                           d.MovimientosDocumentos.OrderBy(Function(m) m.FechaMovimiento).FirstOrDefault().Origen,
                                           "Inicio (S/D)")
                          }) _
                          .ToList() ' <--- Aquí se ejecuta el SQL y se traen los datos a memoria

            ' ETAPA 2: Formatear en memoria (VB.NET)
            Dim listaFinal = datosCrudos.Select(Function(x) New With {
                              .Fecha = If(x.FechaRaw.HasValue, x.FechaRaw.Value.ToString("dd/MM/yyyy HH:mm"), ""),
                              .Tipo = x.Tipo,
                              .Numero = x.Numero,
                              .Asunto = x.Asunto,
                              .Origen = x.Origen
                          }).ToList()

            dgvResultados.DataSource = listaFinal
        End Using
        ConfigurarColumnas("Asunto")
    End Sub

    ' =========================================================
    ' 2. LIBRO DE SALIDAS
    ' =========================================================
    Private Sub ReporteSalidas(desde As DateTime, hasta As DateTime)
        Using db As New PoloNuevoEntities()
            ' ETAPA 1: SQL
            Dim datosCrudos = db.MovimientosDocumentos _
                          .Where(Function(m) m.EsSalida = True _
                                             And m.Documentos.Extension = ".phy" _
                                             And m.FechaMovimiento >= desde And m.FechaMovimiento <= hasta) _
                          .OrderBy(Function(m) m.FechaMovimiento) _
                          .Select(Function(m) New With {
                              .FechaRaw = m.FechaMovimiento,
                              .Tipo = m.Documentos.TiposDocumento.Nombre,
                              .Numero = m.Documentos.ReferenciaExterna,
                              .Destino = m.Destino,
                              .Observaciones = m.Observaciones
                          }) _
                          .ToList()

            ' ETAPA 2: Memoria
            Dim listaFinal = datosCrudos.Select(Function(x) New With {
                              .Fecha_Salida = x.FechaRaw.ToString("dd/MM/yyyy HH:mm"),
                              .Tipo = x.Tipo,
                              .Numero = x.Numero,
                              .Destino = x.Destino,
                              .Detalle_Salida = If(x.Observaciones Is Nothing, "", x.Observaciones)
                          }).ToList()

            dgvResultados.DataSource = listaFinal
        End Using
        ConfigurarColumnas("Detalle_Salida")
    End Sub

    ' =========================================================
    ' 3. PENDIENTES (En Cartera)
    ' =========================================================
    Private Sub ReportePendientes(desde As DateTime, hasta As DateTime)
        Using db As New PoloNuevoEntities()
            ' ETAPA 1: Traer documentos candidatos
            Dim queryDocs = db.Documentos _
                              .Where(Function(d) d.Extension = ".phy" And d.FechaCarga >= desde And d.FechaCarga <= hasta) _
                              .Include("MovimientosDocumentos") _
                              .Include("TiposDocumento") _
                              .ToList()

            ' ETAPA 2: Lógica compleja en memoria (Aquí no falla LINQ)
            Dim pendientes = queryDocs.Select(Function(d) New With {
                                    .Doc = d,
                                    .UltimoMov = d.MovimientosDocumentos.OrderByDescending(Function(m) m.FechaMovimiento).FirstOrDefault()
                                 }) _
                                 .Where(Function(x) x.UltimoMov Is Nothing OrElse x.UltimoMov.EsSalida = False) _
                                 .Select(Function(x) New With {
                                    .Dias_Espera = (DateTime.Now - x.Doc.FechaCarga.GetValueOrDefault(DateTime.Now)).Days,
                                    .Fecha_Ingreso = x.Doc.FechaCarga.GetValueOrDefault(DateTime.Now).ToString("dd/MM/yyyy"),
                                    .Ubicacion_Actual = If(x.UltimoMov Is Nothing, "Mesa de Entrada (Nuevo)", x.UltimoMov.Destino),
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
    ' 4. ESTADÍSTICAS
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
    ' EXPORTAR A CSV
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
                            Dim valor As String = If(cell.Value Is Nothing, "", cell.Value.ToString())
                            valor = valor.Replace(";", ",").Replace(vbCr, " ").Replace(vbLf, " ")
                            celdas.Add(valor)
                        Next
                        sb.AppendLine(String.Join(";", celdas))
                    Next

                    File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8)
                    MessageBox.Show("Exportado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Catch ex As Exception
                    MessageBox.Show("Error al exportar: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        End Using
    End Sub

End Class