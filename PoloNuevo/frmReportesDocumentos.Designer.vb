<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmReportesDocumentos
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
        Me.pnlFiltros = New System.Windows.Forms.Panel()
        Me.btnExportar = New System.Windows.Forms.Button()
        Me.btnGenerar = New System.Windows.Forms.Button()
        Me.cmbTipoReporte = New System.Windows.Forms.ComboBox()
        Me.lblReporte = New System.Windows.Forms.Label()
        Me.dtpHasta = New System.Windows.Forms.DateTimePicker()
        Me.lblHasta = New System.Windows.Forms.Label()
        Me.dtpDesde = New System.Windows.Forms.DateTimePicker()
        Me.lblDesde = New System.Windows.Forms.Label()
        Me.dgvResultados = New System.Windows.Forms.DataGridView()
        Me.pnlFiltros.SuspendLayout()
        CType(Me.dgvResultados, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnlFiltros
        '
        Me.pnlFiltros.BackColor = System.Drawing.Color.WhiteSmoke
        Me.pnlFiltros.Controls.Add(Me.btnExportar)
        Me.pnlFiltros.Controls.Add(Me.btnGenerar)
        Me.pnlFiltros.Controls.Add(Me.cmbTipoReporte)
        Me.pnlFiltros.Controls.Add(Me.lblReporte)
        Me.pnlFiltros.Controls.Add(Me.dtpHasta)
        Me.pnlFiltros.Controls.Add(Me.lblHasta)
        Me.pnlFiltros.Controls.Add(Me.dtpDesde)
        Me.pnlFiltros.Controls.Add(Me.lblDesde)
        Me.pnlFiltros.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlFiltros.Location = New System.Drawing.Point(0, 0)
        Me.pnlFiltros.Name = "pnlFiltros"
        Me.pnlFiltros.Size = New System.Drawing.Size(984, 80)
        Me.pnlFiltros.TabIndex = 0
        '
        'btnExportar
        '
        Me.btnExportar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnExportar.BackColor = System.Drawing.Color.DarkOliveGreen
        Me.btnExportar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnExportar.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnExportar.ForeColor = System.Drawing.Color.White
        Me.btnExportar.Location = New System.Drawing.Point(852, 23)
        Me.btnExportar.Name = "btnExportar"
        Me.btnExportar.Size = New System.Drawing.Size(120, 35)
        Me.btnExportar.TabIndex = 7
        Me.btnExportar.Text = "EXPORTAR CSV"
        Me.btnExportar.UseVisualStyleBackColor = False
        '
        'btnGenerar
        '
        Me.btnGenerar.BackColor = System.Drawing.Color.SteelBlue
        Me.btnGenerar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnGenerar.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnGenerar.ForeColor = System.Drawing.Color.White
        Me.btnGenerar.Location = New System.Drawing.Point(620, 23)
        Me.btnGenerar.Name = "btnGenerar"
        Me.btnGenerar.Size = New System.Drawing.Size(120, 35)
        Me.btnGenerar.TabIndex = 6
        Me.btnGenerar.Text = "GENERAR"
        Me.btnGenerar.UseVisualStyleBackColor = False
        '
        'cmbTipoReporte
        '
        Me.cmbTipoReporte.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTipoReporte.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.cmbTipoReporte.FormattingEnabled = True
        Me.cmbTipoReporte.Items.AddRange(New Object() {"LIBRO DE ENTRADAS", "LIBRO DE SALIDAS", "DOCUMENTOS PENDIENTES", "ESTADÍSTICA POR TIPO"})
        Me.cmbTipoReporte.Location = New System.Drawing.Point(360, 29)
        Me.cmbTipoReporte.Name = "cmbTipoReporte"
        Me.cmbTipoReporte.Size = New System.Drawing.Size(250, 25)
        Me.cmbTipoReporte.TabIndex = 5
        '
        'lblReporte
        '
        Me.lblReporte.AutoSize = True
        Me.lblReporte.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblReporte.Location = New System.Drawing.Point(360, 11)
        Me.lblReporte.Name = "lblReporte"
        Me.lblReporte.Size = New System.Drawing.Size(100, 15)
        Me.lblReporte.TabIndex = 4
        Me.lblReporte.Text = "Tipo de Reporte:"
        '
        'dtpHasta
        '
        Me.dtpHasta.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpHasta.Location = New System.Drawing.Point(180, 30)
        Me.dtpHasta.Name = "dtpHasta"
        Me.dtpHasta.Size = New System.Drawing.Size(120, 20)
        Me.dtpHasta.TabIndex = 3
        '
        'lblHasta
        '
        Me.lblHasta.AutoSize = True
        Me.lblHasta.Location = New System.Drawing.Point(180, 13)
        Me.lblHasta.Name = "lblHasta"
        Me.lblHasta.Size = New System.Drawing.Size(38, 13)
        Me.lblHasta.TabIndex = 2
        Me.lblHasta.Text = "Hasta:"
        '
        'dtpDesde
        '
        Me.dtpDesde.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpDesde.Location = New System.Drawing.Point(20, 30)
        Me.dtpDesde.Name = "dtpDesde"
        Me.dtpDesde.Size = New System.Drawing.Size(120, 20)
        Me.dtpDesde.TabIndex = 1
        '
        'lblDesde
        '
        Me.lblDesde.AutoSize = True
        Me.lblDesde.Location = New System.Drawing.Point(20, 13)
        Me.lblDesde.Name = "lblDesde"
        Me.lblDesde.Size = New System.Drawing.Size(41, 13)
        Me.lblDesde.TabIndex = 0
        Me.lblDesde.Text = "Desde:"
        '
        'dgvResultados
        '
        Me.dgvResultados.AllowUserToAddRows = False
        Me.dgvResultados.AllowUserToDeleteRows = False
        Me.dgvResultados.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvResultados.BackgroundColor = System.Drawing.Color.White
        Me.dgvResultados.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvResultados.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvResultados.Location = New System.Drawing.Point(0, 80)
        Me.dgvResultados.Name = "dgvResultados"
        Me.dgvResultados.ReadOnly = True
        Me.dgvResultados.RowHeadersVisible = False
        Me.dgvResultados.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvResultados.Size = New System.Drawing.Size(984, 481)
        Me.dgvResultados.TabIndex = 1
        '
        'frmReportesDocumentos
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(984, 561)
        Me.Controls.Add(Me.dgvResultados)
        Me.Controls.Add(Me.pnlFiltros)
        Me.Name = "frmReportesDocumentos"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Centro de Reportes Documentales"
        Me.pnlFiltros.ResumeLayout(False)
        Me.pnlFiltros.PerformLayout()
        CType(Me.dgvResultados, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents pnlFiltros As System.Windows.Forms.Panel
    Friend WithEvents lblDesde As System.Windows.Forms.Label
    Friend WithEvents dtpDesde As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtpHasta As System.Windows.Forms.DateTimePicker
    Friend WithEvents lblHasta As System.Windows.Forms.Label
    Friend WithEvents lblReporte As System.Windows.Forms.Label
    Friend WithEvents cmbTipoReporte As System.Windows.Forms.ComboBox
    Friend WithEvents btnGenerar As System.Windows.Forms.Button
    Friend WithEvents dgvResultados As System.Windows.Forms.DataGridView
    Friend WithEvents btnExportar As System.Windows.Forms.Button
End Class