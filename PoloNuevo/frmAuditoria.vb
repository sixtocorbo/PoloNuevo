Public Class frmAuditoria

    Private Sub frmAuditoria_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CargarDocumentosBorrados()
    End Sub

    ' 1. Cargar la lista de ARRIBA (Documentos eliminados)
    Private Sub CargarDocumentosBorrados()
        Try
            Using db As New PoloNuevoEntities()
                Dim lista = db.AuditoriaDocumentos _
                              .OrderByDescending(Function(a) a.FechaEliminacion) _
                              .Select(Function(a) New With {
                                  .Id = a.Id,
                                  .Fecha_Eliminacion = a.FechaEliminacion,
                                  .Usuario = a.UsuarioResponsable,
                                  .Motivo = a.MotivoEliminacion,
                                  .Doc_Original = a.DocReferencia & " - " & a.DocAsunto
                              }) _
                              .ToList()

                dgvAuditoria.DataSource = lista
                If dgvAuditoria.Columns("Id") IsNot Nothing Then dgvAuditoria.Columns("Id").Visible = False

                ' Seleccionar primera fila si existe para mostrar detalles
                If lista.Count > 0 Then CargarDetalle(lista(0).Id)
            End Using
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub

    ' 2. Evento al hacer clic en una fila de arriba
    Private Sub dgvAuditoria_SelectionChanged(sender As Object, e As EventArgs) Handles dgvAuditoria.SelectionChanged
        If dgvAuditoria.SelectedRows.Count > 0 Then
            Dim idAuditoria As Integer = Convert.ToInt32(dgvAuditoria.SelectedRows(0).Cells("Id").Value)
            CargarDetalle(idAuditoria)
        End If
    End Sub

    ' 3. Cargar la lista de ABAJO (Movimientos recuperados de ese documento)
    Private Sub CargarDetalle(idAuditoria As Integer)
        Try
            Using db As New PoloNuevoEntities()
                Dim movimientos = db.AuditoriaMovimientos _
                                    .Where(Function(m) m.AuditoriaDocId = idAuditoria) _
                                    .OrderBy(Function(m) m.FechaMovimientoOriginal) _
                                    .Select(Function(m) New With {
                                        .Fecha_Orig = m.FechaMovimientoOriginal,
                                        .Tipo = m.TipoMovimiento,
                                        .Origen = m.Origen,
                                        .Destino = m.Destino,
                                        .Observaciones = m.Observaciones
                                    }) _
                                    .ToList()

                dgvDetalle.DataSource = movimientos
            End Using
        Catch ex As Exception
            ' Silencioso para no molestar al navegar rápido
        End Try
    End Sub
End Class