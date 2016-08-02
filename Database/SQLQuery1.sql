
--EXEC search_orders_1 
--EXEC search_orders_1 @custid  = 'ALFKI'
--EXEC search_orders_1 @prodid  = 76
--EXEC search_orders_1 @prodid  = 76, @custid = 'RATTC'
--EXEC search_orders_1 @fromdate = '19980205', @todate = '19980209'
--EXEC search_orders_1 @city = 'Bräcke', @prodid = 76,@debug=1
-- http://www.sommarskog.se/dyn-search-2008.html


alter PROCEDURE search_orders_1                                   
                 @orderid     int          = NULL,                
                 @fromdate    datetime     = NULL,                
                 @todate      datetime     = NULL,                
                 @minprice    money        = NULL,                 
                 @maxprice    money        = NULL,                
                 @custid      nchar(5)     = NULL,                
                 @custname    nvarchar(40) = NULL,                 
                 @city        nvarchar(15) = NULL,                 
                 @region      nvarchar(15) = NULL,                 
                 @country     nvarchar(15) = NULL,                 
                 @prodid      int          = NULL,                 
                 @prodname    nvarchar(40) = NULL,                 
                 @employeestr varchar(MAX) = NULL,                 
                       
                 @debug       bit          = 0 AS                 
                                                                   
DECLARE @sql        nvarchar(MAX),                                
        @paramlist  nvarchar(4000),                               
        @nl         char(2) = char(13) + char(10)                 
                                                                  
SELECT @sql =                                                      
    'SELECT o.OrderID, o.OrderDate, od.UnitPrice, od.Quantity,    
            c.CustomerID, c.CompanyName, c.Address, c.City,        
            c.Region,  c.PostalCode, c.Country, c.Phone,           
            p.ProductID, p.ProductName, p.UnitsInStock,            
            p.UnitsOnOrder, o.EmployeeID                           
     FROM   dbo.Orders o                                           
     JOIN   dbo.[Order Details] od ON o.OrderID = od.OrderID       
     JOIN   dbo.Customers c ON o.CustomerID = c.CustomerID         
     JOIN   dbo.Products p ON p.ProductID = od.ProductID           
     WHERE  1 = 1' + @nl                                           
                                                                   
IF @orderid IS NOT NULL                                            
   SELECT @sql += ' AND o.OrderID = @orderid' +                   
                  ' AND od.OrderID = @orderid' + @nl              
                                                                   
IF @fromdate IS NOT NULL                                           
   SELECT @sql += ' AND o.OrderDate >= @fromdate' + @nl            
                                                                   
IF @todate IS NOT NULL                                            
   SELECT @sql += ' AND o.OrderDate <= @todate'  + @nl            
                                                                   
IF @minprice IS NOT NULL                                           
   SELECT @sql += ' AND od.UnitPrice >= @minprice'  + @nl          
                                                                  
IF @maxprice IS NOT NULL                                         
   SELECT @sql += ' AND od.UnitPrice <= @maxprice'  + @nl         
                                                                  
IF @custid IS NOT NULL                                            
   SELECT @sql += ' AND o.CustomerID = @custid' +                  
                  ' AND c.CustomerID = @custid' + @nl             
                                                                   
IF @custname IS NOT NULL                                           
   SELECT @sql += ' AND c.CompanyName LIKE @custname + ''%''' + @nl 
                                                                   
IF @city IS NOT NULL                                              
   SELECT @sql += ' AND c.City = @city' + @nl                      
                                                                  
IF @region IS NOT NULL                                           
   SELECT @sql += ' AND c.Region = @region' + @nl                
                                                                  
IF @country IS NOT NULL                                           
   SELECT @sql += ' AND c.Country = @country' + @nl               
                                                                  
IF @prodid IS NOT NULL                                             
   SELECT @sql += ' AND od.ProductID = @prodid' +                  
                  ' AND p.ProductID = @prodid' + @nl               
                                                                   
IF @prodname IS NOT NULL                                            
   SELECT @sql += ' AND p.ProductName LIKE @prodname + ''%''' + @nl
                                                                   
IF @employeestr IS NOT NULL                                       
   SELECT @sql += ' AND o.EmployeeID IN' +                       
                  ' (SELECT number FROM dbo.intlist_to_tbl(@employeestr))' + @nl
                                                                  

                                                                  
SELECT @sql += ' ORDER BY o.OrderID' + @nl                        
                                                                  
IF @debug = 1                                                     
   PRINT @sql                                                   
                                                                  
SELECT @paramlist = '@orderid     int,                            
                     @fromdate    datetime,                        
                     @todate      datetime,                       
                     @minprice    money,                          
                     @maxprice    money,                           
                     @custid      nchar(5),                        
                     @custname    nvarchar(40),                   
                     @city        nvarchar(15),                   
                     @region      nvarchar(15),                   
                     @country     nvarchar(15),                    
                     @prodid      int,                            
                     @prodname    nvarchar(40),                    
                     @employeestr varchar(MAX)                   
                     '        
                                                                  
EXEC sp_executesql @sql, @paramlist,                               
                   @orderid, @fromdate, @todate, @minprice,       
                   @maxprice,  @custid, @custname, @city, @region, 
                   @country, @prodid, @prodname, @employeestr