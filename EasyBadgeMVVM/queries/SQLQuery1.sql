SELECT * FROM EventFieldSets ORDER BY EventID_Event;
SELECT * FROM EventFieldUserSets WHERE Value = 'De Man'

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

DELETE FROM TargetSets WHERE ID_Target > 3

INSERT INTO PrintBadgeSets(PrintDate, PrintBy, Comment, UserID_User, EventID_Event)
VALUES (CURRENT_TIMESTAMP, 'PC4', NULL, 3, 1);

UPDATE FieldSets SET Name = 'VIP' WHERE ID_Field = 7

SELECT * FROM PositionSets
SELECT * FROM RuleSets

INSERT INTO TargetSets(Name) VALUES('Window')
INSERT INTO TargetSets(Name) VALUES('List')
INSERT INTO TargetSets(Name) VALUES('Badge')

