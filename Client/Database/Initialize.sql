INSERT INTO DocumentKind
			(
			DocumentKindID,
			Title
			)
VALUES(
1,
'��������� ������'
);
INSERT INTO DocSequence
			(
			CurrentNumber
			)
VALUES(
1
);

INSERT INTO Settings
			(
			Version
			)
VALUES(
1
);

INSERT INTO ProjectRole
           (
           ProjectRoleID
           ,Title
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
	;
	
INSERT INTO DimensionKind
           (
		   DimensionKindID
           ,Title
           ,ShortTitle
		   )
SELECT 1,'�����','��';


INSERT INTO StatusKind
           (
		   StatusKindID,
           Title
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
;

INSERT INTO TimelineKind
           (
		   TimelineKindID,
		   Title           
		   
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
'����� �����';