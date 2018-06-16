--����� �������������
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
	'��� ����',
	'����',
	'�. �����',
	'admin',
	'$2a$10$JgPV0GGu5/GBiJHER6OoceTFxI9KOiak2Syu4tAQAILdmG5OUaAiW'
)
--������ ��� �������

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


--��� �������
INSERT INTO [dbo].[TimelineKind]
           (
		   [TimelineKindID]
           ,[Title]           
		   )
SELECT
1,
'�������� ��������'
UNION ALL
SELECT
2,
'�������� �����������'
UNION ALL
SELECT
3,
'��������� ������'
UNION ALL
SELECT
4,
'��������� ��������'
UNION ALL
SELECT
5,
'�������� �����������'
UNION ALL
SELECT
7,
'������� ������ ������'
UNION ALL
SELECT
100,
'����� �����'
GO
--��� �������
INSERT INTO [dbo].[StatusKind]
           (
		   [StatusKindID],
           [Title]
		   )
SELECT
1,
'�����'
UNION ALL
SELECT
2,
'�� ����������'
UNION ALL
SELECT
3,
'����������'
UNION ALL
SELECT
4,
'�����������'
UNION ALL
SELECT
5,
'��������'
GO
--�������
INSERT INTO [dbo].[OrderStatus]
           ([Title]
           ,[StatusKindID],
           [UserDomainID]           
           )
 SELECT
 '�����',
 1,
 'B98E8EA3-BA51-49C6-916D-E1E978F5BEA3'
UNION ALL

SELECT
 '�� ����������',
 2,
 'B98E8EA3-BA51-49C6-916D-E1E978F5BEA3'
UNION ALL
SELECT
 '�� ������������',
 3,
 'B98E8EA3-BA51-49C6-916D-E1E978F5BEA3'
UNION ALL
SELECT
 '���� ��������',
 3,
 'B98E8EA3-BA51-49C6-916D-E1E978F5BEA3'
UNION ALL
SELECT
 '�����������',
 4,
 'B98E8EA3-BA51-49C6-916D-E1E978F5BEA3'
UNION ALL
SELECT
 '������',
5,
'B98E8EA3-BA51-49C6-916D-E1E978F5BEA3'

Go

--���� � ����������
INSERT INTO [dbo].[ProjectRole]
           (
           [ProjectRoleID]
           ,[Title]
           )
SELECT
	1,
	'�������������'
UNION ALL
SELECT
	2,
	'��������'
UNION ALL
SELECT
	3,
	'�������'
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
           '������',
           '����',
           '��������',
           '89610881290',
           'ya@ya.ru',
           'B98E8EA3-BA51-49C6-916D-E1E978F5BEA3'
           )



go
--���� ���������
INSERT INTO [dbo].[DocumentKind]
           (
           [DocumentKindID]
           ,[Title]
           )
SELECT
1,
'��������� ������'

go
--��� ������ �������


INSERT INTO [dbo].[FinancialItemKind]
           (
           [FinancialItemKindID],
           [Title]
           )
SELECT
	1,
	'����������������'
UNION ALL
SELECT
	2,
	'������ ������� �� ������ �������'
UNION ALL
SELECT
	3,
	'������ �������� �� ������� ������������� �� ���������'
go

INSERT INTO [dbo].[TransactionKind]
           (
           [TransactionKindID],
           [Title]
           )
SELECT
	1,
	'������'
UNION ALL
SELECT
	2,
	'�������'	
           
GO

INSERT INTO [dbo].[DimensionKind]
           (
		   [DimensionKindID]
           ,[Title]
           ,[ShortTitle]
		   )
SELECT 1,'�����','��'
GO


INSERT INTO [dbo].[AutocompleteKind]
           (
		   [AutocompleteKindID]
           ,[Title])
SELECT
1,
'�����'
UNION ALL
SELECT
2,
'������������'
UNION ALL
SELECT
3,
'������� ���'
GO
INSERT INTO 
[dbo].[InterestKind]
           (
		   [InterestKindID]
           ,[Title]
		   )
SELECT
0,
'�����������'
UNION ALL
SELECT
1,
'�������'


GO