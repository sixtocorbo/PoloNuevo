Imports System.Linq

Public Class frmMenuPrincipal

    ' =========================================================================
    ' EVENTO LOAD: AL INICIAR
    ' =========================================================================
    Private Sub frmMenuPrincipal_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Mostrar quién está conectado
        lblUsuarioConectado.Text = "Usuario: " & Sesion.UsuarioActual.Nombre & " (" & Sesion.UsuarioActual.Rol & ")"

        ' BLOQUEO DE SEGURIDAD
        If Not Sesion.UsuarioActual.EsAdmin Then
            ' Si NO es admin, deshabilitamos o ocultamos el botón de crear usuarios
            btnGestionUsuarios.Enabled = False
            btnGestionUsuarios.Visible = False

            ' Opcional: Bloquear otras cosas críticas como "Eliminar Documento" si quisieras
        End If
        ' Abrimos el Dashboard al iniciar, maximizado dentro del contenedor
        Dim dash As New frmDashboard()
        dash.MdiParent = Me
        dash.WindowState = FormWindowState.Maximized
        dash.Show()
    End Sub
    ' Botón para abrir el ABM de usuarios
    Private Sub btnGestionUsuarios_Click(sender As Object, e As EventArgs) Handles btnGestionUsuarios.Click
        Dim frm As New frmUsuarios()
        frm.ShowDialog()
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


    ' 3. MESA DE ENTRADA (F2)
    Private Sub MesaDeEntradaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MesaDeEntradaToolStripMenuItem.Click
        AbrirFormulario(Of frmMesaEntrada)()
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
    Private Sub btnVerLogs_Click(sender As Object, e As EventArgs) Handles btnVerLogs.Click
        ' Opcional: Solo permitir ver esto a los Admins
        If Not Sesion.UsuarioActual.EsAdmin Then
            MessageBox.Show("Solo administradores pueden ver el log técnico.", "Acceso Restringido", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim frm As New frmLogSistema()
        frm.ShowDialog()
    End Sub
End Class