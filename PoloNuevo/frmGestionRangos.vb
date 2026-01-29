Public Class frmGestionRangos

    Private _idRangoEditar As Integer = 0

    Private Sub frmGestionRangos_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CargarTipos()
        CargarRangos()
        Limpiar() ' Esto asegura que arranque limpio y sin selección
    End Sub

    Private Sub CargarTipos()
        Using db As New PoloNuevoEntities()
            cmbTipo.DataSource = db.TiposDocumento.Where(Function(t) t.Nombre <> "ARCHIVO").ToList()
            cmbTipo.DisplayMember = "Nombre"
            cmbTipo.ValueMember = "Id"
        End Using
    End Sub

    Private Sub CargarRangos()
        Using db As New PoloNuevoEntities()
            Dim lista = db.NumeracionRangos _
                          .Select(Function(r) New With {
                              .Id = r.Id,
                              .Tipo = r.TiposDocumento.Nombre,
                              .Rango = r.NombreRango,
                              .Inicio = r.NumeroInicio,
                              .Fin = r.NumeroFin,
                              .Actual = r.UltimoUtilizado,
                              .Activo = r.Activo
                          }).ToList()
            dgvRangos.DataSource = lista
            If dgvRangos.Columns("Id") IsNot Nothing Then dgvRangos.Columns("Id").Visible = False
        End Using
    End Sub

    Private Sub Limpiar()
        _idRangoEditar = 0
        txtNombre.Text = ""
        numInicio.Value = 1
        numFin.Value = 1000
        numUltimo.Value = 0
        chkActivo.Checked = True
        If cmbTipo.Items.Count > 0 Then cmbTipo.SelectedIndex = 0

        btnEliminar.Enabled = False
        btnGuardar.Text = "GUARDAR RANGO"

        ' === CORRECCIÓN CLAVE ===
        ' Quitamos la selección visual de la grilla.
        ' Esto permite que si el usuario vuelve a hacer clic en la misma fila,
        ' el evento SelectionChanged se dispare de nuevo.
        dgvRangos.ClearSelection()
    End Sub

    Private Sub dgvRangos_SelectionChanged(sender As Object, e As EventArgs) Handles dgvRangos.SelectionChanged
        If dgvRangos.SelectedRows.Count > 0 Then
            Dim id As Integer = Convert.ToInt32(dgvRangos.SelectedRows(0).Cells("Id").Value)
            CargarEdicion(id)
        End If
    End Sub

    Private Sub CargarEdicion(id As Integer)
        Using db As New PoloNuevoEntities()
            Dim r = db.NumeracionRangos.Find(id)
            If r IsNot Nothing Then
                _idRangoEditar = r.Id
                cmbTipo.SelectedValue = r.TipoDocumentoId
                txtNombre.Text = r.NombreRango
                numInicio.Value = r.NumeroInicio
                numFin.Value = r.NumeroFin
                numUltimo.Value = r.UltimoUtilizado
                chkActivo.Checked = If(r.Activo.HasValue, r.Activo.Value, False)

                btnEliminar.Enabled = True
                btnGuardar.Text = "ACTUALIZAR RANGO"
            End If
        End Using
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        ' 1. Validaciones de Formulario (Básicas)
        If txtNombre.Text.Trim = "" Then
            MessageBox.Show("Ingrese un nombre para el rango (Ej: Libro A 2026).", "Falta Nombre", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        If numInicio.Value >= numFin.Value Then
            MessageBox.Show("El número de inicio debe ser menor al número de fin.", "Rango Inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        If numUltimo.Value < numInicio.Value - 1 Then
            numUltimo.Value = numInicio.Value - 1
        End If

        Dim idTipo As Integer = Convert.ToInt32(cmbTipo.SelectedValue)
        Dim nuevoInicio As Integer = Convert.ToInt32(numInicio.Value)
        Dim nuevoFin As Integer = Convert.ToInt32(numFin.Value)

        Try
            Using db As New PoloNuevoEntities()

                ' =================================================================
                ' 2. VALIDACIÓN DE SUPERPOSICIÓN Y UNICIDAD (NUEVO BLINDAJE)
                ' =================================================================
                If chkActivo.Checked Then
                    ' Buscamos otros rangos del mismo tipo que estén ACTIVOS y que NO sean el que estoy editando
                    Dim rangosActivos = db.NumeracionRangos _
                                          .Where(Function(r) r.TipoDocumentoId = idTipo _
                                                             And r.Activo = True _
                                                             And r.Id <> _idRangoEditar) _
                                          .ToList()

                    For Each r In rangosActivos
                        ' Fórmula matemática para detectar superposición de rangos:
                        ' (InicioA <= FinB) y (FinA >= InicioB)
                        If (nuevoInicio <= r.NumeroFin) And (nuevoFin >= r.NumeroInicio) Then
                            MessageBox.Show($"ERROR CRÍTICO DE NUMERACIÓN:{vbCrLf}{vbCrLf}" &
                                            $"El rango que intenta guardar ({nuevoInicio} - {nuevoFin}) " &
                                            $"se superpone con otro rango ya activo:{vbCrLf}" &
                                            $"NOMBRE: {r.NombreRango}{vbCrLf}" &
                                            $"RANGO: {r.NumeroInicio} - {r.NumeroFin}{vbCrLf}{vbCrLf}" &
                                            "Por seguridad, no se puede guardar. Desactive el rango anterior o modifique los números.",
                                            "Conflicto de Rangos", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Return ' DETENEMOS TODO
                        End If
                    Next
                End If
                ' =================================================================

                ' 3. Guardado (Si pasó las validaciones)
                Dim rango As NumeracionRangos
                If _idRangoEditar = 0 Then
                    rango = New NumeracionRangos()
                    db.NumeracionRangos.Add(rango)
                Else
                    rango = db.NumeracionRangos.Find(_idRangoEditar)
                End If

                rango.TipoDocumentoId = idTipo
                rango.NombreRango = txtNombre.Text.Trim()
                rango.NumeroInicio = nuevoInicio
                rango.NumeroFin = nuevoFin
                rango.UltimoUtilizado = Convert.ToInt32(numUltimo.Value)
                rango.Activo = chkActivo.Checked

                db.SaveChanges()
                MessageBox.Show("Rango guardado y verificado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)

                CargarRangos()
                Limpiar()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error técnico: " & ex.Message)
        End Try
    End Sub

    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        If _idRangoEditar = 0 Then Return

        If MessageBox.Show("¿Seguro que desea eliminar este rango?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
            Try
                Using db As New PoloNuevoEntities()
                    Dim r = db.NumeracionRangos.Find(_idRangoEditar)
                    If r IsNot Nothing Then
                        db.NumeracionRangos.Remove(r)
                        db.SaveChanges()
                    End If
                End Using

                CargarRangos()
                Limpiar()
            Catch ex As Exception
                MessageBox.Show("No se puede eliminar porque ya se usaron números de este rango. Desactívelo en su lugar.", "Error de Integridad")
            End Try
        End If
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        Limpiar()
    End Sub
End Class