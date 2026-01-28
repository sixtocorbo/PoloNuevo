<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmNuevoDocumento
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
        Me.lblTipo = New System.Windows.Forms.Label()
        Me.cmbTipo = New System.Windows.Forms.ComboBox()
        Me.lblDesc = New System.Windows.Forms.Label()
        Me.txtDescripcion = New System.Windows.Forms.TextBox()
        Me.grpArchivo = New System.Windows.Forms.GroupBox()
        Me.lblArchivoSeleccionado = New System.Windows.Forms.Label()
        Me.btnExaminar = New System.Windows.Forms.Button()
        Me.btnGuardar = New System.Windows.Forms.Button()
        Me.btnCancelar = New System.Windows.Forms.Button()
        Me.grpArchivo.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblTipo
        '
        Me.lblTipo.AutoSize = True
        Me.lblTipo.Location = New System.Drawing.Point(20, 20)
        Me.lblTipo.Name = "lblTipo"
        Me.lblTipo.Size = New System.Drawing.Size(104, 13)
        Me.lblTipo.TabIndex = 0
        Me.lblTipo.Text = "Tipo de Documento:"
        '
        'cmbTipo
        '
        Me.cmbTipo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTipo.FormattingEnabled = True
        Me.cmbTipo.Location = New System.Drawing.Point(23, 37)
        Me.cmbTipo.Name = "cmbTipo"
        Me.cmbTipo.Size = New System.Drawing.Size(340, 21)
        Me.cmbTipo.TabIndex = 1
        '
        'lblDesc
        '
        Me.lblDesc.AutoSize = True
        Me.lblDesc.Location = New System.Drawing.Point(20, 75)
        Me.lblDesc.Name = "lblDesc"
        Me.lblDesc.Size = New System.Drawing.Size(161, 13)
        Me.lblDesc.TabIndex = 2
        Me.lblDesc.Text = "Descripción / Referencia / Nro:"
        '
        'txtDescripcion
        '
        Me.txtDescripcion.Location = New System.Drawing.Point(23, 92)
        Me.txtDescripcion.Multiline = True
        Me.txtDescripcion.Name = "txtDescripcion"
        Me.txtDescripcion.Size = New System.Drawing.Size(340, 60)
        Me.txtDescripcion.TabIndex = 3
        '
        'grpArchivo
        '
        Me.grpArchivo.Controls.Add(Me.lblArchivoSeleccionado)
        Me.grpArchivo.Controls.Add(Me.btnExaminar)
        Me.grpArchivo.Location = New System.Drawing.Point(23, 170)
        Me.grpArchivo.Name = "grpArchivo"
        Me.grpArchivo.Size = New System.Drawing.Size(340, 80)
        Me.grpArchivo.TabIndex = 4
        Me.grpArchivo.TabStop = False
        Me.grpArchivo.Text = "Adjuntar Archivo (PDF o Imagen)"
        '
        'lblArchivoSeleccionado
        '
        Me.lblArchivoSeleccionado.AutoEllipsis = True
        Me.lblArchivoSeleccionado.ForeColor = System.Drawing.Color.DimGray
        Me.lblArchivoSeleccionado.Location = New System.Drawing.Point(105, 30)
        Me.lblArchivoSeleccionado.Name = "lblArchivoSeleccionado"
        Me.lblArchivoSeleccionado.Size = New System.Drawing.Size(220, 35)
        Me.lblArchivoSeleccionado.TabIndex = 1
        Me.lblArchivoSeleccionado.Text = "Ningún archivo seleccionado"
        '
        'btnExaminar
        '
        Me.btnExaminar.Location = New System.Drawing.Point(15, 25)
        Me.btnExaminar.Name = "btnExaminar"
        Me.btnExaminar.Size = New System.Drawing.Size(84, 30)
        Me.btnExaminar.TabIndex = 0
        Me.btnExaminar.Text = "Examinar..."
        Me.btnExaminar.UseVisualStyleBackColor = True
        '
        'btnGuardar
        '
        Me.btnGuardar.BackColor = System.Drawing.Color.SteelBlue
        Me.btnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnGuardar.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnGuardar.ForeColor = System.Drawing.Color.White
        Me.btnGuardar.Location = New System.Drawing.Point(198, 270)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(165, 40)
        Me.btnGuardar.TabIndex = 5
        Me.btnGuardar.Text = "GUARDAR DOCUMENTO"
        Me.btnGuardar.UseVisualStyleBackColor = False
        '
        'btnCancelar
        '
        Me.btnCancelar.Location = New System.Drawing.Point(23, 270)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(100, 40)
        Me.btnCancelar.TabIndex = 6
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'frmNuevoDocumento
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(394, 331)
        Me.Controls.Add(Me.btnCancelar)
        Me.Controls.Add(Me.btnGuardar)
        Me.Controls.Add(Me.grpArchivo)
        Me.Controls.Add(Me.txtDescripcion)
        Me.Controls.Add(Me.lblDesc)
        Me.Controls.Add(Me.cmbTipo)
        Me.Controls.Add(Me.lblTipo)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmNuevoDocumento"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Cargar Nuevo Documento"
        Me.grpArchivo.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblTipo As System.Windows.Forms.Label
    Friend WithEvents cmbTipo As System.Windows.Forms.ComboBox
    Friend WithEvents lblDesc As System.Windows.Forms.Label
    Friend WithEvents txtDescripcion As System.Windows.Forms.TextBox
    Friend WithEvents grpArchivo As System.Windows.Forms.GroupBox
    Friend WithEvents btnExaminar As System.Windows.Forms.Button
    Friend WithEvents lblArchivoSeleccionado As System.Windows.Forms.Label
    Friend WithEvents btnGuardar As System.Windows.Forms.Button
    Friend WithEvents btnCancelar As System.Windows.Forms.Button
End Class