Imports System.Data.Entity

Public Class frmHistorial

    Private _idDocInicial As Integer

    ' Constructor que recibe el ID y la Referencia para configurar la ventana
    Public Sub New(idDoc As Integer, referencia As String)
        InitializeComponent()
        _idDocInicial = idDoc
        Me.Text = "Historia Clínica - " & referencia
    End Sub

    Private Sub frmHistorial_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CargarDatos()
    End Sub

    Private Sub CargarDatos()
        Try
            dgvHistorial.Rows.Clear()

            Using db As New PoloNuevoEntities()
                ' -----------------------------------------------------------
                ' 1. BÚSQUEDA DE LA FAMILIA (RECURSIVA)
                ' -----------------------------------------------------------
                Dim familiaIds As New List(Of Integer) From {_idDocInicial}
                Dim nuevosEncontrados As Boolean = True
                Dim iteraciones As Integer = 0

                While nuevosEncontrados AndAlso iteraciones < 50
                    nuevosEncontrados = False
                    iteraciones += 1
                    Dim listaActual = familiaIds.ToList()
                    Dim vinculos = db.DocumentoVinculos _
                                     .Where(Function(v) listaActual.Contains(v.IdDocumentoPadre) Or listaActual.Contains(v.IdDocumentoHijo)) _
                                     .ToList()

                    For Each v In vinculos
                        If Not familiaIds.Contains(v.IdDocumentoPadre) Then
                            familiaIds.Add(v.IdDocumentoPadre)
                            nuevosEncontrados = True
                        End If
                        If Not familiaIds.Contains(v.IdDocumentoHijo) Then
                            familiaIds.Add(v.IdDocumentoHijo)
                            nuevosEncontrados = True
                        End If
                    Next
                End While

                ' -----------------------------------------------------------
                ' 2. OBTENER MOVIMIENTOS
                ' -----------------------------------------------------------
                Dim movimientos = db.MovimientosDocumentos _
                                    .Include("Documentos").Include("Documentos.TiposDocumento") _
                                    .Where(Function(m) familiaIds.Contains(m.DocumentoId)) _
                                    .OrderBy(Function(m) m.FechaMovimiento) _
                                    .ToList()

                If movimientos.Count = 0 Then
                    MessageBox.Show("No se encontraron movimientos para este expediente.", "Vacío", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return
                End If

                ' -----------------------------------------------------------
                ' 3. LLENADO MANUAL DE LA GRILLA (ESTO EVITA ERRORES)
                ' -----------------------------------------------------------
                For Each m In movimientos
                    ' a. Lógica visual de la acción
                    Dim accionHumana As String = "Trámite"
                    Dim obs As String = If(m.Observaciones, "").ToUpper()

                    If m.Origen = "SISTEMA" Then
                        accionHumana = "✨ GENERACIÓN INICIAL"
                    ElseIf obs.Contains("VINCULADO") Or obs.Contains("ADJUNTO") Then
                        accionHumana = "🔗 VINCULACIÓN"
                    ElseIf m.EsSalida Then
                        accionHumana = "📤 PASE / ENVÍO"
                    Else
                        accionHumana = "📥 RECEPCIÓN"
                    End If

                    ' b. Construcción segura del nombre del documento
                    Dim docRef As String = "Desconocido"
                    If m.Documentos IsNot Nothing Then
                        Dim tipo As String = If(m.Documentos.TiposDocumento IsNot Nothing, m.Documentos.TiposDocumento.Nombre, "DOC")
                        Dim ref As String = If(m.Documentos.ReferenciaExterna, "S/N")
                        docRef = tipo & " " & ref
                    End If

                    ' c. Marcar si es el documento actual
                    If m.DocumentoId = _idDocInicial Then
                        docRef = "➤ " & docRef
                    End If

                    ' d. Agregar Fila Directamente (Sin DataSource)
                    ' Orden: Fecha, Documento, Acción, Origen, Destino, Nota
                    dgvHistorial.Rows.Add(
                        m.FechaMovimiento.ToString("dd/MM/yyyy HH:mm"),
                        docRef,
                        accionHumana,
                        If(m.Origen, ""),
                        If(m.Destino, ""),
                        obs
                    )
                Next

                ' Colorear filas especiales (opcional)
                For Each row As DataGridViewRow In dgvHistorial.Rows
                    Dim accion As String = row.Cells("colAccion").Value.ToString()
                    If accion.Contains("VINCULACIÓN") Then
                        row.DefaultCellStyle.ForeColor = Color.Blue
                    ElseIf accion.Contains("GENERACIÓN") Then
                        row.DefaultCellStyle.ForeColor = Color.Green
                    End If
                    ' Negrita para el documento seleccionado
                    If row.Cells("colDocumento").Value.ToString().StartsWith("➤") Then
                        row.DefaultCellStyle.Font = New Font(dgvHistorial.Font, FontStyle.Bold)
                    End If
                Next

                dgvHistorial.ClearSelection()

            End Using
        Catch ex As Exception
            MessageBox.Show("Error al cargar historial: " & ex.Message)
        End Try
    End Sub

End Class