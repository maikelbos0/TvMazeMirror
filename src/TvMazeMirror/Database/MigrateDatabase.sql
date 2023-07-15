IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230715112447_Initial')
BEGIN
    CREATE TABLE [Shows] (
        [Id] int NOT NULL IDENTITY,
        [TvMazeId] int NULL,
        [Name] nvarchar(max) NOT NULL,
        [Language] nvarchar(max) NULL,
        [Premiered] datetime2 NULL,
        [Summary] nvarchar(max) NULL,
        CONSTRAINT [PK_Shows] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230715112447_Initial')
BEGIN
    CREATE TABLE [ShowGenre] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [ShowId] int NULL,
        CONSTRAINT [PK_ShowGenre] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ShowGenre_Shows_ShowId] FOREIGN KEY ([ShowId]) REFERENCES [Shows] ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230715112447_Initial')
BEGIN
    CREATE INDEX [IX_ShowGenre_ShowId] ON [ShowGenre] ([ShowId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230715112447_Initial')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230715112447_Initial', N'7.0.9');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230715132356_ShowGenre-Fixes')
BEGIN
    ALTER TABLE [ShowGenre] DROP CONSTRAINT [FK_ShowGenre_Shows_ShowId];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230715132356_ShowGenre-Fixes')
BEGIN
    ALTER TABLE [ShowGenre] DROP CONSTRAINT [PK_ShowGenre];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230715132356_ShowGenre-Fixes')
BEGIN
    EXEC sp_rename N'[ShowGenre]', N'ShowGenres';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230715132356_ShowGenre-Fixes')
BEGIN
    EXEC sp_rename N'[ShowGenres].[IX_ShowGenre_ShowId]', N'IX_ShowGenres_ShowId', N'INDEX';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230715132356_ShowGenre-Fixes')
BEGIN
    DROP INDEX [IX_ShowGenres_ShowId] ON [ShowGenres];
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ShowGenres]') AND [c].[name] = N'ShowId');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [ShowGenres] DROP CONSTRAINT [' + @var0 + '];');
    EXEC(N'UPDATE [ShowGenres] SET [ShowId] = 0 WHERE [ShowId] IS NULL');
    ALTER TABLE [ShowGenres] ALTER COLUMN [ShowId] int NOT NULL;
    ALTER TABLE [ShowGenres] ADD DEFAULT 0 FOR [ShowId];
    CREATE INDEX [IX_ShowGenres_ShowId] ON [ShowGenres] ([ShowId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230715132356_ShowGenre-Fixes')
BEGIN
    ALTER TABLE [ShowGenres] ADD CONSTRAINT [PK_ShowGenres] PRIMARY KEY ([Id]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230715132356_ShowGenre-Fixes')
BEGIN
    ALTER TABLE [ShowGenres] ADD CONSTRAINT [FK_ShowGenres_Shows_ShowId] FOREIGN KEY ([ShowId]) REFERENCES [Shows] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230715132356_ShowGenre-Fixes')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230715132356_ShowGenre-Fixes', N'7.0.9');
END;
GO

COMMIT;
GO

