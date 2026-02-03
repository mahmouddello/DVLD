using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.EntityLayer
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Country()
        {

        }

        public Country(int countryId, string countryName)
        {
            this.Id = countryId;
            this.Name = countryName;
        }
    }
}
