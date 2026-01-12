using Microsoft.EntityFrameworkCore;
using TransportationAttendance.Domain.Entities.Central;

namespace TransportationAttendance.Infrastructure.Persistence;

public class CentralStudentDbContext : DbContext
{
    public CentralStudentDbContext(DbContextOptions<CentralStudentDbContext> options) 
        : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    public DbSet<RegtUser> RegtUsers => Set<RegtUser>();
    public DbSet<RegtUserRole> RegtUserRoles => Set<RegtUserRole>();
    public DbSet<StudentHalaqaInfo> StudentHalaqaInfos => Set<StudentHalaqaInfo>();
    public DbSet<SetPeriod> SetPeriods => Set<SetPeriod>();
    public DbSet<SetAgeGroup> SetAgeGroups => Set<SetAgeGroup>();
    public DbSet<HalaqatLocation> HalaqatLocations => Set<HalaqatLocation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Regt_User table configuration
        modelBuilder.Entity<RegtUser>(entity =>
        {
            entity.ToTable("Regt_User");
            entity.HasKey(e => e.UserId);
            entity.Property(e => e.UserId).HasColumnName("User_Id");
            entity.Property(e => e.Username).HasColumnName("Username");
            entity.Property(e => e.UserPassword).HasColumnName("UserPassword");
            entity.Property(e => e.RoleIdCommaSep).HasColumnName("Role_ID_Comma_Sep");
            entity.Property(e => e.FullNameOfficialAr).HasColumnName("Full_Name_Official_AR");
            entity.Property(e => e.FullNameOfficialEn).HasColumnName("Full_Name_Official_EN");
            entity.Property(e => e.EmailId).HasColumnName("Email_id");
            entity.Property(e => e.MobileNo).HasColumnName("Mobile_no");
            entity.Property(e => e.UserLanguage).HasColumnName("User_Language");
            // IsActive - ignore if column doesn't exist in CentralDB
            entity.Ignore(e => e.IsActive);
        });

        // Regt_User_Role table configuration
        modelBuilder.Entity<RegtUserRole>(entity =>
        {
            entity.ToTable("Regt_User_Role");
            entity.HasKey(e => e.RoleId);
            entity.Property(e => e.RoleId).HasColumnName("Role_ID");
            entity.Property(e => e.RoleName).HasColumnName("Role_Name");
            entity.Property(e => e.RoleDesc).HasColumnName("Role_Desc");
        });

        // Student Halaqa Info View configuration
        modelBuilder.Entity<StudentHalaqaInfo>(entity =>
        {
            entity.ToView("vw_Student_Halaqa_Teacher_information_Transportation_Dep");
            entity.HasNoKey();
            entity.Property(e => e.StudentUserId).HasColumnName("student_user_id");
            entity.Property(e => e.StudentId).HasColumnName("student_id");
            entity.Property(e => e.StudentName).HasColumnName("student_name");
            entity.Property(e => e.StudentLanguage).HasColumnName("student_language");
            entity.Property(e => e.EducationTypeId).HasColumnName("education_type_id");
            entity.Property(e => e.EducationTypeName).HasColumnName("education_type_name");
            entity.Property(e => e.EducationTypeCode).HasColumnName("education_type_code");
            entity.Property(e => e.HalaqaTypeId).HasColumnName("halaqa_type_id");
            entity.Property(e => e.HalaqaTypeName).HasColumnName("halaqa_type_name");
            entity.Property(e => e.HalaqaTypeCode).HasColumnName("halaqa_type_code");
            entity.Property(e => e.MemorizationLevelId).HasColumnName("memorization_level_id");
            entity.Property(e => e.MemorizationLevelName).HasColumnName("memorization_level_name");
            entity.Property(e => e.TeacherDbId).HasColumnName("teacher_db_id");
            entity.Property(e => e.TeacherId).HasColumnName("teacher_id");
            entity.Property(e => e.TeacherName).HasColumnName("teacher_name");
            entity.Property(e => e.TeacherUserId).HasColumnName("teacher_user_id");
            entity.Property(e => e.PeriodId).HasColumnName("period_id");
            entity.Property(e => e.PeriodName).HasColumnName("period_name");
            entity.Property(e => e.PeriodCode).HasColumnName("period_code");
            entity.Property(e => e.PeriodStartTime).HasColumnName("period_start_time");
            entity.Property(e => e.PeriodEndTime).HasColumnName("period_end_time");
            entity.Property(e => e.AgeGroupId).HasColumnName("age_group_id");
            entity.Property(e => e.AgeGroupName).HasColumnName("age_group_name");
            entity.Property(e => e.MinAgeLimit).HasColumnName("Min_Age_Limit");
            entity.Property(e => e.MaxAgeLimit).HasColumnName("Max_Age_Limit");
            entity.Property(e => e.HalaqaLocationId).HasColumnName("halaqa_location_id");
            entity.Property(e => e.HalaqaLocationName).HasColumnName("halaqa_location_name");
            entity.Property(e => e.HalaqaGender).HasColumnName("halaqa_gender");
            entity.Property(e => e.MapStdEduPeriodId).HasColumnName("map_std_edu_period_id");
            entity.Property(e => e.MapStatus).HasColumnName("map_status");
            entity.Property(e => e.HalaqaSectionId).HasColumnName("halaqa_section_id");
            entity.Property(e => e.HalaqaSectionStatus).HasColumnName("halaqa_section_status");
            entity.Property(e => e.TeacherHalaqaSecId).HasColumnName("teacher_halaqa_sec_id");
            entity.Property(e => e.StudentHalaqaSecId).HasColumnName("student_halaqa_sec_id");
        });

        // SET_Period table configuration
        modelBuilder.Entity<SetPeriod>(entity =>
        {
            entity.ToTable("SET_Period");
            entity.HasKey(e => e.PeriodId);
            entity.Property(e => e.PeriodId).HasColumnName("Period_ID");
            entity.Property(e => e.PeriodDesc).HasColumnName("Period_Desc");
            entity.Property(e => e.PeriodCode).HasColumnName("Period_Code");
            entity.Property(e => e.StartTime).HasColumnName("StartTime");
            entity.Property(e => e.EndTime).HasColumnName("EndTime");
            entity.Property(e => e.LanguageP).HasColumnName("Language_P");
        });

        // SET_Age_Group table configuration
        modelBuilder.Entity<SetAgeGroup>(entity =>
        {
            entity.ToTable("SET_Age_Group");
            entity.HasKey(e => e.AgeGroupId);
            entity.Property(e => e.AgeGroupId).HasColumnName("Age_Group_ID");
            entity.Property(e => e.AgName).HasColumnName("AG_Name");
            entity.Property(e => e.MinAgeLimit).HasColumnName("Min_Age_Limit");
            entity.Property(e => e.MaxAgeLimit).HasColumnName("Max_Age_Limit");
            entity.Property(e => e.AgLanguage).HasColumnName("AG_Language");
        });

        // HALAQAT_Location table configuration
        modelBuilder.Entity<HalaqatLocation>(entity =>
        {
            entity.ToTable("HALAQAT_Location");
            entity.HasKey(e => e.LocationId);
            entity.Property(e => e.LocationId).HasColumnName("Location_ID");
            entity.Property(e => e.Name).HasColumnName("Name");
            entity.Property(e => e.Gender).HasColumnName("Gender");
        });
    }

    // Override SaveChanges to prevent any write operations
    public override int SaveChanges()
    {
        throw new InvalidOperationException("CentralStudentDbContext is read-only. Write operations are not allowed.");
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        throw new InvalidOperationException("CentralStudentDbContext is read-only. Write operations are not allowed.");
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        throw new InvalidOperationException("CentralStudentDbContext is read-only. Write operations are not allowed.");
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        throw new InvalidOperationException("CentralStudentDbContext is read-only. Write operations are not allowed.");
    }
}
