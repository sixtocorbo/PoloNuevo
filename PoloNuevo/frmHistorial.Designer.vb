<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmHistorial
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
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

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: El Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.dgvHistorial = New System.Windows.Forms.DataGridView()
        Me.colFecha = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colDocumento = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colAccion = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colOrigen = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colDestino = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colNota = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.dgvHistorial, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgvHistorial
        '
        Me.dgvHistorial.AllowUserToAddRows = False
        Me.dgvHistorial.AllowUserToDeleteRows = False
        Me.dgvHistorial.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvHistorial.BackgroundColor = System.Drawing.Color.WhiteSmoke
        Me.dgvHistorial.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvHistorial.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.SteelBlue
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.White
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvHistorial.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvHistorial.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvHistorial.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colFecha, Me.colDocumento, Me.colAccion, Me.colOrigen, Me.colDestino, Me.colNota})
        Me.dgvHistorial.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvHistorial.EnableHeadersVisualStyles = False
        Me.dgvHistorial.Location = New System.Drawing.Point(0, 0)
        Me.dgvHistorial.MultiSelect = False
        Me.dgvHistorial.Name = "dgvHistorial"
        Me.dgvHistorial.ReadOnly = True
        Me.dgvHistorial.RowHeadersVisible = False
        Me.dgvHistorial.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvHistorial.Size = New System.Drawing.Size(984, 561)
        Me.dgvHistorial.TabIndex = 0
        '
        'colFecha
        '
        Me.colFecha.HeaderText = "Fecha"
        Me.colFecha.Name = "colFecha"
        Me.colFecha.ReadOnly = True
        Me.colFecha.Width = 110
        '
        'colDocumento
        '
        Me.colDocumento.HeaderText = "Documento / Referencia"
        Me.colDocumento.Name = "colDocumento"
        Me.colDocumento.ReadOnly = True
        '
        'colAccion
        '
        Me.colAccion.HeaderText = "Acción / Evento"
        Me.colAccion.Name = "colAccion"
        Me.colAccion.ReadOnly = True
        '
        'colOrigen
        '
        Me.colOrigen.HeaderText = "Origen"
        Me.colOrigen.Name = "colOrigen"
        Me.colOrigen.ReadOnly = True
        '
        'colDestino
        '
        Me.colDestino.HeaderText = "Destino / Ubicación"
        Me.colDestino.Name = "colDestino"
        Me.colDestino.ReadOnly = True
        '
        'colNota
        '
        Me.colNota.HeaderText = "Nota Técnica"
        Me.colNota.Name = "colNota"
        Me.colNota.ReadOnly = True
        Me.colNota.Visible = False
        '
        'frmHistorial
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(984, 561)
        Me.Controls.Add(Me.dgvHistorial)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "frmHistorial"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Historial Completo del Expediente"
        CType(Me.dgvHistorial, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents dgvHistorial As System.Windows.Forms.DataGridView
    Friend WithEvents colFecha As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colDocumento As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colAccion As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colOrigen As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colDestino As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colNota As System.Windows.Forms.DataGridViewTextBoxColumn
End Class