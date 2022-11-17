using System;
using System.Collections.Generic;

#nullable disable

namespace Sporta.Data.Database.Sporta
{
    public partial class ErrorLog
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public string Information { get; set; }
        public DateTime LogTime { get; set; }
    }
}
