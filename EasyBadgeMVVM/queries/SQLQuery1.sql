SELECT * FROM EventFieldSet ORDER BY EventID_Event;
SELECT * FROM EventFieldUserSet 

SELECT efs.UserID_User, COUNT(UserID_User) FROM EventFieldUserSet efs 
WHERE (efs.Value = 'Mueller' OR efs.Value = 'Jeanine' OR efs.Value = 'Comstruct') AND efs.EventFieldEventID_Event = 1 
GROUP BY efs.UserID_User


SELECT * FROM FieldSet;
SELECT * FROM UserSet;
SELECT * FROM EventSet;

SELECT * FROM BadgeSet
INSERT INTO BadgeSet VALUES(97,116,'XL', 'Butterfly');
INSERT INTO BadgeSet VALUES(97,148,'A6', 'Butterfly');
INSERT INTO BadgeSet VALUES(86,54,'PVC', 'PVC');

SELECT * FROM BadgeEventSet