SELECT * FROM EventFieldSets ORDER BY EventID_Event;
SELECT * FROM EventFieldUserSets 

SELECT * FROM FieldSets;
SELECT * FROM UserSets;
SELECT * FROM EventSets;

SELECT * FROM BadgeSets
INSERT INTO BadgeSets VALUES(97,86, 'A5', 'Butterfly');
INSERT INTO BadgeSets VALUES(97,116,'XL', 'Butterfly');
INSERT INTO BadgeSets VALUES(97,148,'A6', 'Butterfly');
INSERT INTO BadgeSets VALUES(86,54,'PVC', 'PVC');

SELECT * FROM BadgeEventSets;
SELECT * FROM PrintBadgeSets;

INSERT INTO PrintBadgeSets(PrintDate, PrintBy, Comment, UserID_User, EventID_Event)
VALUES (CURRENT_TIMESTAMP, 'PC4', NULL, 8, 1);

SELECT * FROM PositionSets

