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

CREATE TABLE [Account] (
    [UserName] nvarchar(100) NOT NULL,
    [DisplayName] nvarchar(100) NOT NULL DEFAULT ((N'Admin')),
    [PassWord] nvarchar(1000) NOT NULL DEFAULT (((0))),
    [Type] int NOT NULL,
    CONSTRAINT [PK__Account__C9F2845720CEE35B] PRIMARY KEY ([UserName])
);
GO

CREATE TABLE [FoodCategory] (
    [id] int NOT NULL IDENTITY,
    [name] nvarchar(100) NOT NULL DEFAULT ((N'Chưa đặt tên')),
    CONSTRAINT [PK_FoodCategory] PRIMARY KEY ([id])
);
GO

CREATE TABLE [TableFood] (
    [id] int NOT NULL IDENTITY,
    [name] nvarchar(100) NOT NULL DEFAULT ((N'Bàn chưa có tên')),
    [status] nvarchar(100) NOT NULL DEFAULT ((N'Trống')),
    CONSTRAINT [PK_TableFood] PRIMARY KEY ([id])
);
GO

CREATE TABLE [Food] (
    [id] int NOT NULL IDENTITY,
    [name] nvarchar(100) NOT NULL DEFAULT ((N'Chưa đặt tên')),
    [idCategory] int NOT NULL,
    [price] float NOT NULL,
    CONSTRAINT [PK_Food] PRIMARY KEY ([id]),
    CONSTRAINT [FK__Food__price__31EC6D26] FOREIGN KEY ([idCategory]) REFERENCES [FoodCategory] ([id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [Bill] (
    [id] int NOT NULL IDENTITY,
    [DateCheckIn] date NOT NULL DEFAULT ((getdate())),
    [DateCheckOut] date NULL,
    [idTable] int NOT NULL,
    [status] int NOT NULL,
    [discount] int NULL,
    [totalPrice] float NULL,
    CONSTRAINT [PK_Bill] PRIMARY KEY ([id]),
    CONSTRAINT [FK__Bill__status__36B12243] FOREIGN KEY ([idTable]) REFERENCES [TableFood] ([id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [BillInfo] (
    [id] int NOT NULL IDENTITY,
    [idBill] int NOT NULL,
    [idFood] int NOT NULL,
    [count] int NOT NULL,
    CONSTRAINT [PK_BillInfo] PRIMARY KEY ([id]),
    CONSTRAINT [FK__BillInfo__count__3A81B327] FOREIGN KEY ([idBill]) REFERENCES [Bill] ([id]) ON DELETE NO ACTION,
    CONSTRAINT [FK__BillInfo__idFood__3B75D760] FOREIGN KEY ([idFood]) REFERENCES [Food] ([id]) ON DELETE NO ACTION
);
GO

CREATE INDEX [IX_Bill_idTable] ON [Bill] ([idTable]);
GO

CREATE INDEX [IX_BillInfo_idBill] ON [BillInfo] ([idBill]);
GO

CREATE INDEX [IX_BillInfo_idFood] ON [BillInfo] ([idFood]);
GO

CREATE INDEX [IX_Food_idCategory] ON [Food] ([idCategory]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210605171007_V0', N'5.0.6');
GO

COMMIT;
GO

