CREATE TABLE [dbo].[Statistics]
(
    [Продолжительность игры] TIMESTAMP NULL, 
    [Дата партии] NCHAR(10) NULL, 
    [Кто выйграл] NCHAR(10) NOT NULL, 
    [id] INT NOT NULL PRIMARY KEY
)
