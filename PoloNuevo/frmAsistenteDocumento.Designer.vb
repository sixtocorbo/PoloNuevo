<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmAsistenteDocumento
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
        Me.pnlTop = New System.Windows.Forms.Panel()
        Me.lblTituloPaso = New System.Windows.Forms.Label()
        Me.pnlBottom = New System.Windows.Forms.Panel()
        Me.lblEstado = New System.Windows.Forms.Label()
        Me.btnAnterior = New System.Windows.Forms.Button()
        Me.btnSiguiente = New System.Windows.Forms.Button()
        Me.tabWizard = New System.Windows.Forms.TabControl()
        Me.tabTipo = New System.Windows.Forms.TabPage()
        Me.grpModoIngreso = New System.Windows.Forms.GroupBox()
        Me.lblDetalleVinculo = New System.Windows.Forms.Label()
        Me.optVincular = New System.Windows.Forms.RadioButton()
        Me.optNuevo = New System.Windows.Forms.RadioButton()
        Me.lblInstruccion1 = New System.Windows.Forms.Label()
        Me.cmbTipoDoc = New System.Windows.Forms.ComboBox()
        Me.tabDatos = New System.Windows.Forms.TabPage()
        Me.lstSugerenciasOrigen = New System.Windows.Forms.ListBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtOrigen = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtAsunto = New System.Windows.Forms.TextBox()
        Me.lblNumero = New System.Windows.Forms.Label()
        Me.txtNumero = New System.Windows.Forms.TextBox()
        Me.tabAdjunto = New System.Windows.Forms.TabPage()
        Me.btnExaminar = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtRutaArchivo = New System.Windows.Forms.TextBox()
        Me.tabFinal = New System.Windows.Forms.TabPage()
        Me.grpResumen = New System.Windows.Forms.GroupBox()
        Me.lblResumenAdjunto = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.lblResumenOrigen = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.lblResumenAsunto = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.lblResumenTipo = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.pnlTop.SuspendLayout()
        Me.pnlBottom.SuspendLayout()
        Me.tabWizard.SuspendLayout()
        Me.tabTipo.SuspendLayout()
        Me.grpModoIngreso.SuspendLayout()
        Me.tabDatos.SuspendLayout()
        Me.tabAdjunto.SuspendLayout()
        Me.tabFinal.SuspendLayout()
        Me.grpResumen.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlTop
        '
        Me.pnlTop.BackColor = System.Drawing.Color.WhiteSmoke
        Me.pnlTop.Controls.Add(Me.lblTituloPaso)
        Me.pnlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlTop.Location = New System.Drawing.Point(0, 0)
        Me.pnlTop.Name = "pnlTop"
        Me.pnlTop.Size = New System.Drawing.Size(584, 60)
        Me.pnlTop.TabIndex = 0
        '
        'lblTituloPaso
        '
        Me.lblTituloPaso.AutoSize = True
        Me.lblTituloPaso.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTituloPaso.ForeColor = System.Drawing.Color.DimGray
        Me.lblTituloPaso.Location = New System.Drawing.Point(12, 18)
        Me.lblTituloPaso.Name = "lblTituloPaso"
        Me.lblTituloPaso.Size = New System.Drawing.Size(193, 25)
        Me.lblTituloPaso.TabIndex = 0
        Me.lblTituloPaso.Text = "Paso 1: Clasificación"
        '
        'pnlBottom
        '
        Me.pnlBottom.BackColor = System.Drawing.Color.Gainsboro
        Me.pnlBottom.Controls.Add(Me.lblEstado)
        Me.pnlBottom.Controls.Add(Me.btnAnterior)
        Me.pnlBottom.Controls.Add(Me.btnSiguiente)
        Me.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlBottom.Location = New System.Drawing.Point(0, 391)
        Me.pnlBottom.Name = "pnlBottom"
        Me.pnlBottom.Size = New System.Drawing.Size(584, 70)
        Me.pnlBottom.TabIndex = 1
        '
        'lblEstado
        '
        Me.lblEstado.AutoSize = True
        Me.lblEstado.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEstado.ForeColor = System.Drawing.Color.DimGray
        Me.lblEstado.Location = New System.Drawing.Point(14, 29)
        Me.lblEstado.Name = "lblEstado"
        Me.lblEstado.Size = New System.Drawing.Size(19, 13)
        Me.lblEstado.TabIndex = 2
        Me.lblEstado.Text = "..."
        '
        'btnAnterior
        '
        Me.btnAnterior.Location = New System.Drawing.Point(323, 16)
        Me.btnAnterior.Name = "btnAnterior"
        Me.btnAnterior.Size = New System.Drawing.Size(110, 38)
        Me.btnAnterior.TabIndex = 1
        Me.btnAnterior.Text = "< Anterior"
        Me.btnAnterior.UseVisualStyleBackColor = True
        '
        'btnSiguiente
        '
        Me.btnSiguiente.BackColor = System.Drawing.Color.SteelBlue
        Me.btnSiguiente.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSiguiente.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSiguiente.ForeColor = System.Drawing.Color.White
        Me.btnSiguiente.Location = New System.Drawing.Point(439, 16)
        Me.btnSiguiente.Name = "btnSiguiente"
        Me.btnSiguiente.Size = New System.Drawing.Size(133, 38)
        Me.btnSiguiente.TabIndex = 0
        Me.btnSiguiente.Text = "Siguiente >"
        Me.btnSiguiente.UseVisualStyleBackColor = False
        '
        'tabWizard
        '
        Me.tabWizard.Controls.Add(Me.tabTipo)
        Me.tabWizard.Controls.Add(Me.tabDatos)
        Me.tabWizard.Controls.Add(Me.tabAdjunto)
        Me.tabWizard.Controls.Add(Me.tabFinal)
        Me.tabWizard.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabWizard.Location = New System.Drawing.Point(0, 60)
        Me.tabWizard.Name = "tabWizard"
        Me.tabWizard.SelectedIndex = 0
        Me.tabWizard.Size = New System.Drawing.Size(584, 331)
        Me.tabWizard.TabIndex = 2
        '
        'tabTipo
        '
        Me.tabTipo.Controls.Add(Me.grpModoIngreso)
        Me.tabTipo.Controls.Add(Me.lblInstruccion1)
        Me.tabTipo.Controls.Add(Me.cmbTipoDoc)
        Me.tabTipo.Location = New System.Drawing.Point(4, 22)
        Me.tabTipo.Name = "tabTipo"
        Me.tabTipo.Padding = New System.Windows.Forms.Padding(3)
        Me.tabTipo.Size = New System.Drawing.Size(576, 305)
        Me.tabTipo.TabIndex = 0
        Me.tabTipo.Text = "Tipo"
        Me.tabTipo.UseVisualStyleBackColor = True
        '
        'grpModoIngreso
        '
        Me.grpModoIngreso.Controls.Add(Me.lblDetalleVinculo)
        Me.grpModoIngreso.Controls.Add(Me.optVincular)
        Me.grpModoIngreso.Controls.Add(Me.optNuevo)
        Me.grpModoIngreso.Location = New System.Drawing.Point(48, 23)
        Me.grpModoIngreso.Name = "grpModoIngreso"
        Me.grpModoIngreso.Size = New System.Drawing.Size(469, 118)
        Me.grpModoIngreso.TabIndex = 2
        Me.grpModoIngreso.TabStop = False
        Me.grpModoIngreso.Text = "¿Cómo desea registrar este documento?"
        '
        'lblDetalleVinculo
        '
        Me.lblDetalleVinculo.AutoSize = True
        Me.lblDetalleVinculo.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDetalleVinculo.ForeColor = System.Drawing.Color.DimGray
        Me.lblDetalleVinculo.Location = New System.Drawing.Point(36, 85)
        Me.lblDetalleVinculo.Name = "lblDetalleVinculo"
        Me.lblDetalleVinculo.Size = New System.Drawing.Size(262, 13)
        Me.lblDetalleVinculo.TabIndex = 2
        Me.lblDetalleVinculo.Text = "Seleccione 'Vincular' para ver detalles del expediente."
        '
        'optVincular
        '
        Me.optVincular.AutoSize = True
        Me.optVincular.Font = New System.Drawing.Font("Segoe UI", 11.0!)
        Me.optVincular.ForeColor = System.Drawing.Color.RoyalBlue
        Me.optVincular.Location = New System.Drawing.Point(17, 58)
        Me.optVincular.Name = "optVincular"
        Me.optVincular.Size = New System.Drawing.Size(306, 24)
        Me.optVincular.TabIndex = 1
        Me.optVincular.Text = "Es Respuesta o Vínculo a otro Documento"
        Me.optVincular.UseVisualStyleBackColor = True
        '
        'optNuevo
        '
        Me.optNuevo.AutoSize = True
        Me.optNuevo.Checked = True
        Me.optNuevo.Font = New System.Drawing.Font("Segoe UI", 11.0!)
        Me.optNuevo.Location = New System.Drawing.Point(17, 28)
        Me.optNuevo.Name = "optNuevo"
        Me.optNuevo.Size = New System.Drawing.Size(252, 24)
        Me.optNuevo.TabIndex = 0
        Me.optNuevo.TabStop = True
        Me.optNuevo.Text = "Nuevo Ingreso (Independiente)"
        Me.optNuevo.UseVisualStyleBackColor = True
        '
        'lblInstruccion1
        '
        Me.lblInstruccion1.AutoSize = True
        Me.lblInstruccion1.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.lblInstruccion1.Location = New System.Drawing.Point(44, 156)
        Me.lblInstruccion1.Name = "lblInstruccion1"
        Me.lblInstruccion1.Size = New System.Drawing.Size(248, 19)
        Me.lblInstruccion1.TabIndex = 1
        Me.lblInstruccion1.Text = "Seleccione la categoría del documento:"
        '
        'cmbTipoDoc
        '
        Me.cmbTipoDoc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTipoDoc.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbTipoDoc.FormattingEnabled = True
        Me.cmbTipoDoc.Location = New System.Drawing.Point(48, 178)
        Me.cmbTipoDoc.Name = "cmbTipoDoc"
        Me.cmbTipoDoc.Size = New System.Drawing.Size(469, 29)
        Me.cmbTipoDoc.TabIndex = 0
        '
        'tabDatos
        '
        Me.tabDatos.Controls.Add(Me.lstSugerenciasOrigen)
        Me.tabDatos.Controls.Add(Me.Label3)
        Me.tabDatos.Controls.Add(Me.txtOrigen)
        Me.tabDatos.Controls.Add(Me.Label2)
        Me.tabDatos.Controls.Add(Me.txtAsunto)
        Me.tabDatos.Controls.Add(Me.lblNumero)
        Me.tabDatos.Controls.Add(Me.txtNumero)
        Me.tabDatos.Location = New System.Drawing.Point(4, 22)
        Me.tabDatos.Name = "tabDatos"
        Me.tabDatos.Padding = New System.Windows.Forms.Padding(3)
        Me.tabDatos.Size = New System.Drawing.Size(576, 305)
        Me.tabDatos.TabIndex = 1
        Me.tabDatos.Text = "Datos"
        Me.tabDatos.UseVisualStyleBackColor = True
        '
        'lstSugerenciasOrigen
        '
        Me.lstSugerenciasOrigen.FormattingEnabled = True
        Me.lstSugerenciasOrigen.Location = New System.Drawing.Point(48, 202)
        Me.lstSugerenciasOrigen.Name = "lstSugerenciasOrigen"
        Me.lstSugerenciasOrigen.Size = New System.Drawing.Size(469, 95)
        Me.lstSugerenciasOrigen.TabIndex = 6
        Me.lstSugerenciasOrigen.Visible = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(45, 160)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(111, 13)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "Organismo de Origen:"
        '
        'txtOrigen
        '
        Me.txtOrigen.Location = New System.Drawing.Point(48, 176)
        Me.txtOrigen.Name = "txtOrigen"
        Me.txtOrigen.Size = New System.Drawing.Size(469, 20)
        Me.txtOrigen.TabIndex = 4
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(45, 64)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(43, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Asunto:"
        '
        'txtAsunto
        '
        Me.txtAsunto.Location = New System.Drawing.Point(48, 80)
        Me.txtAsunto.Multiline = True
        Me.txtAsunto.Name = "txtAsunto"
        Me.txtAsunto.Size = New System.Drawing.Size(469, 64)
        Me.txtAsunto.TabIndex = 2
        '
        'lblNumero
        '
        Me.lblNumero.AutoSize = True
        Me.lblNumero.Location = New System.Drawing.Point(45, 18)
        Me.lblNumero.Name = "lblNumero"
        Me.lblNumero.Size = New System.Drawing.Size(128, 13)
        Me.lblNumero.TabIndex = 1
        Me.lblNumero.Text = "Nro. Referencia / Oficio:"
        '
        'txtNumero
        '
        Me.txtNumero.Location = New System.Drawing.Point(48, 34)
        Me.txtNumero.Name = "txtNumero"
        Me.txtNumero.Size = New System.Drawing.Size(200, 20)
        Me.txtNumero.TabIndex = 0
        '
        'tabAdjunto
        '
        Me.tabAdjunto.Controls.Add(Me.btnExaminar)
        Me.tabAdjunto.Controls.Add(Me.Label4)
        Me.tabAdjunto.Controls.Add(Me.txtRutaArchivo)
        Me.tabAdjunto.Location = New System.Drawing.Point(4, 22)
        Me.tabAdjunto.Name = "tabAdjunto"
        Me.tabAdjunto.Size = New System.Drawing.Size(576, 305)
        Me.tabAdjunto.TabIndex = 2
        Me.tabAdjunto.Text = "Adjunto"
        Me.tabAdjunto.UseVisualStyleBackColor = True
        '
        'btnExaminar
        '
        Me.btnExaminar.Location = New System.Drawing.Point(407, 108)
        Me.btnExaminar.Name = "btnExaminar"
        Me.btnExaminar.Size = New System.Drawing.Size(107, 30)
        Me.btnExaminar.TabIndex = 2
        Me.btnExaminar.Text = "Examinar..."
        Me.btnExaminar.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(54, 94)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(217, 13)
        Me.Label4.TabIndex = 1
        Me.Label4.Text = "Seleccione un archivo PDF (Recomendado):"
        '
        'txtRutaArchivo
        '
        Me.txtRutaArchivo.Location = New System.Drawing.Point(57, 114)
        Me.txtRutaArchivo.Name = "txtRutaArchivo"
        Me.txtRutaArchivo.ReadOnly = True
        Me.txtRutaArchivo.Size = New System.Drawing.Size(344, 20)
        Me.txtRutaArchivo.TabIndex = 0
        '
        'tabFinal
        '
        Me.tabFinal.Controls.Add(Me.grpResumen)
        Me.tabFinal.Location = New System.Drawing.Point(4, 22)
        Me.tabFinal.Name = "tabFinal"
        Me.tabFinal.Size = New System.Drawing.Size(576, 305)
        Me.tabFinal.TabIndex = 3
        Me.tabFinal.Text = "Final"
        Me.tabFinal.UseVisualStyleBackColor = True
        '
        'grpResumen
        '
        Me.grpResumen.Controls.Add(Me.lblResumenAdjunto)
        Me.grpResumen.Controls.Add(Me.Label8)
        Me.grpResumen.Controls.Add(Me.lblResumenOrigen)
        Me.grpResumen.Controls.Add(Me.Label6)
        Me.grpResumen.Controls.Add(Me.lblResumenAsunto)
        Me.grpResumen.Controls.Add(Me.Label5)
        Me.grpResumen.Controls.Add(Me.lblResumenTipo)
        Me.grpResumen.Controls.Add(Me.Label1)
        Me.grpResumen.Location = New System.Drawing.Point(37, 32)
        Me.grpResumen.Name = "grpResumen"
        Me.grpResumen.Size = New System.Drawing.Size(498, 237)
        Me.grpResumen.TabIndex = 0
        Me.grpResumen.TabStop = False
        Me.grpResumen.Text = "Resumen de Ingreso"
        '
        'lblResumenAdjunto
        '
        Me.lblResumenAdjunto.AutoSize = True
        Me.lblResumenAdjunto.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblResumenAdjunto.Location = New System.Drawing.Point(125, 178)
        Me.lblResumenAdjunto.Name = "lblResumenAdjunto"
        Me.lblResumenAdjunto.Size = New System.Drawing.Size(11, 13)
        Me.lblResumenAdjunto.TabIndex = 7
        Me.lblResumenAdjunto.Text = "-"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(30, 178)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(46, 13)
        Me.Label8.TabIndex = 6
        Me.Label8.Text = "Archivo:"
        '
        'lblResumenOrigen
        '
        Me.lblResumenOrigen.AutoSize = True
        Me.lblResumenOrigen.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblResumenOrigen.Location = New System.Drawing.Point(125, 142)
        Me.lblResumenOrigen.Name = "lblResumenOrigen"
        Me.lblResumenOrigen.Size = New System.Drawing.Size(11, 13)
        Me.lblResumenOrigen.TabIndex = 5
        Me.lblResumenOrigen.Text = "-"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(30, 142)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(41, 13)
        Me.Label6.TabIndex = 4
        Me.Label6.Text = "Origen:"
        '
        'lblResumenAsunto
        '
        Me.lblResumenAsunto.AutoSize = True
        Me.lblResumenAsunto.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblResumenAsunto.Location = New System.Drawing.Point(125, 87)
        Me.lblResumenAsunto.Name = "lblResumenAsunto"
        Me.lblResumenAsunto.Size = New System.Drawing.Size(11, 13)
        Me.lblResumenAsunto.TabIndex = 3
        Me.lblResumenAsunto.Text = "-"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(30, 87)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(43, 13)
        Me.Label5.TabIndex = 2
        Me.Label5.Text = "Asunto:"
        '
        'lblResumenTipo
        '
        Me.lblResumenTipo.AutoSize = True
        Me.lblResumenTipo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblResumenTipo.ForeColor = System.Drawing.Color.Navy
        Me.lblResumenTipo.Location = New System.Drawing.Point(125, 46)
        Me.lblResumenTipo.Name = "lblResumenTipo"
        Me.lblResumenTipo.Size = New System.Drawing.Size(11, 13)
        Me.lblResumenTipo.TabIndex = 1
        Me.lblResumenTipo.Text = "-"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(30, 46)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(31, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Tipo:"
        '
        'frmAsistenteDocumento
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(584, 461)
        Me.Controls.Add(Me.tabWizard)
        Me.Controls.Add(Me.pnlBottom)
        Me.Controls.Add(Me.pnlTop)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmAsistenteDocumento"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Asistente de Nuevo Ingreso"
        Me.pnlTop.ResumeLayout(False)
        Me.pnlTop.PerformLayout()
        Me.pnlBottom.ResumeLayout(False)
        Me.pnlBottom.PerformLayout()
        Me.tabWizard.ResumeLayout(False)
        Me.tabTipo.ResumeLayout(False)
        Me.tabTipo.PerformLayout()
        Me.grpModoIngreso.ResumeLayout(False)
        Me.grpModoIngreso.PerformLayout()
        Me.tabDatos.ResumeLayout(False)
        Me.tabDatos.PerformLayout()
        Me.tabAdjunto.ResumeLayout(False)
        Me.tabAdjunto.PerformLayout()
        Me.tabFinal.ResumeLayout(False)
        Me.grpResumen.ResumeLayout(False)
        Me.grpResumen.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents pnlTop As System.Windows.Forms.Panel
    Friend WithEvents lblTituloPaso As System.Windows.Forms.Label
    Friend WithEvents pnlBottom As System.Windows.Forms.Panel
    Friend WithEvents btnAnterior As System.Windows.Forms.Button
    Friend WithEvents btnSiguiente As System.Windows.Forms.Button
    Friend WithEvents tabWizard As System.Windows.Forms.TabControl
    Friend WithEvents tabTipo As System.Windows.Forms.TabPage
    Friend WithEvents tabDatos As System.Windows.Forms.TabPage
    Friend WithEvents tabAdjunto As System.Windows.Forms.TabPage
    Friend WithEvents tabFinal As System.Windows.Forms.TabPage
    Friend WithEvents cmbTipoDoc As System.Windows.Forms.ComboBox
    Friend WithEvents lblInstruccion1 As System.Windows.Forms.Label
    Friend WithEvents txtAsunto As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtOrigen As System.Windows.Forms.TextBox
    Friend WithEvents lstSugerenciasOrigen As System.Windows.Forms.ListBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtRutaArchivo As System.Windows.Forms.TextBox
    Friend WithEvents btnExaminar As System.Windows.Forms.Button
    Friend WithEvents grpResumen As System.Windows.Forms.GroupBox
    Friend WithEvents lblResumenTipo As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblResumenAdjunto As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents lblResumenOrigen As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents lblResumenAsunto As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents lblEstado As System.Windows.Forms.Label
    Friend WithEvents lblNumero As System.Windows.Forms.Label
    Friend WithEvents txtNumero As System.Windows.Forms.TextBox
    Friend WithEvents grpModoIngreso As System.Windows.Forms.GroupBox
    Friend WithEvents optVincular As System.Windows.Forms.RadioButton
    Friend WithEvents optNuevo As System.Windows.Forms.RadioButton
    Friend WithEvents lblDetalleVinculo As System.Windows.Forms.Label
End Class