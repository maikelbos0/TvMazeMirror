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

