USE DVLD;

PRINT 'Creating Person Master View'
GO

CREATE VIEW vw_PersonDetails AS
SELECT
	p.PersonID,
	p.NationalNo,
	p.FirstName,
	p.SecondName,
	p.ThirdName,
	p.LastName,
	p.DateOfBirth, 
	p.Gender,
	c.CountryName AS Nationality,
	p.Phone,
	p.Email,
	p.Address,
	p.ImagePath
FROM People AS p
INNER JOIN Countries AS c
	ON c.CountryID = p.NationalityCountryID;
GO
