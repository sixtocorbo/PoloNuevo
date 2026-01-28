<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmAsignarTrabajo
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
        Me.lblRecluso = New System.Windows.Forms.Label()
        Me.cmbReclusos = New System.Windows.Forms.ComboBox()
        Me.lblTarea = New System.Windows.Forms.Label()
        Me.cmbTareas = New System.Windows.Forms.ComboBox()
        Me.lblFecha = New System.Windows.Forms.Label()
        Me.dtpInicio = New System.Windows.Forms.DateTimePicker()
        Me.lblObs = New System.Windows.Forms.Label()
        Me.txtObservaciones = New System.Windows.Forms.TextBox()
        Me.btnGuardar = New System.Windows.Forms.Button()
        Me.btnCancelar = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'lblRecluso
        '
        Me.lblRecluso.AutoSize = True
        Me.lblRecluso.Location = New System.Drawing.Point(20, 20)
        Me.lblRecluso.Name = "lblRecluso"
        Me.lblRecluso.Size = New System.Drawing.Size(50, 13)
        Me.lblRecluso.TabIndex = 0
        Me.lblRecluso.Text = "Recluso:"
        '
        'cmbReclusos
        '
        Me.cmbReclusos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbReclusos.FormattingEnabled = True
        Me.cmbReclusos.Location = New System.Drawing.Point(23, 37)
        Me.cmbReclusos.Name = "cmbReclusos"
        Me.cmbReclusos.Size = New System.Drawing.Size(300, 21)
        Me.cmbReclusos.TabIndex = 1
        '
        'lblTarea
        '
        Me.lblTarea.AutoSize = True
        Me.lblTarea.Location = New System.Drawing.Point(20, 70)
        Me.lblTarea.Name = "lblTarea"
        Me.lblTarea.Size = New System.Drawing.Size(38, 13)
        Me.lblTarea.TabIndex = 2
        Me.lblTarea.Text = "Tarea:"
        '
        'cmbTareas
        '
        Me.cmbTareas.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTareas.FormattingEnabled = True
        Me.cmbTareas.Location = New System.Drawing.Point(23, 87)
        Me.cmbTareas.Name = "cmbTareas"
        Me.cmbTareas.Size = New System.Drawing.Size(300, 21)
        Me.cmbTareas.TabIndex = 3
        '
        'lblFecha
        '
        Me.lblFecha.AutoSize = True
        Me.lblFecha.Location = New System.Drawing.Point(20, 120)
        Me.lblFecha.Name = "lblFecha"
        Me.lblFecha.Size = New System.Drawing.Size(83, 13)
        Me.lblFecha.TabIndex = 4
        Me.lblFecha.Text = "Fecha de Inicio:"
        '
        'dtpInicio
        '
        Me.dtpInicio.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpInicio.Location = New System.Drawing.Point(23, 137)
        Me.dtpInicio.Name = "dtpInicio"
        Me.dtpInicio.Size = New System.Drawing.Size(120, 20)
        Me.dtpInicio.TabIndex = 5
        '
        'lblObs
        '
        Me.lblObs.AutoSize = True
        Me.lblObs.Location = New System.Drawing.Point(20, 170)
        Me.lblObs.Name = "lblObs"
        Me.lblObs.Size = New System.Drawing.Size(123, 13)
        Me.lblObs.TabIndex = 6
        Me.lblObs.Text = "Observaciones / Detalle:"
        '
        'txtObservaciones
        '
        Me.txtObservaciones.Location = New System.Drawing.Point(23, 187)
        Me.txtObservaciones.Multiline = True
        Me.txtObservaciones.Name = "txtObservaciones"
        Me.txtObservaciones.Size = New System.Drawing.Size(300, 60)
        Me.txtObservaciones.TabIndex = 7
        '
        'btnGuardar
        '
        Me.btnGuardar.BackColor = System.Drawing.Color.SeaGreen
        Me.btnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnGuardar.ForeColor = System.Drawing.Color.White
        Me.btnGuardar.Location = New System.Drawing.Point(173, 270)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(150, 40)
        Me.btnGuardar.TabIndex = 8
        Me.btnGuardar.Text = "CONFIRMAR"
        Me.btnGuardar.UseVisualStyleBackColor = False
        '
        'btnCancelar
        '
        Me.btnCancelar.Location = New System.Drawing.Point(23, 270)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(100, 40)
        Me.btnCancelar.TabIndex = 9
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'frmAsignarTrabajo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(354, 331)
        Me.Controls.Add(Me.btnCancelar)
        Me.Controls.Add(Me.btnGuardar)
        Me.Controls.Add(Me.txtObservaciones)
        Me.Controls.Add(Me.lblObs)
        Me.Controls.Add(Me.dtpInicio)
        Me.Controls.Add(Me.lblFecha)
        Me.Controls.Add(Me.cmbTareas)
        Me.Controls.Add(Me.lblTarea)
        Me.Controls.Add(Me.cmbReclusos)
        Me.Controls.Add(Me.lblRecluso)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmAsignarTrabajo"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Asignar Comisión Laboral"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblRecluso As System.Windows.Forms.Label
    Friend WithEvents cmbReclusos As System.Windows.Forms.ComboBox
    Friend WithEvents lblTarea As System.Windows.Forms.Label
    Friend WithEvents cmbTareas As System.Windows.Forms.ComboBox
    Friend WithEvents lblFecha As System.Windows.Forms.Label
    Friend WithEvents dtpInicio As System.Windows.Forms.DateTimePicker
    Friend WithEvents lblObs As System.Windows.Forms.Label
    Friend WithEvents txtObservaciones As System.Windows.Forms.TextBox
    Friend WithEvents btnGuardar As System.Windows.Forms.Button
    Friend WithEvents btnCancelar As System.Windows.Forms.Button
End Class