using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sporta.Data.Models
{
    [Keyless]
    public class GetResultDetailByResultId_Result
    {
        public string UserName { get; set; }
        public int TotalQuestions { get; set; }
        public int Attempted { get; set; }
        public int Score { get; set; }
        public string CategoryName { get; set; }

        public string Name { get; set; }
        public string Branch { get; set; }
        public string ContactNumber { get; set; }
    
        public string AllotedTime { get; set; }
       
        public long TimeTaken { get; set; }
      
        public int FocusLost { get; set; }
    }
}
