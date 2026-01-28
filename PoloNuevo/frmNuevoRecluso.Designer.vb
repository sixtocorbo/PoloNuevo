<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmNuevoRecluso
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
        Me.lblNombre = New System.Windows.Forms.Label()
        Me.txtNombre = New System.Windows.Forms.TextBox()
        Me.lblCedula = New System.Windows.Forms.Label()
        Me.txtCedula = New System.Windows.Forms.TextBox()
        Me.lblFechaNac = New System.Windows.Forms.Label()
        Me.dtpNacimiento = New System.Windows.Forms.DateTimePicker()
        Me.grpEstado = New System.Windows.Forms.GroupBox()
        Me.radInactivo = New System.Windows.Forms.RadioButton()
        Me.radActivo = New System.Windows.Forms.RadioButton()
        Me.btnGuardar = New System.Windows.Forms.Button()
        Me.btnCancelar = New System.Windows.Forms.Button()
        Me.grpEstado.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblNombre
        '
        Me.lblNombre.AutoSize = True
        Me.lblNombre.Location = New System.Drawing.Point(20, 20)
        Me.lblNombre.Name = "lblNombre"
        Me.lblNombre.Size = New System.Drawing.Size(94, 13)
        Me.lblNombre.TabIndex = 0
        Me.lblNombre.Text = "Nombre Completo:"
        '
        'txtNombre
        '
        Me.txtNombre.Location = New System.Drawing.Point(23, 37)
        Me.txtNombre.Name = "txtNombre"
        Me.txtNombre.Size = New System.Drawing.Size(300, 20)
        Me.txtNombre.TabIndex = 0
        '
        'lblCedula
        '
        Me.lblCedula.AutoSize = True
        Me.lblCedula.Location = New System.Drawing.Point(20, 70)
        Me.lblCedula.Name = "lblCedula"
        Me.lblCedula.Size = New System.Drawing.Size(98, 13)
        Me.lblCedula.TabIndex = 2
        Me.lblCedula.Text = "Cédula Identidad:"
        '
        'txtCedula
        '
        Me.txtCedula.Location = New System.Drawing.Point(23, 87)
        Me.txtCedula.Name = "txtCedula"
        Me.txtCedula.Size = New System.Drawing.Size(150, 20)
        Me.txtCedula.TabIndex = 1
        '
        'lblFechaNac
        '
        Me.lblFechaNac.AutoSize = True
        Me.lblFechaNac.Location = New System.Drawing.Point(20, 120)
        Me.lblFechaNac.Name = "lblFechaNac"
        Me.lblFechaNac.Size = New System.Drawing.Size(111, 13)
        Me.lblFechaNac.TabIndex = 4
        Me.lblFechaNac.Text = "Fecha de Nacimiento:"
        '
        'dtpNacimiento
        '
        Me.dtpNacimiento.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpNacimiento.Location = New System.Drawing.Point(23, 137)
        Me.dtpNacimiento.Name = "dtpNacimiento"
        Me.dtpNacimiento.Size = New System.Drawing.Size(150, 20)
        Me.dtpNacimiento.TabIndex = 2
        '
        'grpEstado
        '
        Me.grpEstado.Controls.Add(Me.radInactivo)
        Me.grpEstado.Controls.Add(Me.radActivo)
        Me.grpEstado.Location = New System.Drawing.Point(23, 175)
        Me.grpEstado.Name = "grpEstado"
        Me.grpEstado.Size = New System.Drawing.Size(300, 50)
        Me.grpEstado.TabIndex = 3
        Me.grpEstado.TabStop = False
        Me.grpEstado.Text = "Estado Actual"
        '
        'radInactivo
        '
        Me.radInactivo.AutoSize = True
        Me.radInactivo.ForeColor = System.Drawing.Color.DimGray
        Me.radInactivo.Location = New System.Drawing.Point(120, 20)
        Me.radInactivo.Name = "radInactivo"
        Me.radInactivo.Size = New System.Drawing.Size(107, 17)
        Me.radInactivo.TabIndex = 1
        Me.radInactivo.Text = "Inactivo / Libertad"
        Me.radInactivo.UseVisualStyleBackColor = True
        '
        'radActivo
        '
        Me.radActivo.AutoSize = True
        Me.radActivo.Checked = True
        Me.radActivo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.radActivo.ForeColor = System.Drawing.Color.SeaGreen
        Me.radActivo.Location = New System.Drawing.Point(20, 20)
        Me.radActivo.Name = "radActivo"
        Me.radActivo.Size = New System.Drawing.Size(61, 17)
        Me.radActivo.TabIndex = 0
        Me.radActivo.TabStop = True
        Me.radActivo.Text = "Activo"
        Me.radActivo.UseVisualStyleBackColor = True
        '
        'btnGuardar
        '
        Me.btnGuardar.BackColor = System.Drawing.Color.SteelBlue
        Me.btnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnGuardar.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnGuardar.ForeColor = System.Drawing.Color.White
        Me.btnGuardar.Location = New System.Drawing.Point(180, 245)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(143, 40)
        Me.btnGuardar.TabIndex = 4
        Me.btnGuardar.Text = "GUARDAR FICHA"
        Me.btnGuardar.UseVisualStyleBackColor = False
        '
        'btnCancelar
        '
        Me.btnCancelar.Location = New System.Drawing.Point(23, 245)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(100, 40)
        Me.btnCancelar.TabIndex = 5
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'frmNuevoRecluso
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(354, 311)
        Me.Controls.Add(Me.btnCancelar)
        Me.Controls.Add(Me.btnGuardar)
        Me.Controls.Add(Me.grpEstado)
        Me.Controls.Add(Me.dtpNacimiento)
        Me.Controls.Add(Me.lblFechaNac)
        Me.Controls.Add(Me.txtCedula)
        Me.Controls.Add(Me.lblCedula)
        Me.Controls.Add(Me.txtNombre)
        Me.Controls.Add(Me.lblNombre)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmNuevoRecluso"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Ficha de Recluso"
        Me.grpEstado.ResumeLayout(False)
        Me.grpEstado.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblNombre As System.Windows.Forms.Label
    Friend WithEvents txtNombre As System.Windows.Forms.TextBox
    Friend WithEvents lblCedula As System.Windows.Forms.Label
    Friend WithEvents txtCedula As System.Windows.Forms.TextBox
    Friend WithEvents lblFechaNac As System.Windows.Forms.Label
    Friend WithEvents dtpNacimiento As System.Windows.Forms.DateTimePicker
    Friend WithEvents grpEstado As System.Windows.Forms.GroupBox
    Friend WithEvents radInactivo As System.Windows.Forms.RadioButton
    Friend WithEvents radActivo As System.Windows.Forms.RadioButton
    Friend WithEvents btnGuardar As System.Windows.Forms.Button
    Friend WithEvents btnCancelar As System.Windows.Forms.Button
End Class