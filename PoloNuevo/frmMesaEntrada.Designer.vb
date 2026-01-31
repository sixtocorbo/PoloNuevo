<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMesaEntrada
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
        Me.btnVerHistorial = New System.Windows.Forms.Button()
        Me.btnActuar = New System.Windows.Forms.Button()
        Me.btnEliminar = New System.Windows.Forms.Button()
        Me.btnVerDigital = New System.Windows.Forms.Button()
        Me.btnImprimirRecibo = New System.Windows.Forms.Button()
        Me.btnEditar = New System.Windows.Forms.Button()
        Me.grpFiltros = New System.Windows.Forms.GroupBox()
        Me.chkPendientes = New System.Windows.Forms.CheckBox()
        Me.btnLimpiar = New System.Windows.Forms.Button()
        Me.dtpHasta = New System.Windows.Forms.DateTimePicker()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.dtpDesde = New System.Windows.Forms.DateTimePicker()
        Me.chkFechas = New System.Windows.Forms.CheckBox()
        Me.txtBuscar = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnNuevo = New System.Windows.Forms.Button()
        Me.lblTitulo = New System.Windows.Forms.Label()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.dgvMesa = New System.Windows.Forms.DataGridView()
        Me.btnCorregirMov = New System.Windows.Forms.Button()
        Me.dgvMovimientos = New System.Windows.Forms.DataGridView()
        Me.lblHistorial = New System.Windows.Forms.Label()
        Me.PrintDocument1 = New System.Drawing.Printing.PrintDocument()
        Me.PrintDialog1 = New System.Windows.Forms.PrintDialog()
        Me.pnlTop.SuspendLayout()
        Me.grpFiltros.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.dgvMesa, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvMovimientos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnlTop
        '
        Me.pnlTop.BackColor = System.Drawing.Color.WhiteSmoke
        Me.pnlTop.Controls.Add(Me.btnVerHistorial)
        Me.pnlTop.Controls.Add(Me.btnActuar)
        Me.pnlTop.Controls.Add(Me.btnEliminar)
        Me.pnlTop.Controls.Add(Me.btnVerDigital)
        Me.pnlTop.Controls.Add(Me.btnImprimirRecibo)
        Me.pnlTop.Controls.Add(Me.btnEditar)
        Me.pnlTop.Controls.Add(Me.grpFiltros)
        Me.pnlTop.Controls.Add(Me.btnNuevo)
        Me.pnlTop.Controls.Add(Me.lblTitulo)
        Me.pnlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlTop.Location = New System.Drawing.Point(0, 0)
        Me.pnlTop.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.pnlTop.Name = "pnlTop"
        Me.pnlTop.Size = New System.Drawing.Size(1512, 185)
        Me.pnlTop.TabIndex = 0
        '
        'btnVerHistorial
        '
        Me.btnVerHistorial.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnVerHistorial.BackColor = System.Drawing.Color.SeaGreen
        Me.btnVerHistorial.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnVerHistorial.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnVerHistorial.ForeColor = System.Drawing.Color.White
        Me.btnVerHistorial.Location = New System.Drawing.Point(1254, 18)
        Me.btnVerHistorial.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnVerHistorial.Name = "btnVerHistorial"
        Me.btnVerHistorial.Size = New System.Drawing.Size(240, 69)
        Me.btnVerHistorial.TabIndex = 9
        Me.btnVerHistorial.Text = "Ver Historial"
        Me.btnVerHistorial.UseVisualStyleBackColor = False
        '
        'btnActuar
        '
        Me.btnActuar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnActuar.BackColor = System.Drawing.Color.RoyalBlue
        Me.btnActuar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnActuar.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnActuar.ForeColor = System.Drawing.Color.White
        Me.btnActuar.Location = New System.Drawing.Point(778, 18)
        Me.btnActuar.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnActuar.Name = "btnActuar"
        Me.btnActuar.Size = New System.Drawing.Size(220, 69)
        Me.btnActuar.TabIndex = 8
        Me.btnActuar.Text = "⚡ ACTUAR / PASE"
        Me.btnActuar.UseVisualStyleBackColor = False
        '
        'btnEliminar
        '
        Me.btnEliminar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEliminar.BackColor = System.Drawing.Color.Maroon
        Me.btnEliminar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnEliminar.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Bold)
        Me.btnEliminar.ForeColor = System.Drawing.Color.White
        Me.btnEliminar.Location = New System.Drawing.Point(122, 18)
        Me.btnEliminar.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnEliminar.Name = "btnEliminar"
        Me.btnEliminar.Size = New System.Drawing.Size(110, 69)
        Me.btnEliminar.TabIndex = 7
        Me.btnEliminar.Text = "🗑 ELIMINAR"
        Me.btnEliminar.UseVisualStyleBackColor = False
        '
        'btnVerDigital
        '
        Me.btnVerDigital.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnVerDigital.BackColor = System.Drawing.Color.SteelBlue
        Me.btnVerDigital.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnVerDigital.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Bold)
        Me.btnVerDigital.ForeColor = System.Drawing.Color.White
        Me.btnVerDigital.Location = New System.Drawing.Point(240, 18)
        Me.btnVerDigital.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnVerDigital.Name = "btnVerDigital"
        Me.btnVerDigital.Size = New System.Drawing.Size(120, 69)
        Me.btnVerDigital.TabIndex = 6
        Me.btnVerDigital.Text = "👁 DIGITAL"
        Me.btnVerDigital.UseVisualStyleBackColor = False
        '
        'btnImprimirRecibo
        '
        Me.btnImprimirRecibo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnImprimirRecibo.BackColor = System.Drawing.Color.DarkSlateGray
        Me.btnImprimirRecibo.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnImprimirRecibo.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Bold)
        Me.btnImprimirRecibo.ForeColor = System.Drawing.Color.White
        Me.btnImprimirRecibo.Location = New System.Drawing.Point(368, 18)
        Me.btnImprimirRecibo.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnImprimirRecibo.Name = "btnImprimirRecibo"
        Me.btnImprimirRecibo.Size = New System.Drawing.Size(140, 69)
        Me.btnImprimirRecibo.TabIndex = 5
        Me.btnImprimirRecibo.Text = "🖨 RECIBO"
        Me.btnImprimirRecibo.UseVisualStyleBackColor = False
        '
        'btnEditar
        '
        Me.btnEditar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEditar.BackColor = System.Drawing.Color.SlateGray
        Me.btnEditar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnEditar.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnEditar.ForeColor = System.Drawing.Color.White
        Me.btnEditar.Location = New System.Drawing.Point(516, 18)
        Me.btnEditar.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnEditar.Name = "btnEditar"
        Me.btnEditar.Size = New System.Drawing.Size(254, 69)
        Me.btnEditar.TabIndex = 4
        Me.btnEditar.Text = "EDITAR / DETALLE"
        Me.btnEditar.UseVisualStyleBackColor = False
        '
        'grpFiltros
        '
        Me.grpFiltros.Controls.Add(Me.chkPendientes)
        Me.grpFiltros.Controls.Add(Me.btnLimpiar)
        Me.grpFiltros.Controls.Add(Me.dtpHasta)
        Me.grpFiltros.Controls.Add(Me.Label2)
        Me.grpFiltros.Controls.Add(Me.dtpDesde)
        Me.grpFiltros.Controls.Add(Me.chkFechas)
        Me.grpFiltros.Controls.Add(Me.txtBuscar)
        Me.grpFiltros.Controls.Add(Me.Label1)
        Me.grpFiltros.Location = New System.Drawing.Point(26, 85)
        Me.grpFiltros.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.grpFiltros.Name = "grpFiltros"
        Me.grpFiltros.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.grpFiltros.Size = New System.Drawing.Size(1468, 85)
        Me.grpFiltros.TabIndex = 3
        Me.grpFiltros.TabStop = False
        Me.grpFiltros.Text = "Filtros de Búsqueda"
        '
        'chkPendientes
        '
        Me.chkPendientes.AutoSize = True
        Me.chkPendientes.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPendientes.ForeColor = System.Drawing.Color.Firebrick
        Me.chkPendientes.Location = New System.Drawing.Point(943, 31)
        Me.chkPendientes.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.chkPendientes.Name = "chkPendientes"
        Me.chkPendientes.Size = New System.Drawing.Size(238, 29)
        Me.chkPendientes.TabIndex = 8
        Me.chkPendientes.Text = "VER SOLO PENDIENTES"
        Me.chkPendientes.UseVisualStyleBackColor = True
        '
        'btnLimpiar
        '
        Me.btnLimpiar.Location = New System.Drawing.Point(1198, 27)
        Me.btnLimpiar.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnLimpiar.Name = "btnLimpiar"
        Me.btnLimpiar.Size = New System.Drawing.Size(80, 38)
        Me.btnLimpiar.TabIndex = 7
        Me.btnLimpiar.Text = "Reset"
        Me.btnLimpiar.UseVisualStyleBackColor = True
        '
        'dtpHasta
        '
        Me.dtpHasta.Enabled = False
        Me.dtpHasta.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpHasta.Location = New System.Drawing.Point(790, 32)
        Me.dtpHasta.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.dtpHasta.Name = "dtpHasta"
        Me.dtpHasta.Size = New System.Drawing.Size(140, 26)
        Me.dtpHasta.TabIndex = 5
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(724, 37)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(56, 20)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Hasta:"
        '
        'dtpDesde
        '
        Me.dtpDesde.Enabled = False
        Me.dtpDesde.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpDesde.Location = New System.Drawing.Point(572, 32)
        Me.dtpDesde.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.dtpDesde.Name = "dtpDesde"
        Me.dtpDesde.Size = New System.Drawing.Size(140, 26)
        Me.dtpDesde.TabIndex = 3
        '
        'chkFechas
        '
        Me.chkFechas.AutoSize = True
        Me.chkFechas.Location = New System.Drawing.Point(428, 35)
        Me.chkFechas.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.chkFechas.Name = "chkFechas"
        Me.chkFechas.Size = New System.Drawing.Size(132, 24)
        Me.chkFechas.TabIndex = 2
        Me.chkFechas.Text = "Filtrar Fechas"
        Me.chkFechas.UseVisualStyleBackColor = True
        '
        'txtBuscar
        '
        Me.txtBuscar.Location = New System.Drawing.Point(150, 32)
        Me.txtBuscar.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txtBuscar.Name = "txtBuscar"
        Me.txtBuscar.Size = New System.Drawing.Size(262, 26)
        Me.txtBuscar.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(22, 37)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(104, 20)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Texto / Oficio:"
        '
        'btnNuevo
        '
        Me.btnNuevo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNuevo.BackColor = System.Drawing.Color.SeaGreen
        Me.btnNuevo.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnNuevo.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnNuevo.ForeColor = System.Drawing.Color.White
        Me.btnNuevo.Location = New System.Drawing.Point(1006, 18)
        Me.btnNuevo.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnNuevo.Name = "btnNuevo"
        Me.btnNuevo.Size = New System.Drawing.Size(240, 69)
        Me.btnNuevo.TabIndex = 1
        Me.btnNuevo.Text = "+ NUEVO INGRESO"
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
        Me.lblTitulo.Size = New System.Drawing.Size(99, 45)
        Me.lblTitulo.TabIndex = 0
        Me.lblTitulo.Text = "Mesa"
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 185)
        Me.SplitContainer1.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.dgvMesa)
        Me.SplitContainer1.Panel1.Padding = New System.Windows.Forms.Padding(15)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.BackColor = System.Drawing.Color.White
        Me.SplitContainer1.Panel2.Controls.Add(Me.btnCorregirMov)
        Me.SplitContainer1.Panel2.Controls.Add(Me.dgvMovimientos)
        Me.SplitContainer1.Panel2.Controls.Add(Me.lblHistorial)
        Me.SplitContainer1.Panel2.Padding = New System.Windows.Forms.Padding(15)
        Me.SplitContainer1.Size = New System.Drawing.Size(1512, 865)
        Me.SplitContainer1.SplitterDistance = 464
        Me.SplitContainer1.SplitterWidth = 6
        Me.SplitContainer1.TabIndex = 1
        '
        'dgvMesa
        '
        Me.dgvMesa.AllowUserToAddRows = False
        Me.dgvMesa.AllowUserToDeleteRows = False
        Me.dgvMesa.AllowUserToResizeColumns = False
        Me.dgvMesa.AllowUserToResizeRows = False
        Me.dgvMesa.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.dgvMesa.BackgroundColor = System.Drawing.Color.White
        Me.dgvMesa.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvMesa.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvMesa.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvMesa.Location = New System.Drawing.Point(15, 15)
        Me.dgvMesa.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.dgvMesa.MultiSelect = False
        Me.dgvMesa.Name = "dgvMesa"
        Me.dgvMesa.ReadOnly = True
        Me.dgvMesa.RowHeadersVisible = False
        Me.dgvMesa.RowHeadersWidth = 62
        Me.dgvMesa.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvMesa.Size = New System.Drawing.Size(1482, 434)
        Me.dgvMesa.TabIndex = 0
        '
        'btnCorregirMov
        '
        Me.btnCorregirMov.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCorregirMov.BackColor = System.Drawing.Color.IndianRed
        Me.btnCorregirMov.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCorregirMov.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Bold)
        Me.btnCorregirMov.ForeColor = System.Drawing.Color.White
        Me.btnCorregirMov.Location = New System.Drawing.Point(1257, 8)
        Me.btnCorregirMov.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnCorregirMov.Name = "btnCorregirMov"
        Me.btnCorregirMov.Size = New System.Drawing.Size(240, 43)
        Me.btnCorregirMov.TabIndex = 3
        Me.btnCorregirMov.Text = "CORREGIR SELECCIONADO"
        Me.btnCorregirMov.UseVisualStyleBackColor = False
        '
        'dgvMovimientos
        '
        Me.dgvMovimientos.AllowUserToAddRows = False
        Me.dgvMovimientos.AllowUserToDeleteRows = False
        Me.dgvMovimientos.AllowUserToResizeColumns = False
        Me.dgvMovimientos.AllowUserToResizeRows = False
        Me.dgvMovimientos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvMovimientos.BackgroundColor = System.Drawing.Color.WhiteSmoke
        Me.dgvMovimientos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvMovimientos.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvMovimientos.Location = New System.Drawing.Point(15, 60)
        Me.dgvMovimientos.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.dgvMovimientos.Name = "dgvMovimientos"
        Me.dgvMovimientos.ReadOnly = True
        Me.dgvMovimientos.RowHeadersVisible = False
        Me.dgvMovimientos.RowHeadersWidth = 62
        Me.dgvMovimientos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvMovimientos.Size = New System.Drawing.Size(1482, 320)
        Me.dgvMovimientos.TabIndex = 1
        '
        'lblHistorial
        '
        Me.lblHistorial.AutoSize = True
        Me.lblHistorial.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblHistorial.Font = New System.Drawing.Font("Segoe UI", 11.0!, System.Drawing.FontStyle.Bold)
        Me.lblHistorial.ForeColor = System.Drawing.Color.DimGray
        Me.lblHistorial.Location = New System.Drawing.Point(15, 15)
        Me.lblHistorial.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblHistorial.Name = "lblHistorial"
        Me.lblHistorial.Padding = New System.Windows.Forms.Padding(0, 0, 0, 15)
        Me.lblHistorial.Size = New System.Drawing.Size(423, 45)
        Me.lblHistorial.TabIndex = 2
        Me.lblHistorial.Text = "Historial de Movimientos (Trazabilidad)"
        '
        'PrintDocument1
        '
        '
        'PrintDialog1
        '
        Me.PrintDialog1.UseEXDialog = True
        '
        'frmMesaEntrada
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1512, 1050)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.pnlTop)
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Name = "frmMesaEntrada"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Gestión de Mesa de Entrada y Salida"
        Me.pnlTop.ResumeLayout(False)
        Me.pnlTop.PerformLayout()
        Me.grpFiltros.ResumeLayout(False)
        Me.grpFiltros.PerformLayout()
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.Panel2.PerformLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        CType(Me.dgvMesa, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvMovimientos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents pnlTop As System.Windows.Forms.Panel
    Friend WithEvents lblTitulo As System.Windows.Forms.Label
    Friend WithEvents btnNuevo As System.Windows.Forms.Button
    Friend WithEvents btnEditar As System.Windows.Forms.Button
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents dgvMesa As System.Windows.Forms.DataGridView
    Friend WithEvents dgvMovimientos As System.Windows.Forms.DataGridView
    Friend WithEvents lblHistorial As System.Windows.Forms.Label
    Friend WithEvents grpFiltros As System.Windows.Forms.GroupBox
    Friend WithEvents btnLimpiar As System.Windows.Forms.Button
    Friend WithEvents dtpHasta As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents dtpDesde As System.Windows.Forms.DateTimePicker
    Friend WithEvents chkFechas As System.Windows.Forms.CheckBox
    Friend WithEvents txtBuscar As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents chkPendientes As System.Windows.Forms.CheckBox
    Friend WithEvents btnCorregirMov As System.Windows.Forms.Button
    Friend WithEvents btnImprimirRecibo As System.Windows.Forms.Button
    Friend WithEvents btnVerDigital As System.Windows.Forms.Button
    Friend WithEvents PrintDocument1 As System.Drawing.Printing.PrintDocument
    Friend WithEvents PrintDialog1 As System.Windows.Forms.PrintDialog
    Friend WithEvents btnEliminar As System.Windows.Forms.Button
    Friend WithEvents btnActuar As System.Windows.Forms.Button ' <--- BOTÓN NUEVO AGREGADO
    Friend WithEvents btnVerHistorial As Button
End Class