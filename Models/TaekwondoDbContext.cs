using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ManagerStudentSystem.Models;

public partial class TaekwondoDbContext : DbContext
{
    public TaekwondoDbContext()
    {
    }

    public TaekwondoDbContext(DbContextOptions<TaekwondoDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Exam> Exams { get; set; }

    public virtual DbSet<ExamRegistration> ExamRegistrations { get; set; }

    public virtual DbSet<FeePeriod> FeePeriods { get; set; }

    public virtual DbSet<FeePlan> FeePlans { get; set; }

    public virtual DbSet<IdempotencyKey> IdempotencyKeys { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<InvoiceDetail> InvoiceDetails { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<ItemType> ItemTypes { get; set; }

    public virtual DbSet<Level> Levels { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PaymentAttempt> PaymentAttempts { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<PaymentProvider> PaymentProviders { get; set; }

    public virtual DbSet<PaymentTransaction> PaymentTransactions { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<Refund> Refunds { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<StudentFee> StudentFees { get; set; }

    public virtual DbSet<StudentFeePlan> StudentFeePlans { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<WebhookEvent> WebhookEvents { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-OV2K6SL\\SQLEXPRESS;Database=TaekwondoDB;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Exam>(entity =>
        {
            entity.HasKey(e => e.ExamId).HasName("PK__Exams__297521C7B553418B");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Status).HasDefaultValue("Open");
        });

        modelBuilder.Entity<ExamRegistration>(entity =>
        {
            entity.HasKey(e => e.ExamRegistrationId).HasName("PK__ExamRegi__DBFFEBC9038543DE");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Status).HasDefaultValue("Pending");

            entity.HasOne(d => d.Exam).WithMany(p => p.ExamRegistrations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ExamRegistrations_Exams");

            entity.HasOne(d => d.Student).WithMany(p => p.ExamRegistrations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ExamRegistrations_Students");
        });

        modelBuilder.Entity<FeePeriod>(entity =>
        {
            entity.HasKey(e => e.FeePeriodId).HasName("PK__FeePerio__A82FE11A63F92395");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.FeePeriodCode).HasDefaultValue("FP-XXXX");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<FeePlan>(entity =>
        {
            entity.HasKey(e => e.FeePlanId).HasName("PK__FeePlans__D92566C22B52D6B1");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<IdempotencyKey>(entity =>
        {
            entity.HasKey(e => e.IdempotencyKeyId).HasName("PK__Idempote__2B5723BB24B4EA62");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.InvoiceId).HasName("PK__Invoices__D796AAB5BE546AAE");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.PaidAmount).HasDefaultValue(0m);
            entity.Property(e => e.Status).HasDefaultValue("Pending");

            entity.HasOne(d => d.Student).WithMany(p => p.Invoices)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Invoices_Students");
        });

        modelBuilder.Entity<InvoiceDetail>(entity =>
        {
            entity.HasKey(e => e.InvoiceDetailId).HasName("PK__InvoiceD__1F15781149E3B7E0");

            entity.HasOne(d => d.Invoice).WithMany(p => p.InvoiceDetails).HasConstraintName("FK_InvoiceDetails_Invoices");

            entity.HasOne(d => d.ItemType).WithMany(p => p.InvoiceDetails).HasConstraintName("FK_InvoiceDetails_ItemTypes");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PK__Items__727E838B118AF6A2");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.ItemType).WithMany(p => p.Items)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Items_ItemTypes");
        });

        modelBuilder.Entity<ItemType>(entity =>
        {
            entity.HasKey(e => e.ItemTypeId).HasName("PK__ItemType__F51540FB04DA7491");

            entity.Property(e => e.ItemTypeId).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<Level>(entity =>
        {
            entity.HasKey(e => e.LevelId).HasName("PK__Levels__09F03C269A589B95");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payments__9B556A38B1B521B9");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Currency)
                .HasDefaultValue("VND")
                .IsFixedLength();
            entity.Property(e => e.PaymentDate).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Status).HasDefaultValue("Pending");

            entity.HasOne(d => d.Invoice).WithMany(p => p.Payments).HasConstraintName("FK_Payments_Invoices");

            entity.HasOne(d => d.PaymentMethod).WithMany(p => p.Payments).HasConstraintName("FK_Payments_PaymentMethods");

            entity.HasOne(d => d.Provider).WithMany(p => p.Payments).HasConstraintName("FK_Payments_PaymentProvider");
        });

        modelBuilder.Entity<PaymentAttempt>(entity =>
        {
            entity.HasKey(e => e.PaymentAttemptId).HasName("PK__PaymentA__1D02F9A7CBC937B9");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Status).HasDefaultValue("Created");

            entity.HasOne(d => d.PaymentCodeNavigation).WithMany(p => p.PaymentAttempts)
                .HasPrincipalKey(p => p.PaymentCode)
                .HasForeignKey(d => d.PaymentCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PaymentAttempts_Payments");
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.HasKey(e => e.PaymentMethodId).HasName("PK__PaymentM__DC31C1D3F9ECA3D3");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<PaymentProvider>(entity =>
        {
            entity.HasKey(e => e.PaymentProviderId).HasName("PK__PaymentP__3EBBCD69927C15D5");

            entity.Property(e => e.CreateAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<PaymentTransaction>(entity =>
        {
            entity.HasKey(e => e.PaymentTransactionId).HasName("PK__PaymentT__C22AEFE0FA046106");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.PaymentAttempt).WithMany(p => p.PaymentTransactions).HasConstraintName("FK_PaymentTransactions_Attempts");

            entity.HasOne(d => d.Payment).WithMany(p => p.PaymentTransactions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PaymentTransactions_Payments");

            entity.HasOne(d => d.Provider).WithMany(p => p.PaymentTransactions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PaymentTransactions_Providers");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.RefreshTokenId).HasName("PK__RefreshT__F5845E393CE41133");

            entity.HasOne(d => d.User).WithMany(p => p.RefreshTokens).HasConstraintName("fk_RefreshTokens_Users");
        });

        modelBuilder.Entity<Refund>(entity =>
        {
            entity.HasKey(e => e.RefundId).HasName("PK__Refunds__725AB9205FE7A152");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Payment).WithMany(p => p.Refunds)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Refunds_Payments");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE1A48689416");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK__Students__32C52B9961BB3CF4");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.LevelId).HasDefaultValue(1);
            entity.Property(e => e.Status).HasDefaultValue("Active");
            entity.Property(e => e.StudentCode).HasDefaultValue("VXX-XXXX");

            entity.HasOne(d => d.Level).WithMany(p => p.Students).HasConstraintName("FK_Students_Levels");

            entity.HasOne(d => d.User).WithOne(p => p.Student).HasConstraintName("FK_Students_Users");
        });

        modelBuilder.Entity<StudentFee>(entity =>
        {
            entity.HasKey(e => e.StudentFeeId).HasName("PK__StudentF__9D02D873570BDD5F");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Status).HasDefaultValue("Pending");

            entity.HasOne(d => d.FeePeriod).WithMany(p => p.StudentFees).HasConstraintName("FK_StudentFees_FeePeriods");

            entity.HasOne(d => d.Student).WithMany(p => p.StudentFees).HasConstraintName("FK_StudentFees_Students");
        });

        modelBuilder.Entity<StudentFeePlan>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");

            entity.HasOne(d => d.FeePlan).WithMany(p => p.StudentFeePlans)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StudentFeePlans_FeePlans");

            entity.HasOne(d => d.Student).WithMany(p => p.StudentFeePlans)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StudentFeePlans_Students");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CBE742D32");

            entity.Property(e => e.CreateAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserRole",
                    r => r.HasOne<Role>().WithMany()
                        .HasForeignKey("RoleId")
                        .HasConstraintName("fk_UserRole_Roles"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_UserRole_Users"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId").HasName("PK__UserRole__AF2760ADCD77FDEC");
                        j.ToTable("UserRoles", "Taekwondo");
                    });
        });

        modelBuilder.Entity<WebhookEvent>(entity =>
        {
            entity.HasKey(e => e.WebhookEventId).HasName("PK__WebhookE__8883E8B6E3DCDB9C");

            entity.Property(e => e.ReceivedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Provider).WithMany(p => p.WebhookEvents)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WebhookEvents_Providers");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
