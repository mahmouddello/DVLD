USE DVLD;
GO

PRINT 'Creating People Master View';
GO

CREATE VIEW vw_PeopleDetails
AS
SELECT
    p.PersonID,
    p.NationalNo,
    p.FirstName,
    p.SecondName,
    p.ThirdName,
    p.LastName,
    p.DateOfBirth,
    CASE p.Gender
        WHEN 0 THEN 'Male'
        WHEN 1 THEN 'Female'
        ELSE 'Unknown'
    END AS Gender,
    c.CountryName AS Nationality,
    p.Phone,
    p.Email
FROM People AS p
INNER JOIN Countries AS c
    ON c.CountryID = p.NationalityCountryID;
GO
