Imports System.Linq

Public Class frmMenuPrincipal

    ' =========================================================================
    ' EVENTO LOAD: AL INICIAR
    ' =========================================================================
    Private Sub frmMenuPrincipal_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Abrimos el Dashboard al iniciar, maximizado dentro del contenedor
        Dim dash As New frmDashboard()
        dash.MdiParent = Me
        dash.WindowState = FormWindowState.Maximized
        dash.Show()
    End Sub

    ' =========================================================================
    ' MENU ARCHIVO
    ' =========================================================================
    Private Sub SalirToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SalirToolStripMenuItem.Click
        Application.Exit()
    End Sub

    ' =========================================================================
    ' MENU GESTIÓN
    ' =========================================================================

    ' 1. DASHBOARD (F12)
    Private Sub DashboardToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DashboardToolStripMenuItem.Click
        AbrirFormulario(Of frmDashboard)()
    End Sub

    ' 2. LISTADO DE RECLUSOS (F1)
    Private Sub ReclusosToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ReclusosToolStripMenuItem.Click
        AbrirFormulario(Of frmReclusos)()
    End Sub

    ' 3. MESA DE ENTRADA (F2)
    Private Sub MesaDeEntradaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MesaDeEntradaToolStripMenuItem.Click
        AbrirFormulario(Of frmMesaEntrada)()
    End Sub

    ' 4. COMISIONES LABORALES (F3)
    Private Sub ComisionesLaboralesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ComisionesLaboralesToolStripMenuItem.Click
        AbrirFormulario(Of frmComisiones)()
    End Sub

    ' 5. REPORTES DOCUMENTALES (F4)
    Private Sub ReportesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ReportesToolStripMenuItem.Click
        AbrirFormulario(Of frmReportesDocumentos)()
    End Sub

    ' 6. AUDITORÍA DE ELIMINACIONES
    Private Sub AuditoriaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AuditoriaToolStripMenuItem.Click
        AbrirFormulario(Of frmAuditoria)()
    End Sub

    ' =========================================================================
    ' MENU CONFIGURACIÓN (NUEVO)
    ' =========================================================================
    Private Sub RangosToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RangosToolStripMenuItem.Click
        AbrirFormulario(Of frmGestionRangos)()
    End Sub

    ' =========================================================================
    ' MENU VENTANAS
    ' =========================================================================
    Private Sub CerrarTodasToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CerrarTodasToolStripMenuItem.Click
        For Each child As Form In Me.MdiChildren
            child.Close()
        Next
    End Sub

    ' =========================================================================
    ' MÉTODO AUXILIAR GENÉRICO PARA GESTIONAR VENTANAS MDI
    ' Evita abrir la misma ventana dos veces. Si ya está abierta, la trae al frente.
    ' =========================================================================
    Private Sub AbrirFormulario(Of T As {Form, New})()
        Dim formulario = Me.MdiChildren.OfType(Of T)().FirstOrDefault()

        If formulario Is Nothing Then
            ' Si no existe, la creamos nueva
            formulario = New T()
            formulario.MdiParent = Me
            formulario.Show()
        Else
            ' Si existe pero está minimizada, la restauramos
            If formulario.WindowState = FormWindowState.Minimized Then
                formulario.WindowState = FormWindowState.Normal
            End If
            ' La traemos al frente
            formulario.BringToFront()
            ' Importante: Dispara el evento Activated (útil para que el Dashboard se refresque)
            formulario.Activate()
        End If
    End Sub

End Class