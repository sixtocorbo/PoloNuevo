Public Class frmNuevoPase

    Private _idDocumento As Integer
    Private _idMovimientoEditar As Integer = 0

    ' CONSTRUCTOR 1: NUEVO (Solo ID Documento)
    Public Sub New(idDocumento As Integer)
        InitializeComponent()
        _idDocumento = idDocumento
        _idMovimientoEditar = 0
    End Sub

    ' CONSTRUCTOR 2: EDITAR (ID Documento + ID Movimiento)
    Public Sub New(idDocumento As Integer, idMovimiento As Integer)
        InitializeComponent()
        _idDocumento = idDocumento
        _idMovimientoEditar = idMovimiento
    End Sub

    Private Sub frmNuevoPase_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If _idMovimientoEditar > 0 Then
            Me.Text = "Corregir Movimiento"
            btnGuardar.Text = "GUARDAR CAMBIOS"
            btnGuardar.BackColor = Color.SlateGray
            CargarDatosEdicion()
        End If
    End Sub

    Private Sub CargarDatosEdicion()
        Using db As New PoloNuevoEntities()
            Dim mov = db.MovimientosDocumentos.Find(_idMovimientoEditar)
            If mov IsNot Nothing Then
                txtDestino.Text = mov.Destino
                dtpFecha.Value = mov.FechaMovimiento
                chkEsSalida.Checked = mov.EsSalida
            End If
        End Using
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If String.IsNullOrWhiteSpace(txtDestino.Text) Then
            MessageBox.Show("Debe indicar el Destino.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Using db As New PoloNuevoEntities()
                If _idMovimientoEditar = 0 Then
                    ' --- NUEVO ---
                    Dim mov As New MovimientosDocumentos()
                    mov.DocumentoId = _idDocumento
                    mov.FechaMovimiento = dtpFecha.Value
                    mov.Destino = txtDestino.Text.Trim()
                    mov.Origen = "Mesa de Entrada / Administración" ' Origen por defecto
                    mov.EsSalida = chkEsSalida.Checked

                    db.MovimientosDocumentos.Add(mov)
                    db.SaveChanges()
                    MessageBox.Show("Pase registrado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    ' --- EDITAR ---
                    Dim mov = db.MovimientosDocumentos.Find(_idMovimientoEditar)
                    If mov IsNot Nothing Then
                        mov.Destino = txtDestino.Text.Trim()
                        mov.FechaMovimiento = dtpFecha.Value
                        mov.EsSalida = chkEsSalida.Checked

                        db.SaveChanges()
                        MessageBox.Show("Movimiento corregido.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
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