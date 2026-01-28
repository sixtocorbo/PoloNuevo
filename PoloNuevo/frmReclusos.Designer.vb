<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmReclusos
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
        Me.btnEditar = New System.Windows.Forms.Button()
        Me.txtBuscar = New System.Windows.Forms.TextBox()
        Me.lblBuscar = New System.Windows.Forms.Label()
        Me.btnLegajo = New System.Windows.Forms.Button()
        Me.btnNuevo = New System.Windows.Forms.Button()
        Me.lblTitulo = New System.Windows.Forms.Label()
        Me.dgvReclusos = New System.Windows.Forms.DataGridView()
        Me.pnlTop.SuspendLayout()
        CType(Me.dgvReclusos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnlTop
        '
        Me.pnlTop.BackColor = System.Drawing.Color.WhiteSmoke
        Me.pnlTop.Controls.Add(Me.btnEditar)
        Me.pnlTop.Controls.Add(Me.txtBuscar)
        Me.pnlTop.Controls.Add(Me.lblBuscar)
        Me.pnlTop.Controls.Add(Me.btnLegajo)
        Me.pnlTop.Controls.Add(Me.btnNuevo)
        Me.pnlTop.Controls.Add(Me.lblTitulo)
        Me.pnlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlTop.Location = New System.Drawing.Point(0, 0)
        Me.pnlTop.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.pnlTop.Name = "pnlTop"
        Me.pnlTop.Size = New System.Drawing.Size(1476, 123)
        Me.pnlTop.TabIndex = 0
        '
        'btnEditar
        '
        Me.btnEditar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEditar.BackColor = System.Drawing.Color.SlateGray
        Me.btnEditar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnEditar.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnEditar.ForeColor = System.Drawing.Color.White
        Me.btnEditar.Location = New System.Drawing.Point(930, 31)
        Me.btnEditar.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnEditar.Name = "btnEditar"
        Me.btnEditar.Size = New System.Drawing.Size(165, 62)
        Me.btnEditar.TabIndex = 3
        Me.btnEditar.Text = "EDITAR DATOS"
        Me.btnEditar.UseVisualStyleBackColor = False
        '
        'txtBuscar
        '
        Me.txtBuscar.Location = New System.Drawing.Point(112, 69)
        Me.txtBuscar.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txtBuscar.Name = "txtBuscar"
        Me.txtBuscar.Size = New System.Drawing.Size(373, 26)
        Me.txtBuscar.TabIndex = 1
        '
        'lblBuscar
        '
        Me.lblBuscar.AutoSize = True
        Me.lblBuscar.Location = New System.Drawing.Point(39, 74)
        Me.lblBuscar.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblBuscar.Name = "lblBuscar"
        Me.lblBuscar.Size = New System.Drawing.Size(63, 20)
        Me.lblBuscar.TabIndex = 4
        Me.lblBuscar.Text = "Buscar:"
        '
        'btnLegajo
        '
        Me.btnLegajo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnLegajo.BackColor = System.Drawing.Color.SteelBlue
        Me.btnLegajo.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnLegajo.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnLegajo.ForeColor = System.Drawing.Color.White
        Me.btnLegajo.Location = New System.Drawing.Point(712, 31)
        Me.btnLegajo.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnLegajo.Name = "btnLegajo"
        Me.btnLegajo.Size = New System.Drawing.Size(195, 62)
        Me.btnLegajo.TabIndex = 2
        Me.btnLegajo.Text = "VER LEGAJO"
        Me.btnLegajo.UseVisualStyleBackColor = False
        '
        'btnNuevo
        '
        Me.btnNuevo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNuevo.BackColor = System.Drawing.Color.SeaGreen
        Me.btnNuevo.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnNuevo.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnNuevo.ForeColor = System.Drawing.Color.White
        Me.btnNuevo.Location = New System.Drawing.Point(1233, 31)
        Me.btnNuevo.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnNuevo.Name = "btnNuevo"
        Me.btnNuevo.Size = New System.Drawing.Size(225, 62)
        Me.btnNuevo.TabIndex = 4
        Me.btnNuevo.Text = "+ NUEVO RECLUSO"
        Me.btnNuevo.UseVisualStyleBackColor = False
        '
        'lblTitulo
        '
        Me.lblTitulo.AutoSize = True
        Me.lblTitulo.Font = New System.Drawing.Font("Segoe UI", 16.0!, System.Drawing.FontStyle.Bold)
        Me.lblTitulo.ForeColor = System.Drawing.Color.DimGray
        Me.lblTitulo.Location = New System.Drawing.Point(18, 14)
        Me.lblTitulo.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTitulo.Name = "lblTitulo"
        Me.lblTitulo.Size = New System.Drawing.Size(311, 45)
        Me.lblTitulo.TabIndex = 0
        Me.lblTitulo.Text = "Listado de Reclusos"
        '
        'dgvReclusos
        '
        Me.dgvReclusos.AllowUserToAddRows = False
        Me.dgvReclusos.AllowUserToDeleteRows = False
        Me.dgvReclusos.AllowUserToResizeColumns = False
        Me.dgvReclusos.AllowUserToResizeRows = False
        Me.dgvReclusos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvReclusos.BackgroundColor = System.Drawing.Color.White
        Me.dgvReclusos.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvReclusos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvReclusos.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvReclusos.Location = New System.Drawing.Point(0, 123)
        Me.dgvReclusos.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.dgvReclusos.MultiSelect = False
        Me.dgvReclusos.Name = "dgvReclusos"
        Me.dgvReclusos.ReadOnly = True
        Me.dgvReclusos.RowHeadersVisible = False
        Me.dgvReclusos.RowHeadersWidth = 62
        Me.dgvReclusos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvReclusos.Size = New System.Drawing.Size(1476, 740)
        Me.dgvReclusos.TabIndex = 1
        '
        'frmReclusos
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1476, 863)
        Me.Controls.Add(Me.dgvReclusos)
        Me.Controls.Add(Me.pnlTop)
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Name = "frmReclusos"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Gestión de Población"
        Me.pnlTop.ResumeLayout(False)
        Me.pnlTop.PerformLayout()
        CType(Me.dgvReclusos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents pnlTop As System.Windows.Forms.Panel
    Friend WithEvents lblTitulo As System.Windows.Forms.Label
    Friend WithEvents btnNuevo As System.Windows.Forms.Button
    Friend WithEvents btnLegajo As System.Windows.Forms.Button
    Friend WithEvents txtBuscar As System.Windows.Forms.TextBox
    Friend WithEvents lblBuscar As System.Windows.Forms.Label
    Friend WithEvents btnEditar As System.Windows.Forms.Button
    Friend WithEvents dgvReclusos As System.Windows.Forms.DataGridView
End Class