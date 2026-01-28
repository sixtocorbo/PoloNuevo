Imports System.IO

Public Class frmNuevoIngreso

    ' Variables de Estado
    Private _idDocumentoEditar As Integer = 0

    ' Variables para Archivo Digital
    Private _archivoBytes As Byte() = Nothing
    Private _archivoNombre As String = ""
    Private _archivoExt As String = ""

    ' =========================================================
    ' CONSTRUCTORES (NUEVO vs EDITAR)
    ' =========================================================

    ' Constructor 1: NUEVO INGRESO
    Public Sub New()
        InitializeComponent()
        _idDocumentoEditar = 0
        Me.Text = "Nuevo Ingreso Documental"
    End Sub

    ' Constructor 2: EDITAR EXISTENTE
    Public Sub New(idDocumento As Integer)
        InitializeComponent()
        _idDocumentoEditar = idDocumento
        Me.Text = "Editar / Detalle de Documento"
        btnGuardar.Text = "GUARDAR CAMBIOS"
        btnGuardar.BackColor = Color.SlateGray
    End Sub

    ' =========================================================
    ' CARGA INICIAL
    ' =========================================================
    Private Sub frmNuevoIngreso_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CargarListas()

        If _idDocumentoEditar > 0 Then
            CargarDatosEdicion()
        End If
    End Sub

    Private Sub CargarListas()
        Using db As New PoloNuevoEntities()
            ' 1. Tipos de Documento
            cmbTipo.DataSource = db.TiposDocumento.Where(Function(t) t.Nombre <> "ARCHIVO").OrderBy(Function(t) t.Nombre).ToList()
            cmbTipo.DisplayMember = "Nombre"
            cmbTipo.ValueMember = "Id"

            ' 2. Reclusos (Para el buscador)
            ' Cargamos nombre y cédula para facilitar la búsqueda
            Dim listaPresos = db.Reclusos _
                                .Select(Function(r) New With {
                                    .Id = r.Id,
                                    .Texto = r.Nombre & " (" & r.Cedula & ")"
                                }) _
                                .OrderBy(Function(r) r.Texto) _
                                .ToList()

            cmbRecluso.DataSource = listaPresos
            cmbRecluso.DisplayMember = "Texto"
            cmbRecluso.ValueMember = "Id"
            cmbRecluso.SelectedIndex = -1 ' Arrancar vacío
        End Using
    End Sub

    ' =========================================================
    ' LÓGICA DE EDICIÓN (CARGAR DATOS)
    ' =========================================================
    Private Sub CargarDatosEdicion()
        Try
            Using db As New PoloNuevoEntities()
                Dim doc = db.Documentos.Find(_idDocumentoEditar)
                If doc IsNot Nothing Then
                    ' Datos Básicos
                    txtNumero.Text = doc.ReferenciaExterna
                    txtAsunto.Text = doc.Descripcion
                    cmbTipo.SelectedValue = doc.TipoDocumentoId

                    ' Recluso Vinculado
                    If doc.ReclusoId.HasValue Then
                        chkVincular.Checked = True
                        cmbRecluso.Enabled = True
                        cmbRecluso.SelectedValue = doc.ReclusoId.Value
                    Else
                        chkVincular.Checked = False
                        cmbRecluso.Enabled = False
                    End If

                    ' Archivo Adjunto
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
            MessageBox.Show("Error al cargar datos para editar: " & ex.Message)
        End Try
    End Sub

    ' =========================================================
    ' INTERFAZ (CHECKBOX Y ADJUNTAR)
    ' =========================================================
    Private Sub chkVincular_CheckedChanged(sender As Object, e As EventArgs) Handles chkVincular.CheckedChanged
        cmbRecluso.Enabled = chkVincular.Checked
        If Not chkVincular.Checked Then cmbRecluso.SelectedIndex = -1
    End Sub

    Private Sub btnAdjuntar_Click(sender As Object, e As EventArgs) Handles btnAdjuntar.Click
        Using ofd As New OpenFileDialog()
            ofd.Filter = "Archivos|*.pdf;*.jpg;*.jpeg;*.png;*.doc;*.docx"
            If ofd.ShowDialog() = DialogResult.OK Then
                Try
                    _archivoBytes = File.ReadAllBytes(ofd.FileName)
                    _archivoNombre = Path.GetFileName(ofd.FileName)
                    _archivoExt = Path.GetExtension(ofd.FileName)

                    lblArchivoNombre.Text = _archivoNombre
                    lblArchivoNombre.ForeColor = Color.Green
                Catch ex As Exception
                    MessageBox.Show("Error: " & ex.Message)
                End Try
            End If
        End Using
    End Sub

    ' =========================================================
    ' GUARDAR (INSERT O UPDATE)
    ' =========================================================
    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        ' Validaciones
        If txtNumero.Text.Trim = "" Or txtAsunto.Text.Trim = "" Then
            MessageBox.Show("Faltan datos obligatorios (Número o Asunto).", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If chkVincular.Checked And cmbRecluso.SelectedIndex = -1 Then
            MessageBox.Show("Marcó 'Vincular' pero no seleccionó al recluso.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Using db As New PoloNuevoEntities()
                Dim doc As Documentos

                If _idDocumentoEditar = 0 Then
                    ' --- MODO NUEVO ---
                    doc = New Documentos()
                    doc.FechaCarga = DateTime.Now
                    db.Documentos.Add(doc)
                Else
                    ' --- MODO EDICIÓN ---
                    doc = db.Documentos.Find(_idDocumentoEditar)
                End If

                ' Asignar valores comunes
                doc.TipoDocumentoId = Convert.ToInt32(cmbTipo.SelectedValue)
                doc.ReferenciaExterna = txtNumero.Text.Trim()
                doc.Descripcion = txtAsunto.Text.Trim()

                ' Asignar Recluso
                If chkVincular.Checked Then
                    doc.ReclusoId = Convert.ToInt32(cmbRecluso.SelectedValue)
                Else
                    doc.ReclusoId = Nothing
                End If

                ' Asignar Archivo (Solo si se subió uno nuevo)
                If _archivoBytes IsNot Nothing Then
                    doc.Contenido = _archivoBytes
                    doc.NombreArchivo = _archivoNombre
                    doc.Extension = _archivoExt
                ElseIf _idDocumentoEditar = 0 Then
                    ' Si es nuevo y no subieron nada, es físico
                    doc.Extension = ".phy"
                    doc.NombreArchivo = "Fisico"
                End If
                ' NOTA: Si es edición y _archivoBytes es Nothing, mantenemos el archivo viejo (no lo tocamos)

                db.SaveChanges()

                ' Si es NUEVO, creamos el movimiento inicial automático
                If _idDocumentoEditar = 0 Then
                    Dim mov As New MovimientosDocumentos()
                    mov.DocumentoId = doc.Id
                    mov.FechaMovimiento = DateTime.Now
                    mov.Origen = "EXTERNO / RECEPCIÓN"
                    mov.Destino = "MESA DE ENTRADA"
                    mov.EsSalida = False
                    db.MovimientosDocumentos.Add(mov)
                    db.SaveChanges()
                End If

                MessageBox.Show("Guardado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
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

End Class