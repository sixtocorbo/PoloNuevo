Imports System.Data.Entity

Public Class frmLogin

    Private Sub btnIngresar_Click(sender As Object, e As EventArgs) Handles btnIngresar.Click
        Dim u = txtUsuario.Text.Trim()
        Dim p = txtClave.Text.Trim()

        If String.IsNullOrEmpty(u) Or String.IsNullOrEmpty(p) Then
            MessageBox.Show("Ingrese usuario y clave.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Using db As New PoloNuevoEntities()
                ' Buscamos al usuario (Sensible a mayúsculas/minúsculas según configuración de SQL)
                Dim usuario = db.Usuarios.FirstOrDefault(Function(x) x.NombreUsuario = u And x.Clave = p)

                If usuario IsNot Nothing Then
                    ' 1. Guardamos los datos en la Sesión Global
                    Sesion.Iniciar(usuario.Id, usuario.NombreUsuario, usuario.Rol)

                    ' 2. Registramos el evento en auditoría (Opcional pero recomendado)
                    Dim log As New EventosSistema()
                    log.Fecha = DateTime.Now
                    log.Descripcion = "Inicio de Sesión Exitoso"
                    log.UsuarioId = usuario.Id
                    db.EventosSistema.Add(log)
                    db.SaveChanges()

                    ' 3. Abrimos el sistema real
                    Dim menu As New frmMenuPrincipal()
                    menu.Show()
                    Me.Hide() ' Ocultamos el login
                Else
                    MessageBox.Show("Usuario o clave incorrectos.", "Error de Acceso", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("Error de conexión: " & ex.Message)
        End Try
    End Sub

    Private Sub btnSalir_Click(sender As Object, e As EventArgs) Handles btnSalir.Click
        Application.Exit()
    End Sub

    ' Para dar Enter y entrar
    Private Sub txtClave_KeyDown(sender As Object, e As KeyEventArgs) Handles txtClave.KeyDown
        If e.KeyCode = Keys.Enter Then btnIngresar.PerformClick()
    End Sub
End Class