Public Class frmDashboard

    Private Sub frmDashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CargarIndicadores()
        CargarUltimosMovimientos()
    End Sub

    ' Método para refrescar si se hace clic en el fondo
    Private Sub frmDashboard_Activated(sender As Object, e As EventArgs) Handles Me.Activated
        CargarIndicadores()
        CargarUltimosMovimientos()
    End Sub

    Private Sub CargarIndicadores()
        Using db As New PoloNuevoEntities()
            ' 1. DOCUMENTOS PENDIENTES
            ' (Contamos documentos físicos que tienen solo 1 movimiento, o sea Entrada sin Salida)
            Dim pendientes = db.Documentos _
                               .Where(Function(d) d.Extension = ".phy" And d.MovimientosDocumentos.Count() = 1) _
                               .Count()
            lblNumPendientes.Text = pendientes.ToString()

            ' Cambiamos color si hay muchos pendientes
            If pendientes > 10 Then
                pnlPendientes.BackColor = Color.Red
            Else
                pnlPendientes.BackColor = Color.IndianRed
            End If

            ' 2. POBLACIÓN (Si tienes la tabla Estado, filtra por Estado=Activo)
            ' Asumimos todos los registros por ahora
            Dim poblacion = db.Reclusos.Count()
            lblNumPoblacion.Text = poblacion.ToString()

            ' 3. TRABAJADORES (Comisiones Activas)
            Dim trabajando = db.Comisiones _
                               .Where(Function(c) c.Activa = True) _
                               .Count()
            lblNumLaboral.Text = trabajando.ToString()
        End Using
    End Sub

    Private Sub CargarUltimosMovimientos()
        Using db As New PoloNuevoEntities()
            ' Traemos los últimos 10 movimientos registrados en general
            Dim lista = db.MovimientosDocumentos _
                          .OrderByDescending(Function(m) m.FechaMovimiento) _
                          .Take(10) _
                          .Select(Function(m) New With {
                              .Fecha = m.FechaMovimiento,
                              .Documento = m.Documentos.TiposDocumento.Nombre & " " & m.Documentos.ReferenciaExterna,
                              .Acción = If(m.EsSalida, "SALIDA", "ENTRADA"),
                              .Detalle = If(m.EsSalida, "Hacia: " & m.Destino, "Desde: " & m.Origen)
                          }) _
                          .ToList()

            dgvUltimos.DataSource = lista
            dgvUltimos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        End Using
    End Sub

End Class