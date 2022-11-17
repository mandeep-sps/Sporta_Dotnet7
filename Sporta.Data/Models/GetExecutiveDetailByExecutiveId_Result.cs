using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sporta.Data.Models
{
    [Keyless]
    public class GetExecutiveDetailByExecutiveId_Result
    {
        public int ApplicationUserId { get; set; }
        public string Name { get; set; }
        public int DriveId { get; set; }
        public string DriveName { get; set; }
        public int Enrolled { get; set; }
        public bool IsActive { get; set; }
        public DateTime Scheduled { get; set; }
        public int Token { get; set; }
       
    }
}
