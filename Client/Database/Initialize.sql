INSERT INTO DocumentKind
			(
			DocumentKindID,
			Title
			)
VALUES(
1,
'Документы заказа'
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
	'Администратор'
UNION ALL
SELECT
	2,
	'Менеджер'
UNION ALL
SELECT
	3,
	'Инженер'	
	;
	
INSERT INTO DimensionKind
           (
		   DimensionKindID
           ,Title
           ,ShortTitle
		   )
SELECT 1,'Штуки','шт';


INSERT INTO StatusKind
           (
		   StatusKindID,
           Title
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
;

INSERT INTO TimelineKind
           (
		   TimelineKindID,
		   Title           
		   
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
'Товар выдан';