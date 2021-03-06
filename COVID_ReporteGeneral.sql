USE [NS_COVID_PRU]
GO
/****** Object:  StoredProcedure [dbo].[COVID_ReporteGeneral]    Script Date: 10/05/2021 10:55:06 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- ALTER date: <ALTER Date,,>
-- Description:	<Description,,>
--exec COVID_ReporteGeneral @FechaInicial=N'2020-05-02',@FechaFinal=N'2020-05-02', @Sede='0'
-- =============================================
ALTER PROCEDURE [dbo].[COVID_ReporteGeneral]
	@TextoFiltro as nvarchar(100)='',
	@FechaInicial as date,
	@FechaFinal as date,
	@Sede varchar(max)
AS
BEGIN
	
	SET NOCOUNT ON;

	IF @Sede = '0'
		Select  NumeroDocumento, Apellidos + ' ' + Nombres Nombre, 
	        ij.FechaHoraRegistro InicioJornanda, fj.FechaHoraRegistro FinJornada,
	        ij.Id IdRespuestaInicio, fj.Id IdRespuestaFin,
	        ij.GeneroAlerta GeneroAlertaInicio, fj.GeneroAlerta GeneroAlertaFin,
	        tempi.ValorRespuesta TemperaturaInicio, tempf.ValorRespuesta TemperaturaFin
			from    COVID_Personas P
	        Left Join COVID_Respuestas ij on ij.IdPersona=p.Id and ij.Tipo='I' and ij.Dia between @FechaInicial and @FechaFinal
	        Left Join COVID_Respuestas fj on fj.IdPersona=p.Id and fj.Tipo='F' and fj.Dia between @FechaInicial and @FechaFinal
	        Left Join COVID_DetalleRespuestas tempi on tempi.IdRespuesta=ij.Id and tempi.IdPregunta=(Select Id from COVID_Preguntas where Descripcion='Temperatura')
	        Left Join COVID_DetalleRespuestas tempf on tempf.IdRespuesta=fj.Id and tempf.IdPregunta=(Select Id from COVID_Preguntas where Descripcion='Temperatura')
	        Left Join COVID_DetalleRespuestas telet on telet.IdRespuesta=ij.Id and telet.IdPregunta=(Select Id from COVID_Preguntas where Descripcion='Casa')
			Where   (Apellidos + ' ' + Nombres like '%' + upper(@TextoFiltro) + '%' 
	            or NumeroDocumento like '%' + @TextoFiltro + '%') 
				and ij.FechaHoraRegistro is not null
				Order By ij.dia, Apellidos, Nombres
	ELSE
		Select  NumeroDocumento, Apellidos + ' ' + Nombres Nombre, 
	        ij.FechaHoraRegistro InicioJornanda, fj.FechaHoraRegistro FinJornada,
	        ij.Id IdRespuestaInicio, fj.Id IdRespuestaFin,
	        ij.GeneroAlerta GeneroAlertaInicio, fj.GeneroAlerta GeneroAlertaFin,
	        tempi.ValorRespuesta TemperaturaInicio, tempf.ValorRespuesta TemperaturaFin
			from    COVID_Personas P
	        Left Join COVID_Respuestas ij on ij.IdPersona=p.Id and ij.Tipo='I' and ij.Dia between @FechaInicial and @FechaFinal
	        Left Join COVID_Respuestas fj on fj.IdPersona=p.Id and fj.Tipo='F' and fj.Dia between @FechaInicial and @FechaFinal
	        Left Join COVID_DetalleRespuestas tempi on tempi.IdRespuesta=ij.Id and tempi.IdPregunta=(Select Id from COVID_Preguntas where Descripcion='Temperatura')
	        Left Join COVID_DetalleRespuestas tempf on tempf.IdRespuesta=fj.Id and tempf.IdPregunta=(Select Id from COVID_Preguntas where Descripcion='Temperatura')
	        Left Join COVID_DetalleRespuestas telet on telet.IdRespuesta=ij.Id and telet.IdPregunta=(Select Id from COVID_Preguntas where Descripcion='Casa')
			Left Join COVID_DetalleRespuestas telets on telets.IdRespuesta=ij.Id and telets.IdPregunta=(Select Id from COVID_Preguntas where Descripcion='Sede')
			Where   (Apellidos + ' ' + Nombres like '%' + upper(@TextoFiltro) + '%' 
	            or NumeroDocumento like '%' + @TextoFiltro + '%') 
				and telets.ValorRespuesta = @Sede 
				and ij.FechaHoraRegistro is not null
				Order By ij.dia, Apellidos, Nombres
    
END