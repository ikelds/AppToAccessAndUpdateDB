--DROP DATABASE ITPark


CREATE DATABASE ITPark
GO

USE ITPark
GO

DROP TABLE IF EXISTS users;
DROP TABLE IF EXISTS nomenclature
GO

CREATE TABLE users
(
	id_user int PRIMARY KEY IDENTITY NOT NULL,
	login nvarchar(150) UNIQUE NOT NULL,
	pass nvarchar(150) NOT NULL
)
GO

CREATE TABLE nomenclature
(
	id_nomenclature int PRIMARY KEY IDENTITY NOT NULL,
	name nvarchar(150) NOT NULL,
	price numeric(18,2) NOT NULL,
	fromDate date NOT NULL,
	toDate date NOT NULL
)
GO

INSERT INTO users VALUES
	('user1', 'pass1'),
	('user2', 'pass2')
GO

INSERT INTO nomenclature VALUES
	('apple', 150.45, '2019-10-26', '2019-12-31'),
	('avocado', 300.49, '2019-06-02', '2020-01-15'),
	('kiwi', 40.55, '2019-08-10', '2019-11-18')
GO

--SELECT * FROM users
--SELECT * FROM nomenclature

CREATE OR ALTER FUNCTION sel_users()
RETURNS TABLE
AS
RETURN
(
	SELECT login Login, pass Pass FROM users
)
GO

SELECT * FROM sel_users()
GO


CREATE OR ALTER FUNCTION check_authentication(@login nvarchar(150), @pass nvarchar(150))
RETURNS INT 
AS
BEGIN	
	DECLARE @count INT	
	DECLARE @t TABLE(loginT nvarchar(150), passT nvarchar(150))
	
	INSERT INTO @t 
	SELECT login, pass FROM users WHERE login = @login AND pass = @pass

	SELECT @count = COUNT(*) FROM @t

	IF @count = 0 RETURN 0
	
	RETURN 1 
END;
GO

--SELECT dbo.check_authentication('user1', 'pass1') AS Result
--GO

CREATE OR ALTER FUNCTION sel_nomenclature()
RETURNS TABLE
AS
RETURN
(
	SELECT name, price, fromDate, toDate FROM nomenclature
)
GO

--SELECT * FROM sel_nomenclature()
--GO





------- опнжедспю

CREATE OR ALTER PROCEDURE iud_nomenclature
	@flag nchar(1),
	@id_nomenclature int = NULL,
	@name nvarchar(150) = NULL,
	@price numeric(18,2) = NULL,
	@fromDate date = NULL,
	@toDate date = NULL	
AS
BEGIN

IF (@flag = 'I')
	BEGIN
		INSERT INTO nomenclature 
			VALUES(@name, @price, @fromDate, @toDate)
	END
ELSE IF (@flag = 'U')
	BEGIN
		UPDATE nomenclature
		SET name = @name, price = @price, fromDate = @fromDate, toDate = @toDate
		WHERE id_nomenclature = @id_nomenclature
	END
ELSE IF @flag = 'D'
	BEGIN
		DELETE 
		FROM nomenclature
		WHERE id_nomenclature = @id_nomenclature
	END
END
			



--EXEC iud_nomenclature 'I', DEFAULT, 'test5', 45.99, '2019-05-01', '2019-07-18'
--SELECT * FROM nomenclature

--EXEC iud_nomenclature 'U', 1, 'BANANA', 99.99, '2030-05-01', '2019-07-18'
--SELECT * FROM nomenclature

--EXEC iud_nomenclature 'D', 1
--SELECT * FROM nomenclature
