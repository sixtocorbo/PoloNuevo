Public Class frmNuevoIngreso

    Private _idDocumentoEditar As Integer = 0
    Private Const PLACEHOLDER As String = "Escriba para buscar..."

    Private Class ItemRecluso
        Public Property Id As Integer
        Public Property Texto As String
    End Class

    Private _listaCompleta As List(Of ItemRecluso)

    Public Sub New()
        InitializeComponent()
        _idDocumentoEditar = 0
    End Sub

    Public Sub New(idDocumento As Integer)
        InitializeComponent()
        _idDocumentoEditar = idDocumento
    End Sub

    Private Sub frmNuevoIngreso_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CargarTiposDocumento() ' <--- NUEVO: Cargar los tipos (Oficio, Nota, etc)
        CargarListaMaestra()

        If _idDocumentoEditar > 0 Then
            ConfigurarModoEdicion()
        End If
    End Sub

    ' --- MÉTODO NUEVO PARA CARGAR EL COMBO DE TIPOS ---
    Private Sub CargarTiposDocumento()
        Using db As New PoloNuevoEntities()
            ' Cargamos todos los tipos ordenados alfabéticamente
            Dim tipos = db.TiposDocumento.OrderBy(Function(t) t.Nombre).ToList()

            cmbTipo.DataSource = tipos
            cmbTipo.DisplayMember = "Nombre"
            cmbTipo.ValueMember = "Id"

            ' Intentamos seleccionar "OFICIO" por defecto si existe, sino el primero
            Dim oficio = tipos.FirstOrDefault(Function(t) t.Nombre.Contains("OFICIO"))
            If oficio IsNot Nothing Then
                cmbTipo.SelectedValue = oficio.Id
            End If
        End Using
    End Sub

    Private Sub CargarListaMaestra()
        Using db As New PoloNuevoEntities()
            _listaCompleta = db.Reclusos _
                               .Select(Function(r) New ItemRecluso With {
                                   .Id = r.Id,
                                   .Texto = r.Nombre & " (" & r.Cedula & ")"
                               }) _
                               .OrderBy(Function(x) x.Texto) _
                               .ToList()
        End Using
        cmbReclusos.DataSource = Nothing
    End Sub

    ' --- LÓGICA DE BÚSQUEDA ---
    Private Sub txtBuscarRecluso_TextChanged(sender As Object, e As EventArgs) Handles txtBuscarRecluso.TextChanged
        If txtBuscarRecluso.Text = PLACEHOLDER OrElse txtBuscarRecluso.ForeColor = Color.Gray Then Return
        FiltrarReclusos(txtBuscarRecluso.Text)
    End Sub

    Private Sub FiltrarReclusos(texto As String)
        If _listaCompleta Is Nothing Then Return

        Dim filtro = texto.Trim().ToLower()
        If String.IsNullOrEmpty(filtro) Then
            cmbReclusos.DataSource = Nothing
            Return
        End If

        Dim palabras = filtro.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)
        Dim listaFiltrada = _listaCompleta.Where(Function(item)
                                                     Dim itemTexto = item.Texto.ToLower()
                                                     Return palabras.All(Function(p) itemTexto.Contains(p))
                                                 End Function) _
                                          .Take(30) _
                                          .ToList()

        cmbReclusos.DataSource = listaFiltrada
        cmbReclusos.DisplayMember = "Texto"
        cmbReclusos.ValueMember = "Id"

        If listaFiltrada.Count > 0 Then
            cmbReclusos.DroppedDown = True
        End If
    End Sub

    Private Sub txtBuscarRecluso_GotFocus(sender As Object, e As EventArgs) Handles txtBuscarRecluso.GotFocus
        If txtBuscarRecluso.Text = PLACEHOLDER Then
            txtBuscarRecluso.Text = ""
            txtBuscarRecluso.ForeColor = Color.Black
        End If
    End Sub

    Private Sub txtBuscarRecluso_LostFocus(sender As Object, e As EventArgs) Handles txtBuscarRecluso.LostFocus
        If String.IsNullOrWhiteSpace(txtBuscarRecluso.Text) Then
            txtBuscarRecluso.Text = PLACEHOLDER
            txtBuscarRecluso.ForeColor = Color.Gray
        End If
    End Sub

    ' Teclado en buscador
    Private Sub txtBuscarRecluso_KeyDown(sender As Object, e As KeyEventArgs) Handles txtBuscarRecluso.KeyDown
        If cmbReclusos.Items.Count = 0 Then Return
        If e.KeyCode = Keys.Down Then
            e.Handled = True
            If cmbReclusos.SelectedIndex < cmbReclusos.Items.Count - 1 Then cmbReclusos.SelectedIndex += 1
        ElseIf e.KeyCode = Keys.Up Then
            e.Handled = True
            If cmbReclusos.SelectedIndex > 0 Then cmbReclusos.SelectedIndex -= 1
        ElseIf e.KeyCode = Keys.Enter Then
            e.Handled = True
            e.SuppressKeyPress = True
            cmbReclusos.DroppedDown = False
            btnGuardar.Focus()
        End If
    End Sub

    ' --- MODO EDICIÓN ---
    Private Sub ConfigurarModoEdicion()
        Me.Text = "Editar Ingreso / Detalle"
        btnGuardar.Text = "GUARDAR CAMBIOS"

        Using db As New PoloNuevoEntities()
            Dim doc = db.Documentos.Find(_idDocumentoEditar)
            If doc IsNot Nothing Then
                dtpFecha.Value = doc.FechaCarga
                txtReferencia.Text = doc.ReferenciaExterna
                txtAsunto.Text = doc.Descripcion

                ' Seleccionamos el TIPO correcto en el combo
                cmbTipo.SelectedValue = doc.TipoDocumentoId

                If doc.ReclusoId.HasValue Then
                    chkVincular.Checked = True
                    ' Cargar el combo con ese solo ítem para que se vea
                    Dim item = _listaCompleta.FirstOrDefault(Function(x) x.Id = doc.ReclusoId)
                    If item IsNot Nothing Then
                        cmbReclusos.DataSource = New List(Of ItemRecluso) From {item}
                        cmbReclusos.DisplayMember = "Texto"
                        cmbReclusos.ValueMember = "Id"
                        cmbReclusos.SelectedValue = doc.ReclusoId

                        txtBuscarRecluso.Text = item.Texto
                        txtBuscarRecluso.ForeColor = Color.Black
                    End If
                End If

                Dim primerMov = db.MovimientosDocumentos _
                                  .Where(Function(m) m.DocumentoId = _idDocumentoEditar) _
                                  .OrderBy(Function(m) m.FechaMovimiento) _
                                  .FirstOrDefault()
                If primerMov IsNot Nothing Then txtOrigen.Text = primerMov.Origen
            End If
        End Using
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If String.IsNullOrWhiteSpace(txtOrigen.Text) Then
            MessageBox.Show("Falta el Origen.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        If String.IsNullOrWhiteSpace(txtAsunto.Text) Then
            MessageBox.Show("Falta el Asunto.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Using db As New PoloNuevoEntities()
                If _idDocumentoEditar = 0 Then
                    ' --- NUEVO ---
                    Dim doc As New Documentos()
                    LlenarObjeto(doc) ' Ahora pasamos el objeto sin la DB porque el ID viene del combo

                    doc.FechaCarga = dtpFecha.Value
                    doc.NombreArchivo = "Reg_" & DateTime.Now.Ticks
                    doc.Contenido = New Byte() {0}
                    doc.Extension = ".phy"

                    db.Documentos.Add(doc)
                    db.SaveChanges()

                    Dim mov As New MovimientosDocumentos()
                    mov.DocumentoId = doc.Id
                    mov.FechaMovimiento = dtpFecha.Value
                    mov.Origen = txtOrigen.Text.Trim()
                    mov.EsSalida = False
                    db.MovimientosDocumentos.Add(mov)
                    db.SaveChanges()

                    MessageBox.Show("Ingreso creado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    ' --- EDITAR ---
                    Dim doc = db.Documentos.Find(_idDocumentoEditar)
                    If doc IsNot Nothing Then
                        LlenarObjeto(doc) ' Actualizamos tipo, asunto, ref

                        Dim primerMov = db.MovimientosDocumentos _
                                          .Where(Function(m) m.DocumentoId = _idDocumentoEditar) _
                                          .OrderBy(Function(m) m.FechaMovimiento) _
                                          .FirstOrDefault()
                        If primerMov IsNot Nothing Then
                            primerMov.Origen = txtOrigen.Text.Trim()
                            primerMov.FechaMovimiento = dtpFecha.Value
                        End If
                        db.SaveChanges()
                        MessageBox.Show("Actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End If

                Me.DialogResult = DialogResult.OK
                Me.Close()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub

    ' Método auxiliar para llenar los datos
    Private Sub LlenarObjeto(doc As Documentos)
        doc.Descripcion = txtAsunto.Text.Trim()
        doc.ReferenciaExterna = txtReferencia.Text.Trim()

        ' AQUÍ LA CORRECCIÓN: Tomamos el ID del combo
        If cmbTipo.SelectedValue IsNot Nothing Then
            doc.TipoDocumentoId = Convert.ToInt32(cmbTipo.SelectedValue)
        Else
            ' Si por alguna razón falla, valor por defecto 1 (seguridad)
            doc.TipoDocumentoId = 1
        End If

        If chkVincular.Checked AndAlso cmbReclusos.SelectedValue IsNot Nothing Then
            doc.ReclusoId = Convert.ToInt32(cmbReclusos.SelectedValue)
        Else
            doc.ReclusoId = Nothing
        End If
    End Sub

    Private Sub chkVincular_CheckedChanged(sender As Object, e As EventArgs) Handles chkVincular.CheckedChanged
        Dim activo = chkVincular.Checked
        txtBuscarRecluso.Enabled = activo
        cmbReclusos.Enabled = activo
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Me.Close()
    End Sub

End Class