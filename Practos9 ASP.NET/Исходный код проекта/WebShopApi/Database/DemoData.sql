USE WebShopDB;
GO

SET NOCOUNT ON;
SET XACT_ABORT ON;
BEGIN TRANSACTION;

DECLARE @UserRole int = (SELECT MIN(RoleID) FROM dbo.Roles WHERE RoleName = N'User');

IF @UserRole IS NULL
BEGIN
    INSERT dbo.Roles (RoleName) VALUES (N'User');
    SET @UserRole = CONVERT(int, SCOPE_IDENTITY());
END;

IF NOT EXISTS (SELECT 1 FROM dbo.Roles WHERE RoleName = N'Admin')
    INSERT dbo.Roles (RoleName) VALUES (N'Admin');

IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Login = N'demo.anna.webshop')
   AND NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Login = N'demo.ivan.webshop')
BEGIN
    -- Demo accounts have random placeholders with no known sign-in password.
    INSERT dbo.Users (Login, PasswordHash, RoleID)
    VALUES (N'demo.anna.webshop', CONVERT(varchar(128), CRYPT_GEN_RANDOM(64), 2), @UserRole);
    DECLARE @Anna int = CONVERT(int, SCOPE_IDENTITY());

    INSERT dbo.Users (Login, PasswordHash, RoleID)
    VALUES (N'demo.ivan.webshop', CONVERT(varchar(128), CRYPT_GEN_RANDOM(64), 2), @UserRole);
    DECLARE @Ivan int = CONVERT(int, SCOPE_IDENTITY());

    INSERT dbo.Categories (CategoryName) VALUES (N'Books');
    DECLARE @Books int = CONVERT(int, SCOPE_IDENTITY());

    INSERT dbo.Categories (CategoryName) VALUES (N'Stationery');
    DECLARE @Stationery int = CONVERT(int, SCOPE_IDENTITY());

    INSERT dbo.Products (ProductName, Price, CategoryID) VALUES (N'Learning C#', 950.00, @Books);
    DECLARE @Book int = CONVERT(int, SCOPE_IDENTITY());

    INSERT dbo.Products (ProductName, Price, CategoryID) VALUES (N'A5 notebook', 240.00, @Stationery);
    DECLARE @Notebook int = CONVERT(int, SCOPE_IDENTITY());

    INSERT dbo.Products (ProductName, Price, CategoryID) VALUES (N'Black gel pen', 75.00, @Stationery);
    DECLARE @Pen int = CONVERT(int, SCOPE_IDENTITY());

    INSERT dbo.Carts (UserID) VALUES (@Anna);
    DECLARE @AnnaCart int = CONVERT(int, SCOPE_IDENTITY());

    INSERT dbo.Carts (UserID) VALUES (@Ivan);
    DECLARE @IvanCart int = CONVERT(int, SCOPE_IDENTITY());

    INSERT dbo.CartItems (CartID, ProductID, Quantity)
    VALUES (@AnnaCart, @Notebook, 1), (@IvanCart, @Pen, 2);

    INSERT dbo.Orders (UserID, OrderDate, Status, TotalAmount)
    VALUES (@Anna, '2026-10-20T12:00:00', N'Completed', 950.00);
    DECLARE @FirstOrder int = CONVERT(int, SCOPE_IDENTITY());

    INSERT dbo.Orders (UserID, OrderDate, Status, TotalAmount)
    VALUES (@Ivan, '2026-10-21T09:30:00', N'New', 315.00);
    DECLARE @SecondOrder int = CONVERT(int, SCOPE_IDENTITY());

    INSERT dbo.Orders (UserID, OrderDate, Status, TotalAmount)
    VALUES (@Anna, '2026-10-22T15:45:00', N'Paid', 480.00);
    DECLARE @ThirdOrder int = CONVERT(int, SCOPE_IDENTITY());

    INSERT dbo.OrderItems (OrderID, ProductID, Quantity, UnitPrice)
    VALUES (@FirstOrder, @Book, 1, 950.00), (@SecondOrder, @Notebook, 1, 240.00),
           (@SecondOrder, @Pen, 1, 75.00), (@ThirdOrder, @Notebook, 2, 240.00);
END;

COMMIT TRANSACTION;

SELECT UserID, Login FROM dbo.Users
WHERE Login IN (N'demo.anna.webshop', N'demo.ivan.webshop')
ORDER BY UserID;

SELECT OrderID, UserID, OrderDate, Status, TotalAmount
FROM dbo.Orders
ORDER BY OrderID;
GO
