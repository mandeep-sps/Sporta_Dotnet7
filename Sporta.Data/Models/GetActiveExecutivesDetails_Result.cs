using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sporta.Data.Models
{
    [Keyless]
    public class GetActiveExecutivesDetails_Result
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string DriveName { get; set; }

    }
}
