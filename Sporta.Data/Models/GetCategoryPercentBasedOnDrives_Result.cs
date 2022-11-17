using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sporta.Data.Models
{
    [Keyless]
    public class GetCategoryPercentBasedOnDrives_Result
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int category_count { get; set; }
        public decimal category_percent { get; set; }

    }
}
