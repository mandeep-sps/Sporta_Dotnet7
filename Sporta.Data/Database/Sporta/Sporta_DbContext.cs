using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Sporta.Data.Database.Sporta
{
    public partial class Sporta_DbContext : DbContext
    {
        public Sporta_DbContext()
        {
        }

        public Sporta_DbContext(DbContextOptions<Sporta_DbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ApplicationRole> ApplicationRoles { get; set; }
        public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public virtual DbSet<Branch> Branches { get; set; }
        public virtual DbSet<Candidate> Candidates { get; set; }
        public virtual DbSet<CandidateStatus> CandidateStatuses { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Drive> Drives { get; set; }
        public virtual DbSet<DriveAssignee> DriveAssignees { get; set; }
        public virtual DbSet<DriveCategory> DriveCategories { get; set; }
        public virtual DbSet<ErrorLog> ErrorLogs { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Result> Results { get; set; }
        public virtual DbSet<ResultDetail> ResultDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<ApplicationRole>(entity =>
            {
                entity.ToTable("ApplicationRole");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable("ApplicationUser");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserPassword)
                    .IsRequired()
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.ApplicationRole)
                    .WithMany(p => p.ApplicationUsers)
                    .HasForeignKey(d => d.ApplicationRoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserRole");
            });

            modelBuilder.Entity<Branch>(entity =>
            {
                entity.ToTable("Branch");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.BranchName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            });

            modelBuilder.Entity<Candidate>(entity =>
            {
                entity.ToTable("Candidate");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Branch)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ContactNumber)
                    .IsRequired()
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.RollNo)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.CandidateStatus)
                    .WithMany(p => p.Candidates)
                    .HasForeignKey(d => d.CandidateStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Candidate_CandidateStatus");

                entity.HasOne(d => d.Drive)
                    .WithMany(p => p.Candidates)
                    .HasForeignKey(d => d.DriveId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Candidate_Drive");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.Candidate)
                    .HasForeignKey<Candidate>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Candidate_ApplicationUser");
            });

            modelBuilder.Entity<CandidateStatus>(entity =>
            {
                entity.ToTable("CandidateStatus");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.StatusName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            });

            modelBuilder.Entity<Drive>(entity =>
            {
                entity.ToTable("Drive");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DriveName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.Scheduled).HasColumnType("datetime");
            });

            modelBuilder.Entity<DriveAssignee>(entity =>
            {
                entity.ToTable("DriveAssignee");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.HasOne(d => d.ApplicationUser)
                    .WithMany(p => p.DriveAssignees)
                    .HasForeignKey(d => d.ApplicationUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DriveAssignee_User");

                entity.HasOne(d => d.Drive)
                    .WithMany(p => p.DriveAssignees)
                    .HasForeignKey(d => d.DriveId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DriveAssignee_Drive");
            });

            modelBuilder.Entity<DriveCategory>(entity =>
            {
                entity.Property(e => e.AllotedTime)
                    .IsRequired()
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.QuestionIds)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.DriveCategories)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DriveCategory_Category");

                entity.HasOne(d => d.Drive)
                    .WithMany(p => p.DriveCategories)
                    .HasForeignKey(d => d.DriveId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DriveCategory_Drive");
            });

            modelBuilder.Entity<ErrorLog>(entity =>
            {
                entity.ToTable("ErrorLog");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Information)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.LogTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.ToTable("Question");

                entity.Property(e => e.Answer)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.OptionA)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.OptionB)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.OptionC)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.OptionD)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Question1)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasColumnName("Question");

                entity.Property(e => e.QuestionLevel)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Question_Category");
            });

            modelBuilder.Entity<Result>(entity =>
            {
                entity.ToTable("Result");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Candidate)
                    .WithMany(p => p.Results)
                    .HasForeignKey(d => d.CandidateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Result_Candidate");
            });

            modelBuilder.Entity<ResultDetail>(entity =>
            {
                entity.ToTable("ResultDetail");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.ResultDetails)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ResultDetail_Category");

                entity.HasOne(d => d.Result)
                    .WithMany(p => p.ResultDetails)
                    .HasForeignKey(d => d.ResultId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ResultDetail_Result");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
