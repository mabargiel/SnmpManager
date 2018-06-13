IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180602115850_InitialMigration')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20180602115850_InitialMigration', N'2.1.0-rtm-30799');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180602121743_CreateTableAgents')
BEGIN
    CREATE TABLE [Watchers] (
        [Id] uniqueidentifier NOT NULL,
        [IpAddress] nvarchar(max) NULL,
        [Mib] nvarchar(max) NULL,
        [UpdatesEvery] int NOT NULL,
        [Method] int NOT NULL,
        CONSTRAINT [PK_Watchers] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180602121743_CreateTableAgents')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20180602121743_CreateTableAgents', N'2.1.0-rtm-30799');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180602130247_RemoveUpdatesEveryColumn')
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Watchers]') AND [c].[name] = N'UpdatesEvery');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Watchers] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [Watchers] DROP COLUMN [UpdatesEvery];
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180602130247_RemoveUpdatesEveryColumn')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20180602130247_RemoveUpdatesEveryColumn', N'2.1.0-rtm-30799');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180606183455_CreateUpdatesEveryColumn')
BEGIN
    ALTER TABLE [Watchers] ADD [UpdatesEvery] int NOT NULL DEFAULT 0;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180606183455_CreateUpdatesEveryColumn')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20180606183455_CreateUpdatesEveryColumn', N'2.1.0-rtm-30799');
END;

GO

