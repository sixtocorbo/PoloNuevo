<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmUsuarios
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
        Me.grpNuevo = New System.Windows.Forms.GroupBox()
        Me.lblNombre = New System.Windows.Forms.Label()
        Me.txtNombre = New System.Windows.Forms.TextBox()
        Me.lblClave = New System.Windows.Forms.Label()
        Me.txtClave = New System.Windows.Forms.TextBox()
        Me.lblRol = New System.Windows.Forms.Label()
        Me.cmbRol = New System.Windows.Forms.ComboBox()
        Me.btnGuardar = New System.Windows.Forms.Button()
        Me.dgvUsuarios = New System.Windows.Forms.DataGridView()
        Me.btnEliminar = New System.Windows.Forms.Button()
        Me.lblTituloLista = New System.Windows.Forms.Label()
        Me.grpNuevo.SuspendLayout()
        CType(Me.dgvUsuarios, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grpNuevo
        '
        Me.grpNuevo.Controls.Add(Me.btnGuardar)
        Me.grpNuevo.Controls.Add(Me.cmbRol)
        Me.grpNuevo.Controls.Add(Me.lblRol)
        Me.grpNuevo.Controls.Add(Me.txtClave)
        Me.grpNuevo.Controls.Add(Me.lblClave)
        Me.grpNuevo.Controls.Add(Me.txtNombre)
        Me.grpNuevo.Controls.Add(Me.lblNombre)
        Me.grpNuevo.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.grpNuevo.Location = New System.Drawing.Point(12, 12)
        Me.grpNuevo.Name = "grpNuevo"
        Me.grpNuevo.Size = New System.Drawing.Size(250, 300)
        Me.grpNuevo.TabIndex = 0
        Me.grpNuevo.TabStop = False
        Me.grpNuevo.Text = "Nuevo Usuario"
        '
        'lblNombre
        '
        Me.lblNombre.AutoSize = True
        Me.lblNombre.Location = New System.Drawing.Point(15, 30)
        Me.lblNombre.Name = "lblNombre"
        Me.lblNombre.Size = New System.Drawing.Size(54, 15)
        Me.lblNombre.TabIndex = 0
        Me.lblNombre.Text = "Nombre:"
        '
        'txtNombre
        '
        Me.txtNombre.Location = New System.Drawing.Point(18, 48)
        Me.txtNombre.Name = "txtNombre"
        Me.txtNombre.Size = New System.Drawing.Size(215, 23)
        Me.txtNombre.TabIndex = 1
        '
        'lblClave
        '
        Me.lblClave.AutoSize = True
        Me.lblClave.Location = New System.Drawing.Point(15, 90)
        Me.lblClave.Name = "lblClave"
        Me.lblClave.Size = New System.Drawing.Size(70, 15)
        Me.lblClave.TabIndex = 2
        Me.lblClave.Text = "Contraseña:"
        '
        'txtClave
        '
        Me.txtClave.Location = New System.Drawing.Point(18, 108)
        Me.txtClave.Name = "txtClave"
        Me.txtClave.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtClave.Size = New System.Drawing.Size(215, 23)
        Me.txtClave.TabIndex = 3
        '
        'lblRol
        '
        Me.lblRol.AutoSize = True
        Me.lblRol.Location = New System.Drawing.Point(15, 150)
        Me.lblRol.Name = "lblRol"
        Me.lblRol.Size = New System.Drawing.Size(27, 15)
        Me.lblRol.TabIndex = 4
        Me.lblRol.Text = "Rol:"
        '
        'cmbRol
        '
        Me.cmbRol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbRol.FormattingEnabled = True
        Me.cmbRol.Items.AddRange(New Object() {"OPERADOR", "ADMINISTRADOR", "CONSULTA"})
        Me.cmbRol.Location = New System.Drawing.Point(18, 168)
        Me.cmbRol.Name = "cmbRol"
        Me.cmbRol.Size = New System.Drawing.Size(215, 23)
        Me.cmbRol.TabIndex = 5
        '
        'btnGuardar
        '
        Me.btnGuardar.BackColor = System.Drawing.Color.SeaGreen
        Me.btnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnGuardar.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnGuardar.ForeColor = System.Drawing.Color.White
        Me.btnGuardar.Location = New System.Drawing.Point(18, 220)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(215, 40)
        Me.btnGuardar.TabIndex = 6
        Me.btnGuardar.Text = "CREAR USUARIO"
        Me.btnGuardar.UseVisualStyleBackColor = False
        '
        'dgvUsuarios
        '
        Me.dgvUsuarios.AllowUserToAddRows = False
        Me.dgvUsuarios.AllowUserToDeleteRows = False
        Me.dgvUsuarios.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvUsuarios.BackgroundColor = System.Drawing.Color.White
        Me.dgvUsuarios.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvUsuarios.Location = New System.Drawing.Point(280, 48)
        Me.dgvUsuarios.Name = "dgvUsuarios"
        Me.dgvUsuarios.ReadOnly = True
        Me.dgvUsuarios.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvUsuarios.Size = New System.Drawing.Size(400, 224)
        Me.dgvUsuarios.TabIndex = 1
        '
        'lblTituloLista
        '
        Me.lblTituloLista.AutoSize = True
        Me.lblTituloLista.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblTituloLista.Location = New System.Drawing.Point(276, 26)
        Me.lblTituloLista.Name = "lblTituloLista"
        Me.lblTituloLista.Size = New System.Drawing.Size(127, 19)
        Me.lblTituloLista.TabIndex = 2
        Me.lblTituloLista.Text = "Usuarios Actuales"
        '
        'btnEliminar
        '
        Me.btnEliminar.BackColor = System.Drawing.Color.IndianRed
        Me.btnEliminar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnEliminar.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnEliminar.ForeColor = System.Drawing.Color.White
        Me.btnEliminar.Location = New System.Drawing.Point(530, 278)
        Me.btnEliminar.Name = "btnEliminar"
        Me.btnEliminar.Size = New System.Drawing.Size(150, 34)
        Me.btnEliminar.TabIndex = 3
        Me.btnEliminar.Text = "Eliminar Seleccionado"
        Me.btnEliminar.UseVisualStyleBackColor = False
        '
        'frmUsuarios
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(700, 330)
        Me.Controls.Add(Me.btnEliminar)
        Me.Controls.Add(Me.lblTituloLista)
        Me.Controls.Add(Me.dgvUsuarios)
        Me.Controls.Add(Me.grpNuevo)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmUsuarios"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Gestión de Usuarios y Seguridad"
        Me.grpNuevo.ResumeLayout(False)
        Me.grpNuevo.PerformLayout()
        CType(Me.dgvUsuarios, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents grpNuevo As System.Windows.Forms.GroupBox
    Friend WithEvents lblNombre As System.Windows.Forms.Label
    Friend WithEvents txtNombre As System.Windows.Forms.TextBox
    Friend WithEvents lblClave As System.Windows.Forms.Label
    Friend WithEvents txtClave As System.Windows.Forms.TextBox
    Friend WithEvents lblRol As System.Windows.Forms.Label
    Friend WithEvents cmbRol As System.Windows.Forms.ComboBox
    Friend WithEvents btnGuardar As System.Windows.Forms.Button
    Friend WithEvents dgvUsuarios As System.Windows.Forms.DataGridView
    Friend WithEvents lblTituloLista As System.Windows.Forms.Label
    Friend WithEvents btnEliminar As System.Windows.Forms.Button
End Class