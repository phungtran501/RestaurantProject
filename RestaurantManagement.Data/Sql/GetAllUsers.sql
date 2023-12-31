CREATE PROCEDURE GetAllUsers
	@skipItem INT,
	@pageSize INT,
	@keyWord NVARCHAR(500),
	@totalRecord INT OUT
AS
BEGIN

	SELECT  app.Id, app.UserName, app.Fullname, app.Email, app.PhoneNumber, app.Address, app.IsActive, app.IsSystem , rol.Name as RoleName,
			ROW_NUMBER() OVER(ORDER BY app.UserName) as RowNo INTO #TBUSER
	FROM ApplicationUser app LEFT JOIN UserRole userrole ON app.Id = userrole.UserId
							 LEFT JOIN [Role] rol ON userrole.RoleId = rol.Id
	WHERE (ISNULL(@keyWord, '') = '' OR ( app.UserName LIKE '%'+@keyWord+'%' OR
										 app.Fullname LIKE '%'+@keyWord+'%' OR
										 app.Email LIKE '%'+@keyWord+'%' OR
										 app.PhoneNumber LIKE '%'+@keyWord+'%' OR
										 app.Address LIKE '%'+@keyWord+'%' 
										 ))
		AND app.UserName <> 'Administrator'
		AND app.IsActive = 1

	SELECT @totalRecord = COUNT(*) FROM #TBUSER

	SELECT * 
	FROM #TBUSER
	WHERE RowNo BETWEEN @skipItem AND @pageSize
	
	DROP TABLE #TBUSER

END