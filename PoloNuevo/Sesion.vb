Public Class Sesion
    ' Propiedad estática para acceder desde cualquier formulario
    Public Shared Property UsuarioActual As UserData

    ' Estructura ligera para no cargar el objeto de EF pesado
    Public Class UserData
        Public Property Id As Integer
        Public Property Nombre As String
        Public Property Rol As String

        Public ReadOnly Property EsAdmin As Boolean
            Get
                Return Rol = "ADMINISTRADOR"
            End Get
        End Property
    End Class

    ' Método para iniciar sesión
    Public Shared Sub Iniciar(id As Integer, nombre As String, rol As String)
        UsuarioActual = New UserData With {
            .Id = id, .Nombre = nombre, .Rol = rol
        }
    End Sub

    ' Método para cerrar sesión
    Public Shared Sub Cerrar()
        UsuarioActual = Nothing
    End Sub
End Class