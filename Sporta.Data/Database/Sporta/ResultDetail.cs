using System;
using System.Collections.Generic;

#nullable disable

namespace Sporta.Data.Database.Sporta
{
    public partial class ResultDetail
    {
        public int Id { get; set; }
        public int ResultId { get; set; }
        public int CategoryId { get; set; }
        public int TotalQuestions { get; set; }
        public int Attempted { get; set; }
        public int Score { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsAttempted { get; set; }
        public int FocusLost { get; set; }
        public long TimeTaken { get; set; }

        public virtual Category Category { get; set; }
        public virtual Result Result { get; set; }
    }
}
