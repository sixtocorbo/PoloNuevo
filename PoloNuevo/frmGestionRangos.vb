Imports System.Data.Entity

Public Class frmGestionRangos

    Private _idEdicion As Integer = 0

    Private Sub frmGestionRangos_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CargarTipos()
        CargarGrilla()
        ModoEdicion(False)
    End Sub

    Private Sub CargarTipos()
        Using db As New PoloNuevoEntities()
            cmbTipo.DataSource = db.TiposDocumento.Where(Function(t) t.Nombre <> "ARCHIVO").OrderBy(Function(t) t.Nombre).ToList()
            cmbTipo.DisplayMember = "Nombre"
            cmbTipo.ValueMember = "Id"
        End Using
    End Sub

    Private Sub CargarGrilla()
        Using db As New PoloNuevoEntities()
            Dim lista = db.NumeracionRangos.Include("TiposDocumento") _
                          .Select(Function(r) New With {
                              .Id = r.Id,
                              .Tipo = r.TiposDocumento.Nombre,
                              .Nombre = r.NombreRango,
                              .Inicio = r.NumeroInicio,
                              .Fin = r.NumeroFin,
                              .Actual = r.UltimoUtilizado,
                              .Activo = r.Activo
                          }).OrderByDescending(Function(r) r.Id).ToList()
            dgvRangos.DataSource = lista
            dgvRangos.Columns("Id").Visible = False
        End Using
    End Sub

    Private Sub ModoEdicion(habilitar As Boolean)
        pnlEditor.Enabled = habilitar
        btnNuevo.Enabled = Not habilitar
        btnEditar.Enabled = Not habilitar
        dgvRangos.Enabled = Not habilitar

        If Not habilitar Then
            ' Limpiar
            txtNombre.Clear()
            txtInicio.Text = "1"
            txtFin.Text = "1000"
            txtUltimo.Text = "0"
            _idEdicion = 0
        End If
    End Sub

    ' --- BOTONES ---

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        ModoEdicion(True)
        txtNombre.Focus()
        chkActivo.Checked = True
    End Sub

    Private Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        If dgvRangos.SelectedRows.Count = 0 Then Return

        _idEdicion = Convert.ToInt32(dgvRangos.SelectedRows(0).Cells("Id").Value)
        Using db As New PoloNuevoEntities()
            Dim r = db.NumeracionRangos.Find(_idEdicion)
            If r IsNot Nothing Then
                cmbTipo.SelectedValue = r.TipoDocumentoId
                txtNombre.Text = r.NombreRango
                txtInicio.Text = r.NumeroInicio.ToString()
                txtFin.Text = r.NumeroFin.ToString()
                txtUltimo.Text = r.UltimoUtilizado.ToString()
                chkActivo.Checked = r.Activo
                ModoEdicion(True)
            End If
        End Using
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        ModoEdicion(False)
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        ' 1. Validaciones de formulario (Interfaz)
        If txtNombre.Text = "" Then
            MessageBox.Show("Falta el nombre.")
            Return
        End If

        Dim ini, fin, ult As Integer
        Integer.TryParse(txtInicio.Text, ini)
        Integer.TryParse(txtFin.Text, fin)
        Integer.TryParse(txtUltimo.Text, ult)

        If ini >= fin Then
            MessageBox.Show("El número de Inicio debe ser menor al número Fin.")
            Return
        End If

        ' 2. Intento de Guardado en Base de Datos
        Try
            Using db As New PoloNuevoEntities()
                Dim rango As NumeracionRangos

                ' Determinar si es Nuevo o Edición
                If _idEdicion = 0 Then
                    rango = New NumeracionRangos()
                    db.NumeracionRangos.Add(rango)
                Else
                    rango = db.NumeracionRangos.Find(_idEdicion)
                End If

                ' Asignar valores desde los controles
                rango.TipoDocumentoId = Convert.ToInt32(cmbTipo.SelectedValue)
                rango.NombreRango = txtNombre.Text.Trim()
                rango.NumeroInicio = ini
                rango.NumeroFin = fin
                rango.UltimoUtilizado = ult
                rango.Activo = chkActivo.Checked

                ' Regla de Negocio: Si este rango se activa, desactivar otros del mismo tipo
                If rango.Activo Then
                    Dim otros = db.NumeracionRangos.Where(Function(x) x.TipoDocumentoId = rango.TipoDocumentoId And x.Id <> rango.Id And x.Activo = True).ToList()
                    For Each o In otros
                        o.Activo = False
                    Next
                End If

                ' Guardar Cambios
                db.SaveChanges()

                MessageBox.Show("Rango guardado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)

                ' Limpiar y recargar
                ModoEdicion(False)
                CargarGrilla()
            End Using

            ' --- CAPTURA DE ERRORES DE VALIDACIÓN (CORREGIDO) ---
        Catch dbEx As System.Data.Entity.Validation.DbEntityValidationException
            Dim mensaje As String = ""

            ' CAMBIO: Usamos 'valResult' en lugar de 'err'
            For Each valResult In dbEx.EntityValidationErrors
                For Each validationError In valResult.ValidationErrors
                    mensaje &= "- Campo: " & validationError.PropertyName & vbCrLf &
                               "  Error: " & validationError.ErrorMessage & vbCrLf
                Next
            Next

            MessageBox.Show("No se pudo guardar debido a los siguientes errores de validación:" & vbCrLf & vbCrLf & mensaje, "Error de Datos", MessageBoxButtons.OK, MessageBoxIcon.Warning)

            ' --- CAPTURA DE OTROS ERRORES ---
        Catch ex As Exception
            MessageBox.Show("Ocurrió un error inesperado: " & ex.Message, "Error Crítico", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class