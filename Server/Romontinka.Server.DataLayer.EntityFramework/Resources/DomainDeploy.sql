--Типы заказа
DECLARE @message VARCHAR(255)

BEGIN TRAN
BEGIN TRY	
	INSERT INTO dbo.OrderKind
	(
		Title,
		UserDomainID
	)
	SELECT
	'Ремонт',
	'{0}'
	UNION ALL
	SELECT
	'Гарантия',
	'{0}'
	go

	--Статусы
	INSERT INTO [dbo].[OrderStatus]
			   ([Title]
			   ,[StatusKindID],
			   [UserDomainID]           
			   )
	 SELECT
	 'Новый',
	 1,
	 '{0}'
	UNION ALL

	SELECT
	 'На исполнении',
	 2,
	 '{0}'
	UNION ALL
	SELECT
	 'На согласовании',
	 3,
	 '{0}'
	UNION ALL
	SELECT
	 'Ждет запчасть',
	 3,
	 '{0}'
	UNION ALL
	SELECT
	 'Исполненные',
	 4,
	 '{0}'
	UNION ALL
	SELECT
	 'Закрыт',
	5,
	'{0}'

	GO
	--Емкость для номеров заказа доменов
	INSERT INTO [dbo].[OrderCapacity]
           (
           [UserDomainID]
           ,[OrderNumber]
           )
     SELECT           
        '{0}',
       1
	
	GO

	INSERT INTO [dbo].[CustomReport]
           (
           [Title],           
           [DocumentKindID],
           [UserDomainID],
           [HtmlContent]
           )
SELECT
'Гарантийная',
1,
'{0}',
'{1}'
UNION ALL

SELECT
'приемная',
1,
'{0}',

'{2}'
UNION ALL

SELECT
'товарный чек',
1,
'{0}',
'{3}'



UPDATE u
SET  u.PasswordHash = d.PasswordHash
FROM
	dbo.[User] AS u
INNER JOIN
	dbo.UserDomain AS d
ON u.UserDomainID = d.UserDomainID 
WHERE d.UserDomainID = '{0}'

INSERT INTO [dbo].[Branch]
(
	[Title]
	,[Address]
	,[LegalName]
	,[UserDomainID]
)
SELECT       
	[Trademark]
	,[Address]
	,[LegalName]		
	,[UserDomainID]      
FROM 
	[dbo].[UserDomain]
WHERE
 [UserDomainID] = '{0}'



UPDATE UserDomain SET IsActive = 1 
WHERE UserDomainID = '{0}'

	COMMIT TRAN
END TRY
BEGIN CATCH
	SET @message = ERROR_MESSAGE() 
	ROLLBACK TRAN
	RAISERROR(@message, 16, 1);
END CATCH 