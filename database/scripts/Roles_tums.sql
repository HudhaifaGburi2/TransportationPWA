  BEGIN TRANSACTION;

-- Admin_Tums
IF NOT EXISTS (
    SELECT 1
    FROM [Haram].[dbo].[Regt_User_Role]
    WHERE Role_Name = 'Admin_Tums'
)
BEGIN
    INSERT INTO [Haram].[dbo].[Regt_User_Role]
    (
        Role_ID,
        Role_Name,
        Role_Desc,
        Created_BY,
        Created_ON
    )
    VALUES
    (
        600,
        'Admin_Tums',
        'Administrator role for TUMS (Transportation User Management System)',
        2,
        GETDATE()
    );
END;

-- Stuff_Tums
IF NOT EXISTS (
    SELECT 1
    FROM [Haram].[dbo].[Regt_User_Role]
    WHERE Role_Name = 'Stuff_Tums'
)
BEGIN
    INSERT INTO [Haram].[dbo].[Regt_User_Role]
    (
        Role_ID,
        Role_Name,
        Role_Desc,
        Created_BY,
        Created_ON
    )
    VALUES
    (
        601,
        'Stuff_Tums',
        'Staff role for TUMS operations and daily management',
        2,
        GETDATE()
    );
END;

-- Driver_Tums
IF NOT EXISTS (
    SELECT 1
    FROM [Haram].[dbo].[Regt_User_Role]
    WHERE Role_Name = 'Driver_Tums'
)
BEGIN
    INSERT INTO [Haram].[dbo].[Regt_User_Role]
    (
        Role_ID,
        Role_Name,
        Role_Desc,
        Created_BY,
        Created_ON
    )
    VALUES
    (
        602,
        'Driver_Tums',
        'Driver role for TUMS transportation module',
        2,
        GETDATE()
    );
END;

COMMIT TRANSACTION;
