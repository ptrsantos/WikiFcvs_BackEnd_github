/****** Script do comando SelectTopNRows de SSMS  ******/
SELECT 
		t.[Id] as TemaId
		,et.Titulo as TituloTema
		,et.Id as EdicaoTemaId
		,pet.EditadoEm as TituloEditadoEm
		,pet.EditadoPorEmail as TituloEditadoPor
		,a.Id as ArtigoId
		,ea.Id as EdicaoArtigoId
		,ea.Titulo as ArtigoTitulo
		,ea.Conteudo as ArtigoConteudo
		,pea.EditadoEm as ArtigoEditadoEm
		,pea.EditadoPorEmail as ArtigoEditadoPor
  FROM [WikiFCVS].[dbo].[Temas] as t
  left join [WikiFCVS].[dbo].[Edicoes] as et
  on et.TemaId = t.Id
  left join [WikiFCVS].[dbo].[Protocolos] as pet
  on pet.Id = et.EdicaoEfetuadaId
  left join [WikiFCVS].[dbo].[Artigos] as a
  on a.TemaId = t.Id
  left join [WikiFCVS].[dbo].[Edicoes] as ea
  on ea.ArtigoId = a.Id
  left join [WikiFCVS].[dbo].[Protocolos] as pea
  on pea.Id = et.EdicaoEfetuadaId