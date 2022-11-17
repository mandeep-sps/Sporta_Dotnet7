using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sporta.Data.Models
{
    [Keyless]
    public  class GetResultByDrive_Result
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string RollNo { get; set; }
        public string Branch { get; set; }
        public string DriveName { get; set; }
        public string ContactNumber { get; internal set; }
        public int TotalQuestions { get; set; }
        public int Attempted { get; set; }
        public int Score { get; set; }
        public bool IsPass { get; set; }
        public string StatusName { get; set; }
    }
}
