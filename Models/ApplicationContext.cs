using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace VipcoTraining.Models
{
    public partial class ApplicationContext : DbContext
    {
        public virtual DbSet<TblAttachFile> TblAttachFile { get; set; }
        public virtual DbSet<TblBasicCourse> TblBasicCourse { get; set; }
        public virtual DbSet<TblCourseHasAttach> TblCourseHasAttach { get; set; }
        public virtual DbSet<TblDepartment> TblDepartment { get; set; }
        public virtual DbSet<TblDivision> TblDivision { get; set; }
        public virtual DbSet<TblEducation> TblEducation { get; set; }
        public virtual DbSet<TblEmployee> TblEmployee { get; set; }
        public virtual DbSet<TblEmpType> TblEmpType { get; set; }
        public virtual DbSet<TblGroupName> TblGroupName { get; set; }
        public virtual DbSet<TblLocation> TblLocation { get; set; }
        public virtual DbSet<TblLoginName> TblLoginName { get; set; }
        public virtual DbSet<TblPlace> TblPlace { get; set; }
        public virtual DbSet<TblPosition> TblPosition { get; set; }
        public virtual DbSet<TblSection> TblSection { get; set; }
        public virtual DbSet<TblStandardCourse> TblStandardCourse { get; set; }
        public virtual DbSet<TblSupplementCourse> TblSupplementCourse { get; set; }
        public virtual DbSet<TblTrainEmployee> TblTrainEmployee { get; set; }
        public virtual DbSet<TblTrainingCousre> TblTrainingCousre { get; set; }
        public virtual DbSet<TblTrainingDetail> TblTrainingDetail { get; set; }
        public virtual DbSet<TblTrainingLevel> TblTrainingLevel { get; set; }
        public virtual DbSet<TblTrainingMaster> TblTrainingMaster { get; set; }
        public virtual DbSet<TblTrainingMasterHasPlace> TblTrainingMasterHasPlace { get; set; }
        public virtual DbSet<TblTrainingPlanningDetail> TblTrainingPlanningDetail { get; set; }
        public virtual DbSet<TblTrainingPlanningMaster> TblTrainingPlanningMaster { get; set; }
        public virtual DbSet<TblTrainingProgramHasGroup> TblTrainingProgramHasGroup { get; set; }
        public virtual DbSet<TblTrainingProgramHasLevel> TblTrainingProgramHasLevel { get; set; }
        public virtual DbSet<TblTrainingProgramHasPosition> TblTrainingProgramHasPosition { get; set; }
        public virtual DbSet<TblTrainingPrograms> TblTrainingPrograms { get; set; }
        public virtual DbSet<TblTrainingRequestDetail> TblTrainingRequestDetail { get; set; }
        public virtual DbSet<TblTrainingRequestMaster> TblTrainingRequestMaster { get; set; }
        public virtual DbSet<TblTrainingType> TblTrainingType { get; set; }
        public virtual DbSet<TblTrainning> TblTrainning { get; set; }
        public virtual DbSet<TblTrainObJect> TblTrainObJect { get; set; }
        public virtual DbSet<TblTrainType> TblTrainType { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TblAttachFile>(entity =>
            {
                entity.HasKey(e => e.AttactId);

                entity.ToTable("tblAttachFile", "Training");

                entity.Property(e => e.AttactId).HasColumnName("AttactID");

                entity.Property(e => e.AttachAddress).HasMaxLength(250);

                entity.Property(e => e.AttachFileName).HasMaxLength(50);

                entity.Property(e => e.Creator).HasMaxLength(25);

                entity.Property(e => e.Modifyer).HasMaxLength(25);
            });

            modelBuilder.Entity<TblBasicCourse>(entity =>
            {
                entity.HasKey(e => e.BasicCourseId);

                entity.ToTable("tblBasicCourse", "Training");

                entity.Property(e => e.Creator).HasMaxLength(50);

                entity.Property(e => e.Modifyer).HasMaxLength(50);

                entity.HasOne(d => d.TrainingCousre)
                    .WithMany(p => p.TblBasicCourse)
                    .HasForeignKey(d => d.TrainingCousreId)
                    .HasConstraintName("FK_tblBasicCourse_tblTrainingCousre");

                entity.HasOne(d => d.TrainingProgram)
                    .WithMany(p => p.TblBasicCourse)
                    .HasForeignKey(d => d.TrainingProgramId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_tblBasicCourse_tblTrainingPrograms");
            });

            modelBuilder.Entity<TblCourseHasAttach>(entity =>
            {
                entity.HasKey(e => e.CourseHasAttachId);

                entity.ToTable("tblCourseHasAttach", "Training");

                entity.Property(e => e.CourseHasAttachId).HasColumnName("CourseHasAttachID");

                entity.Property(e => e.AttactId).HasColumnName("AttactID");

                entity.Property(e => e.Creator).HasMaxLength(25);

                entity.Property(e => e.Modifyer).HasMaxLength(25);

                entity.HasOne(d => d.Attact)
                    .WithMany(p => p.TblCourseHasAttach)
                    .HasForeignKey(d => d.AttactId)
                    .HasConstraintName("FK_tblCourseHasAttach_tblAttachFile");

                entity.HasOne(d => d.TrainingCousre)
                    .WithMany(p => p.TblCourseHasAttach)
                    .HasForeignKey(d => d.TrainingCousreId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_tblCourseHasAttach_tblTrainingCousre");
            });

            modelBuilder.Entity<TblDepartment>(entity =>
            {
                entity.HasKey(e => e.DeptCode);

                entity.ToTable("tblDepartment");

                entity.Property(e => e.DeptCode)
                    .HasMaxLength(20)
                    .ValueGeneratedNever();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Creator).HasMaxLength(25);

                entity.Property(e => e.DeptEname)
                    .HasColumnName("DeptEName")
                    .HasMaxLength(80);

                entity.Property(e => e.DeptName).HasMaxLength(80);

                entity.Property(e => e.DeptRemark).HasMaxLength(80);

                entity.Property(e => e.ModifyDate).HasColumnType("datetime");

                entity.Property(e => e.Modifyer).HasMaxLength(25);
            });

            modelBuilder.Entity<TblDivision>(entity =>
            {
                entity.HasKey(e => e.DivisionCode);

                entity.ToTable("tblDivision");

                entity.Property(e => e.DivisionCode)
                    .HasMaxLength(10)
                    .ValueGeneratedNever();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Creator).HasMaxLength(25);

                entity.Property(e => e.DivisionEname)
                    .HasColumnName("DivisionEName")
                    .HasMaxLength(80);

                entity.Property(e => e.DivisionName).HasMaxLength(80);

                entity.Property(e => e.DivisionRemark).HasMaxLength(80);

                entity.Property(e => e.ModifyDate).HasColumnType("datetime");

                entity.Property(e => e.Modifyer).HasMaxLength(25);
            });

            modelBuilder.Entity<TblEducation>(entity =>
            {
                entity.HasKey(e => e.EducationId);

                entity.ToTable("tblEducation", "Training");

                entity.Property(e => e.Creator).HasMaxLength(50);

                entity.Property(e => e.Detail).HasMaxLength(200);

                entity.Property(e => e.EducationName).HasMaxLength(200);

                entity.Property(e => e.Modifyer).HasMaxLength(50);
            });

            modelBuilder.Entity<TblEmployee>(entity =>
            {
                entity.HasKey(e => e.EmpCode);

                entity.ToTable("tblEmployee");

                entity.Property(e => e.EmpCode)
                    .HasMaxLength(20)
                    .ValueGeneratedNever();

                entity.Property(e => e.AreaId)
                    .HasColumnName("AreaID")
                    .HasMaxLength(20);

                entity.Property(e => e.BeginDate).HasColumnType("datetime");

                entity.Property(e => e.DeptCode).HasMaxLength(20);

                entity.Property(e => e.DivisionCode).HasMaxLength(10);

                entity.Property(e => e.EduBranch)
                    .HasColumnName("Edu_Branch")
                    .HasMaxLength(50);

                entity.Property(e => e.EduCertificate)
                    .HasColumnName("Edu_Certificate")
                    .HasMaxLength(50);

                entity.Property(e => e.EduInstitute)
                    .HasColumnName("Edu_Institute")
                    .HasMaxLength(80);

                entity.Property(e => e.EduLevel)
                    .HasColumnName("Edu_Level")
                    .HasMaxLength(50);

                entity.Property(e => e.EmpCard)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.EmpPict).HasColumnType("image");

                entity.Property(e => e.EmptypeCode).HasMaxLength(20);

                entity.Property(e => e.GroupCode).HasMaxLength(20);

                entity.Property(e => e.GroupId)
                    .HasColumnName("GroupID")
                    .HasMaxLength(10);

                entity.Property(e => e.GroupMis)
                    .HasColumnName("GroupMIS")
                    .HasMaxLength(20);

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Jdno)
                    .HasColumnName("JDNo")
                    .HasMaxLength(10);

                entity.Property(e => e.LevelCode).HasMaxLength(20);

                entity.Property(e => e.LevelId)
                    .HasColumnName("Level_ID")
                    .HasMaxLength(10);

                entity.Property(e => e.LocateId)
                    .HasColumnName("LocateID")
                    .HasMaxLength(20);

                entity.Property(e => e.NameEng).HasMaxLength(100);

                entity.Property(e => e.NameThai).HasMaxLength(100);

                entity.Property(e => e.NickName).HasMaxLength(20);

                entity.Property(e => e.OutDate).HasColumnType("datetime");

                entity.Property(e => e.PositionCode).HasMaxLength(20);

                entity.Property(e => e.ResidentDate).HasColumnType("datetime");

                entity.Property(e => e.SectionCode).HasMaxLength(10);

                entity.Property(e => e.Sex).HasMaxLength(6);

                entity.Property(e => e.Title).HasMaxLength(10);

                entity.Property(e => e.WorkId)
                    .HasColumnName("Work_ID")
                    .HasMaxLength(20);

                entity.HasOne(d => d.DeptCodeNavigation)
                    .WithMany(p => p.TblEmployee)
                    .HasForeignKey(d => d.DeptCode)
                    .HasConstraintName("FK_tblEmployee_tblDepartment");

                entity.HasOne(d => d.DivisionCodeNavigation)
                    .WithMany(p => p.TblEmployee)
                    .HasForeignKey(d => d.DivisionCode)
                    .HasConstraintName("FK_tblEmployee_tblDivision");

                entity.HasOne(d => d.EmptypeCodeNavigation)
                    .WithMany(p => p.TblEmployee)
                    .HasForeignKey(d => d.EmptypeCode)
                    .HasConstraintName("FK_tblEmployee_tblEmpType");

                entity.HasOne(d => d.GroupCodeNavigation)
                    .WithMany(p => p.TblEmployee)
                    .HasForeignKey(d => d.GroupCode)
                    .HasConstraintName("FK_tblEmployee_tblGroupName");

                entity.HasOne(d => d.Locate)
                    .WithMany(p => p.TblEmployee)
                    .HasForeignKey(d => d.LocateId)
                    .HasConstraintName("FK_tblEmployee_tblLocation");

                entity.HasOne(d => d.PositionCodeNavigation)
                    .WithMany(p => p.TblEmployee)
                    .HasForeignKey(d => d.PositionCode)
                    .HasConstraintName("FK_tblEmployee_tblPosition");

                entity.HasOne(d => d.SectionCodeNavigation)
                    .WithMany(p => p.TblEmployee)
                    .HasForeignKey(d => d.SectionCode)
                    .HasConstraintName("FK_tblEmployee_tblSection");
            });

            modelBuilder.Entity<TblEmpType>(entity =>
            {
                entity.HasKey(e => e.EmpTypeCode);

                entity.ToTable("tblEmpType");

                entity.Property(e => e.EmpTypeCode)
                    .HasMaxLength(20)
                    .ValueGeneratedNever();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Creator).HasMaxLength(25);

                entity.Property(e => e.EmpTypeDesc).HasMaxLength(50);

                entity.Property(e => e.EmpTypeRemark).HasMaxLength(50);

                entity.Property(e => e.ModifyDate).HasColumnType("datetime");

                entity.Property(e => e.Modifyer).HasMaxLength(25);
            });

            modelBuilder.Entity<TblGroupName>(entity =>
            {
                entity.HasKey(e => e.GroupCode);

                entity.ToTable("tblGroupName");

                entity.Property(e => e.GroupCode)
                    .HasMaxLength(20)
                    .ValueGeneratedNever();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Creator).HasMaxLength(25);

                entity.Property(e => e.GroupDesc).HasMaxLength(80);

                entity.Property(e => e.GroupEdesc)
                    .HasColumnName("GroupEDesc")
                    .HasMaxLength(80);

                entity.Property(e => e.GroupRemark).HasMaxLength(80);

                entity.Property(e => e.ModifyDate).HasColumnType("datetime");

                entity.Property(e => e.Modifyer).HasMaxLength(25);
            });

            modelBuilder.Entity<TblLocation>(entity =>
            {
                entity.HasKey(e => e.LocateId);

                entity.ToTable("tblLocation");

                entity.Property(e => e.LocateId)
                    .HasColumnName("LocateID")
                    .HasMaxLength(20)
                    .ValueGeneratedNever();

                entity.Property(e => e.Address).HasMaxLength(40);

                entity.Property(e => e.Amphur).HasMaxLength(30);

                entity.Property(e => e.Company).HasMaxLength(100);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Creator).HasMaxLength(25);

                entity.Property(e => e.District).HasMaxLength(30);

                entity.Property(e => e.LocateDesc).HasMaxLength(50);

                entity.Property(e => e.ModifyDate).HasColumnType("datetime");

                entity.Property(e => e.Modifyer).HasMaxLength(25);

                entity.Property(e => e.Post).HasMaxLength(10);

                entity.Property(e => e.Province).HasMaxLength(30);

                entity.Property(e => e.Remark).HasMaxLength(50);

                entity.Property(e => e.Soil).HasMaxLength(30);

                entity.Property(e => e.Street).HasMaxLength(30);

                entity.Property(e => e.Telephone).HasMaxLength(30);
            });

            modelBuilder.Entity<TblLoginName>(entity =>
            {
                entity.HasKey(e => e.UserName);

                entity.ToTable("tblLoginName");

                entity.Property(e => e.UserName)
                    .HasMaxLength(25)
                    .ValueGeneratedNever();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Creator).HasMaxLength(25);

                entity.Property(e => e.EmpCode).HasMaxLength(20);

                entity.Property(e => e.ModifyDate).HasColumnType("datetime");

                entity.Property(e => e.Modifyer).HasMaxLength(25);

                entity.Property(e => e.Password).HasMaxLength(25);
            });

            modelBuilder.Entity<TblPlace>(entity =>
            {
                entity.HasKey(e => e.PlaceId);

                entity.ToTable("tblPlace", "Training");

                entity.Property(e => e.PlaceId).HasColumnName("PlaceID");

                entity.Property(e => e.Creator).HasMaxLength(25);

                entity.Property(e => e.Modifyer).HasMaxLength(25);

                entity.Property(e => e.PlaceName)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<TblPosition>(entity =>
            {
                entity.HasKey(e => e.PositionCode);

                entity.ToTable("tblPosition");

                entity.Property(e => e.PositionCode)
                    .HasMaxLength(20)
                    .ValueGeneratedNever();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Creator).HasMaxLength(25);

                entity.Property(e => e.ModifyDate).HasColumnType("datetime");

                entity.Property(e => e.Modifyer).HasMaxLength(25);

                entity.Property(e => e.PositionEname)
                    .HasColumnName("PositionEName")
                    .HasMaxLength(50);

                entity.Property(e => e.PositionName).HasMaxLength(50);

                entity.Property(e => e.PositionRemark).HasMaxLength(50);
            });

            modelBuilder.Entity<TblSection>(entity =>
            {
                entity.HasKey(e => e.SectionCode);

                entity.ToTable("tblSection");

                entity.Property(e => e.SectionCode)
                    .HasMaxLength(10)
                    .ValueGeneratedNever();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Creator).HasMaxLength(25);

                entity.Property(e => e.ModifyDate).HasColumnType("datetime");

                entity.Property(e => e.Modifyer).HasMaxLength(25);

                entity.Property(e => e.SectionEname)
                    .HasColumnName("SectionEName")
                    .HasMaxLength(80);

                entity.Property(e => e.SectionName).HasMaxLength(80);

                entity.Property(e => e.SectionRemark).HasMaxLength(80);
            });

            modelBuilder.Entity<TblStandardCourse>(entity =>
            {
                entity.HasKey(e => e.StandardCourseId);

                entity.ToTable("tblStandardCourse", "Training");

                entity.Property(e => e.Creator).HasMaxLength(50);

                entity.Property(e => e.Modifyer).HasMaxLength(50);

                entity.HasOne(d => d.TrainingCousre)
                    .WithMany(p => p.TblStandardCourse)
                    .HasForeignKey(d => d.TrainingCousreId)
                    .HasConstraintName("FK_tblStandardCourse_tblTrainingCousre");

                entity.HasOne(d => d.TrainingProgram)
                    .WithMany(p => p.TblStandardCourse)
                    .HasForeignKey(d => d.TrainingProgramId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_tblStandardCourse_tblTrainingPrograms");
            });

            modelBuilder.Entity<TblSupplementCourse>(entity =>
            {
                entity.HasKey(e => e.SupplermentCourseId);

                entity.ToTable("tblSupplementCourse", "Training");

                entity.Property(e => e.Creator).HasMaxLength(50);

                entity.Property(e => e.Modifyer).HasMaxLength(50);

                entity.HasOne(d => d.TrainingCousre)
                    .WithMany(p => p.TblSupplementCourse)
                    .HasForeignKey(d => d.TrainingCousreId)
                    .HasConstraintName("FK_tblSupplementCourse_tblTrainingCousre");

                entity.HasOne(d => d.TrainingProgram)
                    .WithMany(p => p.TblSupplementCourse)
                    .HasForeignKey(d => d.TrainingProgramId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_tblSupplementCourse_tblTrainingPrograms");
            });

            modelBuilder.Entity<TblTrainEmployee>(entity =>
            {
                entity.HasKey(e => new { e.TrainCode, e.EmpCode });

                entity.Property(e => e.TrainCode).HasMaxLength(20);

                entity.Property(e => e.EmpCode).HasMaxLength(20);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Creator).HasMaxLength(25);

                entity.Property(e => e.DeptCode).HasMaxLength(20);

                entity.Property(e => e.DivisionCode).HasMaxLength(20);

                entity.Property(e => e.EmptypeCode).HasMaxLength(20);

                entity.Property(e => e.GroupCode).HasMaxLength(20);

                entity.Property(e => e.LocateId)
                    .HasColumnName("LocateID")
                    .HasMaxLength(20);

                entity.Property(e => e.ModifyDate).HasColumnType("datetime");

                entity.Property(e => e.Modifyer).HasMaxLength(25);

                entity.Property(e => e.PositionCode).HasMaxLength(20);

                entity.Property(e => e.Remark).HasMaxLength(50);

                entity.Property(e => e.SectionCode).HasMaxLength(20);
            });

            modelBuilder.Entity<TblTrainingCousre>(entity =>
            {
                entity.HasKey(e => e.TrainingCousreId);

                entity.ToTable("tblTrainingCousre", "Training");

                entity.Property(e => e.Creator).HasMaxLength(50);

                entity.Property(e => e.Detail).HasMaxLength(200);

                entity.Property(e => e.Modifyer).HasMaxLength(50);

                entity.Property(e => e.Remark).HasMaxLength(200);

                entity.Property(e => e.TrainingCousreCode).HasMaxLength(50);

                entity.Property(e => e.TrainingCousreName).HasMaxLength(200);

                entity.HasOne(d => d.EducationRequirement)
                    .WithMany(p => p.TblTrainingCousre)
                    .HasForeignKey(d => d.EducationRequirementId)
                    .HasConstraintName("FK_tblTrainingCousre_tblEducation");

                entity.HasOne(d => d.TrainingLevel)
                    .WithMany(p => p.TblTrainingCousre)
                    .HasForeignKey(d => d.TrainingLevelId)
                    .HasConstraintName("FK_tblTrainingCousre_tblTrainingLevel");

                entity.HasOne(d => d.TrainingType)
                    .WithMany(p => p.TblTrainingCousre)
                    .HasForeignKey(d => d.TrainingTypeId)
                    .HasConstraintName("FK_tblTrainingCousre_tblTrainingType");
            });

            modelBuilder.Entity<TblTrainingDetail>(entity =>
            {
                entity.HasKey(e => e.TrainingDetailId);

                entity.ToTable("tblTrainingDetail", "Training");

                entity.Property(e => e.Creator).HasMaxLength(50);

                entity.Property(e => e.EmployeeTraining).HasMaxLength(20);

                entity.Property(e => e.Modifyer).HasMaxLength(50);

                entity.HasOne(d => d.EmployeeTrainingNavigation)
                    .WithMany(p => p.TblTrainingDetail)
                    .HasForeignKey(d => d.EmployeeTraining)
                    .HasConstraintName("FK_tblTrainingDetail_tblEmployee");

                entity.HasOne(d => d.TrainingMaster)
                    .WithMany(p => p.TblTrainingDetail)
                    .HasForeignKey(d => d.TrainingMasterId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_tblTrainingDetail_tblTrainingMaster");
            });

            modelBuilder.Entity<TblTrainingLevel>(entity =>
            {
                entity.HasKey(e => e.TrainingLevelId);

                entity.ToTable("tblTrainingLevel", "Training");

                entity.Property(e => e.Creator).HasMaxLength(50);

                entity.Property(e => e.Detail).HasMaxLength(200);

                entity.Property(e => e.Modifyer).HasMaxLength(50);

                entity.Property(e => e.TrainingLevel).HasMaxLength(50);
            });

            modelBuilder.Entity<TblTrainingMaster>(entity =>
            {
                entity.HasKey(e => e.TrainingMasterId);

                entity.ToTable("tblTrainingMaster", "Training");

                entity.Property(e => e.Creator).HasMaxLength(50);

                entity.Property(e => e.Detail).HasMaxLength(200);

                entity.Property(e => e.LecturerName).HasMaxLength(200);

                entity.Property(e => e.Modifyer).HasMaxLength(50);

                entity.Property(e => e.Remark).HasMaxLength(200);

                entity.Property(e => e.TrainingCode).HasMaxLength(50);

                entity.Property(e => e.TrainingName).HasMaxLength(200);

                entity.HasOne(d => d.TrainingCousre)
                    .WithMany(p => p.TblTrainingMaster)
                    .HasForeignKey(d => d.TrainingCousreId)
                    .HasConstraintName("FK_tblTrainingMaster_tblTrainingCousre");
            });

            modelBuilder.Entity<TblTrainingMasterHasPlace>(entity =>
            {
                entity.HasKey(e => e.TrainingMasterHasPlaceId);

                entity.ToTable("tblTrainingMasterHasPlace", "Training");

                entity.Property(e => e.TrainingMasterHasPlaceId).HasColumnName("TrainingMasterHasPlaceID");

                entity.Property(e => e.Creator).HasMaxLength(25);

                entity.Property(e => e.Modifyer).HasMaxLength(25);

                entity.Property(e => e.PlaceId).HasColumnName("PlaceID");

                entity.HasOne(d => d.Place)
                    .WithMany(p => p.TblTrainingMasterHasPlace)
                    .HasForeignKey(d => d.PlaceId)
                    .HasConstraintName("FK_tblTrainingMasterHasPlace_tblPlace");

                entity.HasOne(d => d.TrainingMaster)
                    .WithMany(p => p.TblTrainingMasterHasPlace)
                    .HasForeignKey(d => d.TrainingMasterId)
                    .HasConstraintName("FK_tblTrainingMasterHasPlace_tblTrainingMaster");
            });

            modelBuilder.Entity<TblTrainingPlanningDetail>(entity =>
            {
                entity.HasKey(e => e.TrainingPlanningDetailId);

                entity.ToTable("tblTrainingPlanningDetail", "Training");

                entity.Property(e => e.Creator).HasMaxLength(50);

                entity.Property(e => e.Modifyer).HasMaxLength(50);

                entity.Property(e => e.Remark).HasMaxLength(200);

                entity.HasOne(d => d.TrainingCousre)
                    .WithMany(p => p.TblTrainingPlanningDetail)
                    .HasForeignKey(d => d.TrainingCousreId)
                    .HasConstraintName("FK_tblTrainingPlanningDetail_tblTrainingCousre");

                entity.HasOne(d => d.TrainingPlanningMaster)
                    .WithMany(p => p.TblTrainingPlanningDetail)
                    .HasForeignKey(d => d.TrainingPlanningMasterId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_tblTrainingPlanningDetail_tblTrainingPlanningMaster");
            });

            modelBuilder.Entity<TblTrainingPlanningMaster>(entity =>
            {
                entity.HasKey(e => e.TrainingPlanningMasterId);

                entity.ToTable("tblTrainingPlanningMaster", "Training");

                entity.Property(e => e.Creator).HasMaxLength(50);

                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.Modifyer).HasMaxLength(50);

                entity.Property(e => e.PlanningName).HasMaxLength(50);

                entity.Property(e => e.Remark).HasMaxLength(200);
            });

            modelBuilder.Entity<TblTrainingProgramHasGroup>(entity =>
            {
                entity.HasKey(e => e.ProgramHasGroupId);

                entity.ToTable("tblTrainingProgramHasGroup", "Training");

                entity.Property(e => e.Creator).HasMaxLength(25);

                entity.Property(e => e.GroupCode).HasMaxLength(20);

                entity.Property(e => e.Modifyer).HasMaxLength(25);

                entity.HasOne(d => d.GroupCodeNavigation)
                    .WithMany(p => p.TblTrainingProgramHasGroup)
                    .HasForeignKey(d => d.GroupCode)
                    .HasConstraintName("FK_TrainingProgramHasGroup_tblGroupName");

                entity.HasOne(d => d.TrainingProgram)
                    .WithMany(p => p.TblTrainingProgramHasGroup)
                    .HasForeignKey(d => d.TrainingProgramId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_TrainingProgramHasGroup_tblTrainingPrograms");
            });

            modelBuilder.Entity<TblTrainingProgramHasLevel>(entity =>
            {
                entity.HasKey(e => e.ProgramHasLevelId);

                entity.ToTable("tblTrainingProgramHasLevel", "Training");

                entity.Property(e => e.Creator).HasMaxLength(25);

                entity.Property(e => e.Modifyer).HasMaxLength(25);
            });

            modelBuilder.Entity<TblTrainingProgramHasPosition>(entity =>
            {
                entity.HasKey(e => e.ProgramHasPositionId);

                entity.ToTable("tblTrainingProgramHasPosition", "Training");

                entity.Property(e => e.Creator).HasMaxLength(50);

                entity.Property(e => e.Modifyer).HasMaxLength(50);

                entity.Property(e => e.PositionCode).HasMaxLength(20);

                entity.HasOne(d => d.PositionCodeNavigation)
                    .WithMany(p => p.TblTrainingProgramHasPosition)
                    .HasForeignKey(d => d.PositionCode)
                    .HasConstraintName("FK_tblTrainingProgramHasPosition_tblPosition");

                entity.HasOne(d => d.TrainingProgram)
                    .WithMany(p => p.TblTrainingProgramHasPosition)
                    .HasForeignKey(d => d.TrainingProgramId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_tblTrainingProgramHasPosition_tblTrainingPrograms");
            });

            modelBuilder.Entity<TblTrainingPrograms>(entity =>
            {
                entity.HasKey(e => e.TrainingProgramId);

                entity.ToTable("tblTrainingPrograms", "Training");

                entity.Property(e => e.Creator).HasMaxLength(50);

                entity.Property(e => e.Detail).HasMaxLength(200);

                entity.Property(e => e.Modifyer).HasMaxLength(50);

                entity.Property(e => e.Remark).HasMaxLength(200);

                entity.Property(e => e.TrainingProgramCode).HasMaxLength(50);

                entity.Property(e => e.TrainingProgramLevelString).HasMaxLength(50);

                entity.Property(e => e.TrainingProgramName).HasMaxLength(200);
            });

            modelBuilder.Entity<TblTrainingRequestDetail>(entity =>
            {
                entity.HasKey(e => e.TrainingRequestDetailId);

                entity.ToTable("tblTrainingRequestDetail", "Training");

                entity.Property(e => e.Creator).HasMaxLength(50);

                entity.Property(e => e.EmployeeNeedTraining).HasMaxLength(20);

                entity.Property(e => e.Modifyer).HasMaxLength(50);

                entity.HasOne(d => d.EmployeeNeedTrainingNavigation)
                    .WithMany(p => p.TblTrainingRequestDetail)
                    .HasForeignKey(d => d.EmployeeNeedTraining)
                    .HasConstraintName("FK_tblTrainingRequestDetail_tblEmployee");

                entity.HasOne(d => d.TrainingRequestMaster)
                    .WithMany(p => p.TblTrainingRequestDetail)
                    .HasForeignKey(d => d.TrainingRequestMasterId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_tblTrainingRequestDetail_tblTrainingRequestMaster");
            });

            modelBuilder.Entity<TblTrainingRequestMaster>(entity =>
            {
                entity.HasKey(e => e.TrainingRequestMasterId);

                entity.ToTable("tblTrainingRequestMaster", "Training");

                entity.Property(e => e.Creator).HasMaxLength(50);

                entity.Property(e => e.EmployeeRequest).HasMaxLength(20);

                entity.Property(e => e.Modifyer).HasMaxLength(50);

                entity.HasOne(d => d.EmployeeRequestNavigation)
                    .WithMany(p => p.TblTrainingRequestMaster)
                    .HasForeignKey(d => d.EmployeeRequest)
                    .HasConstraintName("FK_tblTrainingRequestMaster_tblEmployee");

                entity.HasOne(d => d.TrainingCousre)
                    .WithMany(p => p.TblTrainingRequestMaster)
                    .HasForeignKey(d => d.TrainingCousreId)
                    .HasConstraintName("FK_tblTrainingRequestMaster_tblTrainingCousre");
            });

            modelBuilder.Entity<TblTrainingType>(entity =>
            {
                entity.HasKey(e => e.TrainingTypeId);

                entity.ToTable("tblTrainingType", "Training");

                entity.Property(e => e.Creator).HasMaxLength(50);

                entity.Property(e => e.Detail).HasMaxLength(200);

                entity.Property(e => e.Modifyer).HasMaxLength(50);

                entity.Property(e => e.TrainingTypeName).HasMaxLength(50);

                entity.HasOne(d => d.TrainingTypeParent)
                    .WithMany(p => p.InverseTrainingTypeParent)
                    .HasForeignKey(d => d.TrainingTypeParentId)
                    .HasConstraintName("FK_tblTrainingType_tblTrainingType");
            });

            modelBuilder.Entity<TblTrainning>(entity =>
            {
                entity.HasKey(e => e.TrainCode);

                entity.Property(e => e.TrainCode)
                    .HasMaxLength(20)
                    .ValueGeneratedNever();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Creator).HasMaxLength(25);

                entity.Property(e => e.DocNo).HasMaxLength(20);

                entity.Property(e => e.Edate)
                    .HasColumnName("EDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Etime)
                    .HasColumnName("ETime")
                    .HasMaxLength(10);

                entity.Property(e => e.ExName).HasMaxLength(80);

                entity.Property(e => e.ModifyDate).HasColumnType("datetime");

                entity.Property(e => e.Modifyer).HasMaxLength(25);

                entity.Property(e => e.Ojcode)
                    .HasColumnName("OJCode")
                    .HasMaxLength(10);

                entity.Property(e => e.Remark).HasMaxLength(80);

                entity.Property(e => e.Sdate)
                    .HasColumnName("SDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Stime)
                    .HasColumnName("STime")
                    .HasMaxLength(10);

                entity.Property(e => e.TypeCode).HasMaxLength(10);
            });

            modelBuilder.Entity<TblTrainObJect>(entity =>
            {
                entity.HasKey(e => e.Ojcode);

                entity.Property(e => e.Ojcode)
                    .HasColumnName("OJCode")
                    .HasMaxLength(10)
                    .ValueGeneratedNever();

                entity.Property(e => e.Ojname)
                    .HasColumnName("OJName")
                    .HasMaxLength(50);

                entity.Property(e => e.Remark).HasMaxLength(50);
            });

            modelBuilder.Entity<TblTrainType>(entity =>
            {
                entity.HasKey(e => e.TypeCode);

                entity.Property(e => e.TypeCode)
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.Remark).HasMaxLength(50);

                entity.Property(e => e.TypeName).HasMaxLength(50);
            });
        }
    }
}
