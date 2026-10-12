USE TaekwondoDB;

DECLARE @sql NVARCHAR(MAX) = N'';

-- Xóa FK của schema Taekwondo
SELECT @sql +=
    N'ALTER TABLE '
    + QUOTENAME(SCHEMA_NAME(t.schema_id))
    + N'.' + QUOTENAME(t.name)
    + N' DROP CONSTRAINT '
    + QUOTENAME(fk.name)
    + N';' + CHAR(13)
FROM sys.foreign_keys fk
INNER JOIN sys.tables t
    ON fk.parent_object_id = t.object_id
WHERE SCHEMA_NAME(t.schema_id) = 'Taekwondo';

EXEC sp_executesql @sql;


-- Xóa bảng
SET @sql = N'';

SELECT @sql +=
    N'DROP TABLE '
    + QUOTENAME(SCHEMA_NAME(schema_id))
    + N'.' + QUOTENAME(name)
    + N';' + CHAR(13)
FROM sys.tables
WHERE SCHEMA_NAME(schema_id) = 'Taekwondo';

EXEC sp_executesql @sql;