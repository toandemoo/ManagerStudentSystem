USE master;

ALTER DATABASE TaekwondoDB
SET SINGLE_USER
WITH ROLLBACK IMMEDIATE;

DROP DATABASE TaekwondoDB;

-- delete From Taekwondo.Users Where UserId = 1;