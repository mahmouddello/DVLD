using System;

namespace DVLD.EntityLayer
{
    public class DetainLicense
    {
        public int Id { get; set; } = -1;
        public int LicenseId { get; set; } = -1;
        public DateTime DetainDate { get; set; } = DateTime.MinValue;
        public decimal FineFees { get; set; } = 0m;
        public int CreatedByUserId { get; set; } = -1;
        public bool IsReleased { get; set; } = false;
        public DateTime ReleasedDate { get; set; } = DateTime.MinValue;
        public int ReleasedByUserId { get; set; } = -1;
        public int ReleaseApplicationId { get; set; } = -1;

        // Helpers
        public bool IsNew => Id == -1;
        public bool IsValid =>
            LicenseId > 0 &&
            DetainDate != DateTime.MinValue &&
            FineFees >= 0 &&
            CreatedByUserId > 0 &&
            (!IsReleased || (
                ReleasedDate != DateTime.MinValue &&
                ReleasedByUserId > 0 &&
                ReleaseApplicationId > 0
            ));


        public DetainLicense()
        {

        }

        // Copy Constructor
        public DetainLicense(DetainLicense other)
        {
            Id = other.Id; 
            LicenseId = other.LicenseId;
            DetainDate = other.DetainDate;
            FineFees = other.FineFees;
            CreatedByUserId = other.CreatedByUserId;
            IsReleased = other.IsReleased;
            ReleasedDate = other.ReleasedDate;
            ReleasedByUserId = other.ReleasedByUserId;
            ReleaseApplicationId = other.ReleaseApplicationId;
        }

        // Detain Record 1.st parametrized constructor
        public DetainLicense(int licenseId, DateTime detainDate, decimal fineFees, int createdByUserId)
        {
            LicenseId = licenseId;
            DetainDate = detainDate;
            FineFees = fineFees;
            CreatedByUserId = createdByUserId;
        }

        public DetainLicense(int id, int licenseId, DateTime detainDate, decimal fineFees, int createdByUserId, bool isReleased, DateTime releasedDate, int releasedByUserId, int releaseApplicationId)
        {
            Id = id;
            LicenseId = licenseId;
            DetainDate = detainDate;
            FineFees = fineFees;
            CreatedByUserId = createdByUserId;
            IsReleased = isReleased;
            ReleasedDate = releasedDate;
            ReleasedByUserId = releasedByUserId;
            ReleaseApplicationId = releaseApplicationId;
        }
    }
}
