using DVLD.DataAccessLayer;
using DVLD.EntityLayer;
using System;
using System.Data;

namespace DVLD.BusinessLayer
{
    public class DetainLicenseService
    {
        public DetainLicense Record;

        public DetainLicenseService(DetainLicense record)
        {
            Record = record ?? throw new ArgumentNullException(nameof(record));
        }

        // Static Methods
        public static int DetainLicense(License license, decimal fees, int createdByUserId)
        {
            if (license == null)
                throw new ArgumentNullException(nameof(license));

            var detainRecord = new DetainLicense(
                   licenseId: license.Id,
                   detainDate: DateTime.Now,
                   fineFees: fees,
                   createdByUserId: createdByUserId
               );

            var service = new DetainLicenseService(detainRecord);
            return service.Save() ? detainRecord.Id : -1;
        }

        public static DataTable GetAll() => DetainLicenseData.GetAllAsTable();

        public static DetainLicense FindById(int detainId) => DetainLicenseData.GetById(detainId);

        public static DetainLicense FindByLicenseId(int licenseId) => DetainLicenseData.GetByLicenseId(licenseId);

        public static bool ExistsById(int detainId) => DetainLicenseData.ExistsById(detainId);

        public static bool ExistsByLicenseId(int licenseId) => DetainLicenseData.ExistsByLicenseId(licenseId);

        // Instance methods
        public bool Save()
        {
            if (!Record.IsValid)
                return false;

            if (Record.IsNew)
            {
                Record.Id = DetainLicenseData.Add(Record);
                return !Record.IsNew;
            }

            // When updating, we pass only release info
            return DetainLicenseData.UpdateReleaseInfo(Record);
        }
    }
}
