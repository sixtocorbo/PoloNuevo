<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMenuPrincipal
    Inherits System.Windows.Forms.Form

    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.ArchivoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SalirToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GestiónToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DashboardToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ReclusosToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MesaDeEntradaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ComisionesLaboralesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ReportesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AuditoriaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        ' --- NUEVO MENU CONFIGURACION ---
        Me.ConfiguracionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RangosToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        ' --------------------------------
        Me.VentanasToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CerrarTodasToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.lblEstado = New System.Windows.Forms.ToolStripStatusLabel()
        Me.MenuStrip1.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.GripMargin = New System.Windows.Forms.Padding(2, 2, 0, 2)
        Me.MenuStrip1.ImageScalingSize = New System.Drawing.Size(24, 24)
        ' Se agrega ConfiguracionToolStripMenuItem a la colección
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ArchivoToolStripMenuItem, Me.GestiónToolStripMenuItem, Me.ConfiguracionToolStripMenuItem, Me.VentanasToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.MdiWindowListItem = Me.VentanasToolStripMenuItem
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1512, 35)
        Me.MenuStrip1.TabIndex = 1
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'ArchivoToolStripMenuItem
        '
        Me.ArchivoToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SalirToolStripMenuItem})
        Me.ArchivoToolStripMenuItem.Name = "ArchivoToolStripMenuItem"
        Me.ArchivoToolStripMenuItem.Size = New System.Drawing.Size(88, 29)
        Me.ArchivoToolStripMenuItem.Text = "Archivo"
        '
        'SalirToolStripMenuItem
        '
        Me.SalirToolStripMenuItem.Name = "SalirToolStripMenuItem"
        Me.SalirToolStripMenuItem.Size = New System.Drawing.Size(147, 34)
        Me.SalirToolStripMenuItem.Text = "Salir"
        '
        'GestiónToolStripMenuItem
        '
        Me.GestiónToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DashboardToolStripMenuItem, Me.ReclusosToolStripMenuItem, Me.MesaDeEntradaToolStripMenuItem, Me.ComisionesLaboralesToolStripMenuItem, Me.ReportesToolStripMenuItem, Me.AuditoriaToolStripMenuItem})
        Me.GestiónToolStripMenuItem.Name = "GestiónToolStripMenuItem"
        Me.GestiónToolStripMenuItem.Size = New System.Drawing.Size(88, 29)
        Me.GestiónToolStripMenuItem.Text = "Gestión"
        '
        'DashboardToolStripMenuItem
        '
        Me.DashboardToolStripMenuItem.Name = "DashboardToolStripMenuItem"
        Me.DashboardToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.DashboardToolStripMenuItem.Size = New System.Drawing.Size(395, 34)
        Me.DashboardToolStripMenuItem.Text = "Panel de Control (Dashboard)"
        '
        'ReclusosToolStripMenuItem
        '
        Me.ReclusosToolStripMenuItem.Name = "ReclusosToolStripMenuItem"
        Me.ReclusosToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.ReclusosToolStripMenuItem.Size = New System.Drawing.Size(395, 34)
        Me.ReclusosToolStripMenuItem.Text = "Listado de Reclusos"
        '
        'MesaDeEntradaToolStripMenuItem
        '
        Me.MesaDeEntradaToolStripMenuItem.Name = "MesaDeEntradaToolStripMenuItem"
        Me.MesaDeEntradaToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2
        Me.MesaDeEntradaToolStripMenuItem.Size = New System.Drawing.Size(395, 34)
        Me.MesaDeEntradaToolStripMenuItem.Text = "Mesa de Entrada"
        '
        'ComisionesLaboralesToolStripMenuItem
        '
        Me.ComisionesLaboralesToolStripMenuItem.Name = "ComisionesLaboralesToolStripMenuItem"
        Me.ComisionesLaboralesToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.ComisionesLaboralesToolStripMenuItem.Size = New System.Drawing.Size(395, 34)
        Me.ComisionesLaboralesToolStripMenuItem.Text = "Comisiones Laborales"
        '
        'ReportesToolStripMenuItem
        '
        Me.ReportesToolStripMenuItem.Name = "ReportesToolStripMenuItem"
        Me.ReportesToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F4
        Me.ReportesToolStripMenuItem.Size = New System.Drawing.Size(395, 34)
        Me.ReportesToolStripMenuItem.Text = "Reportes Documentales"
        '
        'AuditoriaToolStripMenuItem
        '
        Me.AuditoriaToolStripMenuItem.ForeColor = System.Drawing.Color.DarkRed
        Me.AuditoriaToolStripMenuItem.Name = "AuditoriaToolStripMenuItem"
        Me.AuditoriaToolStripMenuItem.Size = New System.Drawing.Size(395, 34)
        Me.AuditoriaToolStripMenuItem.Text = "Auditoría de Eliminaciones"
        '
        'ConfiguracionToolStripMenuItem (NUEVO)
        '
        Me.ConfiguracionToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RangosToolStripMenuItem})
        Me.ConfiguracionToolStripMenuItem.Name = "ConfiguracionToolStripMenuItem"
        Me.ConfiguracionToolStripMenuItem.Size = New System.Drawing.Size(139, 29)
        Me.ConfiguracionToolStripMenuItem.Text = "Configuración"
        '
        'RangosToolStripMenuItem (NUEVO)
        '
        Me.RangosToolStripMenuItem.Name = "RangosToolStripMenuItem"
        Me.RangosToolStripMenuItem.Size = New System.Drawing.Size(325, 34)
        Me.RangosToolStripMenuItem.Text = "Rangos de Numeración"
        '
        'VentanasToolStripMenuItem
        '
        Me.VentanasToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CerrarTodasToolStripMenuItem})
        Me.VentanasToolStripMenuItem.Name = "VentanasToolStripMenuItem"
        Me.VentanasToolStripMenuItem.Size = New System.Drawing.Size(99, 29)
        Me.VentanasToolStripMenuItem.Text = "Ventanas"
        '
        'CerrarTodasToolStripMenuItem
        '
        Me.CerrarTodasToolStripMenuItem.Name = "CerrarTodasToolStripMenuItem"
        Me.CerrarTodasToolStripMenuItem.Size = New System.Drawing.Size(211, 34)
        Me.CerrarTodasToolStripMenuItem.Text = "Cerrar todas"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.lblEstado})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 1018)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Padding = New System.Windows.Forms.Padding(2, 0, 21, 0)
        Me.StatusStrip1.Size = New System.Drawing.Size(1512, 32)
        Me.StatusStrip1.TabIndex = 2
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'lblEstado
        '
        Me.lblEstado.Name = "lblEstado"
        Me.lblEstado.Size = New System.Drawing.Size(376, 25)
        Me.lblEstado.Text = "Sistema de Gestión Penitenciaria Polo - v.2026"
        '
        'frmMenuPrincipal
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1512, 1050)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.IsMdiContainer = True
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Name = "frmMenuPrincipal"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Sistema POLO - Gestión Integral"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents ArchivoToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SalirToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GestiónToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DashboardToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ReclusosToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MesaDeEntradaToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ComisionesLaboralesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ReportesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AuditoriaToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    ' --- NUEVOS AMIGOS ---
    Friend WithEvents ConfiguracionToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RangosToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    ' ---------------------
    Friend WithEvents VentanasToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CerrarTodasToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents lblEstado As System.Windows.Forms.ToolStripStatusLabel
End Class