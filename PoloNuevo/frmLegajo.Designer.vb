<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmLegajo
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
        Me.pnlCabecera = New System.Windows.Forms.Panel()
        Me.btnEditarDatos = New System.Windows.Forms.Button()
        Me.btnCambiarFoto = New System.Windows.Forms.Button()
        Me.lblEstado = New System.Windows.Forms.Label()
        Me.lblUbicacion = New System.Windows.Forms.Label()
        Me.lblCedula = New System.Windows.Forms.Label()
        Me.lblNombre = New System.Windows.Forms.Label()
        Me.picFoto = New System.Windows.Forms.PictureBox()
        Me.tabControl = New System.Windows.Forms.TabControl()
        Me.tabDocumentos = New System.Windows.Forms.TabPage()
        Me.dgvDocumentos = New System.Windows.Forms.DataGridView()
        Me.tabLegal = New System.Windows.Forms.TabPage()
        Me.btnGuardarLegal = New System.Windows.Forms.Button()
        Me.grpVencimientos = New System.Windows.Forms.GroupBox()
        Me.lblVtoPena = New System.Windows.Forms.Label()
        Me.dtpVtoPena = New System.Windows.Forms.DateTimePicker()
        Me.lblInfoOperativo = New System.Windows.Forms.Label()
        Me.dtpInfoOperativo = New System.Windows.Forms.DateTimePicker()
        Me.tabLaboral = New System.Windows.Forms.TabPage()
        Me.btnBajaTrabajo = New System.Windows.Forms.Button()
        Me.btnAsignarTrabajo = New System.Windows.Forms.Button()
        Me.dgvLaboral = New System.Windows.Forms.DataGridView()
        Me.pnlCabecera.SuspendLayout()
        CType(Me.picFoto, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabControl.SuspendLayout()
        Me.tabDocumentos.SuspendLayout()
        CType(Me.dgvDocumentos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabLegal.SuspendLayout()
        Me.grpVencimientos.SuspendLayout()
        Me.tabLaboral.SuspendLayout()
        CType(Me.dgvLaboral, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnlCabecera
        '
        Me.pnlCabecera.BackColor = System.Drawing.Color.WhiteSmoke
        Me.pnlCabecera.Controls.Add(Me.btnEditarDatos)
        Me.pnlCabecera.Controls.Add(Me.btnCambiarFoto)
        Me.pnlCabecera.Controls.Add(Me.lblEstado)
        Me.pnlCabecera.Controls.Add(Me.lblUbicacion)
        Me.pnlCabecera.Controls.Add(Me.lblCedula)
        Me.pnlCabecera.Controls.Add(Me.lblNombre)
        Me.pnlCabecera.Controls.Add(Me.picFoto)
        Me.pnlCabecera.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlCabecera.Location = New System.Drawing.Point(0, 0)
        Me.pnlCabecera.Name = "pnlCabecera"
        Me.pnlCabecera.Size = New System.Drawing.Size(984, 150)
        Me.pnlCabecera.TabIndex = 0
        '
        'btnEditarDatos
        '
        Me.btnEditarDatos.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEditarDatos.BackColor = System.Drawing.Color.SlateGray
        Me.btnEditarDatos.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnEditarDatos.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnEditarDatos.ForeColor = System.Drawing.Color.White
        Me.btnEditarDatos.Location = New System.Drawing.Point(830, 12)
        Me.btnEditarDatos.Name = "btnEditarDatos"
        Me.btnEditarDatos.Size = New System.Drawing.Size(142, 35)
        Me.btnEditarDatos.TabIndex = 6
        Me.btnEditarDatos.Text = "EDITAR DATOS"
        Me.btnEditarDatos.UseVisualStyleBackColor = False
        '
        'btnCambiarFoto
        '
        Me.btnCambiarFoto.Font = New System.Drawing.Font("Segoe UI", 8.0!)
        Me.btnCambiarFoto.Location = New System.Drawing.Point(12, 118)
        Me.btnCambiarFoto.Name = "btnCambiarFoto"
        Me.btnCambiarFoto.Size = New System.Drawing.Size(120, 25)
        Me.btnCambiarFoto.TabIndex = 5
        Me.btnCambiarFoto.Text = "Cambiar Foto..."
        Me.btnCambiarFoto.UseVisualStyleBackColor = True
        '
        'lblEstado
        '
        Me.lblEstado.AutoSize = True
        Me.lblEstado.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold)
        Me.lblEstado.ForeColor = System.Drawing.Color.SeaGreen
        Me.lblEstado.Location = New System.Drawing.Point(160, 100)
        Me.lblEstado.Name = "lblEstado"
        Me.lblEstado.Size = New System.Drawing.Size(134, 21)
        Me.lblEstado.TabIndex = 4
        Me.lblEstado.Text = "ESTADO ACTIVO"
        '
        'lblUbicacion
        '
        Me.lblUbicacion.AutoSize = True
        Me.lblUbicacion.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.lblUbicacion.ForeColor = System.Drawing.Color.DimGray
        Me.lblUbicacion.Location = New System.Drawing.Point(160, 75)
        Me.lblUbicacion.Name = "lblUbicacion"
        Me.lblUbicacion.Size = New System.Drawing.Size(130, 19)
        Me.lblUbicacion.TabIndex = 3
        Me.lblUbicacion.Text = "Ubicación / Módulo"
        '
        'lblCedula
        '
        Me.lblCedula.AutoSize = True
        Me.lblCedula.Font = New System.Drawing.Font("Segoe UI", 14.0!)
        Me.lblCedula.Location = New System.Drawing.Point(160, 45)
        Me.lblCedula.Name = "lblCedula"
        Me.lblCedula.Size = New System.Drawing.Size(125, 25)
        Me.lblCedula.TabIndex = 2
        Me.lblCedula.Text = "C.I. 1.234.567-8"
        '
        'lblNombre
        '
        Me.lblNombre.AutoSize = True
        Me.lblNombre.Font = New System.Drawing.Font("Segoe UI", 20.0!, System.Drawing.FontStyle.Bold)
        Me.lblNombre.ForeColor = System.Drawing.Color.DarkSlateGray
        Me.lblNombre.Location = New System.Drawing.Point(155, 9)
        Me.lblNombre.Name = "lblNombre"
        Me.lblNombre.Size = New System.Drawing.Size(350, 37)
        Me.lblNombre.TabIndex = 1
        Me.lblNombre.Text = "APELLIDO NOMBRE, JUANA"
        '
        'picFoto
        '
        Me.picFoto.BackColor = System.Drawing.Color.Silver
        Me.picFoto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.picFoto.Location = New System.Drawing.Point(12, 12)
        Me.picFoto.Name = "picFoto"
        Me.picFoto.Size = New System.Drawing.Size(120, 100)
        Me.picFoto.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.picFoto.TabIndex = 0
        Me.picFoto.TabStop = False
        '
        'tabControl
        '
        Me.tabControl.Controls.Add(Me.tabDocumentos)
        Me.tabControl.Controls.Add(Me.tabLegal)
        Me.tabControl.Controls.Add(Me.tabLaboral)
        Me.tabControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabControl.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.tabControl.Location = New System.Drawing.Point(0, 150)
        Me.tabControl.Name = "tabControl"
        Me.tabControl.SelectedIndex = 0
        Me.tabControl.Size = New System.Drawing.Size(984, 461)
        Me.tabControl.TabIndex = 1
        '
        'tabDocumentos
        '
        Me.tabDocumentos.Controls.Add(Me.dgvDocumentos)
        Me.tabDocumentos.Location = New System.Drawing.Point(4, 24)
        Me.tabDocumentos.Name = "tabDocumentos"
        Me.tabDocumentos.Padding = New System.Windows.Forms.Padding(3)
        Me.tabDocumentos.Size = New System.Drawing.Size(976, 433)
        Me.tabDocumentos.TabIndex = 0
        Me.tabDocumentos.Text = "Historial Documentos"
        Me.tabDocumentos.UseVisualStyleBackColor = True
        '
        'dgvDocumentos
        '
        Me.dgvDocumentos.AllowUserToAddRows = False
        Me.dgvDocumentos.AllowUserToDeleteRows = False
        Me.dgvDocumentos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvDocumentos.BackgroundColor = System.Drawing.Color.White
        Me.dgvDocumentos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvDocumentos.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvDocumentos.Location = New System.Drawing.Point(3, 3)
        Me.dgvDocumentos.Name = "dgvDocumentos"
        Me.dgvDocumentos.ReadOnly = True
        Me.dgvDocumentos.RowHeadersVisible = False
        Me.dgvDocumentos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvDocumentos.Size = New System.Drawing.Size(970, 427)
        Me.dgvDocumentos.TabIndex = 0
        '
        'tabLegal
        '
        Me.tabLegal.BackColor = System.Drawing.Color.WhiteSmoke
        Me.tabLegal.Controls.Add(Me.btnGuardarLegal)
        Me.tabLegal.Controls.Add(Me.grpVencimientos)
        Me.tabLegal.Location = New System.Drawing.Point(4, 24)
        Me.tabLegal.Name = "tabLegal"
        Me.tabLegal.Padding = New System.Windows.Forms.Padding(20)
        Me.tabLegal.Size = New System.Drawing.Size(976, 433)
        Me.tabLegal.TabIndex = 1
        Me.tabLegal.Text = "Situación Jurídica"
        '
        'btnGuardarLegal
        '
        Me.btnGuardarLegal.BackColor = System.Drawing.Color.SeaGreen
        Me.btnGuardarLegal.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnGuardarLegal.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnGuardarLegal.ForeColor = System.Drawing.Color.White
        Me.btnGuardarLegal.Location = New System.Drawing.Point(23, 190)
        Me.btnGuardarLegal.Name = "btnGuardarLegal"
        Me.btnGuardarLegal.Size = New System.Drawing.Size(180, 40)
        Me.btnGuardarLegal.TabIndex = 1
        Me.btnGuardarLegal.Text = "GUARDAR CAMBIOS"
        Me.btnGuardarLegal.UseVisualStyleBackColor = False
        '
        'grpVencimientos
        '
        Me.grpVencimientos.BackColor = System.Drawing.Color.White
        Me.grpVencimientos.Controls.Add(Me.lblVtoPena)
        Me.grpVencimientos.Controls.Add(Me.dtpVtoPena)
        Me.grpVencimientos.Controls.Add(Me.lblInfoOperativo)
        Me.grpVencimientos.Controls.Add(Me.dtpInfoOperativo)
        Me.grpVencimientos.Location = New System.Drawing.Point(23, 23)
        Me.grpVencimientos.Name = "grpVencimientos"
        Me.grpVencimientos.Size = New System.Drawing.Size(450, 150)
        Me.grpVencimientos.TabIndex = 0
        Me.grpVencimientos.TabStop = False
        Me.grpVencimientos.Text = "Vencimientos y Fechas Clave"
        '
        'lblVtoPena
        '
        Me.lblVtoPena.AutoSize = True
        Me.lblVtoPena.Location = New System.Drawing.Point(20, 35)
        Me.lblVtoPena.Name = "lblVtoPena"
        Me.lblVtoPena.Size = New System.Drawing.Size(107, 15)
        Me.lblVtoPena.TabIndex = 0
        Me.lblVtoPena.Text = "Vencimiento Pena:"
        '
        'dtpVtoPena
        '
        Me.dtpVtoPena.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpVtoPena.Location = New System.Drawing.Point(150, 32)
        Me.dtpVtoPena.Name = "dtpVtoPena"
        Me.dtpVtoPena.Size = New System.Drawing.Size(150, 23)
        Me.dtpVtoPena.TabIndex = 1
        '
        'lblInfoOperativo
        '
        Me.lblInfoOperativo.AutoSize = True
        Me.lblInfoOperativo.Location = New System.Drawing.Point(20, 75)
        Me.lblInfoOperativo.Name = "lblInfoOperativo"
        Me.lblInfoOperativo.Size = New System.Drawing.Size(117, 15)
        Me.lblInfoOperativo.TabIndex = 2
        Me.lblInfoOperativo.Text = "Informe Operativo:"
        '
        'dtpInfoOperativo
        '
        Me.dtpInfoOperativo.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpInfoOperativo.Location = New System.Drawing.Point(150, 72)
        Me.dtpInfoOperativo.Name = "dtpInfoOperativo"
        Me.dtpInfoOperativo.Size = New System.Drawing.Size(150, 23)
        Me.dtpInfoOperativo.TabIndex = 3
        '
        'tabLaboral
        '
        Me.tabLaboral.Controls.Add(Me.btnBajaTrabajo)
        Me.tabLaboral.Controls.Add(Me.btnAsignarTrabajo)
        Me.tabLaboral.Controls.Add(Me.dgvLaboral)
        Me.tabLaboral.Location = New System.Drawing.Point(4, 24)
        Me.tabLaboral.Name = "tabLaboral"
        Me.tabLaboral.Padding = New System.Windows.Forms.Padding(3)
        Me.tabLaboral.Size = New System.Drawing.Size(976, 433)
        Me.tabLaboral.TabIndex = 2
        Me.tabLaboral.Text = "Trabajo y Conducta"
        Me.tabLaboral.UseVisualStyleBackColor = True
        '
        'btnBajaTrabajo
        '
        Me.btnBajaTrabajo.BackColor = System.Drawing.Color.IndianRed
        Me.btnBajaTrabajo.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnBajaTrabajo.ForeColor = System.Drawing.Color.White
        Me.btnBajaTrabajo.Location = New System.Drawing.Point(180, 6)
        Me.btnBajaTrabajo.Name = "btnBajaTrabajo"
        Me.btnBajaTrabajo.Size = New System.Drawing.Size(160, 30)
        Me.btnBajaTrabajo.TabIndex = 2
        Me.btnBajaTrabajo.Text = "FINALIZAR / DAR BAJA"
        Me.btnBajaTrabajo.UseVisualStyleBackColor = False
        '
        'btnAsignarTrabajo
        '
        Me.btnAsignarTrabajo.BackColor = System.Drawing.Color.SeaGreen
        Me.btnAsignarTrabajo.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAsignarTrabajo.ForeColor = System.Drawing.Color.White
        Me.btnAsignarTrabajo.Location = New System.Drawing.Point(6, 6)
        Me.btnAsignarTrabajo.Name = "btnAsignarTrabajo"
        Me.btnAsignarTrabajo.Size = New System.Drawing.Size(160, 30)
        Me.btnAsignarTrabajo.TabIndex = 1
        Me.btnAsignarTrabajo.Text = "ASIGNAR NUEVA TAREA"
        Me.btnAsignarTrabajo.UseVisualStyleBackColor = False
        '
        'dgvLaboral
        '
        Me.dgvLaboral.AllowUserToAddRows = False
        Me.dgvLaboral.AllowUserToDeleteRows = False
        Me.dgvLaboral.BackgroundColor = System.Drawing.Color.White
        Me.dgvLaboral.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvLaboral.Location = New System.Drawing.Point(6, 45)
        Me.dgvLaboral.Name = "dgvLaboral"
        Me.dgvLaboral.ReadOnly = True
        Me.dgvLaboral.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvLaboral.Size = New System.Drawing.Size(964, 382)
        Me.dgvLaboral.TabIndex = 0
        '
        'frmLegajo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(984, 611)
        Me.Controls.Add(Me.tabControl)
        Me.Controls.Add(Me.pnlCabecera)
        Me.Name = "frmLegajo"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Legajo Digital del Recluso"
        Me.pnlCabecera.ResumeLayout(False)
        Me.pnlCabecera.PerformLayout()
        CType(Me.picFoto, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabControl.ResumeLayout(False)
        Me.tabDocumentos.ResumeLayout(False)
        CType(Me.dgvDocumentos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabLegal.ResumeLayout(False)
        Me.grpVencimientos.ResumeLayout(False)
        Me.grpVencimientos.PerformLayout()
        Me.tabLaboral.ResumeLayout(False)
        CType(Me.dgvLaboral, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents pnlCabecera As System.Windows.Forms.Panel
    Friend WithEvents picFoto As System.Windows.Forms.PictureBox
    Friend WithEvents lblNombre As System.Windows.Forms.Label
    Friend WithEvents lblCedula As System.Windows.Forms.Label
    Friend WithEvents lblUbicacion As System.Windows.Forms.Label
    Friend WithEvents lblEstado As System.Windows.Forms.Label
    Friend WithEvents tabControl As System.Windows.Forms.TabControl
    Friend WithEvents tabDocumentos As System.Windows.Forms.TabPage
    Friend WithEvents tabLegal As System.Windows.Forms.TabPage
    Friend WithEvents tabLaboral As System.Windows.Forms.TabPage
    Friend WithEvents dgvDocumentos As System.Windows.Forms.DataGridView
    Friend WithEvents grpVencimientos As System.Windows.Forms.GroupBox
    Friend WithEvents lblVtoPena As System.Windows.Forms.Label
    Friend WithEvents dtpVtoPena As System.Windows.Forms.DateTimePicker
    Friend WithEvents lblInfoOperativo As System.Windows.Forms.Label
    Friend WithEvents dtpInfoOperativo As System.Windows.Forms.DateTimePicker
    Friend WithEvents dgvLaboral As System.Windows.Forms.DataGridView
    Friend WithEvents btnEditarDatos As System.Windows.Forms.Button
    Friend WithEvents btnCambiarFoto As System.Windows.Forms.Button
    Friend WithEvents btnGuardarLegal As System.Windows.Forms.Button
    Friend WithEvents btnBajaTrabajo As System.Windows.Forms.Button
    Friend WithEvents btnAsignarTrabajo As System.Windows.Forms.Button
End Class