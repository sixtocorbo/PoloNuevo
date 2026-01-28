<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmComisiones
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.pnlTop = New System.Windows.Forms.Panel()
        Me.lblTitulo = New System.Windows.Forms.Label()
        Me.chkVerHistorial = New System.Windows.Forms.CheckBox()
        Me.btnVerLegajo = New System.Windows.Forms.Button()
        Me.dgvComisiones = New System.Windows.Forms.DataGridView()
        Me.pnlTop.SuspendLayout()
        CType(Me.dgvComisiones, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnlTop
        '
        Me.pnlTop.BackColor = System.Drawing.Color.WhiteSmoke
        Me.pnlTop.Controls.Add(Me.btnVerLegajo)
        Me.pnlTop.Controls.Add(Me.chkVerHistorial)
        Me.pnlTop.Controls.Add(Me.lblTitulo)
        Me.pnlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlTop.Location = New System.Drawing.Point(0, 0)
        Me.pnlTop.Name = "pnlTop"
        Me.pnlTop.Size = New System.Drawing.Size(884, 60)
        Me.pnlTop.TabIndex = 0
        '
        'lblTitulo
        '
        Me.lblTitulo.AutoSize = True
        Me.lblTitulo.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Bold)
        Me.lblTitulo.ForeColor = System.Drawing.Color.DimGray
        Me.lblTitulo.Location = New System.Drawing.Point(12, 15)
        Me.lblTitulo.Name = "lblTitulo"
        Me.lblTitulo.Size = New System.Drawing.Size(264, 25)
        Me.lblTitulo.TabIndex = 0
        Me.lblTitulo.Text = "Tablero General de Trabajos"
        '
        'chkVerHistorial
        '
        Me.chkVerHistorial.AutoSize = True
        Me.chkVerHistorial.Location = New System.Drawing.Point(300, 22)
        Me.chkVerHistorial.Name = "chkVerHistorial"
        Me.chkVerHistorial.Size = New System.Drawing.Size(185, 17)
        Me.chkVerHistorial.TabIndex = 1
        Me.chkVerHistorial.Text = "Ver historial completo (Finalizadas)"
        Me.chkVerHistorial.UseVisualStyleBackColor = True
        '
        'btnVerLegajo
        '
        Me.btnVerLegajo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnVerLegajo.BackColor = System.Drawing.Color.SteelBlue
        Me.btnVerLegajo.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnVerLegajo.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnVerLegajo.ForeColor = System.Drawing.Color.White
        Me.btnVerLegajo.Location = New System.Drawing.Point(652, 12)
        Me.btnVerLegajo.Name = "btnVerLegajo"
        Me.btnVerLegajo.Size = New System.Drawing.Size(220, 35)
        Me.btnVerLegajo.TabIndex = 2
        Me.btnVerLegajo.Text = "IR AL LEGAJO / GESTIONAR"
        Me.btnVerLegajo.UseVisualStyleBackColor = False
        '
        'dgvComisiones
        '
        Me.dgvComisiones.AllowUserToAddRows = False
        Me.dgvComisiones.AllowUserToDeleteRows = False
        Me.dgvComisiones.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvComisiones.BackgroundColor = System.Drawing.Color.White
        Me.dgvComisiones.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvComisiones.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvComisiones.Location = New System.Drawing.Point(0, 60)
        Me.dgvComisiones.Name = "dgvComisiones"
        Me.dgvComisiones.ReadOnly = True
        Me.dgvComisiones.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvComisiones.Size = New System.Drawing.Size(884, 501)
        Me.dgvComisiones.TabIndex = 1
        '
        'frmComisiones
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(884, 561)
        Me.Controls.Add(Me.dgvComisiones)
        Me.Controls.Add(Me.pnlTop)
        Me.Name = "frmComisiones"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Consulta Global de Comisiones Laborales"
        Me.pnlTop.ResumeLayout(False)
        Me.pnlTop.PerformLayout()
        CType(Me.dgvComisiones, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents pnlTop As System.Windows.Forms.Panel
    Friend WithEvents lblTitulo As System.Windows.Forms.Label
    Friend WithEvents chkVerHistorial As System.Windows.Forms.CheckBox
    Friend WithEvents btnVerLegajo As System.Windows.Forms.Button
    Friend WithEvents dgvComisiones As System.Windows.Forms.DataGridView
End Class