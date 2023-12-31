CREATE PROCEDURE GetAllCart
	@skipItem INT,
	@pageSize INT,
	@keyWord NVARCHAR(500),
	@totalRecord INT OUT

AS
BEGIN
	
	SELECT c.Id, app.UserName, c.Note, c.CreateDate, c.Status,
		ROW_NUMBER() OVER(ORDER BY c.CreateDate) as RowNo INTO #TBCART
	FROM Cards c  LEFT JOIN ApplicationUser app ON c.UserId = app.Id
	WHERE (ISNULL(@keyWord, '') = '' OR ( app.UserName LIKE '%'+@keyWord+'%' OR
										c.Note LIKE '%'+@keyWord+'%'
											))
	SELECT @totalRecord = COUNT(*) FROM #TBCART

	SELECT * 
	FROM #TBCART
	WHERE RowNo BETWEEN @skipItem AND @pageSize

	DROP TABLE #TBCART

END