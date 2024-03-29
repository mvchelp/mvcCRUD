
--EXEC search_Employee_Paging @PageSize =6,@PageIndex =2, @debug=1

--EXEC search_Employee_Paging 
--EXEC search_Employee_Paging @custid  = 'ALFKI'
--EXEC search_Employee_Paging @prodid  = 76
--EXEC search_Employee_Paging @prodid  = 76, @custid = 'RATTC'
--EXEC search_Employee_Paging @fromdate = '19980205', @todate = '19980209'
--EXEC search_Employee_Paging @city = 'Seattle', @EmployeeID = 1,@debug=1
-- http://www.sommarskog.se/dyn-search-2008.html

Alter PROCEDURE [dbo].[search_Employee_Paging] 

		@EmployeeID int = NULL,
		@fromBirthDate datetime = NULL,
		@toBirthDate datetime = NULL,
		@fromHireDate datetime = NULL,
		@toHireDate datetime = NULL,
		@city nvarchar(15) = NULL,
		@region nvarchar(15) = NULL,
		@country nvarchar(15) = NULL,
		@LastName nvarchar(15) = NULL,
		@PageSize int = 10,
		@PageIndex int = 1,
		@debug bit = 0
AS

  DECLARE @sql nvarchar(max),
          @paramlist nvarchar(4000),
          @nl char(2) = CHAR(13) + CHAR(10)

  SELECT
    @sql =
    ';WITH Results_CTE AS
    (   SELECT [EmployeeID]
       ,ROW_NUMBER() OVER (ORDER BY EmployeeID DESC) AS RowNumber
       ,PageIndex =(@PageIndex)
      ,[LastName]
      ,[FirstName]
      ,[Title]
      ,[TitleOfCourtesy]
      ,[BirthDate]
      ,[HireDate]
      ,[Address]
      ,[City]
      ,[Region]
      ,[PostalCode]
      ,[Country]
      ,[HomePhone]
      ,[Extension]
      ,[Photo]
      ,[Notes]
      ,[ReportsTo]
      ,[PhotoPath]
      ,TotalRows=Count(*) OVER()
      FROM [NORTHWND].[dbo].[Employees] )                                         
               
    ' + @nl
  SELECT
    @sql += 'Select * from Results_CTE WHERE  1 = 1' + @nl

  IF @EmployeeID IS NOT NULL
    SELECT
      @sql += ' AND EmployeeID= @EmployeeID' + @nl

  IF @fromBirthDate IS NOT NULL
    SELECT
      @sql += ' AND BirthDate >= @fromBirthDate' + @nl

  IF @toBirthDate IS NOT NULL
    SELECT
      @sql += ' AND BirthDate <= @toBirthDate' + @nl

  IF @fromHireDate IS NOT NULL
    SELECT
      @sql += ' AND BirthDate >= @fromHireDate' + @nl

  IF @toHireDate IS NOT NULL
    SELECT
      @sql += ' AND BirthDate <= @toHireDate' + @nl

  IF @LastName IS NOT NULL
    SELECT
      @sql += ' AND LastName  LIKE @LastName + ''%''' + @nl

  IF @city IS NOT NULL
    SELECT
      @sql += ' AND City = @city' + @nl

  IF @region IS NOT NULL
    SELECT
      @sql += ' AND Region = @region' + @nl

  IF @country IS NOT NULL
    SELECT
      @sql += ' AND Country = @country' + @nl


  IF (@PageIndex IS NOT NULL
    AND @PageSize IS NOT NULL)
    SELECT
      @sql += ' AND RowNumber                      
    BETWEEN (' + CAST(@PageIndex AS varchar(5)) + ' - 1) * ' + CAST(@PageSize AS varchar(6)) +
      ' + 1 AND ' + CAST(@PageIndex AS varchar(5)) + '*' + CAST(@PageSize AS varchar(6)) + '' + @nl

  SELECT
    @sql += ' ORDER BY LastName' + @nl




  IF @debug = 1
    PRINT @sql

  SELECT
    @paramlist = '@EmployeeID int = NULL,
					@fromBirthDate datetime = NULL,
					@toBirthDate datetime = NULL,
					@fromHireDate datetime = NULL,
					@toHireDate datetime = NULL,
					@city nvarchar(15) = NULL,
					@region nvarchar(15) = NULL,
					@country nvarchar(15) = NULL,
					@LastName nvarchar(15) = NULL,
					@PageSize int = 10,
					@PageIndex int=1
					 '

  EXEC sp_executesql @sql,
                     @paramlist,
                     @EmployeeID,
                     @fromBirthDate,
                     @toBirthDate,
                     @fromHireDate,
                     @toHireDate,
                     @city,
                     @region,
                     @country,
                     @LastName,
                     @PageSize,
                     @PageIndex