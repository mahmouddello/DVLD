using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.EntityLayer
{
    public class TestType
    {
        public int Id { get; set; } = -1;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Fees { get; set; } = 0;

        public TestType(int id, string title, string description, decimal fees)
        {
            this.Id = id;
            this.Title = title;
            this.Description = description;
            this.Fees = fees;
        }
    }
}
