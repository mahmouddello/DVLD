using System;
using DVLD.DataAccessLayer;
using DVLD.EntityLayer;

namespace DVLD.BusinessLayer
{
    public class ApplicationService
    {
        public Application Info { get; private set; }

        public ApplicationService(Application app)
        {
            Info = app ?? throw new ArgumentNullException(nameof(app));
        }

        // ── Static Lookups ────────────────────────────────
        public static bool ExistsById(int id) => ApplicationData.ExistsById(id);

        public static bool Delete(int id) => ApplicationData.Delete(id);

        public static Application FindById(int id)
        {
            var app = ApplicationData.GetById(id);

            if (app == null)
                return null;

            ResolveNavigationProperties(app);
            return app;
        }

        private static void ResolveNavigationProperties(Application app)
        {
            app.ApplicantPersonInfo = PersonService.FindById(app.ApplicantPersonId);
            app.ApplicationTypeInfo = ApplicationTypeService.FindById(app.ApplicationTypeId);
            app.CreatorUserInfo = UserService.FindById(app.CreatedByUserId);
        }

        // ── Instance Methods ──────────────────────────────
        public bool Save()
        {
            if (Info.ApplicantPersonId == -1) return false;
            if (Info.ApplicationTypeId == -1) return false;
            if (Info.CreatedByUserId == -1) return false;
            if (Info.PaidFees < 0) return false;

            if (Info.IsNew)
            {
                Info.Id = ApplicationData.Add(Info);
                return !Info.IsNew;
            }

            return ApplicationData.Update(Info);
        }

        public bool Cancel()
        {
            if (Info.IsCompleted) return false;
            if (Info.IsCancelled) return false;

            bool updated = ApplicationData.UpdateStatus(Info.Id, enApplicationStatus.Cancelled);

            if (updated)
                Info.Status = enApplicationStatus.Cancelled;

            return updated;
        }

        public bool Complete()
        {
            if (Info.IsCancelled) return false;
            if (Info.IsCompleted) return false;

            bool updated = ApplicationData.UpdateStatus(Info.Id, enApplicationStatus.Completed);

            if (updated)
                Info.Status = enApplicationStatus.Completed;

            return updated;
        }

        public static bool HasSameClassApplication(int applicantId, int licenseClassId)
        {
            return ApplicationData.ExistsSameClassApplication(applicantId, licenseClassId);
        }

        public static bool MeetsMinimumAgeRequirement(int licenseClassId, int applicantId)
        {
            LicenseClass licenseClass = LicenseClassService.FindById(licenseClassId);
            Person applicantPerson = PersonService.FindById(applicantId);

            if (licenseClass == null || applicantPerson == null)
                return false;

            return applicantPerson.Age >= licenseClass.MinimumAllowedAge;
        }
    }
}
