using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sporta.Data.Models
{
    [Keyless]
    public class GetDrivePercentByMonths_Result
    {
        public int month_date { get; set; }
        public int drive_count { get; set; }
        public decimal drive_percent { get; set; }
       
    }
}
