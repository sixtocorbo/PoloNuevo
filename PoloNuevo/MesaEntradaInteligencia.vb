Imports System.Collections.Generic
Imports System.Linq

Public Module MesaEntradaInteligencia

    Public Function ObtenerUltimosMovimientos(documentos As IEnumerable(Of Documentos)) As Dictionary(Of Integer, MovimientosDocumentos)
        Return documentos.ToDictionary(Function(d) d.Id,
                                       Function(d)
                                           Return d.MovimientosDocumentos _
                                               .OrderByDescending(Function(m) m.FechaMovimiento) _
                                               .ThenByDescending(Function(m) m.Id) _
                                               .FirstOrDefault()
                                       End Function)
    End Function

    Public Function ObtenerPadreSupremoPorDoc(documentos As IEnumerable(Of Documentos),
                                              vinculos As IEnumerable(Of DocumentoVinculos)) As Dictionary(Of Integer, Integer)
        Dim padresPorHijo As Dictionary(Of Integer, Integer) = vinculos _
            .GroupBy(Function(v) v.IdDocumentoHijo) _
            .ToDictionary(Function(g) g.Key, Function(g) g.First().IdDocumentoPadre)

        Dim padreSupremoPorDoc As New Dictionary(Of Integer, Integer)
        For Each doc In documentos
            Dim idRastro As Integer = doc.Id
            Dim iteraciones As Integer = 0
            While padresPorHijo.ContainsKey(idRastro) AndAlso iteraciones < 50
                iteraciones += 1
                idRastro = padresPorHijo(idRastro)
            End While
            padreSupremoPorDoc(doc.Id) = idRastro
        Next

        Return padreSupremoPorDoc
    End Function

    Public Function ObtenerPendientesPorPadre(documentos As IEnumerable(Of Documentos),
                                              ultimosMovimientos As Dictionary(Of Integer, MovimientosDocumentos),
                                              padreSupremoPorDoc As Dictionary(Of Integer, Integer)) As Dictionary(Of Integer, Integer)
        Dim pendientePorPadre As New Dictionary(Of Integer, Integer)

        For Each doc In documentos
            Dim ultimoMov As MovimientosDocumentos = Nothing
            ultimosMovimientos.TryGetValue(doc.Id, ultimoMov)

            Dim destino As String = If(ultimoMov IsNot Nothing, If(ultimoMov.Destino, ""), "MESA DE ENTRADA")
            Dim enMesa As Boolean = destino.Trim().ToUpper() = "MESA DE ENTRADA"

            If enMesa Then
                Dim idPadre = padreSupremoPorDoc(doc.Id)
                If Not pendientePorPadre.ContainsKey(idPadre) Then
                    pendientePorPadre(idPadre) = doc.Id
                Else
                    Dim idActual = pendientePorPadre(idPadre)
                    Dim movActual As MovimientosDocumentos = Nothing
                    ultimosMovimientos.TryGetValue(idActual, movActual)

                    Dim fechaActual As DateTime = If(movActual IsNot Nothing, movActual.FechaMovimiento, DateTime.MinValue)
                    Dim fechaNuevo As DateTime = If(ultimoMov IsNot Nothing, ultimoMov.FechaMovimiento, DateTime.MinValue)
                    Dim idMovActual As Integer = If(movActual IsNot Nothing, movActual.Id, 0)
                    Dim idMovNuevo As Integer = If(ultimoMov IsNot Nothing, ultimoMov.Id, 0)

                    If fechaNuevo > fechaActual OrElse (fechaNuevo = fechaActual AndAlso idMovNuevo > idMovActual) Then
                        pendientePorPadre(idPadre) = doc.Id
                    End If
                End If
            End If
        Next

        Return pendientePorPadre
    End Function

End Module
