/*SELECT *
FROM UserSet u
LEFT OUTER JOIN FieldUserSet fu ON u.ID_User = fu.UserID_User
LEFT OUTER JOIN FieldSet f ON fu.FieldID_Field = f.ID_Field
LEFT OUTER JOIN UserEventSet ue ON u.ID_User = ue.UserID_User AND ue.FieldUserID_FieldUser1 = fu.ID_FieldUser	
LEFT OUTER JOIN EventSet e ON e.ID_Event = ue.EventID_Event;*/

/*SELECT u.Barcode, u.CreationDate, e.Name, e.DateOfEvent, f.Name, fu.Value, fu.AdditionnalInformation
FROM UserSet u, FieldSet f, EventSet e, FieldUserSet fu, UserEventSet ue
WHERE u.ID_User = fu.UserID_User AND f.ID_Field = fu.FieldID_Field AND e.ID_Event = ue.EventID_Event 
		AND u.ID_User = ue.UserID_User AND fu.ID_FieldUser = ue.FieldUserID_FieldUser1 AND u.Active = 1;*/
SELECT *
FROM UserSet u, FieldSet f, EventSet e, FieldUserSet fu, UserEventSet ue
WHERE u.ID_User = fu.UserID_User AND f.ID_Field = fu.FieldID_Field AND e.ID_Event = ue.EventID_Event 
		AND u.ID_User = ue.UserID_User AND fu.ID_FieldUser = ue.FieldUserID_FieldUser1 AND u.Active = 1;

SELECT u.ID_User, u.Barcode, f.Name, fu.Value 
FROM FieldUserSet fu, UserSet u, FieldSet f 
WHERE fu.UserID_User = u.ID_User AND f.ID_Field = fu.FieldID_Field

SELECT * FROM FieldSet

SELECT * FROM EventSet

SELECT * FROM FieldUserSet

SELECT * FROM UserSet

SELECT * 
FROM UserEventSet ue, FieldUserSet fu, FieldSet f
WHERE ue.FieldUserID_FieldUser1 = fu.ID_FieldUser 
	AND fu.ID_FieldUser = f.ID_Field AND 
	(f.Name = 'LastName' OR f.Name = 'FirstName') AND (fu.Value = 'Mccoy' OR fu.Value = 'Abbott')