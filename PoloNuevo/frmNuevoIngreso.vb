Imports System.IO
Imports System.Data

Public Class frmNuevoIngreso

    ' Variables de Estado
    Private _idDocumentoEditar As Integer = 0
    Private _listaCompletaReclusos As New List(Of ReclusoItem)
    Private _archivoBytes As Byte() = Nothing
    Private _archivoNombre As String = ""
    Private _archivoExt As String = ""

    Public Class ReclusoItem
        Public Property Id As Integer
        Public Property Texto As String
    End Class

    Public Sub New()
        InitializeComponent()
        _idDocumentoEditar = 0
        Me.Text = "Nuevo Ingreso Documental"
    End Sub

    Public Sub New(idDocumento As Integer)
        InitializeComponent()
        ApplyModernUi()
        _idDocumentoEditar = idDocumento
        Me.Text = "Editar / Detalle de Documento"
        btnGuardar.Text = "GUARDAR CAMBIOS"
        btnGuardar.BackColor = Color.SlateGray
    End Sub

    Private Sub frmNuevoIngreso_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CargarListas()
        If _idDocumentoEditar > 0 Then
            CargarDatosEdicion()
        End If
    End Sub

    Private Sub CargarListas()
        Using db As New PoloNuevoEntities()
            cmbTipo.DataSource = db.TiposDocumento.Where(Function(t) t.Nombre <> "ARCHIVO").OrderBy(Function(t) t.Nombre).ToList()
            cmbTipo.DisplayMember = "Nombre"
            cmbTipo.ValueMember = "Id"

            _listaCompletaReclusos = db.Reclusos _
                                        .Select(Function(r) New ReclusoItem With {
                                            .Id = r.Id,
                                            .Texto = r.Nombre & " (" & r.Cedula & ")"
                                        }) _
                                        .OrderBy(Function(r) r.Texto) _
                                        .ToList()

            ActualizarListaReclusos(_listaCompletaReclusos)
        End Using
    End Sub

    Private Sub ActualizarListaReclusos(items As List(Of ReclusoItem), Optional selectedId As Integer? = Nothing)
        lstReclusos.DataSource = Nothing
        lstReclusos.DataSource = items
        lstReclusos.DisplayMember = "Texto"
        lstReclusos.ValueMember = "Id"
        If selectedId.HasValue Then
            lstReclusos.SelectedValue = selectedId.Value
        Else
            lstReclusos.SelectedIndex = -1
        End If
    End Sub

    Private Sub txtBuscarRecluso_TextChanged(sender As Object, e As EventArgs) Handles txtBuscarRecluso.TextChanged
        Dim textoBusqueda As String = txtBuscarRecluso.Text.Trim().ToLower()
        Dim selectedId As Integer? = Nothing

        If lstReclusos.SelectedValue IsNot Nothing Then
            selectedId = Convert.ToInt32(lstReclusos.SelectedValue)
        End If

        If String.IsNullOrWhiteSpace(textoBusqueda) Then
            ActualizarListaReclusos(_listaCompletaReclusos, selectedId)
        Else
            Dim palabras = textoBusqueda.Split({" "c}, StringSplitOptions.RemoveEmptyEntries)
            Dim filtrados = _listaCompletaReclusos.Where(Function(r)
                                                             Return palabras.All(Function(p) r.Texto.ToLower().Contains(p))
                                                         End Function).ToList()
            ActualizarListaReclusos(filtrados, selectedId)
        End If
    End Sub

    ' =========================================================
    ' LÓGICA VENCIMIENTOS (NUEVO)
    ' =========================================================
    Private Sub chkVencimiento_CheckedChanged(sender As Object, e As EventArgs) Handles chkVencimiento.CheckedChanged
        dtpVencimiento.Visible = chkVencimiento.Checked
        dtpVencimiento.Enabled = chkVencimiento.Checked
    End Sub

    Private Sub CargarDatosEdicion()
        Try
            Using db As New PoloNuevoEntities()
                Dim doc = db.Documentos.Find(_idDocumentoEditar)
                If doc IsNot Nothing Then
                    txtNumero.Text = doc.ReferenciaExterna
                    txtAsunto.Text = doc.Descripcion
                    cmbTipo.SelectedValue = doc.TipoDocumentoId

                    If doc.ReclusoId.HasValue Then
                        chkVincular.Checked = True
                        txtBuscarRecluso.Enabled = True
                        lstReclusos.Enabled = True
                        ActualizarListaReclusos(_listaCompletaReclusos, doc.ReclusoId.Value)
                    Else
                        chkVincular.Checked = False
                        txtBuscarRecluso.Enabled = False
                        lstReclusos.Enabled = False
                    End If

                    ' CARGAR VENCIMIENTO
                    If doc.FechaVencimiento.HasValue Then
                        chkVencimiento.Checked = True
                        dtpVencimiento.Value = doc.FechaVencimiento.Value
                        dtpVencimiento.Visible = True
                    Else
                        chkVencimiento.Checked = False
                        dtpVencimiento.Visible = False
                    End If

                    If doc.Extension <> ".phy" Then
                        lblArchivoNombre.Text = "Archivo actual: " & doc.NombreArchivo
                        lblArchivoNombre.ForeColor = Color.Blue
                        btnAdjuntar.Text = "Reemplazar..."
                    Else
                        lblArchivoNombre.Text = "Registro Físico (Sin digitalizar)"
                    End If
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("Error al cargar datos: " & ex.Message)
        End Try
    End Sub

    Private Sub chkVincular_CheckedChanged(sender As Object, e As EventArgs) Handles chkVincular.CheckedChanged
        txtBuscarRecluso.Enabled = chkVincular.Checked
        lstReclusos.Enabled = chkVincular.Checked
        If Not chkVincular.Checked Then
            txtBuscarRecluso.Text = ""
            ActualizarListaReclusos(_listaCompletaReclusos)
        End If
    End Sub

    Private Sub btnAdjuntar_Click(sender As Object, e As EventArgs) Handles btnAdjuntar.Click
        Using ofd As New OpenFileDialog()
            ofd.Filter = "Archivos|*.pdf;*.jpg;*.jpeg;*.png;*.doc;*.docx"
            If ofd.ShowDialog() = DialogResult.OK Then
                _archivoBytes = File.ReadAllBytes(ofd.FileName)
                _archivoNombre = Path.GetFileName(ofd.FileName)
                _archivoExt = Path.GetExtension(ofd.FileName)
                lblArchivoNombre.Text = _archivoNombre
                lblArchivoNombre.ForeColor = Color.Green
            End If
        End Using
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If txtNumero.Text.Trim = "" Or txtAsunto.Text.Trim = "" Then
            MessageBox.Show("Faltan datos obligatorios.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Using db As New PoloNuevoEntities()
                Dim doc As Documentos
                If _idDocumentoEditar = 0 Then
                    doc = New Documentos()
                    doc.FechaCarga = DateTime.Now
                    db.Documentos.Add(doc)
                Else
                    doc = db.Documentos.Find(_idDocumentoEditar)
                End If

                doc.TipoDocumentoId = Convert.ToInt32(cmbTipo.SelectedValue)
                doc.ReferenciaExterna = txtNumero.Text.Trim()
                doc.Descripcion = txtAsunto.Text.Trim()

                If chkVincular.Checked And lstReclusos.SelectedValue IsNot Nothing Then
                    doc.ReclusoId = Convert.ToInt32(lstReclusos.SelectedValue)
                Else
                    doc.ReclusoId = Nothing
                End If

                ' GUARDAR VENCIMIENTO
                If chkVencimiento.Checked Then
                    doc.FechaVencimiento = dtpVencimiento.Value.Date
                Else
                    doc.FechaVencimiento = Nothing
                End If

                If _archivoBytes IsNot Nothing Then
                    doc.Contenido = _archivoBytes
                    doc.NombreArchivo = _archivoNombre
                    doc.Extension = _archivoExt
                ElseIf _idDocumentoEditar = 0 Then
                    doc.Extension = ".phy"
                    doc.NombreArchivo = "Fisico"
                End If

                db.SaveChanges()

                If _idDocumentoEditar = 0 Then
                    Dim mov As New MovimientosDocumentos()
                    mov.DocumentoId = doc.Id
                    mov.FechaMovimiento = DateTime.Now
                    mov.Origen = "EXTERNO / RECEPCIÓN"
                    mov.Destino = "MESA DE ENTRADA"
                    db.MovimientosDocumentos.Add(mov)
                    db.SaveChanges()
                End If

                MessageBox.Show("Guardado correctamente.", "Éxito")
                Me.DialogResult = DialogResult.OK
                Me.Close()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error al guardar: " & ex.Message)
        End Try
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Me.Close()
    End Sub

    Private Sub ApplyModernUi()
        Me.SuspendLayout()
        Me.Font = New Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point)
        Me.BackColor = Color.White
        Me.DoubleBuffered = True
        Me.Padding = New Padding(16)
        Me.FormBorderStyle = FormBorderStyle.Sizable
        Me.MaximizeBox = False
        Me.MinimumSize = New Size(780, 820)
        Me.AcceptButton = btnGuardar
        Me.CancelButton = btnCancelar

        pnlTop.BackColor = Color.FromArgb(245, 246, 248)
        pnlTop.Padding = New Padding(16, 16, 16, 12)
        pnlTop.Height = 64

        lblTitulo.Font = New Font("Segoe UI", 14.0F, FontStyle.Bold, GraphicsUnit.Point)
        lblTitulo.ForeColor = Color.FromArgb(55, 55, 55)

        For Each lbl In New Label() {lblTipo, lblNumero, lblAsunto}
            lbl.ForeColor = Color.FromArgb(70, 70, 70)
        Next

        cmbTipo.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        txtNumero.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        txtAsunto.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right

        txtNumero.BorderStyle = BorderStyle.FixedSingle
        txtAsunto.BorderStyle = BorderStyle.FixedSingle
        txtBuscarRecluso.BorderStyle = BorderStyle.FixedSingle

        StyleGroupBox(grpVinculacion)
        StyleGroupBox(grpArchivo)

        lstReclusos.BorderStyle = BorderStyle.FixedSingle
        lstReclusos.IntegralHeight = False
        lstReclusos.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Bottom
        txtBuscarRecluso.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right

        btnAdjuntar.FlatStyle = FlatStyle.Flat
        btnAdjuntar.FlatAppearance.BorderSize = 1
        btnAdjuntar.FlatAppearance.BorderColor = Color.FromArgb(210, 210, 210)
        btnAdjuntar.BackColor = Color.White
        btnAdjuntar.ForeColor = Color.FromArgb(60, 60, 60)
        btnAdjuntar.Cursor = Cursors.Hand

        lblArchivoNombre.ForeColor = Color.FromArgb(110, 110, 110)

        StylePrimaryButton(btnGuardar)
        StyleSecondaryButton(btnCancelar)
        Me.ResumeLayout(True)
    End Sub

    Private Sub StyleGroupBox(g As GroupBox)
        g.ForeColor = Color.FromArgb(70, 70, 70)
        g.Padding = New Padding(10, 26, 10, 10)
    End Sub

    Private Sub StylePrimaryButton(b As Button)
        b.FlatStyle = FlatStyle.Flat
        b.FlatAppearance.BorderSize = 0
        b.BackColor = Color.FromArgb(46, 125, 50)
        b.ForeColor = Color.White
        b.Font = New Font("Segoe UI", 10.0F, FontStyle.Bold, GraphicsUnit.Point)
        b.Cursor = Cursors.Hand
    End Sub

    Private Sub StyleSecondaryButton(b As Button)
        b.FlatStyle = FlatStyle.Flat
        b.FlatAppearance.BorderSize = 1
        b.FlatAppearance.BorderColor = Color.FromArgb(210, 210, 210)
        b.BackColor = Color.White
        b.ForeColor = Color.FromArgb(60, 60, 60)
        b.Font = New Font("Segoe UI", 10.0F, FontStyle.Regular, GraphicsUnit.Point)
        b.Cursor = Cursors.Hand
    End Sub
End Class