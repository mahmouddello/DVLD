using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.EntityLayer
{
    public class ApplicationType
    {
        public int Id { get; } = -1;
        public string Title { get; set; } = string.Empty;
        public decimal Fees { get; set; } = 0;

        public ApplicationType(int id, string title, decimal fees)
        {
            this.Id = id;
            this.Title = title;
            this.Fees = fees;
        }
    }
}
