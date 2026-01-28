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
        Me.lblFecha = New System.Windows.Forms.Label()
        Me.dtpFecha = New System.Windows.Forms.DateTimePicker()
        Me.lblDestino = New System.Windows.Forms.Label()
        Me.cmbDestino = New System.Windows.Forms.ComboBox()
        Me.chkEsSalida = New System.Windows.Forms.CheckBox()
        Me.btnGuardar = New System.Windows.Forms.Button()
        Me.btnCancelar = New System.Windows.Forms.Button()
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
        Me.lblDestino.Location = New System.Drawing.Point(20, 70)
        Me.lblDestino.Name = "lblDestino"
        Me.lblDestino.Size = New System.Drawing.Size(129, 13)
        Me.lblDestino.TabIndex = 2
        Me.lblDestino.Text = "Destino / Oficina (¿A dónde va?):"
        '
        'cmbDestino
        '
        Me.cmbDestino.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmbDestino.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbDestino.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.cmbDestino.FormattingEnabled = True
        Me.cmbDestino.Location = New System.Drawing.Point(23, 87)
        Me.cmbDestino.Name = "cmbDestino"
        Me.cmbDestino.Size = New System.Drawing.Size(300, 25)
        Me.cmbDestino.TabIndex = 3
        '
        'chkEsSalida
        '
        Me.chkEsSalida.AutoSize = True
        Me.chkEsSalida.Checked = True
        Me.chkEsSalida.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkEsSalida.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkEsSalida.ForeColor = System.Drawing.Color.Firebrick
        Me.chkEsSalida.Location = New System.Drawing.Point(23, 130)
        Me.chkEsSalida.Name = "chkEsSalida"
        Me.chkEsSalida.Size = New System.Drawing.Size(268, 17)
        Me.chkEsSalida.TabIndex = 4
        Me.chkEsSalida.Text = "¿Es una SALIDA DEFINITIVA del Penal?"
        Me.chkEsSalida.UseVisualStyleBackColor = True
        '
        'btnGuardar
        '
        Me.btnGuardar.BackColor = System.Drawing.Color.SteelBlue
        Me.btnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnGuardar.ForeColor = System.Drawing.Color.White
        Me.btnGuardar.Location = New System.Drawing.Point(173, 170)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(150, 40)
        Me.btnGuardar.TabIndex = 5
        Me.btnGuardar.Text = "CONFIRMAR PASE"
        Me.btnGuardar.UseVisualStyleBackColor = False
        '
        'btnCancelar
        '
        Me.btnCancelar.Location = New System.Drawing.Point(23, 170)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(100, 40)
        Me.btnCancelar.TabIndex = 6
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'frmNuevoPase
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(354, 231)
        Me.Controls.Add(Me.btnCancelar)
        Me.Controls.Add(Me.btnGuardar)
        Me.Controls.Add(Me.chkEsSalida)
        Me.Controls.Add(Me.cmbDestino)
        Me.Controls.Add(Me.lblDestino)
        Me.Controls.Add(Me.dtpFecha)
        Me.Controls.Add(Me.lblFecha)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmNuevoPase"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Registrar Movimiento"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblFecha As System.Windows.Forms.Label
    Friend WithEvents dtpFecha As System.Windows.Forms.DateTimePicker
    Friend WithEvents lblDestino As System.Windows.Forms.Label
    Friend WithEvents cmbDestino As System.Windows.Forms.ComboBox
    Friend WithEvents chkEsSalida As System.Windows.Forms.CheckBox
    Friend WithEvents btnGuardar As System.Windows.Forms.Button
    Friend WithEvents btnCancelar As System.Windows.Forms.Button
End Class