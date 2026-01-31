Imports System.Data.Entity
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.Window

Public Class frmLogSistema

    Private Sub frmLogSistema_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CargarEventos()
    End Sub

    Private Sub CargarEventos()
        Try
            Using db As New PoloNuevoEntities()
                ' Traemos los últimos 200 eventos ordenados por fecha (lo más nuevo arriba)
                Dim lista = db.EventosSistema.Include("Usuarios") _
                              .OrderByDescending(Function(x) x.Fecha) _
                              .Take(200) _
                              .Select(Function(x) New With {
                                  .Fecha = x.Fecha,
                                  .Usuario = x.Usuarios.NombreUsuario, ' Gracias al Include podemos ver el nombre
                                  .Rol = x.Usuarios.Rol,
                                  .Evento = x.Descripcion
                              }) _
                              .ToList()

                dgvLog.DataSource = lista

                ' Ajustes visuales
                If dgvLog.Columns.Count > 0 Then
                    dgvLog.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
                    dgvLog.Columns("Fecha").Width = 120
                    dgvLog.Columns("Usuario").Width = 100
                    dgvLog.Columns("Rol").Width = 100
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("Error al cargar log: " & ex.Message)
        End Try
    End Sub

    Private Sub btnCerrar_Click(sender As Object, e As EventArgs) Handles btnCerrar.Click
        Me.Close()
    End Sub
End Class