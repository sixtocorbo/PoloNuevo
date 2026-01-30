<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmNuevoPase
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
        Me.lblFecha = New System.Windows.Forms.Label()
        Me.dtpFecha = New System.Windows.Forms.DateTimePicker()
        Me.lblDestino = New System.Windows.Forms.Label()
        Me.txtDestino = New System.Windows.Forms.TextBox()
        Me.lstSugerencias = New System.Windows.Forms.ListBox()
        Me.btnGuardar = New System.Windows.Forms.Button()
        Me.btnCancelar = New System.Windows.Forms.Button()
        Me.chkGenerarActuacion = New System.Windows.Forms.CheckBox()
        Me.grpActuacion = New System.Windows.Forms.GroupBox()
        Me.lblInfoRango = New System.Windows.Forms.Label()
        Me.lblArchivo = New System.Windows.Forms.Label()
        Me.btnAdjuntar = New System.Windows.Forms.Button()
        Me.txtAsunto = New System.Windows.Forms.TextBox()
        Me.lblAsunto = New System.Windows.Forms.Label()
        Me.txtNumero = New System.Windows.Forms.TextBox()
        Me.lblNumero = New System.Windows.Forms.Label()
        Me.cmbTipo = New System.Windows.Forms.ComboBox()
        Me.lblTipo = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.grpActuacion.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblFecha
        '
        Me.lblFecha.AutoSize = True
        Me.lblFecha.Location = New System.Drawing.Point(20, 20)
        Me.lblFecha.Name = "lblFecha"
        Me.lblFecha.Size = New System.Drawing.Size(40, 13)
        Me.lblFecha.TabIndex = 0
        Me.lblFecha.Text = "Fecha:"
        '
        'dtpFecha
        '
        Me.dtpFecha.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpFecha.Location = New System.Drawing.Point(23, 37)
        Me.dtpFecha.Name = "dtpFecha"
        Me.dtpFecha.Size = New System.Drawing.Size(120, 20)
        Me.dtpFecha.TabIndex = 1
        '
        'lblDestino
        '
        Me.lblDestino.AutoSize = True
        Me.lblDestino.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDestino.Location = New System.Drawing.Point(20, 70)
        Me.lblDestino.Name = "lblDestino"
        Me.lblDestino.Size = New System.Drawing.Size(200, 13)
        Me.lblDestino.TabIndex = 2
        Me.lblDestino.Text = "Destino (Escriba para buscar...):"
        '
        'txtDestino
        '
        Me.txtDestino.BackColor = System.Drawing.Color.AliceBlue
        Me.txtDestino.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.txtDestino.Location = New System.Drawing.Point(23, 87)
        Me.txtDestino.Name = "txtDestino"
        Me.txtDestino.Size = New System.Drawing.Size(300, 25)
        Me.txtDestino.TabIndex = 2
        '
        'lstSugerencias
        '
        Me.lstSugerencias.FormattingEnabled = True
        Me.lstSugerencias.Location = New System.Drawing.Point(23, 115)
        Me.lstSugerencias.Name = "lstSugerencias"
        Me.lstSugerencias.Size = New System.Drawing.Size(300, 82)
        Me.lstSugerencias.TabIndex = 3
        Me.lstSugerencias.Visible = False
        '
        'btnGuardar
        '
        Me.btnGuardar.BackColor = System.Drawing.Color.SteelBlue
        Me.btnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnGuardar.ForeColor = System.Drawing.Color.White
        Me.btnGuardar.Location = New System.Drawing.Point(173, 400)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(150, 40)
        Me.btnGuardar.TabIndex = 6
        Me.btnGuardar.Text = "CONFIRMAR"
        Me.btnGuardar.UseVisualStyleBackColor = False
        '
        'btnCancelar
        '
        Me.btnCancelar.Location = New System.Drawing.Point(23, 400)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(100, 40)
        Me.btnCancelar.TabIndex = 7
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'chkGenerarActuacion
        '
        Me.chkGenerarActuacion.AutoSize = True
        Me.chkGenerarActuacion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkGenerarActuacion.ForeColor = System.Drawing.Color.DarkBlue
        Me.chkGenerarActuacion.Location = New System.Drawing.Point(23, 130)
        Me.chkGenerarActuacion.Name = "chkGenerarActuacion"
        Me.chkGenerarActuacion.Size = New System.Drawing.Size(280, 17)
        Me.chkGenerarActuacion.TabIndex = 4
        Me.chkGenerarActuacion.Text = "Generar Actuación / Respuesta (Opcional)"
        Me.chkGenerarActuacion.UseVisualStyleBackColor = True
        '
        'grpActuacion
        '
        Me.grpActuacion.BackColor = System.Drawing.Color.WhiteSmoke
        Me.grpActuacion.Controls.Add(Me.lblInfoRango)
        Me.grpActuacion.Controls.Add(Me.lblArchivo)
        Me.grpActuacion.Controls.Add(Me.btnAdjuntar)
        Me.grpActuacion.Controls.Add(Me.txtAsunto)
        Me.grpActuacion.Controls.Add(Me.lblAsunto)
        Me.grpActuacion.Controls.Add(Me.txtNumero)
        Me.grpActuacion.Controls.Add(Me.lblNumero)
        Me.grpActuacion.Controls.Add(Me.cmbTipo)
        Me.grpActuacion.Controls.Add(Me.lblTipo)
        Me.grpActuacion.Location = New System.Drawing.Point(23, 155)
        Me.grpActuacion.Name = "grpActuacion"
        Me.grpActuacion.Size = New System.Drawing.Size(310, 230)
        Me.grpActuacion.TabIndex = 5
        Me.grpActuacion.TabStop = False
        Me.grpActuacion.Text = "Datos del Nuevo Documento"
        Me.grpActuacion.Visible = False
        '
        'lblInfoRango
        '
        Me.lblInfoRango.AutoSize = True
        Me.lblInfoRango.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInfoRango.ForeColor = System.Drawing.Color.DimGray
        Me.lblInfoRango.Location = New System.Drawing.Point(191, 65)
        Me.lblInfoRango.Name = "lblInfoRango"
        Me.lblInfoRango.Size = New System.Drawing.Size(10, 12)
        Me.lblInfoRango.TabIndex = 8
        Me.lblInfoRango.Text = "-"
        '
        'lblArchivo
        '
        Me.lblArchivo.AutoSize = True
        Me.lblArchivo.ForeColor = System.Drawing.Color.DimGray
        Me.lblArchivo.Location = New System.Drawing.Point(140, 169)
        Me.lblArchivo.Name = "lblArchivo"
        Me.lblArchivo.Size = New System.Drawing.Size(65, 13)
        Me.lblArchivo.TabIndex = 7
        Me.lblArchivo.Text = "Sin adjunto"
        '
        'btnAdjuntar
        '
        Me.btnAdjuntar.Location = New System.Drawing.Point(13, 160)
        Me.btnAdjuntar.Name = "btnAdjuntar"
        Me.btnAdjuntar.Size = New System.Drawing.Size(120, 30)
        Me.btnAdjuntar.TabIndex = 3
        Me.btnAdjuntar.Text = "Adjuntar Archivo..."
        Me.btnAdjuntar.UseVisualStyleBackColor = True
        '
        'txtAsunto
        '
        Me.txtAsunto.Location = New System.Drawing.Point(13, 96)
        Me.txtAsunto.Multiline = True
        Me.txtAsunto.Name = "txtAsunto"
        Me.txtAsunto.Size = New System.Drawing.Size(280, 50)
        Me.txtAsunto.TabIndex = 2
        '
        'lblAsunto
        '
        Me.lblAsunto.AutoSize = True
        Me.lblAsunto.Location = New System.Drawing.Point(10, 80)
        Me.lblAsunto.Name = "lblAsunto"
        Me.lblAsunto.Size = New System.Drawing.Size(43, 13)
        Me.lblAsunto.TabIndex = 4
        Me.lblAsunto.Text = "Asunto:"
        '
        'txtNumero
        '
        Me.txtNumero.BackColor = System.Drawing.Color.White
        Me.txtNumero.Location = New System.Drawing.Point(193, 42)
        Me.txtNumero.Name = "txtNumero"
        Me.txtNumero.ReadOnly = True
        Me.txtNumero.Size = New System.Drawing.Size(90, 20)
        Me.txtNumero.TabIndex = 1
        '
        'lblNumero
        '
        Me.lblNumero.AutoSize = True
        Me.lblNumero.Location = New System.Drawing.Point(190, 25)
        Me.lblNumero.Name = "lblNumero"
        Me.lblNumero.Size = New System.Drawing.Size(47, 13)
        Me.lblNumero.TabIndex = 2
        Me.lblNumero.Text = "Número:"
        '
        'cmbTipo
        '
        Me.cmbTipo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTipo.FormattingEnabled = True
        Me.cmbTipo.Location = New System.Drawing.Point(13, 41)
        Me.cmbTipo.Name = "cmbTipo"
        Me.cmbTipo.Size = New System.Drawing.Size(170, 21)
        Me.cmbTipo.TabIndex = 0
        '
        'lblTipo
        '
        Me.lblTipo.AutoSize = True
        Me.lblTipo.Location = New System.Drawing.Point(10, 25)
        Me.lblTipo.Name = "lblTipo"
        Me.lblTipo.Size = New System.Drawing.Size(31, 13)
        Me.lblTipo.TabIndex = 0
        Me.lblTipo.Text = "Tipo:"
        '
        'frmNuevoPase
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(354, 460)
        Me.Controls.Add(Me.lstSugerencias)
        Me.Controls.Add(Me.grpActuacion)
        Me.Controls.Add(Me.chkGenerarActuacion)
        Me.Controls.Add(Me.btnCancelar)
        Me.Controls.Add(Me.btnGuardar)
        Me.Controls.Add(Me.txtDestino)
        Me.Controls.Add(Me.lblDestino)
        Me.Controls.Add(Me.dtpFecha)
        Me.Controls.Add(Me.lblFecha)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmNuevoPase"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Gestión de Movimiento"
        Me.grpActuacion.ResumeLayout(False)
        Me.grpActuacion.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblFecha As System.Windows.Forms.Label
    Friend WithEvents dtpFecha As System.Windows.Forms.DateTimePicker
    Friend WithEvents lblDestino As System.Windows.Forms.Label
    Friend WithEvents txtDestino As System.Windows.Forms.TextBox
    Friend WithEvents lstSugerencias As System.Windows.Forms.ListBox
    Friend WithEvents btnGuardar As System.Windows.Forms.Button
    Friend WithEvents btnCancelar As System.Windows.Forms.Button
    Friend WithEvents chkGenerarActuacion As System.Windows.Forms.CheckBox
    Friend WithEvents grpActuacion As System.Windows.Forms.GroupBox
    Friend WithEvents cmbTipo As System.Windows.Forms.ComboBox
    Friend WithEvents lblTipo As System.Windows.Forms.Label
    Friend WithEvents txtNumero As System.Windows.Forms.TextBox
    Friend WithEvents lblNumero As System.Windows.Forms.Label
    Friend WithEvents txtAsunto As System.Windows.Forms.TextBox
    Friend WithEvents lblAsunto As System.Windows.Forms.Label
    Friend WithEvents btnAdjuntar As System.Windows.Forms.Button
    Friend WithEvents lblArchivo As System.Windows.Forms.Label
    Friend WithEvents lblInfoRango As System.Windows.Forms.Label
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
End Class