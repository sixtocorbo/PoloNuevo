<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmGenerarDocumento
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
        Me.lblRefExterna = New System.Windows.Forms.Label()
        Me.lblTipo = New System.Windows.Forms.Label()
        Me.cmbTipo = New System.Windows.Forms.ComboBox()
        Me.lblNumero = New System.Windows.Forms.Label()
        Me.txtNumero = New System.Windows.Forms.TextBox()
        Me.lblAsunto = New System.Windows.Forms.Label()
        Me.txtAsunto = New System.Windows.Forms.TextBox()
        Me.lblDestino = New System.Windows.Forms.Label()
        Me.cmbDestino = New System.Windows.Forms.ComboBox()
        Me.grpAdjunto = New System.Windows.Forms.GroupBox()
        Me.lblArchivo = New System.Windows.Forms.Label()
        Me.btnAdjuntar = New System.Windows.Forms.Button()
        Me.btnGuardar = New System.Windows.Forms.Button()
        Me.btnCancelar = New System.Windows.Forms.Button()
        Me.chkMoverOriginal = New System.Windows.Forms.CheckBox()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.pnlTop.SuspendLayout()
        Me.grpAdjunto.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlTop
        '
        Me.pnlTop.BackColor = System.Drawing.Color.FromArgb(245, 246, 248)
        Me.pnlTop.Controls.Add(Me.lblTitulo)
        Me.pnlTop.Controls.Add(Me.lblRefExterna)
        Me.pnlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlTop.Location = New System.Drawing.Point(0, 0)
        Me.pnlTop.Name = "pnlTop"
        Me.pnlTop.Size = New System.Drawing.Size(600, 90)
        Me.pnlTop.TabIndex = 0
        '
        'lblTitulo
        '
        Me.lblTitulo.AutoSize = True
        Me.lblTitulo.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Bold)
        Me.lblTitulo.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64)
        Me.lblTitulo.Location = New System.Drawing.Point(20, 15)
        Me.lblTitulo.Name = "lblTitulo"
        Me.lblTitulo.Size = New System.Drawing.Size(306, 25)
        Me.lblTitulo.TabIndex = 0
        Me.lblTitulo.Text = "Generar Documento / Respuesta"
        '
        'lblRefExterna
        '
        Me.lblRefExterna.AutoSize = True
        Me.lblRefExterna.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Italic)
        Me.lblRefExterna.ForeColor = System.Drawing.Color.DimGray
        Me.lblRefExterna.Location = New System.Drawing.Point(22, 50)
        Me.lblRefExterna.Name = "lblRefExterna"
        Me.lblRefExterna.Size = New System.Drawing.Size(236, 19)
        Me.lblRefExterna.TabIndex = 1
        Me.lblRefExterna.Text = "En referencia a: [Documento Externo]"
        '
        'lblTipo
        '
        Me.lblTipo.AutoSize = True
        Me.lblTipo.Location = New System.Drawing.Point(31, 110)
        Me.lblTipo.Name = "lblTipo"
        Me.lblTipo.Size = New System.Drawing.Size(172, 17)
        Me.lblTipo.TabIndex = 1
        Me.lblTipo.Text = "Tipo de Documento (Salida):"
        '
        'cmbTipo
        '
        Me.cmbTipo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTipo.FormattingEnabled = True
        Me.cmbTipo.Location = New System.Drawing.Point(34, 130)
        Me.cmbTipo.Name = "cmbTipo"
        Me.cmbTipo.Size = New System.Drawing.Size(260, 25)
        Me.cmbTipo.TabIndex = 2
        '
        'lblNumero
        '
        Me.lblNumero.AutoSize = True
        Me.lblNumero.Location = New System.Drawing.Point(310, 110)
        Me.lblNumero.Name = "lblNumero"
        Me.lblNumero.Size = New System.Drawing.Size(166, 17)
        Me.lblNumero.TabIndex = 3
        Me.lblNumero.Text = "Número Oficial (Asignado):"
        '
        'txtNumero
        '
        Me.txtNumero.Location = New System.Drawing.Point(314, 130)
        Me.txtNumero.Name = "txtNumero"
        Me.txtNumero.Size = New System.Drawing.Size(240, 25)
        Me.txtNumero.TabIndex = 4
        '
        'lblAsunto
        '
        Me.lblAsunto.AutoSize = True
        Me.lblAsunto.Location = New System.Drawing.Point(31, 170)
        Me.lblAsunto.Name = "lblAsunto"
        Me.lblAsunto.Size = New System.Drawing.Size(202, 17)
        Me.lblAsunto.TabIndex = 5
        Me.lblAsunto.Text = "Asunto / Detalle de la Respuesta:"
        '
        'txtAsunto
        '
        Me.txtAsunto.Location = New System.Drawing.Point(34, 190)
        Me.txtAsunto.Multiline = True
        Me.txtAsunto.Name = "txtAsunto"
        Me.txtAsunto.Size = New System.Drawing.Size(520, 80)
        Me.txtAsunto.TabIndex = 6
        '
        'lblDestino
        '
        Me.lblDestino.AutoSize = True
        Me.lblDestino.Location = New System.Drawing.Point(31, 285)
        Me.lblDestino.Name = "lblDestino"
        Me.lblDestino.Size = New System.Drawing.Size(221, 17)
        Me.lblDestino.TabIndex = 7
        Me.lblDestino.Text = "Destino del Pase (A quién se envía):"
        '
        'cmbDestino
        '
        Me.cmbDestino.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmbDestino.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbDestino.FormattingEnabled = True
        Me.cmbDestino.Location = New System.Drawing.Point(34, 305)
        Me.cmbDestino.Name = "cmbDestino"
        Me.cmbDestino.Size = New System.Drawing.Size(520, 25)
        Me.cmbDestino.TabIndex = 8
        '
        'grpAdjunto
        '
        Me.grpAdjunto.Controls.Add(Me.lblArchivo)
        Me.grpAdjunto.Controls.Add(Me.btnAdjuntar)
        Me.grpAdjunto.Location = New System.Drawing.Point(34, 350)
        Me.grpAdjunto.Name = "grpAdjunto"
        Me.grpAdjunto.Size = New System.Drawing.Size(520, 70)
        Me.grpAdjunto.TabIndex = 9
        Me.grpAdjunto.TabStop = False
        Me.grpAdjunto.Text = "Copia Digital (Opcional)"
        '
        'lblArchivo
        '
        Me.lblArchivo.AutoSize = True
        Me.lblArchivo.ForeColor = System.Drawing.Color.Gray
        Me.lblArchivo.Location = New System.Drawing.Point(130, 32)
        Me.lblArchivo.Name = "lblArchivo"
        Me.lblArchivo.Size = New System.Drawing.Size(76, 17)
        Me.lblArchivo.TabIndex = 1
        Me.lblArchivo.Text = "Sin archivo."
        '
        'btnAdjuntar
        '
        Me.btnAdjuntar.Location = New System.Drawing.Point(20, 25)
        Me.btnAdjuntar.Name = "btnAdjuntar"
        Me.btnAdjuntar.Size = New System.Drawing.Size(100, 30)
        Me.btnAdjuntar.TabIndex = 0
        Me.btnAdjuntar.Text = "Adjuntar..."
        Me.btnAdjuntar.UseVisualStyleBackColor = True
        '
        'chkMoverOriginal
        '
        Me.chkMoverOriginal.AutoSize = True
        Me.chkMoverOriginal.Checked = True
        Me.chkMoverOriginal.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkMoverOriginal.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.chkMoverOriginal.ForeColor = System.Drawing.Color.DarkSlateBlue
        Me.chkMoverOriginal.Location = New System.Drawing.Point(34, 435)
        Me.chkMoverOriginal.Name = "chkMoverOriginal"
        Me.chkMoverOriginal.Size = New System.Drawing.Size(332, 19)
        Me.chkMoverOriginal.TabIndex = 10
        Me.chkMoverOriginal.Text = "Enviar el documento original junto con esta respuesta"
        Me.chkMoverOriginal.UseVisualStyleBackColor = True
        '
        'btnGuardar
        '
        Me.btnGuardar.BackColor = System.Drawing.Color.SeaGreen
        Me.btnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnGuardar.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnGuardar.ForeColor = System.Drawing.Color.White
        Me.btnGuardar.Location = New System.Drawing.Point(334, 480)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(220, 50)
        Me.btnGuardar.TabIndex = 11
        Me.btnGuardar.Text = "GENERAR Y DAR PASE"
        Me.btnGuardar.UseVisualStyleBackColor = False
        '
        'btnCancelar
        '
        Me.btnCancelar.Location = New System.Drawing.Point(34, 480)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(120, 50)
        Me.btnCancelar.TabIndex = 12
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'frmGenerarDocumento
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(600, 560)
        Me.Controls.Add(Me.chkMoverOriginal)
        Me.Controls.Add(Me.btnCancelar)
        Me.Controls.Add(Me.btnGuardar)
        Me.Controls.Add(Me.grpAdjunto)
        Me.Controls.Add(Me.cmbDestino)
        Me.Controls.Add(Me.lblDestino)
        Me.Controls.Add(Me.txtAsunto)
        Me.Controls.Add(Me.lblAsunto)
        Me.Controls.Add(Me.txtNumero)
        Me.Controls.Add(Me.lblNumero)
        Me.Controls.Add(Me.cmbTipo)
        Me.Controls.Add(Me.lblTipo)
        Me.Controls.Add(Me.pnlTop)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmGenerarDocumento"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Generar Documento Interno"
        Me.pnlTop.ResumeLayout(False)
        Me.pnlTop.PerformLayout()
        Me.grpAdjunto.ResumeLayout(False)
        Me.grpAdjunto.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents pnlTop As System.Windows.Forms.Panel
    Friend WithEvents lblTitulo As System.Windows.Forms.Label
    Friend WithEvents lblRefExterna As System.Windows.Forms.Label
    Friend WithEvents lblTipo As System.Windows.Forms.Label
    Friend WithEvents cmbTipo As System.Windows.Forms.ComboBox
    Friend WithEvents lblNumero As System.Windows.Forms.Label
    Friend WithEvents txtNumero As System.Windows.Forms.TextBox
    Friend WithEvents lblAsunto As System.Windows.Forms.Label
    Friend WithEvents txtAsunto As System.Windows.Forms.TextBox
    Friend WithEvents lblDestino As System.Windows.Forms.Label
    Friend WithEvents cmbDestino As System.Windows.Forms.ComboBox
    Friend WithEvents grpAdjunto As System.Windows.Forms.GroupBox
    Friend WithEvents lblArchivo As System.Windows.Forms.Label
    Friend WithEvents btnAdjuntar As System.Windows.Forms.Button
    Friend WithEvents btnGuardar As System.Windows.Forms.Button
    Friend WithEvents btnCancelar As System.Windows.Forms.Button
    Friend WithEvents chkMoverOriginal As System.Windows.Forms.CheckBox
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
End Class