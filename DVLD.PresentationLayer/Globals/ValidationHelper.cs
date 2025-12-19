using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DVLD.BusinessLayer;

namespace DVLD.PresentationLayer
{
    internal class ValidationHelper
    {
        public static bool IsValidEmail(string email)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);

            return match.Success;
        }

        public static bool IsUniqueNationalNo(string nationalNo)
        {
            return !PersonBusiness.ExistsByNationalNo(nationalNo);
        }
    }
}