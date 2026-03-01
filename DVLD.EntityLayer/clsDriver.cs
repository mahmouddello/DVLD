using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.EntityLayer
{
    public class clsDriver
    {
        public int Id { get; set; } = -1;
        public int PersonId { get; set; } = -1;
        public int CreatedByUserId { get; set; } = -1;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // default constructor
        public clsDriver()
        {

        }

        // parametrized constructor
        public clsDriver(int id,  int personId, int createdByUserId, DateTime createdAt)
        {
            this.Id = id;
            this.PersonId = personId;
            this.CreatedByUserId = createdByUserId;
            this.CreatedAt = createdAt;
        }
    }
}
