SELECT * FROM EventFieldSet;
SELECT * FROM EventFieldUserSet efs WHERE efs.UserID_User in (
		SELECT UserID_User FROM EventFieldUserSet ef WHERE ef.Value LIKE '%Cl' OR ef.Value LIKE 'Cl%' );
SELECT * FROM FieldSet;
SELECT * FROM UserSet;

SELECT * FROM EventSet;