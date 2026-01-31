Imports System.Data.Entity ' Necesario para funciones de fecha si usas EF

Public Class frmDashboard

    Private Sub frmDashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ConfigurarTitulos() ' Cambiamos los textos de los labels viejos
        CargarIndicadores()
        CargarUltimosMovimientos()
    End Sub

    ' Método para refrescar si se hace clic en el fondo o se vuelve al form
    Private Sub frmDashboard_Activated(sender As Object, e As EventArgs) Handles Me.Activated
        CargarIndicadores()
        CargarUltimosMovimientos()
    End Sub

    Private Sub ConfigurarTitulos()
        ' Reutilizamos los controles existentes cambiándoles el propósito visualmente
        lblTitPendientes.Text = "EN MESA DE ENTRADA"

        lblTitPoblacion.Text = "INGRESADOS HOY"     ' Antes Población
        pnlPoblacion.BackColor = Color.SteelBlue    ' Mantenemos Azul o cambiamos a gusto

        lblTitLaboral.Text = "SALIDAS / PASES HOY"  ' Antes Laboral
        pnlLaboral.BackColor = Color.SeaGreen       ' Mantenemos Verde
    End Sub

    Private Sub CargarIndicadores()
        Try
            Using db As New PoloNuevoEntities()
                Dim hoy As Date = DateTime.Today
                Dim manana As Date = hoy.AddDays(1)

                ' ---------------------------------------------------------
                ' 1. DOCUMENTOS ACTUALMENTE EN MESA (PENDIENTES DE ACCIÓN)
                ' ---------------------------------------------------------
                ' Lógica: Buscamos documentos cuyo ÚLTIMO movimiento tenga destino "MESA DE ENTRADA"
                Dim pendientes = db.Documentos _
                                   .Where(Function(d) d.MovimientosDocumentos _
                                            .OrderByDescending(Function(m) m.FechaMovimiento) _
                                            .FirstOrDefault().Destino = "MESA DE ENTRADA") _
                                   .Count()

                lblNumPendientes.Text = pendientes.ToString()

                ' Alerta visual: Si hay muchos papeles acumulados, se pone rojo intenso
                If pendientes > 15 Then
                    pnlPendientes.BackColor = Color.Firebrick
                Else
                    pnlPendientes.BackColor = Color.IndianRed
                End If

                ' ---------------------------------------------------------
                ' 2. INGRESOS DEL DÍA (Métrica de Productividad)
                ' ---------------------------------------------------------
                ' Usamos el panel de "Población" para esto
                Dim ingresosHoy = db.Documentos _
                                    .Where(Function(d) d.FechaCarga >= hoy And d.FechaCarga < manana) _
                                    .Count()

                lblNumPoblacion.Text = ingresosHoy.ToString()

                ' ---------------------------------------------------------
                ' 3. SALIDAS / PASES DEL DÍA (Métrica de Gestión)
                ' ---------------------------------------------------------
                ' Usamos el panel de "Laboral" para esto. Contamos movimientos de salida de hoy.
                Dim salidasHoy = db.MovimientosDocumentos _
                                   .Where(Function(m) m.EsSalida = True And
                                                      m.FechaMovimiento >= hoy And
                                                      m.FechaMovimiento < manana) _
                                   .Count()

                lblNumLaboral.Text = salidasHoy.ToString()

            End Using
        Catch ex As Exception
            lblNumPendientes.Text = "-"
            lblNumPoblacion.Text = "-"
            lblNumLaboral.Text = "-"
        End Try
    End Sub

    Private Sub CargarUltimosMovimientos()
        Try
            Using db As New PoloNuevoEntities()
                ' Traemos los últimos 15 movimientos registrados para tener panorama completo
                Dim lista = db.MovimientosDocumentos _
                              .Include("Documentos").Include("Documentos.TiposDocumento") _
                              .OrderByDescending(Function(m) m.FechaMovimiento) _
                              .Take(15) _
                              .ToList() _
                              .Select(Function(m) New With {
                                  .Fecha = m.FechaMovimiento.ToString("dd/MM HH:mm"),
                                  .Referencia = m.Documentos.TiposDocumento.Nombre & " " & m.Documentos.ReferenciaExterna,
                                  .Acción = If(m.EsSalida, "📤 SALIDA", "📥 ENTRADA"),
                                  .Detalle = If(m.EsSalida, "Hacia: " & m.Destino, "Desde: " & m.Origen),
                                  .Obs = m.Observaciones
                              }) _
                              .ToList()

                dgvUltimos.DataSource = lista

                ' Ajustes visuales de la grilla
                ConfigurarGrilla()
            End Using
        Catch ex As Exception
            ' Ignorar error en diseño o carga inicial
        End Try
    End Sub

    Private Sub ConfigurarGrilla()
        If dgvUltimos.Columns.Count > 0 Then
            dgvUltimos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            dgvUltimos.Columns("Fecha").Width = 100
            dgvUltimos.Columns("Fecha").HeaderText = "Hora"
            dgvUltimos.Columns("Referencia").Width = 150
            dgvUltimos.Columns("Acción").Width = 80
            dgvUltimos.Columns("Detalle").Width = 200

            ' Ocultamos o ajustamos observaciones si ocupan mucho
            dgvUltimos.Columns("Obs").HeaderText = "Observaciones"
        End If
    End Sub

End Class