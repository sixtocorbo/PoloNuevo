Public Class frmReclusos

    Private Sub frmReclusos_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CargarGrid()
    End Sub

    Private Sub CargarGrid()
        Using db As New PoloNuevoEntities()
            Dim query = db.Reclusos.AsQueryable()

            ' Buscador Inteligente
            Dim texto = txtBuscar.Text.Trim().ToLower()
            If Not String.IsNullOrEmpty(texto) Then
                Dim palabras = texto.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)

                For Each p In palabras
                    Dim term = p

                    ' CORRECCIÓN 1: Convertimos Cedula a String para poder usar Contains
                    ' Nota: Si tu versión de Entity Framework es muy vieja, esto podría dar error en ejecución.
                    ' Si pasa eso, avísame y lo cambiamos a búsqueda exacta.
                    query = query.Where(Function(r) r.Nombre.Contains(term) Or r.Cedula.ToString().Contains(term))
                Next
            End If

            ' Proyección y Ejecución
            Dim lista = query.OrderBy(Function(r) r.Nombre) _
                             .Select(Function(r) New With {
                                 .Id = r.Id,
                                 .Nombre = r.Nombre,
                                 .Cedula = r.Cedula,
                                 .Nacimiento = r.FechaNacimiento,
                                 .Estado = "GENERAL" ' CORRECCIÓN 2: Texto fijo porque no existe columna 'Activo'
                             }) _
                             .Take(100) _ ' Límite visual para rapidez
                             .ToList()

            dgvReclusos.DataSource = lista
            If dgvReclusos.Columns("Id") IsNot Nothing Then dgvReclusos.Columns("Id").Visible = False
        End Using
    End Sub

    Private Sub txtBuscar_TextChanged(sender As Object, e As EventArgs) Handles txtBuscar.TextChanged
        CargarGrid()
    End Sub

    ' 1. NUEVO
    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        Dim frm As New frmNuevoRecluso() ' Sin ID = Nuevo
        If frm.ShowDialog() = DialogResult.OK Then
            CargarGrid()
        End If
    End Sub

    ' 2. EDITAR
    Private Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        If dgvReclusos.SelectedRows.Count = 0 Then Return
        Dim id As Integer = Convert.ToInt32(dgvReclusos.SelectedRows(0).Cells("Id").Value)

        Dim frm As New frmNuevoRecluso(id) ' Con ID = Editar
        If frm.ShowDialog() = DialogResult.OK Then
            CargarGrid()
        End If
    End Sub

    ' 3. LEGAJO
    Private Sub btnLegajo_Click(sender As Object, e As EventArgs) Handles btnLegajo.Click
        If dgvReclusos.SelectedRows.Count = 0 Then Return
        Dim id As Integer = Convert.ToInt32(dgvReclusos.SelectedRows(0).Cells("Id").Value)

        ' Abrimos el formulario de Legajo
        Dim frm As New frmLegajo(id)
        frm.ShowDialog()
    End Sub

    ' Doble Clic para editar rápido
    Private Sub dgvReclusos_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvReclusos.CellDoubleClick
        If e.RowIndex >= 0 Then
            btnEditar.PerformClick()
        End If
    End Sub

End Class