﻿CREATE FUNCTION [dbo].[DATETIMEDEFAULT]()
RETURNS DATETIME
AS
BEGIN
	RETURN CAST('2000-01-01 00:00:00.000' as datetime)
END
