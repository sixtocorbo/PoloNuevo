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
        Me.lblNumero = New System.Windows.Forms.Label()
        Me.txtNumero = New System.Windows.Forms.TextBox()
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
        Me.lblTipo.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblTipo.Location = New System.Drawing.Point(20, 20)
        Me.lblTipo.Name = "lblTipo"
        Me.lblTipo.Size = New System.Drawing.Size(115, 15)
        Me.lblTipo.TabIndex = 0
        Me.lblTipo.Text = "Tipo de Documento:"
        '
        'cmbTipo
        '
        Me.cmbTipo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTipo.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.cmbTipo.FormattingEnabled = True
        Me.cmbTipo.Location = New System.Drawing.Point(23, 38)
        Me.cmbTipo.Name = "cmbTipo"
        Me.cmbTipo.Size = New System.Drawing.Size(340, 23)
        Me.cmbTipo.TabIndex = 1
        '
        'lblNumero
        '
        Me.lblNumero.AutoSize = True
        Me.lblNumero.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblNumero.Location = New System.Drawing.Point(20, 75)
        Me.lblNumero.Name = "lblNumero"
        Me.lblNumero.Size = New System.Drawing.Size(147, 15)
        Me.lblNumero.TabIndex = 2
        Me.lblNumero.Text = "Nro. Referencia / Externo:"
        '
        'txtNumero
        '
        Me.txtNumero.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.txtNumero.Location = New System.Drawing.Point(23, 93)
        Me.txtNumero.Name = "txtNumero"
        Me.txtNumero.Size = New System.Drawing.Size(340, 23)
        Me.txtNumero.TabIndex = 3
        '
        'lblDesc
        '
        Me.lblDesc.AutoSize = True
        Me.lblDesc.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblDesc.Location = New System.Drawing.Point(20, 130)
        Me.lblDesc.Name = "lblDesc"
        Me.lblDesc.Size = New System.Drawing.Size(117, 15)
        Me.lblDesc.TabIndex = 4
        Me.lblDesc.Text = "Descripción / Asunto:"
        '
        'txtDescripcion
        '
        Me.txtDescripcion.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.txtDescripcion.Location = New System.Drawing.Point(23, 148)
        Me.txtDescripcion.Multiline = True
        Me.txtDescripcion.Name = "txtDescripcion"
        Me.txtDescripcion.Size = New System.Drawing.Size(340, 60)
        Me.txtDescripcion.TabIndex = 5
        '
        'grpArchivo
        '
        Me.grpArchivo.Controls.Add(Me.lblArchivoSeleccionado)
        Me.grpArchivo.Controls.Add(Me.btnExaminar)
        Me.grpArchivo.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.grpArchivo.Location = New System.Drawing.Point(23, 225)
        Me.grpArchivo.Name = "grpArchivo"
        Me.grpArchivo.Size = New System.Drawing.Size(340, 80)
        Me.grpArchivo.TabIndex = 6
        Me.grpArchivo.TabStop = False
        Me.grpArchivo.Text = "Adjuntar Archivo Digital"
        '
        'lblArchivoSeleccionado
        '
        Me.lblArchivoSeleccionado.AutoEllipsis = True
        Me.lblArchivoSeleccionado.ForeColor = System.Drawing.Color.DimGray
        Me.lblArchivoSeleccionado.Location = New System.Drawing.Point(110, 30)
        Me.lblArchivoSeleccionado.Name = "lblArchivoSeleccionado"
        Me.lblArchivoSeleccionado.Size = New System.Drawing.Size(215, 35)
        Me.lblArchivoSeleccionado.TabIndex = 1
        Me.lblArchivoSeleccionado.Text = "Ningún archivo seleccionado"
        '
        'btnExaminar
        '
        Me.btnExaminar.Location = New System.Drawing.Point(15, 25)
        Me.btnExaminar.Name = "btnExaminar"
        Me.btnExaminar.Size = New System.Drawing.Size(89, 30)
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
        Me.btnGuardar.Location = New System.Drawing.Point(198, 325)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(165, 40)
        Me.btnGuardar.TabIndex = 7
        Me.btnGuardar.Text = "GUARDAR DOCUMENTO"
        Me.btnGuardar.UseVisualStyleBackColor = False
        '
        'btnCancelar
        '
        Me.btnCancelar.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnCancelar.Location = New System.Drawing.Point(23, 325)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(100, 40)
        Me.btnCancelar.TabIndex = 8
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'frmNuevoDocumento
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(394, 391)
        Me.Controls.Add(Me.btnCancelar)
        Me.Controls.Add(Me.btnGuardar)
        Me.Controls.Add(Me.grpArchivo)
        Me.Controls.Add(Me.txtDescripcion)
        Me.Controls.Add(Me.lblDesc)
        Me.Controls.Add(Me.txtNumero)
        Me.Controls.Add(Me.lblNumero)
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
    Friend WithEvents lblNumero As System.Windows.Forms.Label
    Friend WithEvents txtNumero As System.Windows.Forms.TextBox
    Friend WithEvents lblDesc As System.Windows.Forms.Label
    Friend WithEvents txtDescripcion As System.Windows.Forms.TextBox
    Friend WithEvents grpArchivo As System.Windows.Forms.GroupBox
    Friend WithEvents btnExaminar As System.Windows.Forms.Button
    Friend WithEvents lblArchivoSeleccionado As System.Windows.Forms.Label
    Friend WithEvents btnGuardar As System.Windows.Forms.Button
    Friend WithEvents btnCancelar As System.Windows.Forms.Button
End Class