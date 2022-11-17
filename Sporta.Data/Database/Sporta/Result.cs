using System;
using System.Collections.Generic;

#nullable disable

namespace Sporta.Data.Database.Sporta
{
    public partial class Result
    {
        public Result()
        {
            ResultDetails = new HashSet<ResultDetail>();
        }

        public int Id { get; set; }
        public int CandidateId { get; set; }
        public int TotalQuestions { get; set; }
        public int Attempted { get; set; }
        public int Score { get; set; }
        public bool IsPass { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Candidate Candidate { get; set; }
        public virtual ICollection<ResultDetail> ResultDetails { get; set; }
    }
}
