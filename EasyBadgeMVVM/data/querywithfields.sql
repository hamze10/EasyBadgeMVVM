SELECT *
FROM UserSet u
LEFT OUTER JOIN FieldUserSet fu ON u.ID_User = fu.UserID_User
LEFT OUTER JOIN FieldSet f ON fu.FieldID_Field = f.ID_Field
LEFT OUTER JOIN UserEventSet ue ON u.ID_User = ue.UserID_User AND ue.EventID_Event = 1 AND ue.FieldUserID_FieldUser1 = fu.ID_FieldUser	