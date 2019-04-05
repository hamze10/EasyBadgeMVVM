SELECT * FROM EventFieldSet;
SELECT * FROM EventFieldUserSet efs WHERE efs.UserID_User in (
		SELECT UserID_User FROM EventFieldUserSet ef WHERE ef.Value = 'Mueller' );

SELECT efs.UserID_User, COUNT(UserID_User) FROM EventFieldUserSet efs 
WHERE (efs.Value = 'Mueller' OR efs.Value = 'Jeanine' OR efs.Value = 'Comstruct') AND efs.EventFieldEventID_Event = 1 
GROUP BY efs.UserID_User


SELECT * FROM FieldSet;
SELECT * FROM UserSet;

SELECT * FROM EventSet;