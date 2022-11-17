using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sporta.Data.Database.Sporta
{
    public interface ISportaAudit
    {
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
    }

    public partial class ApplicationRole : ISportaAudit { }
    public partial class ApplicationUser : ISportaAudit { }
    public partial class Candidate : ISportaAudit { }
    public partial class CandidateStatus : ISportaAudit { }
    public partial class Category : ISportaAudit { }
    public partial class Drive : ISportaAudit { }
    public partial class DriveAssignee : ISportaAudit { }
    public partial class DriveCategory : ISportaAudit { }
    public partial class Question : ISportaAudit { }
    public partial class Result : ISportaAudit { }
    public partial class ResultDetail : ISportaAudit { }

}
