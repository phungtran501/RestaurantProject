CREATE PROCEDURE GetAllFood
	@skipItem INT,
	@pageSize INT,
	@keyWord NVARCHAR(500),
	@totalRecord INT OUT

AS
BEGIN
	
	SELECT f.Id, f.Name as FoodName, f.Description, f.Price, f.Available, f.IsActive, cat.Name as CategoryName,
		ROW_NUMBER() OVER(ORDER BY f.Id) as RowNo INTO #TBFOOD
	FROM Food f  LEFT JOIN Categogy cat ON f.CategoryId = cat.Id
	WHERE (ISNULL(@keyWord, '') = '' OR ( f.Name LIKE '%'+@keyWord+'%' OR
										f.Description LIKE '%'+@keyWord+'%'
	
											))
	AND f.IsActive = 1

	SELECT @totalRecord = COUNT(*) FROM #TBFOOD

	SELECT * 
	FROM #TBFOOD
	WHERE RowNo BETWEEN @skipItem AND @pageSize

	DROP TABLE #TBFOOD

END