Imports System.IO

Public Class frmLegajo

    Private _idRecluso As Integer

    Public Sub New(id As Integer)
        InitializeComponent()
        _idRecluso = id
    End Sub

    Private Sub frmLegajo_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        RecargarTodo()
    End Sub

    Private Sub RecargarTodo()
        CargarCabecera()
        CargarFoto()
        CargarDocumentos()
        CargarSituacionLegal()
        CargarLaboral()
    End Sub

    ' =========================================================
    ' 1. DATOS PERSONALES
    ' =========================================================
    Private Sub CargarCabecera()
        Using db As New PoloNuevoEntities()
            Dim r = db.Reclusos.Find(_idRecluso)
            If r IsNot Nothing Then
                lblNombre.Text = r.Nombre.ToUpper()
                lblCedula.Text = "C.I.: " & r.Cedula.ToString()
                lblUbicacion.Text = "Ubicación: Módulo " & r.Modulo.ToString()
                lblEstado.Text = "ESTADO GENERAL"
            End If
        End Using
    End Sub

    Private Sub btnEditarDatos_Click(sender As Object, e As EventArgs) Handles btnEditarDatos.Click
        Dim frm As New frmNuevoRecluso(_idRecluso)
        If frm.ShowDialog() = DialogResult.OK Then
            RecargarTodo()
        End If
    End Sub

    ' =========================================================
    ' 2. FOTO DE PERFIL
    ' =========================================================
    Private Sub CargarFoto()
        Try
            Using db As New PoloNuevoEntities()
                Dim docFoto = db.Documentos _
                                .Where(Function(d) d.ReclusoId = _idRecluso And d.TiposDocumento.Nombre = "ARCHIVO") _
                                .OrderByDescending(Function(d) d.FechaCarga) _
                                .FirstOrDefault()

                If docFoto IsNot Nothing AndAlso docFoto.Contenido IsNot Nothing Then
                    Using ms As New MemoryStream(docFoto.Contenido)
                        picFoto.Image = Image.FromStream(ms)
                    End Using
                Else
                    picFoto.Image = Nothing
                End If
            End Using
        Catch ex As Exception
            picFoto.Image = Nothing
        End Try
    End Sub

    Private Sub btnCambiarFoto_Click(sender As Object, e As EventArgs) Handles btnCambiarFoto.Click
        Using ofd As New OpenFileDialog()
            ofd.Filter = "Imágenes|*.jpg;*.jpeg;*.png;*.bmp"
            If ofd.ShowDialog() = DialogResult.OK Then
                Try
                    Dim bytes As Byte() = File.ReadAllBytes(ofd.FileName)
                    Using db As New PoloNuevoEntities()
                        Dim tipoArchivo = db.TiposDocumento.FirstOrDefault(Function(t) t.Nombre = "ARCHIVO")
                        Dim idTipo As Integer = If(tipoArchivo IsNot Nothing, tipoArchivo.Id, 11)

                        Dim doc As New Documentos()
                        doc.ReclusoId = _idRecluso
                        doc.TipoDocumentoId = idTipo
                        doc.Descripcion = "Foto Perfil Actualizada"
                        doc.NombreArchivo = Path.GetFileName(ofd.FileName)
                        doc.Extension = Path.GetExtension(ofd.FileName)
                        doc.Contenido = bytes
                        doc.FechaCarga = DateTime.Now

                        db.Documentos.Add(doc)
                        db.SaveChanges()
                    End Using
                    CargarFoto()
                    CargarDocumentos()
                    MessageBox.Show("Foto actualizada.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Catch ex As Exception
                    MessageBox.Show("Error: " & ex.Message)
                End Try
            End If
        End Using
    End Sub

    ' =========================================================
    ' 3. SITUACIÓN LEGAL
    ' =========================================================
    Private Sub CargarSituacionLegal()
        Using db As New PoloNuevoEntities()
            Dim legal = db.VencimientosLegales.FirstOrDefault(Function(v) v.ReclusoId = _idRecluso)
            If legal IsNot Nothing Then
                If legal.FechaVencimientoPena.HasValue Then dtpVtoPena.Value = legal.FechaVencimientoPena.Value
                If legal.FechaInformeOperativo.HasValue Then dtpInfoOperativo.Value = legal.FechaInformeOperativo.Value
            Else
                dtpVtoPena.Value = DateTime.Now
                dtpInfoOperativo.Value = DateTime.Now
            End If
        End Using
    End Sub

    Private Sub btnGuardarLegal_Click(sender As Object, e As EventArgs) Handles btnGuardarLegal.Click
        Try
            Using db As New PoloNuevoEntities()
                Dim legal = db.VencimientosLegales.FirstOrDefault(Function(v) v.ReclusoId = _idRecluso)
                If legal Is Nothing Then
                    legal = New VencimientosLegales()
                    legal.ReclusoId = _idRecluso
                    db.VencimientosLegales.Add(legal)
                End If
                legal.FechaVencimientoPena = dtpVtoPena.Value
                legal.FechaInformeOperativo = dtpInfoOperativo.Value
                db.SaveChanges()
                MessageBox.Show("Fechas guardadas.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Using
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub

    ' =========================================================
    ' 4. HISTORIAL DE DOCUMENTOS
    ' =========================================================
    Private Sub CargarDocumentos()
        Using db As New PoloNuevoEntities()
            Dim docs = db.Documentos _
                         .Where(Function(d) d.ReclusoId = _idRecluso And d.Extension = ".phy") _
                         .OrderByDescending(Function(d) d.FechaCarga) _
                         .Select(Function(d) New With {
                             .Fecha = d.FechaCarga,
                             .Tipo = d.TiposDocumento.Nombre,
                             .Numero = d.ReferenciaExterna,
                             .Asunto = d.Descripcion,
                             .Estado = If(d.MovimientosDocumentos.Count() = 1, "PENDIENTE", "MOVIDO")
                         }) _
                         .ToList()
            dgvDocumentos.DataSource = docs
            If dgvDocumentos.Columns("Asunto") IsNot Nothing Then dgvDocumentos.Columns("Asunto").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        End Using
    End Sub

    ' =========================================================
    ' 5. GESTIÓN LABORAL (ALTA Y BAJA)
    ' =========================================================
    Private Sub CargarLaboral()
        Using db As New PoloNuevoEntities()
            Dim trabajos = db.Comisiones _
                             .Where(Function(c) c.ReclusoId = _idRecluso) _
                             .OrderByDescending(Function(c) c.FechaInicio) _
                             .Select(Function(c) New With {
                                 .Id = c.Id,
                                 .Tarea = c.Tareas.Nombre,
                                 .Inicio = c.FechaInicio,
                                 .Fin = c.FechaFin,
                                 .Estado = If(c.Activa, "ACTIVA", "FINALIZADA"),
                                 .Detalle = c.Observaciones
                             }) _
                             .ToList()

            dgvLaboral.DataSource = trabajos
            dgvLaboral.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            If dgvLaboral.Columns("Id") IsNot Nothing Then dgvLaboral.Columns("Id").Visible = False
        End Using
    End Sub

    Private Sub btnAsignarTrabajo_Click(sender As Object, e As EventArgs) Handles btnAsignarTrabajo.Click
        ' Abrimos el formulario de asignación pasando el ID de este preso
        Dim frm As New frmAsignarTrabajo(_idRecluso)
        If frm.ShowDialog() = DialogResult.OK Then
            CargarLaboral() ' Refrescar la lista tras asignar
        End If
    End Sub

    Private Sub btnBajaTrabajo_Click(sender As Object, e As EventArgs) Handles btnBajaTrabajo.Click
        If dgvLaboral.SelectedRows.Count = 0 Then
            MessageBox.Show("Seleccione una tarea para finalizar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim estado As String = dgvLaboral.SelectedRows(0).Cells("Estado").Value.ToString()
        If estado = "FINALIZADA" Then
            MessageBox.Show("Esta tarea ya está finalizada.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        If MessageBox.Show("¿Confirmar baja de esta actividad laboral?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Dim idComision As Integer = Convert.ToInt32(dgvLaboral.SelectedRows(0).Cells("Id").Value)
            DarBajaLaboral(idComision)
            CargarLaboral()
        End If
    End Sub

    Private Sub DarBajaLaboral(id As Integer)
        Using db As New PoloNuevoEntities()
            Dim com = db.Comisiones.Find(id)
            If com IsNot Nothing Then
                com.Activa = False
                com.FechaFin = DateTime.Now
                com.MotivoBaja = "Baja desde Legajo"
                db.SaveChanges()
            End If
        End Using
    End Sub

End Class