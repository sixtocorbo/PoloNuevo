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
        Me.components = New System.ComponentModel.Container()
        Me.pnlTop = New System.Windows.Forms.Panel()
        Me.lblTitulo = New System.Windows.Forms.Label()
        Me.lblTipo = New System.Windows.Forms.Label()
        Me.cmbTipo = New System.Windows.Forms.ComboBox()
        Me.lblNumero = New System.Windows.Forms.Label()
        Me.txtNumero = New System.Windows.Forms.TextBox()
        Me.lblOrigen = New System.Windows.Forms.Label()
        Me.txtOrigen = New System.Windows.Forms.TextBox()
        Me.lstSugerenciasOrigen = New System.Windows.Forms.ListBox()
        Me.lblAsunto = New System.Windows.Forms.Label()
        Me.txtAsunto = New System.Windows.Forms.TextBox()
        Me.grpArchivo = New System.Windows.Forms.GroupBox()
        Me.lblArchivoNombre = New System.Windows.Forms.Label()
        Me.btnAdjuntar = New System.Windows.Forms.Button()
        Me.btnGuardar = New System.Windows.Forms.Button()
        Me.btnCancelar = New System.Windows.Forms.Button()
        Me.chkVencimiento = New System.Windows.Forms.CheckBox()
        Me.dtpVencimiento = New System.Windows.Forms.DateTimePicker()
        Me.grpRelacion = New System.Windows.Forms.GroupBox()
        Me.lblInfoPadre = New System.Windows.Forms.Label()
        Me.txtIdPadre = New System.Windows.Forms.TextBox()
        Me.chkEsRespuesta = New System.Windows.Forms.CheckBox()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.pnlTop.SuspendLayout()
        Me.grpArchivo.SuspendLayout()
        Me.grpRelacion.SuspendLayout()
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
        Me.pnlTop.Size = New System.Drawing.Size(685, 92)
        Me.pnlTop.TabIndex = 0
        '
        'lblTitulo
        '
        Me.lblTitulo.AutoSize = True
        Me.lblTitulo.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold)
        Me.lblTitulo.ForeColor = System.Drawing.Color.DimGray
        Me.lblTitulo.Location = New System.Drawing.Point(27, 28)
        Me.lblTitulo.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTitulo.Name = "lblTitulo"
        Me.lblTitulo.Size = New System.Drawing.Size(230, 32)
        Me.lblTitulo.TabIndex = 0
        Me.lblTitulo.Text = "Detalle del Ingreso"
        '
        'lblTipo
        '
        Me.lblTipo.AutoSize = True
        Me.lblTipo.Location = New System.Drawing.Point(57, 110)
        Me.lblTipo.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTipo.Name = "lblTipo"
        Me.lblTipo.Size = New System.Drawing.Size(43, 20)
        Me.lblTipo.TabIndex = 1
        Me.lblTipo.Text = "Tipo:"
        '
        'cmbTipo
        '
        Me.cmbTipo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTipo.FormattingEnabled = True
        Me.cmbTipo.Location = New System.Drawing.Point(63, 141)
        Me.cmbTipo.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.cmbTipo.Name = "cmbTipo"
        Me.cmbTipo.Size = New System.Drawing.Size(353, 28)
        Me.cmbTipo.TabIndex = 2
        '
        'lblNumero
        '
        Me.lblNumero.AutoSize = True
        Me.lblNumero.Location = New System.Drawing.Point(420, 110)
        Me.lblNumero.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblNumero.Name = "lblNumero"
        Me.lblNumero.Size = New System.Drawing.Size(124, 20)
        Me.lblNumero.TabIndex = 3
        Me.lblNumero.Text = "Nro. Referencia:"
        '
        'txtNumero
        '
        Me.txtNumero.Location = New System.Drawing.Point(424, 141)
        Me.txtNumero.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txtNumero.Name = "txtNumero"
        Me.txtNumero.Size = New System.Drawing.Size(255, 26)
        Me.txtNumero.TabIndex = 4
        '
        'lblOrigen
        '
        Me.lblOrigen.AutoSize = True
        Me.lblOrigen.Location = New System.Drawing.Point(55, 225)
        Me.lblOrigen.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblOrigen.Name = "lblOrigen"
        Me.lblOrigen.Size = New System.Drawing.Size(163, 20)
        Me.lblOrigen.TabIndex = 5
        Me.lblOrigen.Text = "Organismo de Origen:"
        '
        'txtOrigen
        '
        Me.txtOrigen.Location = New System.Drawing.Point(61, 256)
        Me.txtOrigen.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txtOrigen.Name = "txtOrigen"
        Me.txtOrigen.Size = New System.Drawing.Size(616, 26)
        Me.txtOrigen.TabIndex = 6
        '
        'lstSugerenciasOrigen
        '
        Me.lstSugerenciasOrigen.FormattingEnabled = True
        Me.lstSugerenciasOrigen.ItemHeight = 20
        Me.lstSugerenciasOrigen.Location = New System.Drawing.Point(61, 282)
        Me.lstSugerenciasOrigen.Name = "lstSugerenciasOrigen"
        Me.lstSugerenciasOrigen.Size = New System.Drawing.Size(616, 104)
        Me.lstSugerenciasOrigen.TabIndex = 20
        Me.lstSugerenciasOrigen.Visible = False
        '
        'lblAsunto
        '
        Me.lblAsunto.AutoSize = True
        Me.lblAsunto.Location = New System.Drawing.Point(55, 305)
        Me.lblAsunto.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblAsunto.Name = "lblAsunto"
        Me.lblAsunto.Size = New System.Drawing.Size(64, 20)
        Me.lblAsunto.TabIndex = 7
        Me.lblAsunto.Text = "Asunto:"
        '
        'txtAsunto
        '
        Me.txtAsunto.Location = New System.Drawing.Point(61, 336)
        Me.txtAsunto.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txtAsunto.Multiline = True
        Me.txtAsunto.Name = "txtAsunto"
        Me.txtAsunto.Size = New System.Drawing.Size(616, 90)
        Me.txtAsunto.TabIndex = 8
        '
        'grpArchivo
        '
        Me.grpArchivo.Controls.Add(Me.lblArchivoNombre)
        Me.grpArchivo.Controls.Add(Me.btnAdjuntar)
        Me.grpArchivo.Location = New System.Drawing.Point(61, 560)
        Me.grpArchivo.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.grpArchivo.Name = "grpArchivo"
        Me.grpArchivo.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.grpArchivo.Size = New System.Drawing.Size(619, 104)
        Me.grpArchivo.TabIndex = 12
        Me.grpArchivo.TabStop = False
        Me.grpArchivo.Text = "Archivo Digital"
        '
        'lblArchivoNombre
        '
        Me.lblArchivoNombre.AutoSize = True
        Me.lblArchivoNombre.ForeColor = System.Drawing.Color.DimGray
        Me.lblArchivoNombre.Location = New System.Drawing.Point(292, 62)
        Me.lblArchivoNombre.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblArchivoNombre.Name = "lblArchivoNombre"
        Me.lblArchivoNombre.Size = New System.Drawing.Size(93, 20)
        Me.lblArchivoNombre.TabIndex = 1
        Me.lblArchivoNombre.Text = "Sin adjunto."
        '
        'btnAdjuntar
        '
        Me.btnAdjuntar.Location = New System.Drawing.Point(33, 46)
        Me.btnAdjuntar.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnAdjuntar.Name = "btnAdjuntar"
        Me.btnAdjuntar.Size = New System.Drawing.Size(225, 54)
        Me.btnAdjuntar.TabIndex = 0
        Me.btnAdjuntar.Text = "Examinar..."
        Me.btnAdjuntar.UseVisualStyleBackColor = True
        '
        'btnGuardar
        '
        Me.btnGuardar.BackColor = System.Drawing.Color.SeaGreen
        Me.btnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnGuardar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold)
        Me.btnGuardar.ForeColor = System.Drawing.Color.White
        Me.btnGuardar.Location = New System.Drawing.Point(316, 685)
        Me.btnGuardar.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(364, 85)
        Me.btnGuardar.TabIndex = 13
        Me.btnGuardar.Text = "GUARDAR"
        Me.btnGuardar.UseVisualStyleBackColor = False
        '
        'btnCancelar
        '
        Me.btnCancelar.Location = New System.Drawing.Point(60, 685)
        Me.btnCancelar.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(247, 85)
        Me.btnCancelar.TabIndex = 14
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'chkVencimiento
        '
        Me.chkVencimiento.AutoSize = True
        Me.chkVencimiento.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.chkVencimiento.ForeColor = System.Drawing.Color.DarkOrange
        Me.chkVencimiento.Location = New System.Drawing.Point(61, 181)
        Me.chkVencimiento.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.chkVencimiento.Name = "chkVencimiento"
        Me.chkVencimiento.Size = New System.Drawing.Size(291, 29)
        Me.chkVencimiento.TabIndex = 9
        Me.chkVencimiento.Text = "¿Requiere Respuesta / Vence?"
        Me.chkVencimiento.UseVisualStyleBackColor = True
        '
        'dtpVencimiento
        '
        Me.dtpVencimiento.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpVencimiento.Location = New System.Drawing.Point(424, 181)
        Me.dtpVencimiento.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.dtpVencimiento.Name = "dtpVencimiento"
        Me.dtpVencimiento.Size = New System.Drawing.Size(144, 26)
        Me.dtpVencimiento.TabIndex = 10
        Me.dtpVencimiento.Visible = False
        '
        'grpRelacion
        '
        Me.grpRelacion.Controls.Add(Me.lblInfoPadre)
        Me.grpRelacion.Controls.Add(Me.txtIdPadre)
        Me.grpRelacion.Controls.Add(Me.chkEsRespuesta)
        Me.grpRelacion.Location = New System.Drawing.Point(61, 434)
        Me.grpRelacion.Name = "grpRelacion"
        Me.grpRelacion.Size = New System.Drawing.Size(619, 109)
        Me.grpRelacion.TabIndex = 15
        Me.grpRelacion.TabStop = False
        Me.grpRelacion.Text = "Trazabilidad / Relación"
        '
        'lblInfoPadre
        '
        Me.lblInfoPadre.AutoSize = True
        Me.lblInfoPadre.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInfoPadre.ForeColor = System.Drawing.Color.Blue
        Me.lblInfoPadre.Location = New System.Drawing.Point(29, 73)
        Me.lblInfoPadre.Name = "lblInfoPadre"
        Me.lblInfoPadre.Size = New System.Drawing.Size(522, 20)
        Me.lblInfoPadre.TabIndex = 4
        Me.lblInfoPadre.Text = "Marque la casilla para vincular con el documento seleccionado en MESA."
        '
        'txtIdPadre
        '
        Me.txtIdPadre.Enabled = False
        Me.txtIdPadre.Location = New System.Drawing.Point(267, 14)
        Me.txtIdPadre.Name = "txtIdPadre"
        Me.txtIdPadre.Size = New System.Drawing.Size(24, 26)
        Me.txtIdPadre.TabIndex = 1
        Me.txtIdPadre.Visible = False
        '
        'chkEsRespuesta
        '
        Me.chkEsRespuesta.AutoSize = True
        Me.chkEsRespuesta.Location = New System.Drawing.Point(33, 35)
        Me.chkEsRespuesta.Name = "chkEsRespuesta"
        Me.chkEsRespuesta.Size = New System.Drawing.Size(345, 24)
        Me.chkEsRespuesta.TabIndex = 0
        Me.chkEsRespuesta.Text = "¿Este documento se vincula a uno anterior?"
        Me.chkEsRespuesta.UseVisualStyleBackColor = True
        '
        'frmNuevoIngreso
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(685, 800)
        Me.Controls.Add(Me.lstSugerenciasOrigen)
        Me.Controls.Add(Me.grpRelacion)
        Me.Controls.Add(Me.btnCancelar)
        Me.Controls.Add(Me.btnGuardar)
        Me.Controls.Add(Me.grpArchivo)
        Me.Controls.Add(Me.dtpVencimiento)
        Me.Controls.Add(Me.chkVencimiento)
        Me.Controls.Add(Me.txtAsunto)
        Me.Controls.Add(Me.lblAsunto)
        Me.Controls.Add(Me.txtOrigen)
        Me.Controls.Add(Me.lblOrigen)
        Me.Controls.Add(Me.txtNumero)
        Me.Controls.Add(Me.lblNumero)
        Me.Controls.Add(Me.cmbTipo)
        Me.Controls.Add(Me.lblTipo)
        Me.Controls.Add(Me.pnlTop)
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmNuevoIngreso"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Gestión de Ingreso"
        Me.pnlTop.ResumeLayout(False)
        Me.pnlTop.PerformLayout()
        Me.grpArchivo.ResumeLayout(False)
        Me.grpArchivo.PerformLayout()
        Me.grpRelacion.ResumeLayout(False)
        Me.grpRelacion.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents pnlTop As System.Windows.Forms.Panel
    Friend WithEvents lblTitulo As System.Windows.Forms.Label
    Friend WithEvents lblTipo As System.Windows.Forms.Label
    Friend WithEvents cmbTipo As System.Windows.Forms.ComboBox
    Friend WithEvents lblNumero As System.Windows.Forms.Label
    Friend WithEvents txtNumero As System.Windows.Forms.TextBox
    Friend WithEvents lblOrigen As System.Windows.Forms.Label
    Friend WithEvents txtOrigen As System.Windows.Forms.TextBox
    Friend WithEvents lstSugerenciasOrigen As System.Windows.Forms.ListBox
    Friend WithEvents lblAsunto As System.Windows.Forms.Label
    Friend WithEvents txtAsunto As System.Windows.Forms.TextBox
    Friend WithEvents grpArchivo As System.Windows.Forms.GroupBox
    Friend WithEvents btnAdjuntar As System.Windows.Forms.Button
    Friend WithEvents lblArchivoNombre As System.Windows.Forms.Label
    Friend WithEvents btnGuardar As System.Windows.Forms.Button
    Friend WithEvents btnCancelar As System.Windows.Forms.Button
    Friend WithEvents chkVencimiento As System.Windows.Forms.CheckBox
    Friend WithEvents dtpVencimiento As System.Windows.Forms.DateTimePicker
    Friend WithEvents grpRelacion As System.Windows.Forms.GroupBox
    Friend WithEvents lblInfoPadre As System.Windows.Forms.Label
    Friend WithEvents txtIdPadre As System.Windows.Forms.TextBox
    Friend WithEvents chkEsRespuesta As System.Windows.Forms.CheckBox
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
End Class