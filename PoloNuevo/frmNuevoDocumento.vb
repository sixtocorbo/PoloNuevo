Imports System.IO
Imports System.Data.Entity

Public Class frmNuevoDocumento

    Private _rutaArchivo As String = ""
    Private _archivoBytes As Byte() = Nothing
    Private _archivoNombre As String = ""
    Private _archivoExt As String = ""

    ' =========================================================
    ' CONSTRUCTOR (Simplificado)
    ' =========================================================
    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub frmNuevoDocumento_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CargarTiposDocumento()
        lblArchivoSeleccionado.Text = "Ningún archivo seleccionado"
        lblArchivoSeleccionado.ForeColor = Color.Gray
    End Sub

    Private Sub CargarTiposDocumento()
        Using db As New PoloNuevoEntities()
            ' Cargamos los tipos, excluyendo ARCHIVO si es necesario para mantener consistencia
            cmbTipo.DataSource = db.TiposDocumento.Where(Function(t) t.Nombre <> "ARCHIVO").OrderBy(Function(t) t.Nombre).ToList()
            cmbTipo.DisplayMember = "Nombre"
            cmbTipo.ValueMember = "Id"
        End Using
    End Sub

    ' =========================================================
    ' SELECCIÓN DE ARCHIVO
    ' =========================================================
    Private Sub btnExaminar_Click(sender As Object, e As EventArgs) Handles btnExaminar.Click
        Using op As New OpenFileDialog()
            op.Filter = "Documentos y Fotos|*.pdf;*.jpg;*.jpeg;*.png;*.doc;*.docx;*.xls;*.xlsx|Todos los archivos|*.*"
            op.Title = "Seleccione el documento a cargar"

            If op.ShowDialog() = DialogResult.OK Then

                ' Validar tamaño (Ejemplo: Máximo 20MB)
                Dim info As New FileInfo(op.FileName)
                If info.Length > 20 * 1024 * 1024 Then
                    MessageBox.Show("El archivo es demasiado grande (Máx 20MB).", "Tamaño Excedido", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    LimpiarSeleccionArchivo()
                    Return
                End If

                Try
                    ' Leemos el archivo a memoria inmediatamente para evitar bloqueos
                    _archivoBytes = File.ReadAllBytes(op.FileName)
                    _archivoNombre = info.Name
                    _archivoExt = info.Extension.ToLower()
                    _rutaArchivo = op.FileName ' Solo referencia visual

                    lblArchivoSeleccionado.Text = _archivoNombre
                    lblArchivoSeleccionado.ForeColor = Color.Black
                Catch ex As Exception
                    MessageBox.Show("Error al leer el archivo: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    LimpiarSeleccionArchivo()
                End Try
            End If
        End Using
    End Sub

    Private Sub LimpiarSeleccionArchivo()
        _rutaArchivo = ""
        _archivoBytes = Nothing
        _archivoNombre = ""
        _archivoExt = ""
        lblArchivoSeleccionado.Text = "Ningún archivo seleccionado"
        lblArchivoSeleccionado.ForeColor = Color.Gray
    End Sub

    ' =========================================================
    ' GUARDAR
    ' =========================================================
    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        ' 1. Validaciones
        If _archivoBytes Is Nothing OrElse _archivoBytes.Length = 0 Then
            MessageBox.Show("Debe seleccionar un archivo válido.", "Falta archivo", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        If String.IsNullOrWhiteSpace(txtDescripcion.Text) Then
            MessageBox.Show("Escriba una breve descripción del documento.", "Falta descripción", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Me.Cursor = Cursors.WaitCursor

            Using db As New PoloNuevoEntities()
                Dim nuevoDoc As New Documentos()

                ' Asignación de datos
                ' Nota: Ya no asignamos ReclusoId
                nuevoDoc.TipoDocumentoId = Convert.ToInt32(cmbTipo.SelectedValue)
                nuevoDoc.Descripcion = txtDescripcion.Text.Trim()
                nuevoDoc.FechaCarga = DateTime.Now

                ' Si tu formulario tuviera un campo de Referencia Externa (txtNumero), iría aquí:
                ' nuevoDoc.ReferenciaExterna = txtNumero.Text.Trim() 
                ' Por ahora queda NULL o vacío según la base de datos.

                ' Datos del archivo
                nuevoDoc.Contenido = _archivoBytes
                nuevoDoc.Extension = _archivoExt
                nuevoDoc.NombreArchivo = _archivoNombre

                db.Documentos.Add(nuevoDoc)

                ' Opcional: Crear un movimiento de Entrada automático
                Dim mov As New MovimientosDocumentos()
                mov.DocumentoId = nuevoDoc.Id ' EF asignará el ID temporalmente
                mov.FechaMovimiento = nuevoDoc.FechaCarga
                mov.Origen = "CARGA MANUAL"
                mov.Destino = "MESA DE ENTRADA"
                mov.EsSalida = False
                mov.Observaciones = "Carga directa de archivo"
                db.MovimientosDocumentos.Add(mov)

                db.SaveChanges()
            End Using

            MessageBox.Show("Documento cargado al sistema correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
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