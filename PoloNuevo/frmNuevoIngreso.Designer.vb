<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmNuevoIngreso
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
        Me.pnlTop = New System.Windows.Forms.Panel()
        Me.lblTitulo = New System.Windows.Forms.Label()
        Me.lblTipo = New System.Windows.Forms.Label()
        Me.cmbTipo = New System.Windows.Forms.ComboBox()
        Me.lblNumero = New System.Windows.Forms.Label()
        Me.txtNumero = New System.Windows.Forms.TextBox()
        Me.lblAsunto = New System.Windows.Forms.Label()
        Me.txtAsunto = New System.Windows.Forms.TextBox()
        Me.grpVinculacion = New System.Windows.Forms.GroupBox()
        Me.txtBuscarRecluso = New System.Windows.Forms.TextBox()
        Me.lstReclusos = New System.Windows.Forms.ListBox()
        Me.chkVincular = New System.Windows.Forms.CheckBox()
        Me.grpArchivo = New System.Windows.Forms.GroupBox()
        Me.lblArchivoNombre = New System.Windows.Forms.Label()
        Me.btnAdjuntar = New System.Windows.Forms.Button()
        Me.btnGuardar = New System.Windows.Forms.Button()
        Me.btnCancelar = New System.Windows.Forms.Button()
        Me.pnlTop.SuspendLayout()
        Me.grpVinculacion.SuspendLayout()
        Me.grpArchivo.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlTop
        '
        Me.pnlTop.BackColor = System.Drawing.Color.WhiteSmoke
        Me.pnlTop.Controls.Add(Me.lblTitulo)
        Me.pnlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlTop.Location = New System.Drawing.Point(0, 0)
        Me.pnlTop.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.pnlTop.Name = "pnlTop"
        Me.pnlTop.Size = New System.Drawing.Size(750, 77)
        Me.pnlTop.TabIndex = 0
        '
        'lblTitulo
        '
        Me.lblTitulo.AutoSize = True
        Me.lblTitulo.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold)
        Me.lblTitulo.ForeColor = System.Drawing.Color.DimGray
        Me.lblTitulo.Location = New System.Drawing.Point(18, 23)
        Me.lblTitulo.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTitulo.Name = "lblTitulo"
        Me.lblTitulo.Size = New System.Drawing.Size(230, 32)
        Me.lblTitulo.TabIndex = 0
        Me.lblTitulo.Text = "Detalle del Ingreso"
        '
        'lblTipo
        '
        Me.lblTipo.AutoSize = True
        Me.lblTipo.Location = New System.Drawing.Point(38, 108)
        Me.lblTipo.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTipo.Name = "lblTipo"
        Me.lblTipo.Size = New System.Drawing.Size(43, 20)
        Me.lblTipo.TabIndex = 1
        Me.lblTipo.Text = "Tipo:"
        '
        'cmbTipo
        '
        Me.cmbTipo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmbTipo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTipo.FormattingEnabled = True
        Me.cmbTipo.Location = New System.Drawing.Point(42, 132)
        Me.cmbTipo.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.cmbTipo.Name = "cmbTipo"
        Me.cmbTipo.Size = New System.Drawing.Size(298, 28)
        Me.cmbTipo.TabIndex = 2
        '
        'lblNumero
        '
        Me.lblNumero.AutoSize = True
        Me.lblNumero.Location = New System.Drawing.Point(375, 108)
        Me.lblNumero.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblNumero.Name = "lblNumero"
        Me.lblNumero.Size = New System.Drawing.Size(124, 20)
        Me.lblNumero.TabIndex = 3
        Me.lblNumero.Text = "Nro. Referencia:"
        '
        'txtNumero
        '
        Me.txtNumero.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNumero.Location = New System.Drawing.Point(380, 134)
        Me.txtNumero.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txtNumero.Name = "txtNumero"
        Me.txtNumero.Size = New System.Drawing.Size(298, 26)
        Me.txtNumero.TabIndex = 4
        '
        'lblAsunto
        '
        Me.lblAsunto.AutoSize = True
        Me.lblAsunto.Location = New System.Drawing.Point(38, 192)
        Me.lblAsunto.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblAsunto.Name = "lblAsunto"
        Me.lblAsunto.Size = New System.Drawing.Size(64, 20)
        Me.lblAsunto.TabIndex = 5
        Me.lblAsunto.Text = "Asunto:"
        '
        'txtAsunto
        '
        Me.txtAsunto.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtAsunto.Location = New System.Drawing.Point(42, 217)
        Me.txtAsunto.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txtAsunto.Multiline = True
        Me.txtAsunto.Name = "txtAsunto"
        Me.txtAsunto.Size = New System.Drawing.Size(636, 90)
        Me.txtAsunto.TabIndex = 6
        '
        'grpVinculacion
        '
        Me.grpVinculacion.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpVinculacion.Controls.Add(Me.lstReclusos)
        Me.grpVinculacion.Controls.Add(Me.txtBuscarRecluso)
        Me.grpVinculacion.Controls.Add(Me.chkVincular)
        Me.grpVinculacion.Location = New System.Drawing.Point(42, 338)
        Me.grpVinculacion.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.grpVinculacion.Name = "grpVinculacion"
        Me.grpVinculacion.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.grpVinculacion.Size = New System.Drawing.Size(638, 190)
        Me.grpVinculacion.TabIndex = 7
        Me.grpVinculacion.TabStop = False
        Me.grpVinculacion.Text = "Asociación"
        '
        'txtBuscarRecluso
        '
        Me.txtBuscarRecluso.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtBuscarRecluso.Enabled = False
        Me.txtBuscarRecluso.Location = New System.Drawing.Point(22, 69)
        Me.txtBuscarRecluso.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txtBuscarRecluso.Name = "txtBuscarRecluso"
        Me.txtBuscarRecluso.Size = New System.Drawing.Size(583, 26)
        Me.txtBuscarRecluso.TabIndex = 1
        '
        'lstReclusos
        '
        Me.lstReclusos.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstReclusos.Enabled = False
        Me.lstReclusos.FormattingEnabled = True
        Me.lstReclusos.ItemHeight = 20
        Me.lstReclusos.Location = New System.Drawing.Point(22, 104)
        Me.lstReclusos.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.lstReclusos.Name = "lstReclusos"
        Me.lstReclusos.Size = New System.Drawing.Size(583, 64)
        Me.lstReclusos.TabIndex = 2
        '
        'chkVincular
        '
        Me.chkVincular.AutoSize = True
        Me.chkVincular.Location = New System.Drawing.Point(22, 31)
        Me.chkVincular.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.chkVincular.Name = "chkVincular"
        Me.chkVincular.Size = New System.Drawing.Size(185, 24)
        Me.chkVincular.TabIndex = 0
        Me.chkVincular.Text = "¿Vincular a Recluso?"
        Me.chkVincular.UseVisualStyleBackColor = True
        '
        'grpArchivo
        '
        Me.grpArchivo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpArchivo.Controls.Add(Me.lblArchivoNombre)
        Me.grpArchivo.Controls.Add(Me.btnAdjuntar)
        Me.grpArchivo.Location = New System.Drawing.Point(42, 548)
        Me.grpArchivo.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.grpArchivo.Name = "grpArchivo"
        Me.grpArchivo.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.grpArchivo.Size = New System.Drawing.Size(638, 108)
        Me.grpArchivo.TabIndex = 8
        Me.grpArchivo.TabStop = False
        Me.grpArchivo.Text = "Archivo Digital"
        '
        'lblArchivoNombre
        '
        Me.lblArchivoNombre.AutoSize = True
        Me.lblArchivoNombre.ForeColor = System.Drawing.Color.DimGray
        Me.lblArchivoNombre.Location = New System.Drawing.Point(195, 46)
        Me.lblArchivoNombre.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblArchivoNombre.Name = "lblArchivoNombre"
        Me.lblArchivoNombre.Size = New System.Drawing.Size(93, 20)
        Me.lblArchivoNombre.TabIndex = 1
        Me.lblArchivoNombre.Text = "Sin adjunto."
        '
        'btnAdjuntar
        '
        Me.btnAdjuntar.Location = New System.Drawing.Point(22, 38)
        Me.btnAdjuntar.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnAdjuntar.Name = "btnAdjuntar"
        Me.btnAdjuntar.Size = New System.Drawing.Size(150, 35)
        Me.btnAdjuntar.TabIndex = 0
        Me.btnAdjuntar.Text = "Examinar..."
        Me.btnAdjuntar.UseVisualStyleBackColor = True
        '
        'btnGuardar
        '
        Me.btnGuardar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGuardar.BackColor = System.Drawing.Color.SeaGreen
        Me.btnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnGuardar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnGuardar.ForeColor = System.Drawing.Color.White
        Me.btnGuardar.Location = New System.Drawing.Point(410, 680)
        Me.btnGuardar.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(270, 62)
        Me.btnGuardar.TabIndex = 9
        Me.btnGuardar.Text = "GUARDAR"
        Me.btnGuardar.UseVisualStyleBackColor = False
        '
        'btnCancelar
        '
        Me.btnCancelar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnCancelar.Location = New System.Drawing.Point(42, 680)
        Me.btnCancelar.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(150, 62)
        Me.btnCancelar.TabIndex = 10
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'frmNuevoIngreso
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(750, 775)
        Me.Controls.Add(Me.btnCancelar)
        Me.Controls.Add(Me.btnGuardar)
        Me.Controls.Add(Me.grpArchivo)
        Me.Controls.Add(Me.grpVinculacion)
        Me.Controls.Add(Me.txtAsunto)
        Me.Controls.Add(Me.lblAsunto)
        Me.Controls.Add(Me.txtNumero)
        Me.Controls.Add(Me.lblNumero)
        Me.Controls.Add(Me.cmbTipo)
        Me.Controls.Add(Me.lblTipo)
        Me.Controls.Add(Me.pnlTop)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmNuevoIngreso"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Gestión de Ingreso"
        Me.pnlTop.ResumeLayout(False)
        Me.pnlTop.PerformLayout()
        Me.grpVinculacion.ResumeLayout(False)
        Me.grpVinculacion.PerformLayout()
        Me.grpArchivo.ResumeLayout(False)
        Me.grpArchivo.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents pnlTop As System.Windows.Forms.Panel
    Friend WithEvents lblTitulo As System.Windows.Forms.Label
    Friend WithEvents lblTipo As System.Windows.Forms.Label
    Friend WithEvents cmbTipo As System.Windows.Forms.ComboBox
    Friend WithEvents lblNumero As System.Windows.Forms.Label
    Friend WithEvents txtNumero As System.Windows.Forms.TextBox
    Friend WithEvents lblAsunto As System.Windows.Forms.Label
    Friend WithEvents txtAsunto As System.Windows.Forms.TextBox
    Friend WithEvents grpVinculacion As System.Windows.Forms.GroupBox
    Friend WithEvents chkVincular As System.Windows.Forms.CheckBox
    Friend WithEvents txtBuscarRecluso As System.Windows.Forms.TextBox
    Friend WithEvents lstReclusos As System.Windows.Forms.ListBox
    Friend WithEvents grpArchivo As System.Windows.Forms.GroupBox
    Friend WithEvents btnAdjuntar As System.Windows.Forms.Button
    Friend WithEvents lblArchivoNombre As System.Windows.Forms.Label
    Friend WithEvents btnGuardar As System.Windows.Forms.Button
    Friend WithEvents btnCancelar As System.Windows.Forms.Button
End Class
