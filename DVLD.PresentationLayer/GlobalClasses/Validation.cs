using DVLD.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DVLD.PresentationLayer.GlobalClasses
{
    public static class Validation
    {
        public static bool IsValidEmail(string email)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);

            return match.Success;
        }

        public static bool IsUniqueNationalNo(string nationalNo)
        {
            return !PersonBusiness.IsExists(nationalNo);
        }
    }
}
