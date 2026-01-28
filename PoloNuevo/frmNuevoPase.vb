Public Class frmNuevoPase

    Private _idDocumento As Integer
    Private _idMovimientoEditar As Integer = 0

    ' =========================================================
    ' CONSTRUCTORES
    ' =========================================================

    ' Constructor 1: NUEVO MOVIMIENTO (Solo recibe ID Documento)
    Public Sub New(idDocumento As Integer)
        InitializeComponent()
        _idDocumento = idDocumento
        _idMovimientoEditar = 0
    End Sub

    ' Constructor 2: EDITAR MOVIMIENTO (Recibe ID Doc + ID Movimiento)
    Public Sub New(idDocumento As Integer, idMovimiento As Integer)
        InitializeComponent()
        _idDocumento = idDocumento
        _idMovimientoEditar = idMovimiento
    End Sub

    ' =========================================================
    ' CARGA INICIAL
    ' =========================================================
    Private Sub frmNuevoPase_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' 1. Cargar la lista inteligente de destinos
        CargarHistorialDestinos()

        ' 2. Configurar según si es Nuevo o Edición
        If _idMovimientoEditar > 0 Then
            Me.Text = "Corregir Movimiento"
            btnGuardar.Text = "GUARDAR CAMBIOS"
            btnGuardar.BackColor = Color.SlateGray
            CargarDatosEdicion()
        Else
            Me.Text = "Registrar Nuevo Pase / Salida"
            ' Si es nuevo, sugerimos la fecha actual
            dtpFecha.Value = DateTime.Now
            ' Por defecto asumimos que es Salida (para agilizar)
            chkEsSalida.Checked = True
        End If
    End Sub

    ' =========================================================
    ' MÉTODOS AUXILIARES
    ' =========================================================

    ' Busca todos los destinos únicos en la base para llenar el autocompletado
    Private Sub CargarHistorialDestinos()
        Try
            Using db As New PoloNuevoEntities()
                ' Buscamos destinos distintos que no estén vacíos
                Dim listaDestinos = db.MovimientosDocumentos _
                                      .Where(Function(m) m.Destino IsNot Nothing And m.Destino <> "") _
                                      .Select(Function(m) m.Destino) _
                                      .Distinct() _
                                      .OrderBy(Function(d) d) _
                                      .ToList()

                cmbDestino.DataSource = listaDestinos
                cmbDestino.SelectedIndex = -1 ' Arranca vacío para obligar a elegir o escribir
            End Using
        Catch ex As Exception
            ' Si falla (ej: base vacía), no hacemos nada, el combo queda vacío pero funcional
        End Try
    End Sub

    ' Carga los datos del movimiento que vamos a corregir
    Private Sub CargarDatosEdicion()
        Using db As New PoloNuevoEntities()
            Dim mov = db.MovimientosDocumentos.Find(_idMovimientoEditar)
            If mov IsNot Nothing Then
                ' Usamos .Text para asignar el valor, funcione o no el DataSource
                cmbDestino.Text = mov.Destino
                dtpFecha.Value = mov.FechaMovimiento
                chkEsSalida.Checked = mov.EsSalida
            End If
        End Using
    End Sub

    ' =========================================================
    ' BOTONES: GUARDAR Y CANCELAR
    ' =========================================================
    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        ' Validamos usando .Text (lo que escribió el usuario)
        If String.IsNullOrWhiteSpace(cmbDestino.Text) Then
            MessageBox.Show("Por favor, indique el Destino u Oficina.", "Falta Destino", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cmbDestino.Focus()
            Return
        End If

        Try
            Using db As New PoloNuevoEntities()
                ' Guardamos siempre en MAYÚSCULAS para mantener el orden en el futuro
                Dim destinoFinal As String = cmbDestino.Text.Trim().ToUpper()

                If _idMovimientoEditar = 0 Then
                    ' --- MODO NUEVO ---
                    Dim mov As New MovimientosDocumentos()
                    mov.DocumentoId = _idDocumento
                    mov.FechaMovimiento = dtpFecha.Value
                    mov.Destino = destinoFinal
                    mov.Origen = "MESA DE ENTRADA" ' Origen automático
                    mov.EsSalida = chkEsSalida.Checked

                    ' Opcional: Agregar detalle automático en observaciones si es salida
                    Dim doc = db.Documentos.Find(_idDocumento)
                    If doc IsNot Nothing Then
                        mov.Observaciones = "PASE DE: " & doc.TiposDocumento.Nombre & " " & doc.ReferenciaExterna
                    End If

                    db.MovimientosDocumentos.Add(mov)
                    db.SaveChanges()
                    MessageBox.Show("Pase registrado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    ' --- MODO EDICIÓN ---
                    Dim mov = db.MovimientosDocumentos.Find(_idMovimientoEditar)
                    If mov IsNot Nothing Then
                        mov.Destino = destinoFinal
                        mov.FechaMovimiento = dtpFecha.Value
                        mov.EsSalida = chkEsSalida.Checked

                        db.SaveChanges()
                        MessageBox.Show("Movimiento corregido exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End If

                ' Cerramos el formulario devolviendo OK para que la grilla principal se actualice
                Me.DialogResult = DialogResult.OK
                Me.Close()
            End Using
        Catch ex As Exception
            MessageBox.Show("Ocurrió un error al guardar: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Me.Close()
    End Sub

End Class