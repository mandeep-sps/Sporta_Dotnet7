using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sporta.Data.Models
{
    [Keyless]
    public class GetCountByDate_Result
    {
        public int TodayCount { get; set; }
        public int YesterDayCount { get; set; }
        public int Last7DaysCount { get; set; }
        public int Last30DaysCount { get; set; }
        public int Last90DaysCount { get; set; }
    }
}
