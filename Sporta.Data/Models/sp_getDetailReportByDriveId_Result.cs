using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sporta.Data.Models
{
    [Keyless]
    public class sp_getDetailReportByDriveId_Result
    {
        public string ResultReport { get; set; }

    }
}
