IF DB_ID('NotificationDb') IS NULL
BEGIN
    CREATE DATABASE NotificationDb;
END
GO

USE NotificationDb;
GO

IF OBJECT_ID('dbo.Subscriptions', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Subscriptions
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        EventType NVARCHAR(100) NOT NULL,
        WebhookUrl NVARCHAR(1000) NOT NULL,
        CreatedAtUtc DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
    );
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE name = 'IX_Subscriptions_EventType'
      AND object_id = OBJECT_ID('dbo.Subscriptions')
)
BEGIN
    CREATE INDEX IX_Subscriptions_EventType
    ON dbo.Subscriptions(EventType);
END
GO