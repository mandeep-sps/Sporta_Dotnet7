using Microsoft.EntityFrameworkCore;
using Sporta.Data.Database.Sporta;
using Sporta.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sporta.Data.CustomSql
{
    public class StoredProcedureClass : Sporta_DbContext
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public StoredProcedureClass(DbContextOptions<Sporta_DbContext> options)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
            : base(options)
        {
        }

        public virtual DbSet<CustomSPModel_Get_Result> CustomSPModel_Get_Result { get; set; }
        public virtual DbSet<GetResultByDrive_Result> GetResultByDrive_Result { get; set; }
        public virtual DbSet<GetResultDetailByResultId_Result> GetResultDetailByResultId_Result { get; set; }
        public virtual DbSet<sp_getDetailReportByDriveId_Result> sp_getDetailReportByDriveId_Result { get; set; }
        public virtual DbSet<GetExecutivesDetailsByDriveId_Result> GetExecutivesDetailsByDriveId_Result { get; set; }
        public virtual DbSet<GetActiveExecutivesDetails_Result> GetActiveExecutivesDetails_Result { get; set; }
        public virtual DbSet<GetExecutiveDetailByExecutiveId_Result> GetExecutiveDetailByExecutiveId_Result { get; set; }
        public virtual DbSet<GetTotalCountByExecutiveID_Result> GetTotalCountByExecutiveID_Result { get; set; }
        public virtual DbSet<GetCategoryPercentBasedOnDrives_Result> GetCategoryPercentBasedOnDrives_Result { get; set; }
        public virtual DbSet<GetDrivePercentByMonths_Result> GetDrivePercentByMonths_Result { get; set; }
        public virtual DbSet<GetCountByDate_Result> GetCountByDate_Result { get; set; }
    }
}