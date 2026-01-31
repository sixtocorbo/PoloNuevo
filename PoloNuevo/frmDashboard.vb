Imports System.Data.Entity
Imports System.Linq ' Necesario para las consultas avanzadas

Public Class frmDashboard

    Private Sub frmDashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ConfigurarTitulos()
        CargarIndicadores()
        CargarUltimosMovimientos()
    End Sub

    Private Sub frmDashboard_Activated(sender As Object, e As EventArgs) Handles Me.Activated
        CargarIndicadores()
        CargarUltimosMovimientos()
    End Sub

    Private Sub ConfigurarTitulos()
        lblTitPendientes.Text = "EN MESA DE ENTRADA"
        lblTitPoblacion.Text = "INGRESADOS HOY"
        pnlPoblacion.BackColor = Color.SteelBlue

        lblTitLaboral.Text = "SALIDAS / PASES HOY"
        pnlLaboral.BackColor = Color.SeaGreen
    End Sub

    Private Sub CargarIndicadores()
        Try
            Using db As New PoloNuevoEntities()
                Dim hoy As Date = DateTime.Today
                Dim manana As Date = hoy.AddDays(1)

                ' =========================================================
                ' 1. LÓGICA DE PENDIENTES (CORREGIDA PARA VINCULACIONES)
                ' =========================================================
                ' Traemos todos los documentos activos y sus movimientos
                Dim listaDocs = db.Documentos _
                                  .Where(Function(d) d.TiposDocumento.Nombre <> "ARCHIVO") _
                                  .Include("MovimientosDocumentos") _
                                  .ToList()

                Dim conteoPendientes As Integer = 0

                For Each doc In listaDocs
                    ' Obtenemos el ÚLTIMO movimiento registrado (el más reciente)
                    Dim ultimoMov = doc.MovimientosDocumentos _
                                       .OrderByDescending(Function(m) m.FechaMovimiento) _
                                       .FirstOrDefault()

                    If ultimoMov IsNot Nothing Then
                        ' Preparamos los textos para evitar errores de nulos
                        Dim destino As String = If(ultimoMov.Destino, "").Trim().ToUpper()
                        Dim obs As String = If(ultimoMov.Observaciones, "").Trim().ToUpper()

                        ' CONDICIÓN 1: Físicamente enviado a Mesa
                        Dim estaEnMesa As Boolean = (destino = "MESA DE ENTRADA")

                        ' CONDICIÓN 2: Es una acción administrativa hecha en Mesa (Vinculación/Adjunto)
                        ' Si lo último que se hizo fue vincularlo, significa que el papel lo tengo yo.
                        Dim esVinculacion As Boolean = (obs.Contains("VINCULADO") Or obs.Contains("SE VINCULÓ") Or obs.Contains("ADJUNTO"))

                        ' Si cumple CUALQUIERA de las dos, está pendiente
                        If estaEnMesa Or esVinculacion Then
                            conteoPendientes += 1
                        End If
                    End If
                Next

                lblNumPendientes.Text = conteoPendientes.ToString()

                ' Alerta visual
                If conteoPendientes > 15 Then
                    pnlPendientes.BackColor = Color.Firebrick
                Else
                    pnlPendientes.BackColor = Color.IndianRed
                End If

                ' =========================================================
                ' 2. INGRESOS DEL DÍA
                ' =========================================================
                Dim ingresosHoy = db.Documentos _
                                    .Where(Function(d) d.FechaCarga >= hoy And d.FechaCarga < manana) _
                                    .Count()
                lblNumPoblacion.Text = ingresosHoy.ToString()

                ' =========================================================
                ' 3. SALIDAS / PASES DEL DÍA
                ' =========================================================
                Dim salidasHoy = db.MovimientosDocumentos _
                                   .Where(Function(m) m.EsSalida = True And
                                                      m.FechaMovimiento >= hoy And
                                                      m.FechaMovimiento < manana) _
                                   .Count()
                lblNumLaboral.Text = salidasHoy.ToString()

            End Using
        Catch ex As Exception
            ' En caso de error, mostramos guiones
            lblNumPendientes.Text = "-"
            lblNumPoblacion.Text = "-"
            lblNumLaboral.Text = "-"
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
                                  .Accion = If(m.Origen = "SISTEMA", "✨ NUEVO", If(m.EsSalida, "📤 SALIDA", "📥 ENTRADA")),
                                  .Detalle = If(m.EsSalida, "-> " & m.Destino, "<- " & m.Origen),
                                  .Obs = m.Observaciones
                              }) _
                              .ToList()

                dgvUltimos.DataSource = lista
                ConfigurarGrilla()
            End Using
        Catch ex As Exception
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