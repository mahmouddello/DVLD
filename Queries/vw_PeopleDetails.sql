USE DVLD;

CREATE VIEW vw_PeopleDetails AS
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
       c.CountryName AS Nationality,
       p.Phone,
       p.Email
       FROM People p
       INNER JOIN Countries c
       ON c.CountryID = p.NationalityCountryID;