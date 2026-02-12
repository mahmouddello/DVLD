using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD.DataAccessLayer;
using DVLD.EntityLayer;

namespace DVLD.BusinessLayer
{
    public static class ApplicationBusiness
    {
        public static Application Find(int applicationId)
        {
            DataRow row = ApplicationData.GetById(applicationId);

            if (row == null)
                return null;

            EntityLayer.Application application = new EntityLayer.Application(
                id: Convert.ToInt32(row["ApplicationID"]),
                applicantPersonId: Convert.ToInt32(row["ApplicantPersonID"]),
                date: Convert.ToDateTime(row["ApplicationDate"]),
                applicationTypeId: Convert.ToInt32(row["ApplicationTypeID"]),
                applicationstatus: (Application.ApplicationStatus)Convert.ToInt32(row["ApplicationStatus"]),
                lastApplicationStatusDate: Convert.ToDateTime(row["LastStatusDate"]),
                paidFees: Convert.ToDecimal(row["PaidFees"]),
                createdByUserId: Convert.ToInt32(row["CreatedByUserID"])
            );

            application.ApplicantPersonInfo = PersonBusiness.Find(application.ApplicantPersonId);
            application.ApplicationTypeInfo = ApplicationTypeBusiness.Find
            (
                (ApplicationType.enApplicationType)application.ApplicationTypeId
            );
            application.CreatorUserInfo = UserBusiness.Find(application.CreatedByUserId);

            return application;
        }

        private static bool Add(Application application)
        {
            application.Id = ApplicationData.InsertNew
            (
                application.ApplicantPersonId,
                application.Date,
                application.ApplicationTypeId,
                (int)application.Status,
                application.LastStatusDate,
                application.PaidFees,
                application.CreatedByUserId
            );

            if (application.Id != -1)
            {
                application.ApplicantPersonInfo = PersonBusiness.Find(application.ApplicantPersonId);
                application.ApplicationTypeInfo = ApplicationTypeBusiness.Find
                (
                    (ApplicationType.enApplicationType)application.ApplicationTypeId
                );
                application.CreatorUserInfo = UserBusiness.Find(application.CreatedByUserId);

                return true;
            }

            return false;
        }

        public static bool Save(Application application)
        {
            if (application.Id == -1)
                return Add(application);

            return false;
        }

        public static bool Cancel(Application application)
        {
            return ApplicationData.UpdateApplicationStatus(application.Id, Application.ApplicationStatus.Cancelled);
        }

        public static bool Delete(Application application)
        {
            bool linkedLocalDeleted = LocalDrivingLicenseApplicationBusiness.DeleteLocalApplicationByMainId(application.Id);

            return ApplicationData.DeleteById(application.Id) && linkedLocalDeleted;
        }

        public static bool HasSameClassApplication(int applicantId, int licenseClassId)
        {
            return ApplicationData.ExistsSameClassApplication(applicantId, licenseClassId);
        }

        public static bool MeetsMinimumAgeRequirement(int licenseClassId, int applicantId)
        {
            LicenseClass licenseClass = LicenseClassBusiness.Find(licenseClassId);
            Person applicantPerson = PersonBusiness.Find(applicantId);

            int personAge = DateTime.Now.Year - applicantPerson.DateOfBirth.Year;

            if (licenseClass == null || applicantPerson == null)
                return false;

            return personAge >= licenseClass.MinimumAllowedAge;
        }
    }
}
