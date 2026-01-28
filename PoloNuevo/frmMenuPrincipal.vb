Public Class frmMenuPrincipal

    ' 1. SALIR DEL SISTEMA
    Private Sub SalirToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SalirToolStripMenuItem.Click
        Application.Exit()
    End Sub

    ' 2. ABRIR LISTADO DE RECLUSOS (F1)
    Private Sub ReclusosToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ReclusosToolStripMenuItem.Click
        AbrirFormulario(Of frmReclusos)()
    End Sub

    ' 3. ABRIR MESA DE ENTRADA (F2)
    Private Sub MesaDeEntradaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MesaDeEntradaToolStripMenuItem.Click
        AbrirFormulario(Of frmMesaEntrada)()
    End Sub

    ' 4. ABRIR COMISIONES LABORALES (F3)
    Private Sub ComisionesLaboralesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ComisionesLaboralesToolStripMenuItem.Click
        AbrirFormulario(Of frmComisiones)()
    End Sub

    ' 5. ABRIR REPORTES (F4) <--- NUEVO
    Private Sub ReportesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ReportesToolStripMenuItem.Click
        AbrirFormulario(Of frmReportesDocumentos)()
    End Sub

    ' 6. CERRAR TODAS LAS VENTANAS
    Private Sub CerrarTodasToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CerrarTodasToolStripMenuItem.Click
        For Each child As Form In Me.MdiChildren
            child.Close()
        Next
    End Sub

    ' --- MÉTODO GENÉRICO PARA GESTIONAR VENTANAS MDI ---
    Private Sub AbrirFormulario(Of T As {Form, New})()
        Dim formulario = Me.MdiChildren.OfType(Of T)().FirstOrDefault()

        If formulario Is Nothing Then
            formulario = New T()
            formulario.MdiParent = Me
            formulario.Show()
        Else
            If formulario.WindowState = FormWindowState.Minimized Then
                formulario.WindowState = FormWindowState.Normal
            End If
            formulario.BringToFront()
        End If
    End Sub

    Private Sub frmMenuPrincipal_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Abrimos el Dashboard al iniciar, maximizado dentro del contenedor
        Dim dash As New frmDashboard()
        dash.MdiParent = Me
        dash.WindowState = FormWindowState.Maximized
        dash.Show()
    End Sub
End Class