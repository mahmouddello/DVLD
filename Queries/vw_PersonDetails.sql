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
       Gender =
                CASE 
                    WHEN p.Gendor = 0 THEN 'Male'
                    WHEN p.Gendor = 1 THEN 'Female'
                    ELSE 'Unknown'
                 END,
       p.Address,
       c.CountryName AS Nationality,
       p.Phone,
       p.Email,
       p.ImagePath
       FROM People p
       INNER JOIN Countries c
       ON c.CountryID = p.NationalityCountryID;
GO