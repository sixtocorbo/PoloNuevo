Public Class frmAsignarTrabajo

    Private _idReclusoFijo As Integer = 0

    ' Constructor vacío (Uso general)
    Public Sub New()
        InitializeComponent()
        _idReclusoFijo = 0
    End Sub

    ' Constructor Específico (Viene del Legajo)
    Public Sub New(idRecluso As Integer)
        InitializeComponent()
        _idReclusoFijo = idRecluso
    End Sub

    Private Sub frmAsignarTrabajo_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CargarCombos()

        ' SI VENIMOS DEL LEGAJO (ID FIJO)
        If _idReclusoFijo > 0 Then
            cmbReclusos.SelectedValue = _idReclusoFijo
            cmbReclusos.Enabled = False ' Bloqueamos para evitar errores
        End If
    End Sub

    Private Sub CargarCombos()
        Using db As New PoloNuevoEntities()
            ' --- CORRECCIÓN AQUÍ ---
            ' Quitamos el filtro ".Where(Function(r) r.Activo = True)" 
            ' porque la columna Activo no existe en tu tabla Reclusos.

            Dim reclusos = db.Reclusos _
                             .Select(Function(r) New With {.Id = r.Id, .Nombre = r.Nombre}) _
                             .OrderBy(Function(x) x.Nombre) _
                             .ToList()

            cmbReclusos.DataSource = reclusos
            cmbReclusos.DisplayMember = "Nombre"
            cmbReclusos.ValueMember = "Id"

            ' Cargar Tareas
            Dim tareas = db.Tareas.OrderBy(Function(t) t.Nombre).ToList()
            cmbTareas.DataSource = tareas
            cmbTareas.DisplayMember = "Nombre"
            cmbTareas.ValueMember = "Id"
        End Using
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        ' Validaciones simples
        If cmbReclusos.SelectedValue Is Nothing Then
            MessageBox.Show("Seleccione un recluso.", "Falta Dato", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        If cmbTareas.SelectedValue Is Nothing Then
            MessageBox.Show("Seleccione una tarea.", "Falta Dato", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Using db As New PoloNuevoEntities()
                Dim com As New Comisiones()
                com.ReclusoId = Convert.ToInt32(cmbReclusos.SelectedValue)
                com.TareaId = Convert.ToInt32(cmbTareas.SelectedValue)
                com.FechaInicio = dtpInicio.Value
                com.Activa = True

                ' Ahora sí existe txtObservaciones en el diseño
                com.Observaciones = txtObservaciones.Text.Trim()

                db.Comisiones.Add(com)
                db.SaveChanges()

                MessageBox.Show("Tarea asignada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
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