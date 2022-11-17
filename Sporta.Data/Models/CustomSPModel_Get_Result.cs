using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sporta.Data.Models
{
    [Keyless]
    public class CustomSPModel_Get_Result
    {

        public int ID { get; set; }

        public string Name { get; set; }
    }
}
