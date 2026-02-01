Imports System.Data.Entity
Imports System.Linq

Public Class frmDashboard

    Private Sub frmDashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ConfigurarTitulos()
        CargarIndicadores()
        CargarUltimosMovimientos()
    End Sub

    Private Sub frmDashboard_Activated(sender As Object, e As EventArgs) Handles Me.Activated
        ' Recargar al volver al dashboard para ver cambios recientes
        CargarIndicadores()
        CargarUltimosMovimientos()
    End Sub

    Private Sub btnActualizar_Click(sender As Object, e As EventArgs) Handles btnActualizar.Click
        CargarIndicadores()
        CargarUltimosMovimientos()
    End Sub

    Private Sub ConfigurarTitulos()
        ' Panel Rojo
        lblTitPendientes.Text = "PENDIENTES EN MESA"

        ' Panel Azul (Antes Población, ahora Ingresos del día)
        lblTitActivos.Text = "INGRESADOS HOY"
        pnlPoblacion.BackColor = Color.SteelBlue

        ' Panel Verde (Antes Laboral, ahora Salidas)
        lblTitSalidas.Text = "SALIDAS / PASES HOY"
        pnlLaboral.BackColor = Color.SeaGreen
    End Sub

    Private Sub CargarIndicadores()
        Try
            Me.Cursor = Cursors.WaitCursor
            Using db As New PoloNuevoEntities()
                Dim hoy As Date = DateTime.Today
                Dim manana As Date = hoy.AddDays(1)

                ' =========================================================
                ' 1. LÓGICA DE PENDIENTES (Documentos físicamente en Mesa)
                ' =========================================================
                ' Traemos todos los documentos activos con sus movimientos
                Dim listaDocs = db.Documentos _
                                  .Where(Function(d) d.TiposDocumento.Nombre <> "ARCHIVO") _
                                  .Include("MovimientosDocumentos") _
                                  .ToList()

                Dim conteoPendientes As Integer = 0

                For Each doc In listaDocs
                    ' Buscamos el último movimiento de cada documento
                    Dim ultimoMov = doc.MovimientosDocumentos _
                                       .OrderByDescending(Function(m) m.FechaMovimiento) _
                                       .ThenByDescending(Function(m) m.Id) _
                                       .FirstOrDefault()

                    Dim ubicacionActual As String = "MESA DE ENTRADA"

                    ' Si tiene movimientos, tomamos el destino del último
                    If ultimoMov IsNot Nothing AndAlso Not String.IsNullOrEmpty(ultimoMov.Destino) Then
                        ubicacionActual = ultimoMov.Destino.Trim().ToUpper()
                    End If

                    ' Si la ubicación actual es MESA DE ENTRADA, suma al contador
                    If ubicacionActual = "MESA DE ENTRADA" Then
                        conteoPendientes += 1
                    End If
                Next

                lblNumPendientes.Text = conteoPendientes.ToString()

                ' Alerta visual si se acumulan muchos papeles
                If conteoPendientes > 15 Then
                    pnlPendientes.BackColor = Color.Firebrick ' Rojo oscuro alarma
                Else
                    pnlPendientes.BackColor = Color.IndianRed ' Rojo suave normal
                End If

                ' =========================================================
                ' 2. INGRESOS DEL DÍA (Panel Azul)
                ' =========================================================
                Dim ingresosHoy = db.Documentos _
                                    .Where(Function(d) d.FechaCarga >= hoy And d.FechaCarga < manana) _
                                    .Count()

                ' Usamos lblNumActivos porque así se llama en el Designer que pasaste
                lblNumActivos.Text = ingresosHoy.ToString()

                ' =========================================================
                ' 3. SALIDAS / PASES DEL DÍA (Panel Verde)
                ' =========================================================
                Dim salidasHoy = db.MovimientosDocumentos _
                                   .Where(Function(m) m.EsSalida = True And
                                                      m.FechaMovimiento >= hoy And
                                                      m.FechaMovimiento < manana) _
                                   .Count()

                ' Usamos lblNumSalidas porque así se llama en el Designer que pasaste
                lblNumSalidas.Text = salidasHoy.ToString()

            End Using
        Catch ex As Exception
            ' En caso de error (ej: base de datos desconectada), mostramos guiones
            lblNumPendientes.Text = "-"
            lblNumActivos.Text = "-"
            lblNumSalidas.Text = "-"
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub CargarUltimosMovimientos()
        Try
            Using db As New PoloNuevoEntities()
                Dim lista = db.MovimientosDocumentos _
                              .Include("Documentos").Include("Documentos.TiposDocumento") _
                              .OrderByDescending(Function(m) m.FechaMovimiento) _
                              .Take(15) _
                              .ToList() _
                              .Select(Function(m) New With {
                                  .Fecha = m.FechaMovimiento.ToString("HH:mm"),
                                  .Referencia = m.Documentos.TiposDocumento.Nombre & " " & m.Documentos.ReferenciaExterna,
                                  .Accion = If(m.Origen = "SISTEMA" Or m.Origen = "CARGA MANUAL", "✨ NUEVO", If(m.EsSalida, "📤 SALIDA", "📥 ENTRADA")),
                                  .Detalle = If(m.EsSalida, "-> " & m.Destino, "<- " & m.Origen),
                                  .Obs = m.Observaciones
                              }) _
                              .ToList()

                dgvUltimos.DataSource = lista
                ConfigurarGrilla()
            End Using
        Catch ex As Exception
            ' Fallo silencioso en la grilla para no interrumpir el dashboard
        End Try
    End Sub

    Private Sub ConfigurarGrilla()
        If dgvUltimos.Columns.Count = 0 Then Return
        With dgvUltimos
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            .RowHeadersVisible = False

            If .Columns("Fecha") IsNot Nothing Then
                .Columns("Fecha").Width = 60
                .Columns("Fecha").HeaderText = "Hora"
                .Columns("Fecha").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            End If
            If .Columns("Referencia") IsNot Nothing Then
                .Columns("Referencia").Width = 140
                .Columns("Referencia").HeaderText = "Documento"
            End If
            If .Columns("Accion") IsNot Nothing Then
                .Columns("Accion").Width = 90
                .Columns("Accion").HeaderText = "Acción"
                .Columns("Accion").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            End If
            If .Columns("Detalle") IsNot Nothing Then
                .Columns("Detalle").Width = 180
                .Columns("Detalle").HeaderText = "Movimiento"
            End If
            If .Columns("Obs") IsNot Nothing Then
                .Columns("Obs").HeaderText = "Observaciones"
            End If
        End With
    End Sub

End Class