CREATE TABLE [dbo].[Statistics] (S
    [Продолжительность игры] TIME NULL,
    [Дата партии]            DATETIME NULL,
    [Кто выйграл]            CHAR(10) NOT NULL,
    [id]                     INT        NOT NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

