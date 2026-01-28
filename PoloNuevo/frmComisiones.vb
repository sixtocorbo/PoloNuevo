Public Class frmComisiones

    Private Sub frmComisiones_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ConfigurarGrilla()
        CargarLista()
    End Sub

    Private Sub ConfigurarGrilla()
        dgvComisiones.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvComisiones.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvComisiones.MultiSelect = False
        dgvComisiones.ReadOnly = True
        dgvComisiones.BackgroundColor = Color.White
    End Sub

    Private Sub CargarLista()
        Using db As New PoloNuevoEntities()
            ' CONSULTA LINQ: Traemos datos cruzados
            Dim query = From c In db.Comisiones
                        Join r In db.Reclusos On c.ReclusoId Equals r.Id
                        Join t In db.Tareas On c.TareaId Equals t.Id
                        Select New With {
                            .Id = c.Id,
                            .ReclusoId = c.ReclusoId, ' <--- IMPORTANTE PARA NAVEGAR
                            .Recluso = r.Nombre,
                            .Tarea = t.Nombre,
                            .Desde = c.FechaInicio,
                            .Hasta = c.FechaFin,
                            .Estado = If(c.Activa, "ACTIVA", "FINALIZADA"),
                            .Activa = c.Activa ' Para el filtro
                        }

            ' Filtro: Si no está marcado el check, solo mostramos las ACTIVAS
            If Not chkVerHistorial.Checked Then
                query = query.Where(Function(x) x.Activa = True)
            End If

            ' Ordenamos: Primero activos, luego por fecha
            Dim lista = query.OrderByDescending(Function(x) x.Activa) _
                             .ThenByDescending(Function(x) x.Desde) _
                             .ToList()

            dgvComisiones.DataSource = lista

            ' Ocultar columnas técnicas
            If dgvComisiones.Columns("Id") IsNot Nothing Then dgvComisiones.Columns("Id").Visible = False
            If dgvComisiones.Columns("ReclusoId") IsNot Nothing Then dgvComisiones.Columns("ReclusoId").Visible = False
            If dgvComisiones.Columns("Activa") IsNot Nothing Then dgvComisiones.Columns("Activa").Visible = False
        End Using
    End Sub

    Private Sub chkVerHistorial_CheckedChanged(sender As Object, e As EventArgs) Handles chkVerHistorial.CheckedChanged
        CargarLista()
    End Sub

    ' =========================================================
    ' NAVEGACIÓN: SI QUIEREN EDITAR, LOS MANDAMOS AL LEGAJO
    ' =========================================================
    Private Sub btnVerLegajo_Click(sender As Object, e As EventArgs) Handles btnVerLegajo.Click
        If dgvComisiones.SelectedRows.Count = 0 Then
            MessageBox.Show("Seleccione una línea para ir al legajo.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        ' Obtenemos el ID del Recluso de la fila seleccionada
        Dim idRecluso As Integer = Convert.ToInt32(dgvComisiones.SelectedRows(0).Cells("ReclusoId").Value)

        ' Abrimos el Legajo (donde ahora están los botones de Asignar/Baja)
        Dim frm As New frmLegajo(idRecluso)
        frm.ShowDialog()

        ' Al volver, recargamos la lista por si hubo cambios (ej: dieron de baja)
        CargarLista()
    End Sub

End Class