Public Class frmNuevoRecluso

    Private _idRecluso As Integer = 0

    ' CONSTRUCTOR 1: NUEVO
    Public Sub New()
        InitializeComponent()
        _idRecluso = 0
    End Sub

    ' CONSTRUCTOR 2: EDITAR
    Public Sub New(id As Integer)
        InitializeComponent()
        _idRecluso = id
    End Sub

    Private Sub frmNuevoRecluso_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If _idRecluso > 0 Then
            Me.Text = "Editar Datos del Recluso"
            btnGuardar.Text = "GUARDAR CAMBIOS"
            CargarDatos()
        Else
            ' Ocultar el grupo de estado si es nuevo (asumimos que entra activo)
            grpEstado.Enabled = False
        End If
    End Sub

    Private Sub CargarDatos()
        Using db As New PoloNuevoEntities()
            Dim r = db.Reclusos.Find(_idRecluso)
            If r IsNot Nothing Then
                txtNombre.Text = r.Nombre
                txtCedula.Text = r.Cedula

                ' CORRECCIÓN FECHA: Al ser obligatoria en BD, se asigna directo
                dtpNacimiento.Value = r.FechaNacimiento

                ' CORRECCIÓN ESTADO:
                ' Como no tienes la columna 'Activo', por ahora comentamos esto.
                ' Si en el futuro agregas un campo 'Estado' o 'Activo' en SQL, descomenta esto:
                ' If r.Activo Then
                '    radActivo.Checked = True
                ' Else
                '    radInactivo.Checked = True
                ' End If
            End If
        End Using
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If String.IsNullOrWhiteSpace(txtNombre.Text) Then
            MessageBox.Show("El nombre es obligatorio.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Using db As New PoloNuevoEntities()
                If _idRecluso = 0 Then
                    ' --- NUEVO ---
                    Dim r As New Reclusos()
                    r.Nombre = txtNombre.Text.Trim()
                    r.Cedula = txtCedula.Text.Trim()
                    r.FechaNacimiento = dtpNacimiento.Value

                    ' CORRECCIÓN: Comentamos Activo y FechaIngreso porque no existen en tu tabla aún
                    ' r.Activo = radActivo.Checked
                    ' r.FechaIngreso = DateTime.Now 

                    db.Reclusos.Add(r)
                    db.SaveChanges()
                    MessageBox.Show("Recluso registrado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    ' --- EDITAR ---
                    Dim r = db.Reclusos.Find(_idRecluso)
                    If r IsNot Nothing Then
                        r.Nombre = txtNombre.Text.Trim()
                        r.Cedula = txtCedula.Text.Trim()
                        r.FechaNacimiento = dtpNacimiento.Value

                        ' CORRECCIÓN: Comentamos Activo
                        ' r.Activo = radActivo.Checked

                        db.SaveChanges()
                        MessageBox.Show("Datos actualizados correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End If

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