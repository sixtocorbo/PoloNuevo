<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmDashboard
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
        Me.pnlPendientes = New System.Windows.Forms.Panel()
        Me.lblNumPendientes = New System.Windows.Forms.Label()
        Me.lblTitPendientes = New System.Windows.Forms.Label()
        Me.pnlPoblacion = New System.Windows.Forms.Panel()
        Me.lblNumPoblacion = New System.Windows.Forms.Label()
        Me.lblTitPoblacion = New System.Windows.Forms.Label()
        Me.pnlLaboral = New System.Windows.Forms.Panel()
        Me.lblNumLaboral = New System.Windows.Forms.Label()
        Me.lblTitLaboral = New System.Windows.Forms.Label()
        Me.grpUltimos = New System.Windows.Forms.GroupBox()
        Me.dgvUltimos = New System.Windows.Forms.DataGridView()
        Me.lblBienvenida = New System.Windows.Forms.Label()
        Me.pnlPendientes.SuspendLayout()
        Me.pnlPoblacion.SuspendLayout()
        Me.pnlLaboral.SuspendLayout()
        Me.grpUltimos.SuspendLayout()
        CType(Me.dgvUltimos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnlPendientes (ROJO - ALERTA)
        '
        Me.pnlPendientes.BackColor = System.Drawing.Color.IndianRed
        Me.pnlPendientes.Controls.Add(Me.lblNumPendientes)
        Me.pnlPendientes.Controls.Add(Me.lblTitPendientes)
        Me.pnlPendientes.Location = New System.Drawing.Point(30, 60)
        Me.pnlPendientes.Name = "pnlPendientes"
        Me.pnlPendientes.Size = New System.Drawing.Size(250, 120)
        Me.pnlPendientes.TabIndex = 0
        '
        'lblNumPendientes
        '
        Me.lblNumPendientes.AutoSize = True
        Me.lblNumPendientes.Font = New System.Drawing.Font("Segoe UI", 36.0!, System.Drawing.FontStyle.Bold)
        Me.lblNumPendientes.ForeColor = System.Drawing.Color.White
        Me.lblNumPendientes.Location = New System.Drawing.Point(20, 40)
        Me.lblNumPendientes.Name = "lblNumPendientes"
        Me.lblNumPendientes.Size = New System.Drawing.Size(56, 65)
        Me.lblNumPendientes.TabIndex = 1
        Me.lblNumPendientes.Text = "0"
        '
        'lblTitPendientes
        '
        Me.lblTitPendientes.AutoSize = True
        Me.lblTitPendientes.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold)
        Me.lblTitPendientes.ForeColor = System.Drawing.Color.WhiteSmoke
        Me.lblTitPendientes.Location = New System.Drawing.Point(20, 15)
        Me.lblTitPendientes.Name = "lblTitPendientes"
        Me.lblTitPendientes.Size = New System.Drawing.Size(130, 21)
        Me.lblTitPendientes.TabIndex = 0
        Me.lblTitPendientes.Text = "DOCS. PENDIENTES"
        '
        'pnlPoblacion (AZUL - INFO)
        '
        Me.pnlPoblacion.BackColor = System.Drawing.Color.SteelBlue
        Me.pnlPoblacion.Controls.Add(Me.lblNumPoblacion)
        Me.pnlPoblacion.Controls.Add(Me.lblTitPoblacion)
        Me.pnlPoblacion.Location = New System.Drawing.Point(310, 60)
        Me.pnlPoblacion.Name = "pnlPoblacion"
        Me.pnlPoblacion.Size = New System.Drawing.Size(250, 120)
        Me.pnlPoblacion.TabIndex = 1
        '
        'lblNumPoblacion
        '
        Me.lblNumPoblacion.AutoSize = True
        Me.lblNumPoblacion.Font = New System.Drawing.Font("Segoe UI", 36.0!, System.Drawing.FontStyle.Bold)
        Me.lblNumPoblacion.ForeColor = System.Drawing.Color.White
        Me.lblNumPoblacion.Location = New System.Drawing.Point(20, 40)
        Me.lblNumPoblacion.Name = "lblNumPoblacion"
        Me.lblNumPoblacion.Size = New System.Drawing.Size(56, 65)
        Me.lblNumPoblacion.TabIndex = 1
        Me.lblNumPoblacion.Text = "0"
        '
        'lblTitPoblacion
        '
        Me.lblTitPoblacion.AutoSize = True
        Me.lblTitPoblacion.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold)
        Me.lblTitPoblacion.ForeColor = System.Drawing.Color.WhiteSmoke
        Me.lblTitPoblacion.Location = New System.Drawing.Point(20, 15)
        Me.lblTitPoblacion.Name = "lblTitPoblacion"
        Me.lblTitPoblacion.Size = New System.Drawing.Size(155, 21)
        Me.lblTitPoblacion.TabIndex = 0
        Me.lblTitPoblacion.Text = "POBLACIÓN TOTAL"
        '
        'pnlLaboral (VERDE - POSITIVO)
        '
        Me.pnlLaboral.BackColor = System.Drawing.Color.SeaGreen
        Me.pnlLaboral.Controls.Add(Me.lblNumLaboral)
        Me.pnlLaboral.Controls.Add(Me.lblTitLaboral)
        Me.pnlLaboral.Location = New System.Drawing.Point(590, 60)
        Me.pnlLaboral.Name = "pnlLaboral"
        Me.pnlLaboral.Size = New System.Drawing.Size(250, 120)
        Me.pnlLaboral.TabIndex = 2
        '
        'lblNumLaboral
        '
        Me.lblNumLaboral.AutoSize = True
        Me.lblNumLaboral.Font = New System.Drawing.Font("Segoe UI", 36.0!, System.Drawing.FontStyle.Bold)
        Me.lblNumLaboral.ForeColor = System.Drawing.Color.White
        Me.lblNumLaboral.Location = New System.Drawing.Point(20, 40)
        Me.lblNumLaboral.Name = "lblNumLaboral"
        Me.lblNumLaboral.Size = New System.Drawing.Size(56, 65)
        Me.lblNumLaboral.TabIndex = 1
        Me.lblNumLaboral.Text = "0"
        '
        'lblTitLaboral
        '
        Me.lblTitLaboral.AutoSize = True
        Me.lblTitLaboral.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold)
        Me.lblTitLaboral.ForeColor = System.Drawing.Color.WhiteSmoke
        Me.lblTitLaboral.Location = New System.Drawing.Point(20, 15)
        Me.lblTitLaboral.Name = "lblTitLaboral"
        Me.lblTitLaboral.Size = New System.Drawing.Size(120, 21)
        Me.lblTitLaboral.TabIndex = 0
        Me.lblTitLaboral.Text = "TRABAJANDO"
        '
        'grpUltimos
        '
        Me.grpUltimos.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpUltimos.Controls.Add(Me.dgvUltimos)
        Me.grpUltimos.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.grpUltimos.Location = New System.Drawing.Point(30, 200)
        Me.grpUltimos.Name = "grpUltimos"
        Me.grpUltimos.Size = New System.Drawing.Size(810, 300)
        Me.grpUltimos.TabIndex = 3
        Me.grpUltimos.TabStop = False
        Me.grpUltimos.Text = "Últimos Movimientos en Mesa de Entrada"
        '
        'dgvUltimos
        '
        Me.dgvUltimos.AllowUserToAddRows = False
        Me.dgvUltimos.AllowUserToDeleteRows = False
        Me.dgvUltimos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvUltimos.BackgroundColor = System.Drawing.Color.White
        Me.dgvUltimos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvUltimos.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvUltimos.Location = New System.Drawing.Point(3, 19)
        Me.dgvUltimos.Name = "dgvUltimos"
        Me.dgvUltimos.ReadOnly = True
        Me.dgvUltimos.RowHeadersVisible = False
        Me.dgvUltimos.Size = New System.Drawing.Size(804, 278)
        Me.dgvUltimos.TabIndex = 0
        '
        'lblBienvenida
        '
        Me.lblBienvenida.AutoSize = True
        Me.lblBienvenida.Font = New System.Drawing.Font("Segoe UI Light", 20.0!)
        Me.lblBienvenida.ForeColor = System.Drawing.Color.Gray
        Me.lblBienvenida.Location = New System.Drawing.Point(25, 15)
        Me.lblBienvenida.Name = "lblBienvenida"
        Me.lblBienvenida.Size = New System.Drawing.Size(264, 37)
        Me.lblBienvenida.TabIndex = 4
        Me.lblBienvenida.Text = "Resumen de Situación"
        '
        'frmDashboard
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(884, 561)
        Me.Controls.Add(Me.lblBienvenida)
        Me.Controls.Add(Me.grpUltimos)
        Me.Controls.Add(Me.pnlLaboral)
        Me.Controls.Add(Me.pnlPoblacion)
        Me.Controls.Add(Me.pnlPendientes)
        Me.Name = "frmDashboard"
        Me.Text = "Dashboard"
        Me.pnlPendientes.ResumeLayout(False)
        Me.pnlPendientes.PerformLayout()
        Me.pnlPoblacion.ResumeLayout(False)
        Me.pnlPoblacion.PerformLayout()
        Me.pnlLaboral.ResumeLayout(False)
        Me.pnlLaboral.PerformLayout()
        Me.grpUltimos.ResumeLayout(False)
        CType(Me.dgvUltimos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents pnlPendientes As System.Windows.Forms.Panel
    Friend WithEvents lblNumPendientes As System.Windows.Forms.Label
    Friend WithEvents lblTitPendientes As System.Windows.Forms.Label
    Friend WithEvents pnlPoblacion As System.Windows.Forms.Panel
    Friend WithEvents lblNumPoblacion As System.Windows.Forms.Label
    Friend WithEvents lblTitPoblacion As System.Windows.Forms.Label
    Friend WithEvents pnlLaboral As System.Windows.Forms.Panel
    Friend WithEvents lblNumLaboral As System.Windows.Forms.Label
    Friend WithEvents lblTitLaboral As System.Windows.Forms.Label
    Friend WithEvents grpUltimos As System.Windows.Forms.GroupBox
    Friend WithEvents dgvUltimos As System.Windows.Forms.DataGridView
    Friend WithEvents lblBienvenida As System.Windows.Forms.Label
End Class