using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LingoVerse.Models;

public partial class LinguaVerseContext : DbContext
{
    public LinguaVerseContext()
    {
    }

    public LinguaVerseContext(DbContextOptions<LinguaVerseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<CommunityPost> CommunityPosts { get; set; }

    public virtual DbSet<Language> Languages { get; set; }

    public virtual DbSet<Level> Levels { get; set; }

    public virtual DbSet<MultipleChoiceQuestion> MultipleChoiceQuestions { get; set; }

    public virtual DbSet<OfficialPost> OfficialPosts { get; set; }

    public virtual DbSet<PremiumPlan> PremiumPlans { get; set; }

    public virtual DbSet<Result> Results { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Set> Sets { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserPremiumSubscription> UserPremiumSubscriptions { get; set; }

    public virtual DbSet<Word> Words { get; set; }

    public virtual DbSet<WritingQuestion> WritingQuestions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=MSI\\SQLEXPRESS;Initial Catalog=LinguaVerse; Trusted_Connection=SSPI;Encrypt=false;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasIndex(e => e.CategoryName, "UQ_Categories_CategoryName").IsUnique();

            entity.Property(e => e.CategoryName).HasMaxLength(50);
        });

        modelBuilder.Entity<CommunityPost>(entity =>
        {
            entity.HasKey(e => e.PostId);

            entity.HasIndex(e => e.AuthorId, "IX_CommunityPosts_AuthorId");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.ImageUrl).HasMaxLength(2048);
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .HasDefaultValue("Pending");
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Author).WithMany(p => p.CommunityPosts)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CommunityPosts_Users");
        });

        modelBuilder.Entity<Language>(entity =>
        {
            entity.HasIndex(e => e.LanguageCode, "UQ_Languages_LanguageCode").IsUnique();

            entity.HasIndex(e => e.LanguageName, "UQ_Languages_LanguageName").IsUnique();

            entity.Property(e => e.LanguageCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.LanguageName).HasMaxLength(50);
        });

        modelBuilder.Entity<Level>(entity =>
        {
            entity.HasIndex(e => e.LevelName, "UQ_Levels_LevelName").IsUnique();

            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.LevelName).HasMaxLength(50);
        });

        modelBuilder.Entity<MultipleChoiceQuestion>(entity =>
        {
            entity.HasKey(e => e.QuestionId);

            entity.Property(e => e.CorrectAnswer)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.OptionA).HasMaxLength(255);
            entity.Property(e => e.OptionB).HasMaxLength(255);
            entity.Property(e => e.OptionC).HasMaxLength(255);
            entity.Property(e => e.OptionD).HasMaxLength(255);
            entity.Property(e => e.QuestionText).HasMaxLength(500);

            entity.HasOne(d => d.Post).WithMany(p => p.MultipleChoiceQuestions)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MultipleChoiceQuestions_OfficialPosts");
        });

        modelBuilder.Entity<OfficialPost>(entity =>
        {
            entity.HasKey(e => e.PostId);

            entity.HasIndex(e => e.AuthorId, "IX_OfficialPosts_AuthorId");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.ImageUrl).HasMaxLength(2048);
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .HasDefaultValue("Draft");
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Author).WithMany(p => p.OfficialPosts)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OfficialPosts_Users");

            entity.HasOne(d => d.Category).WithMany(p => p.OfficialPosts)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OfficialPosts_Categories");

            entity.HasOne(d => d.Language).WithMany(p => p.OfficialPosts)
                .HasForeignKey(d => d.LanguageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OfficialPosts_Languages");

            entity.HasOne(d => d.Level).WithMany(p => p.OfficialPosts)
                .HasForeignKey(d => d.LevelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OfficialPosts_Levels");
        });

        modelBuilder.Entity<PremiumPlan>(entity =>
        {
            entity.HasKey(e => e.PlanId);

            entity.HasIndex(e => e.PlanName, "UQ_PremiumPlans_PlanName").IsUnique();

            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.PlanName).HasMaxLength(50);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<Result>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Score).HasColumnType("decimal(5, 2)");

            entity.HasOne(d => d.Post).WithMany(p => p.Results)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Results_OfficialPosts");

            entity.HasOne(d => d.User).WithMany(p => p.Results)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Results_Users");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasIndex(e => e.RoleName, "UQ_RoleName").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<Set>(entity =>
        {
            entity.HasIndex(e => e.LanguageId, "IX_Sets_LanguageId");

            entity.HasIndex(e => e.UserId, "IX_Sets_UserId");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.SetName).HasMaxLength(100);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Category).WithMany(p => p.Sets)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sets_Categories");

            entity.HasOne(d => d.Language).WithMany(p => p.Sets)
                .HasForeignKey(d => d.LanguageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sets_Languages");

            entity.HasOne(d => d.Level).WithMany(p => p.Sets)
                .HasForeignKey(d => d.LevelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sets_Levels");

            entity.HasOne(d => d.User).WithMany(p => p.Sets)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sets_Users");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_Users_RoleId");

            entity.HasIndex(e => e.Email, "UQ_Users_Email").IsUnique();

            entity.HasIndex(e => e.Username, "UQ_Users_Username").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Username).HasMaxLength(50);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_Roles");
        });

        modelBuilder.Entity<UserPremiumSubscription>(entity =>
        {
            entity.HasKey(e => e.SubscriptionId);

            entity.HasIndex(e => e.UserId, "IX_UserPremiumSubscriptions_UserId");

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.StartDate).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Plan).WithMany(p => p.UserPremiumSubscriptions)
                .HasForeignKey(d => d.PlanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserPremiumSubscriptions_PremiumPlans");

            entity.HasOne(d => d.User).WithMany(p => p.UserPremiumSubscriptions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserPremiumSubscriptions_Users");
        });

        modelBuilder.Entity<Word>(entity =>
        {
            entity.HasIndex(e => e.SetId, "IX_Words_SetId");

            entity.HasIndex(e => new { e.SetId, e.Word1 }, "UQ_Words_SetId_Word").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Definition).HasMaxLength(1000);
            entity.Property(e => e.Example).HasMaxLength(1000);
            entity.Property(e => e.Word1)
                .HasMaxLength(100)
                .HasColumnName("Word");

            entity.HasOne(d => d.Set).WithMany(p => p.Words)
                .HasForeignKey(d => d.SetId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Words_Sets");
        });

        modelBuilder.Entity<WritingQuestion>(entity =>
        {
            entity.HasKey(e => e.QuestionId);

            entity.Property(e => e.QuestionText).HasMaxLength(500);

            entity.HasOne(d => d.Post).WithMany(p => p.WritingQuestions)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WritingQuestions_OfficialPosts");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
