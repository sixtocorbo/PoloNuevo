Imports System.IO

Public Class frmNuevoDocumento

    Private _idRecluso As Integer
    Private _rutaArchivo As String = ""

    ' Constructor que recibe el ID del preso al que le vamos a cargar el documento
    Public Sub New(idRecluso As Integer)
        InitializeComponent()
        _idRecluso = idRecluso
    End Sub

    Private Sub frmNuevoDocumento_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CargarTiposDocumento()
    End Sub

    Private Sub CargarTiposDocumento()
        Using db As New PoloNuevoEntities()
            ' Llenamos el combo con tu lista oficial
            cmbTipo.DataSource = db.TiposDocumento.OrderBy(Function(t) t.Nombre).ToList()
            cmbTipo.DisplayMember = "Nombre"
            cmbTipo.ValueMember = "Id"
        End Using
    End Sub

    ' Botón EXAMINAR: Abre el diálogo para elegir archivo
    Private Sub btnExaminar_Click(sender As Object, e As EventArgs) Handles btnExaminar.Click
        Dim op As New OpenFileDialog()
        op.Filter = "Archivos Permitidos|*.pdf;*.jpg;*.jpeg;*.png;*.doc;*.docx|Todos los archivos|*.*"
        op.Title = "Seleccione el documento o foto"

        If op.ShowDialog() = DialogResult.OK Then
            _rutaArchivo = op.FileName

            ' Validar tamaño (opcional, ej: máximo 10MB)
            Dim info As New FileInfo(_rutaArchivo)
            If info.Length > 10 * 1024 * 1024 Then ' 10 MB
                MessageBox.Show("El archivo es demasiado grande (Máx 10MB).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                _rutaArchivo = ""
                lblArchivoSeleccionado.Text = "Ningún archivo seleccionado"
                Return
            End If

            lblArchivoSeleccionado.Text = info.Name
            lblArchivoSeleccionado.ForeColor = Color.Black
        End If
    End Sub

    ' Botón GUARDAR: La lógica Entity Framework
    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        ' 1. Validaciones
        If String.IsNullOrWhiteSpace(_rutaArchivo) Then
            MessageBox.Show("Debe seleccionar un archivo.", "Falta archivo", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        If String.IsNullOrWhiteSpace(txtDescripcion.Text) Then
            MessageBox.Show("Escriba una breve descripción o referencia.", "Falta descripción", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Me.Cursor = Cursors.WaitCursor

            ' 2. Convertir archivo a Bytes (BLOB)
            Dim bytesArchivo As Byte() = File.ReadAllBytes(_rutaArchivo)
            Dim extension As String = Path.GetExtension(_rutaArchivo).ToLower()
            Dim nombreOriginal As String = Path.GetFileName(_rutaArchivo)

            ' 3. Guardar en Base de Datos
            Using db As New PoloNuevoEntities()
                ' --- AQUI ESTABA EL ERROR ---
                ' Cambiamos 'New Documento' por 'New Documentos' (nombre de la tabla)
                Dim nuevoDoc As New Documentos()
                ' -----------------------------

                nuevoDoc.ReclusoId = _idRecluso
                nuevoDoc.TipoDocumentoId = Convert.ToInt32(cmbTipo.SelectedValue)
                nuevoDoc.Descripcion = txtDescripcion.Text.Trim()
                nuevoDoc.FechaCarga = DateTime.Now

                ' Datos del archivo
                nuevoDoc.Contenido = bytesArchivo
                nuevoDoc.Extension = extension
                nuevoDoc.NombreArchivo = nombreOriginal

                db.Documentos.Add(nuevoDoc)
                db.SaveChanges()
            End Using

            MessageBox.Show("Documento guardado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.DialogResult = DialogResult.OK
            Me.Close()

        Catch ex As Exception
            MessageBox.Show("Error al guardar: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Me.Close()
    End Sub
End Class