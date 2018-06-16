--Домен пользователей
INSERT INTO [dbo].[UserDomain]
           ([UserDomainID]
           ,[EventDate]
           ,[RegistredEmail]
           ,[IsActive]
           ,[LegalName]
           ,[Trademark]
           ,[Address]
           ,[UserLogin]
           ,[PasswordHash])
     VALUES
(
	'B98E8EA3-BA51-49C6-916D-E1E978F5BEA3',
	'20150102',
	'alexsoff@yandex.ru',
	1,
	'ООО Тест',
	'Тест',
	'г. Город',
	'admin',
	'$2a$10$JgPV0GGu5/GBiJHER6OoceTFxI9KOiak2Syu4tAQAILdmG5OUaAiW'
)
--Номера для заказов

GO
INSERT INTO [dbo].[OrderCapacity]
           (
           [UserDomainID]
           ,[OrderNumber]
           )
     VALUES
           (
           'B98E8EA3-BA51-49C6-916D-E1E978F5BEA3',
           1
           )
GO


--Тип события
INSERT INTO [dbo].[TimelineKind]
           (
		   [TimelineKindID]
           ,[Title]           
		   )
SELECT
1,
'Назначен менеджер'
UNION ALL
SELECT
2,
'Назначен исполнитель'
UNION ALL
SELECT
3,
'Добавлена работа'
UNION ALL
SELECT
4,
'Добавлена Запчасть'
UNION ALL
SELECT
5,
'Добавлен комментарий'
UNION ALL
SELECT
7,
'Изменен статус заказа'
UNION ALL
SELECT
100,
'Товар выдан'
GO
--Тип статуса
INSERT INTO [dbo].[StatusKind]
           (
		   [StatusKindID],
           [Title]
		   )
SELECT
1,
'Новый'
UNION ALL
SELECT
2,
'На исполнении'
UNION ALL
SELECT
3,
'Отложенные'
UNION ALL
SELECT
4,
'Исполненные'
UNION ALL
SELECT
5,
'Закрытые'
GO
--Статусы
INSERT INTO [dbo].[OrderStatus]
           ([Title]
           ,[StatusKindID],
           [UserDomainID]           
           )
 SELECT
 'Новый',
 1,
 'B98E8EA3-BA51-49C6-916D-E1E978F5BEA3'
UNION ALL

SELECT
 'На исполнении',
 2,
 'B98E8EA3-BA51-49C6-916D-E1E978F5BEA3'
UNION ALL
SELECT
 'На согласовании',
 3,
 'B98E8EA3-BA51-49C6-916D-E1E978F5BEA3'
UNION ALL
SELECT
 'Ждет запчасть',
 3,
 'B98E8EA3-BA51-49C6-916D-E1E978F5BEA3'
UNION ALL
SELECT
 'Исполненные',
 4,
 'B98E8EA3-BA51-49C6-916D-E1E978F5BEA3'
UNION ALL
SELECT
 'Закрыт',
5,
'B98E8EA3-BA51-49C6-916D-E1E978F5BEA3'

Go

--Роли в приложении
INSERT INTO [dbo].[ProjectRole]
           (
           [ProjectRoleID]
           ,[Title]
           )
SELECT
	1,
	'Администратор'
UNION ALL
SELECT
	2,
	'Менеджер'
UNION ALL
SELECT
	3,
	'Инженер'
GO
INSERT INTO [dbo].[User]
           (
           [ProjectRoleID]
           ,[LoginName]
           ,[PasswordHash]
           ,[FirstName]
           ,[LastName]
           ,[MiddleName]
           ,[Phone]
           ,[Email]
           ,[UserDomainID]
           )
     VALUES
           (
           1,
           'admin',
			'$2a$10$JgPV0GGu5/GBiJHER6OoceTFxI9KOiak2Syu4tAQAILdmG5OUaAiW',
           'Иванов',
           'Иван',
           'Иванович',
           '89610881290',
           'ya@ya.ru',
           'B98E8EA3-BA51-49C6-916D-E1E978F5BEA3'
           )



go
--Типы документа
INSERT INTO [dbo].[DocumentKind]
           (
           [DocumentKindID]
           ,[Title]
           )
SELECT
1,
'Документы заказа'

go
--Тип статьи бюджета


INSERT INTO [dbo].[FinancialItemKind]
           (
           [FinancialItemKindID],
           [Title]
           )
SELECT
	1,
	'Пользовательский'
UNION ALL
SELECT
	2,
	'Статья доходов от оплаты заказов'
UNION ALL
SELECT
	3,
	'Статья расходов от закупки комплектующих по накладным'
go

INSERT INTO [dbo].[TransactionKind]
           (
           [TransactionKindID],
           [Title]
           )
SELECT
	1,
	'Доходы'
UNION ALL
SELECT
	2,
	'Расходы'	
           
GO

INSERT INTO [dbo].[DimensionKind]
           (
		   [DimensionKindID]
           ,[Title]
           ,[ShortTitle]
		   )
SELECT 1,'Штуки','шт'
GO


INSERT INTO [dbo].[AutocompleteKind]
           (
		   [AutocompleteKindID]
           ,[Title])
SELECT
1,
'Бренд'
UNION ALL
SELECT
2,
'Комплектация'
UNION ALL
SELECT
3,
'Внешний вид'
GO
INSERT INTO 
[dbo].[InterestKind]
           (
		   [InterestKindID]
           ,[Title]
		   )
SELECT
0,
'Отсутствует'
UNION ALL
SELECT
1,
'Процент'


GO