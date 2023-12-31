CREATE PROCEDURE GetAllUserAddress
	@skipItem INT,
	@pageSize INT,
	@keyWord NVARCHAR(500),
	@totalRecord INT OUT

AS
BEGIN
	
	SELECT ua.Id, app.UserName, ua.Fullname, ua.Address, ua.Phone, ua.IsActive,
		ROW_NUMBER() OVER(ORDER BY ua.Id) as RowNo INTO #TBUSERADDRESS
	FROM UserAddress ua  LEFT JOIN ApplicationUser app ON ua.UserId = app.Id
	WHERE (ISNULL(@keyWord, '') = '' OR ( ua.Address LIKE '%'+@keyWord+'%' OR
										ua.Fullname LIKE '%'+@keyWord+'%' OR
										ua.Phone LIKE '%'+@keyWord+'%' OR
										app.UserName LIKE '%'+@keyWord+'%'
	
											))
	AND ua.IsActive = 1

	SELECT @totalRecord = COUNT(*) FROM #TBUSERADDRESS

	SELECT * 
	FROM #TBUSERADDRESS
	WHERE RowNo BETWEEN @skipItem AND @pageSize

	DROP TABLE #TBUSERADDRESS

END