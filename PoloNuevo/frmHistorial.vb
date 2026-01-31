Imports System.Data.Entity
Imports System.Linq
Imports System.Drawing

Public Class frmHistorial

    Private _idDocSeleccionado As Integer

    ' Constructor: Recibe el ID del documento para rastrear a toda la familia
    Public Sub New(idDoc As Integer, referencia As String)
        InitializeComponent()
        _idDocSeleccionado = idDoc
        Me.Text = "Historia Clínica Unificada - Exp. " & referencia

        ' Ajustes de ventana
        Me.StartPosition = FormStartPosition.CenterParent
        Me.Size = New Size(950, 550)
    End Sub

    Private Sub frmHistorial_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ConfigurarGrilla()
        CargarHistoriaUnificada()
    End Sub

    Private Sub ConfigurarGrilla()
        dgvHistorial.Rows.Clear()
        dgvHistorial.Columns.Clear()

        ' Definición de columnas para la narrativa del expediente
        dgvHistorial.Columns.Add("colFecha", "Fecha")
        dgvHistorial.Columns.Add("colDocumento", "Documento Protagonista")
        dgvHistorial.Columns.Add("colAccion", "Acción / Evento")
        dgvHistorial.Columns.Add("colOrigen", "Origen")
        dgvHistorial.Columns.Add("colDestino", "Destino / Ubicación")

        ' Configuración visual de anchos
        dgvHistorial.Columns("colFecha").Width = 115
        dgvHistorial.Columns("colDocumento").Width = 260
        dgvHistorial.Columns("colAccion").Width = 180
        dgvHistorial.Columns("colOrigen").Width = 180
        dgvHistorial.Columns("colDestino").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill

        ' Estética de la tabla
        dgvHistorial.RowHeadersVisible = False
        dgvHistorial.AllowUserToAddRows = False
        dgvHistorial.ReadOnly = True
        dgvHistorial.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvHistorial.GridColor = Color.WhiteSmoke
        dgvHistorial.BackgroundColor = Color.White

        ' Estilo moderno para la cabecera
        dgvHistorial.EnableHeadersVisualStyles = False
        dgvHistorial.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue
        dgvHistorial.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dgvHistorial.ColumnHeadersDefaultCellStyle.Font = New Font(dgvHistorial.Font, FontStyle.Bold)
    End Sub

    Private Sub CargarHistoriaUnificada()
        Try
            Using db As New PoloNuevoEntities()

                ' -----------------------------------------------------------
                ' 1. BUSCAR LA RAÍZ (SUBIR HASTA EL PADRE SUPREMO)
                ' -----------------------------------------------------------
                Dim idRaiz As Integer = _idDocSeleccionado
                Dim buscandoRaiz As Boolean = True
                Dim seguridad As Integer = 0

                While buscandoRaiz AndAlso seguridad < 50
                    seguridad += 1
                    Dim idActual = idRaiz
                    Dim vinculo = db.DocumentoVinculos.FirstOrDefault(Function(v) v.IdDocumentoHijo = idActual)

                    If vinculo IsNot Nothing Then
                        idRaiz = vinculo.IdDocumentoPadre
                    Else
                        buscandoRaiz = False
                    End If
                End While

                ' -----------------------------------------------------------
                ' 2. BUSCAR TODA LA DESCENDENCIA (BAJAR POR TODAS LAS RAMAS)
                ' -----------------------------------------------------------
                Dim familiaIds As New List(Of Integer)
                familiaIds.Add(idRaiz)
                ObtenerDescendenciaRecursiva(db, idRaiz, familiaIds)

                ' -----------------------------------------------------------
                ' 3. OBTENER MOVIMIENTOS UNIFICADOS (ORDENADOS POR FECHA E ID)
                ' -----------------------------------------------------------
                Dim rawMovs = db.MovimientosDocumentos _
                                .Include("Documentos").Include("Documentos.TiposDocumento") _
                                .Where(Function(m) familiaIds.Contains(m.DocumentoId)) _
                                .OrderBy(Function(m) m.FechaMovimiento) _
                                .ThenBy(Function(m) m.Id) _
                                .ToList()

                ' -----------------------------------------------------------
                ' 4. FILTRADO Y TRADUCCIÓN VISUAL (SIN ALTERAR LA BD)
                ' -----------------------------------------------------------
                For Each m In rawMovs
                    Dim mostrar As Boolean = True
                    Dim obs As String = If(m.Observaciones, "").ToUpper()
                    Dim accionVisual As String = ""

                    ' REGLA DE FILTRADO: Ocultamos movimientos de "arrastre" técnico
                    ' No queremos ver líneas idénticas para cada hijo si el padre ya se movió.
                    If obs.Contains("PASE ADJUNTO") Or obs.Contains("PASE ADJUNTOS") Or obs.Contains("ARRASTRE DE EXPEDIENTE") Then
                        mostrar = False
                    End If

                    ' EXCEPCIÓN: Si es el PADRE SUPREMO el que se mueve, lo mostramos SIEMPRE
                    If m.DocumentoId = idRaiz Then mostrar = True

                    ' EXCEPCIÓN: Si es un NACIMIENTO o VINCULACIÓN manual, lo mostramos SIEMPRE
                    If m.Origen = "SISTEMA" Or obs.Contains("GENERACIÓN") Or obs.Contains("VINCULADO") Or obs.Contains("VINCULACIÓN") Then
                        mostrar = True
                    End If

                    ' --- SI EL MOVIMIENTO PASA EL FILTRO, SE DIBUJA ---
                    If mostrar Then

                        Dim obsMostrar As String = If(String.IsNullOrWhiteSpace(m.Observaciones), "S/D", m.Observaciones)

                        ' 1. Definir la Acción (más descriptiva, similar a la trazabilidad)
                        If m.Origen = "SISTEMA" Or obs.Contains("GENERACIÓN") Then
                            accionVisual = "✨ GESTIÓN / CREACIÓN"

                        ElseIf obs.Contains("RETORNO AUTOMÁTICO") Then
                            ' Más intuitivo: se muestra como retorno/ingreso
                            accionVisual = "📥 RETORNO / REINGRESO (" & obsMostrar & ")"

                        ElseIf obs.Contains("VINCULACIÓN") Or obs.Contains("VINCULADO") Then
                            accionVisual = "🔗 SE VINCULÓ"

                        ElseIf m.EsSalida Then
                            accionVisual = "📤 SALIDA (" & obsMostrar & ")"
                        Else
                            accionVisual = "📥 ENTRADA (" & obsMostrar & ")"
                        End If

                        ' 2. Identificar el Documento
                        Dim nombreDoc As String = m.Documentos.TiposDocumento.Nombre & " " & m.Documentos.ReferenciaExterna

                        ' 3. Aplicar Sangría Jerárquica
                        If m.DocumentoId = idRaiz Then
                            nombreDoc = "📂 " & nombreDoc & " (Principal)"
                        Else
                            nombreDoc = "   ↳ " & nombreDoc
                        End If

                        Dim origenMostrar As String = If(String.IsNullOrWhiteSpace(m.Origen), "—", m.Origen)
                        Dim destinoMostrar As String = If(String.IsNullOrWhiteSpace(m.Destino), "—", m.Destino)

                        dgvHistorial.Rows.Add(
                            m.FechaMovimiento.ToString("dd/MM/yyyy HH:mm"),
                            nombreDoc,
                            accionVisual,
                            origenMostrar,
                            destinoMostrar
                        )
                    End If
                Next

                AplicarColores()

            End Using
        Catch ex As Exception
            MessageBox.Show("Error al cargar la historia unificada: " & ex.Message)
        End Try
    End Sub

    ' Función auxiliar para recorrer el árbol genealógico completo
    Private Sub ObtenerDescendenciaRecursiva(db As PoloNuevoEntities, idPadre As Integer, ByRef listaIds As List(Of Integer))
        Dim hijos = db.DocumentoVinculos.Where(Function(v) v.IdDocumentoPadre = idPadre).Select(Function(v) v.IdDocumentoHijo).ToList()
        For Each idHijo In hijos
            If Not listaIds.Contains(idHijo) Then
                listaIds.Add(idHijo)
                ObtenerDescendenciaRecursiva(db, idHijo, listaIds)
            End If
        Next
    End Sub

    Private Sub AplicarColores()
        For Each row As DataGridViewRow In dgvHistorial.Rows
            Dim doc As String = row.Cells("colDocumento").Value.ToString()
            Dim accion As String = row.Cells("colAccion").Value.ToString()

            ' Negrita y fondo suave para el documento Principal (Carpeta)
            If doc.Contains("📂") Then
                row.DefaultCellStyle.Font = New Font(dgvHistorial.Font, FontStyle.Bold)
                row.DefaultCellStyle.BackColor = Color.AliceBlue
            End If

            ' Colores por tipo de evento
            If accion.Contains("CREACIÓN") Then
                row.DefaultCellStyle.ForeColor = Color.ForestGreen
            ElseIf accion.Contains("VINCULACIÓN") Then
                row.DefaultCellStyle.ForeColor = Color.Purple
            ElseIf accion.Contains("RETORNO") Then
                row.DefaultCellStyle.ForeColor = Color.DarkSlateBlue
            ElseIf accion.Contains("SALIDA") Then
                row.DefaultCellStyle.ForeColor = Color.DarkBlue
            End If
        Next
        dgvHistorial.ClearSelection()
    End Sub

End Class
