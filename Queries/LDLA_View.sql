CREATE VIEW LDLA_View AS
(
SELECT 
    ldla.LocalDrivingLicenseApplicationID,
    lc.ClassName,
    p.NationalNo,
    CONCAT(
       p.FirstName, ' ',
       p.SecondName, ' ',
       ISNULL(p.ThirdName + ' ', ''),
       p.LastName
    ) AS FullName,
    a.ApplicationDate,
    ISNULL(t.PassedTests,0) AS PassedTests,
    CASE 
        WHEN a.ApplicationStatus = 1 THEN 'New'
        WHEN a.ApplicationStatus = 2 THEN 'Cancelled'
        WHEN a.ApplicationStatus = 3 THEN 'Completed'
    END AS [Status]
FROM LocalDrivingLicenseApplications ldla
INNER JOIN LicenseClasses lc 
    ON lc.LicenseClassID = ldla.LicenseClassID
INNER JOIN Applications a 
    ON a.ApplicationID = ldla.ApplicationID
INNER JOIN People p 
    ON p.PersonID = a.ApplicantPersonID
LEFT JOIN
(
    SELECT 
        ta.LocalDrivingLicenseApplicationID,
        COUNT(*) AS PassedTests
    FROM TestAppointments ta
    INNER JOIN Tests t 
        ON t.TestAppointmentID = ta.TestAppointmentID
    WHERE t.TestResult = 1
    GROUP BY ta.LocalDrivingLicenseApplicationID
) t
ON t.LocalDrivingLicenseApplicationID = ldla.LocalDrivingLicenseApplicationID)
