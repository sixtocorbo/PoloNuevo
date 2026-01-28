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
        Me.lblFecha = New System.Windows.Forms.Label()
        Me.dtpFecha = New System.Windows.Forms.DateTimePicker()
        Me.lblOrigen = New System.Windows.Forms.Label()
        Me.txtOrigen = New System.Windows.Forms.TextBox()

        ' --- NUEVO CONTROL PARA TIPO ---
        Me.lblTipo = New System.Windows.Forms.Label()
        Me.cmbTipo = New System.Windows.Forms.ComboBox()
        ' -------------------------------

        Me.lblReferencia = New System.Windows.Forms.Label()
        Me.txtReferencia = New System.Windows.Forms.TextBox()
        Me.lblAsunto = New System.Windows.Forms.Label()
        Me.txtAsunto = New System.Windows.Forms.TextBox()
        Me.chkVincular = New System.Windows.Forms.CheckBox()
        Me.txtBuscarRecluso = New System.Windows.Forms.TextBox()
        Me.cmbReclusos = New System.Windows.Forms.ComboBox()
        Me.btnGuardar = New System.Windows.Forms.Button()
        Me.btnCancelar = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'lblFecha
        '
        Me.lblFecha.AutoSize = True
        Me.lblFecha.Location = New System.Drawing.Point(20, 20)
        Me.lblFecha.Name = "lblFecha"
        Me.lblFecha.Size = New System.Drawing.Size(40, 13)
        Me.lblFecha.Text = "Fecha:"
        '
        'dtpFecha
        '
        Me.dtpFecha.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpFecha.Location = New System.Drawing.Point(23, 37)
        Me.dtpFecha.Name = "dtpFecha"
        Me.dtpFecha.Size = New System.Drawing.Size(120, 20)
        '
        'lblOrigen
        '
        Me.lblOrigen.AutoSize = True
        Me.lblOrigen.Location = New System.Drawing.Point(160, 20)
        Me.lblOrigen.Name = "lblOrigen"
        Me.lblOrigen.Size = New System.Drawing.Size(148, 13)
        Me.lblOrigen.Text = "Origen (¿De dónde viene?):"
        '
        'txtOrigen
        '
        Me.txtOrigen.Location = New System.Drawing.Point(163, 37)
        Me.txtOrigen.Name = "txtOrigen"
        Me.txtOrigen.Size = New System.Drawing.Size(240, 20)

        ' 
        ' lblTipo (NUEVO)
        ' 
        Me.lblTipo.AutoSize = True
        Me.lblTipo.Location = New System.Drawing.Point(20, 70)
        Me.lblTipo.Name = "lblTipo"
        Me.lblTipo.Size = New System.Drawing.Size(31, 13)
        Me.lblTipo.Text = "Tipo:"
        ' 
        ' cmbTipo (NUEVO)
        ' 
        Me.cmbTipo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTipo.FormattingEnabled = True
        Me.cmbTipo.Location = New System.Drawing.Point(23, 86)
        Me.cmbTipo.Name = "cmbTipo"
        Me.cmbTipo.Size = New System.Drawing.Size(120, 21)

        '
        'lblReferencia (MOVIDO A LA DERECHA)
        '
        Me.lblReferencia.AutoSize = True
        Me.lblReferencia.Location = New System.Drawing.Point(160, 70)
        Me.lblReferencia.Name = "lblReferencia"
        Me.lblReferencia.Size = New System.Drawing.Size(125, 13)
        Me.lblReferencia.Text = "Nro. / Referencia:"
        '
        'txtReferencia
        '
        Me.txtReferencia.Location = New System.Drawing.Point(163, 86)
        Me.txtReferencia.Name = "txtReferencia"
        Me.txtReferencia.Size = New System.Drawing.Size(240, 20)
        '
        'lblAsunto
        '
        Me.lblAsunto.AutoSize = True
        Me.lblAsunto.Location = New System.Drawing.Point(20, 120) ' Bajamos un poco
        Me.lblAsunto.Name = "lblAsunto"
        Me.lblAsunto.Size = New System.Drawing.Size(43, 13)
        Me.lblAsunto.Text = "Asunto / Descripción:"
        '
        'txtAsunto
        '
        Me.txtAsunto.Location = New System.Drawing.Point(23, 137)
        Me.txtAsunto.Multiline = True
        Me.txtAsunto.Name = "txtAsunto"
        Me.txtAsunto.Size = New System.Drawing.Size(380, 50)
        '
        'chkVincular
        '
        Me.chkVincular.AutoSize = True
        Me.chkVincular.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkVincular.ForeColor = System.Drawing.Color.SteelBlue
        Me.chkVincular.Location = New System.Drawing.Point(23, 200)
        Me.chkVincular.Name = "chkVincular"
        Me.chkVincular.Size = New System.Drawing.Size(166, 17)
        Me.chkVincular.Text = "¿Vincular a un Recluso?"
        Me.chkVincular.UseVisualStyleBackColor = True
        '
        'txtBuscarRecluso
        '
        Me.txtBuscarRecluso.BackColor = System.Drawing.Color.LemonChiffon
        Me.txtBuscarRecluso.Enabled = False
        Me.txtBuscarRecluso.ForeColor = System.Drawing.Color.Gray
        Me.txtBuscarRecluso.Location = New System.Drawing.Point(23, 225)
        Me.txtBuscarRecluso.Name = "txtBuscarRecluso"
        Me.txtBuscarRecluso.Size = New System.Drawing.Size(380, 20)
        Me.txtBuscarRecluso.Text = "Escriba para buscar..."
        '
        'cmbReclusos
        '
        Me.cmbReclusos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbReclusos.Enabled = False
        Me.cmbReclusos.FormattingEnabled = True
        Me.cmbReclusos.Location = New System.Drawing.Point(23, 251)
        Me.cmbReclusos.Name = "cmbReclusos"
        Me.cmbReclusos.Size = New System.Drawing.Size(380, 21)
        Me.cmbReclusos.MaxDropDownItems = 10
        Me.cmbReclusos.IntegralHeight = False
        '
        'btnGuardar
        '
        Me.btnGuardar.BackColor = System.Drawing.Color.SeaGreen
        Me.btnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnGuardar.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnGuardar.ForeColor = System.Drawing.Color.White
        Me.btnGuardar.Location = New System.Drawing.Point(238, 300)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(165, 40)
        Me.btnGuardar.Text = "GUARDAR INGRESO"
        Me.btnGuardar.UseVisualStyleBackColor = False
        '
        'btnCancelar
        '
        Me.btnCancelar.Location = New System.Drawing.Point(23, 300)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(100, 40)
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.Color.DimGray
        Me.Label1.Location = New System.Drawing.Point(298, 201)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(105, 13)
        Me.Label1.Text = "(Buscar por nombre)"
        '
        'frmNuevoIngreso
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(434, 360) ' Un poco más alto
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnCancelar)
        Me.Controls.Add(Me.btnGuardar)
        Me.Controls.Add(Me.cmbReclusos)
        Me.Controls.Add(Me.txtBuscarRecluso)
        Me.Controls.Add(Me.chkVincular)
        Me.Controls.Add(Me.txtAsunto)
        Me.Controls.Add(Me.lblAsunto)
        Me.Controls.Add(Me.txtReferencia)
        Me.Controls.Add(Me.lblReferencia)
        Me.Controls.Add(Me.cmbTipo) ' NUEVO
        Me.Controls.Add(Me.lblTipo) ' NUEVO
        Me.Controls.Add(Me.txtOrigen)
        Me.Controls.Add(Me.lblOrigen)
        Me.Controls.Add(Me.dtpFecha)
        Me.Controls.Add(Me.lblFecha)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmNuevoIngreso"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Mesa de Entrada: Nuevo Registro"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblFecha As System.Windows.Forms.Label
    Friend WithEvents dtpFecha As System.Windows.Forms.DateTimePicker
    Friend WithEvents lblOrigen As System.Windows.Forms.Label
    Friend WithEvents txtOrigen As System.Windows.Forms.TextBox
    Friend WithEvents lblReferencia As System.Windows.Forms.Label
    Friend WithEvents txtReferencia As System.Windows.Forms.TextBox
    Friend WithEvents lblAsunto As System.Windows.Forms.Label
    Friend WithEvents txtAsunto As System.Windows.Forms.TextBox
    Friend WithEvents chkVincular As System.Windows.Forms.CheckBox
    Friend WithEvents txtBuscarRecluso As System.Windows.Forms.TextBox
    Friend WithEvents cmbReclusos As System.Windows.Forms.ComboBox
    Friend WithEvents btnGuardar As System.Windows.Forms.Button
    Friend WithEvents btnCancelar As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblTipo As System.Windows.Forms.Label
    Friend WithEvents cmbTipo As System.Windows.Forms.ComboBox
End Class