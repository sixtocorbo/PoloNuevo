Public Class frmUsuarios

    Private Sub frmUsuarios_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' SEGURIDAD: Solo Admin entra aquí
        If Sesion.UsuarioActual Is Nothing OrElse Not Sesion.UsuarioActual.EsAdmin Then
            MessageBox.Show("Acceso Denegado. Se requieren permisos de Administrador.", "Seguridad", MessageBoxButtons.OK, MessageBoxIcon.Stop)
            Me.Close()
            Return
        End If

        CargarUsuarios()
        cmbRol.SelectedIndex = 0 ' Default Operador
    End Sub

    Private Sub CargarUsuarios()
        Using db As New PoloNuevoEntities()
            ' Cargamos lista ocultando la clave real por seguridad
            dgvUsuarios.DataSource = db.Usuarios.Select(Function(u) New With {
                .ID = u.Id,
                .Usuario = u.NombreUsuario,
                .Rol = u.Rol
            }).ToList()
        End Using
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If String.IsNullOrWhiteSpace(txtNombre.Text) Or String.IsNullOrWhiteSpace(txtClave.Text) Then
            MessageBox.Show("Complete todos los campos.")
            Return
        End If

        Using db As New PoloNuevoEntities()
            ' Verificamos que no exista el nombre
            Dim nombre = txtNombre.Text.Trim()
            If db.Usuarios.Any(Function(u) u.NombreUsuario = nombre) Then
                MessageBox.Show("El nombre de usuario ya existe.")
                Return
            End If

            Dim nuevo As New Usuarios()
            nuevo.NombreUsuario = nombre
            nuevo.Clave = txtClave.Text.Trim() ' Idealmente usar Hash aquí
            nuevo.Rol = cmbRol.Text

            db.Usuarios.Add(nuevo)
            db.SaveChanges()

            MessageBox.Show("Usuario creado correctamente.")
            CargarUsuarios()
            Limpiar()
        End Using
    End Sub

    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        If dgvUsuarios.SelectedRows.Count = 0 Then Return

        Dim idUser As Integer = Convert.ToInt32(dgvUsuarios.SelectedRows(0).Cells("ID").Value)

        ' 1. Evitar auto-eliminación
        If idUser = Sesion.UsuarioActual.Id Then
            MessageBox.Show("No puedes eliminar tu propio usuario mientras estás conectado.", "Seguridad", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' 2. Confirmación
        If MessageBox.Show("¿Está seguro de eliminar este usuario? Se borrará también su historial de accesos.", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
            Try
                Using db As New PoloNuevoEntities()
                    Dim u = db.Usuarios.Find(idUser)
                    If u IsNot Nothing Then
                        ' --- PASO CRÍTICO: LIMPIEZA DE DEPENDENCIAS ---
                        ' Borramos los logs de "EventosSistema" asociados a este usuario
                        ' Si no hacemos esto, SQL Server bloqueará la eliminación por la FK_Eventos_Usuarios
                        Dim eventos = db.EventosSistema.Where(Function(ev) ev.UsuarioId = idUser).ToList()
                        If eventos.Count > 0 Then
                            db.EventosSistema.RemoveRange(eventos)
                        End If

                        ' AHORA sí podemos borrar al usuario
                        db.Usuarios.Remove(u)
                        db.SaveChanges()

                        MessageBox.Show("Usuario eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        CargarUsuarios()
                    End If
                End Using
            Catch ex As Exception
                ' Si ocurriera otro error (ej: si el usuario creó documentos y guardaste su ID en otra tabla nueva)
                MessageBox.Show("No se pudo eliminar el usuario." & vbCrLf & "Error técnico: " & ex.Message, "Error de Base de Datos", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub Limpiar()
        txtNombre.Text = ""
        txtClave.Text = ""
    End Sub
End Class