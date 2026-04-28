using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using visits.models.Base;
using visits.models.Core;
using visits.models.Policies;
using visits.models.Residences;
using visits.models.Users;
using visits.models.Visitors;

namespace visits.api.Data;

public class AppDbContext : IdentityDbContext<BaseUser, IdentityRole<Guid>, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Core
    public DbSet<Institution> Institutions => Set<Institution>();
    public DbSet<Campus> Campuses => Set<Campus>();
    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<Images> Images => Set<Images>();
    public DbSet<ClassificationValues> ClassificationValues => Set<ClassificationValues>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    // Residences
    public DbSet<Residence> Residences => Set<Residence>();
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<StudentRoom> StudentRooms => Set<StudentRoom>();

    // Policies
    public DbSet<ResidenceAccessPolicy> ResidenceAccessPolicies => Set<ResidenceAccessPolicy>();
    public DbSet<VisitTypePolicy> VisitTypePolicies => Set<VisitTypePolicy>();

    // Users
    public DbSet<Staff> Staff => Set<Staff>();
    public DbSet<Student> Students => Set<Student>();

    // Visitors
    public DbSet<Visitor> Visitors => Set<Visitor>();
    public DbSet<VisitorCode> VisitorCodes => Set<VisitorCode>();
    public DbSet<Visits> Visits => Set<Visits>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // ── BaseUser ────────────────────────────────────────────
        builder.Entity<BaseUser>()
            .HasOne(u => u.UserType)
            .WithMany()
            .HasForeignKey(u => u.UserTypeId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<BaseUser>()
            .HasOne(u => u.Role)
            .WithMany()
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.NoAction);

        // ── Residence ───────────────────────────────────────────
        builder.Entity<Residence>()
            .HasOne(r => r.WardenUser)
            .WithMany()
            .HasForeignKey(r => r.WardenUserId)
            .OnDelete(DeleteBehavior.NoAction);

        // ── Room ────────────────────────────────────────────────
        builder.Entity<Room>()
            .HasOne(r => r.RoomType)
            .WithMany()
            .HasForeignKey(r => r.RoomTypeId)
            .OnDelete(DeleteBehavior.NoAction);

        // ── StudentRoom ─────────────────────────────────────────
        builder.Entity<StudentRoom>()
            .HasOne(sr => sr.User)
            .WithMany()
            .HasForeignKey(sr => sr.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<StudentRoom>()
            .HasOne(sr => sr.Room)
            .WithMany()
            .HasForeignKey(sr => sr.RoomId)
            .OnDelete(DeleteBehavior.NoAction);

        // ── Staff ───────────────────────────────────────────────
        builder.Entity<Staff>()
            .HasOne(s => s.User)
            .WithMany()
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        // ── Student ─────────────────────────────────────────────
        builder.Entity<Student>()
            .HasOne(s => s.User)
            .WithMany()
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Student>()
            .HasOne(s => s.Gender)
            .WithMany()
            .HasForeignKey(s => s.GenderId)
            .OnDelete(DeleteBehavior.NoAction);

        // ── VisitorCode ─────────────────────────────────────────
        builder.Entity<VisitorCode>()
            .HasOne(vc => vc.IssuedByUser)
            .WithMany()
            .HasForeignKey(vc => vc.IssuedByUserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<VisitorCode>()
            .HasOne(vc => vc.Visitor)
            .WithMany()
            .HasForeignKey(vc => vc.VisitorId)
            .OnDelete(DeleteBehavior.NoAction);

        // ── Visits ──────────────────────────────────────────────
        builder.Entity<Visits>()
            .HasOne(v => v.Visitor)
            .WithMany()
            .HasForeignKey(v => v.VisitorId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Visits>()
            .HasOne(v => v.HostUser)
            .WithMany()
            .HasForeignKey(v => v.HostUserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Visits>()
            .HasOne(v => v.Room)
            .WithMany()
            .HasForeignKey(v => v.RoomId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Visits>()
            .HasOne(v => v.VisitType)
            .WithMany()
            .HasForeignKey(v => v.VisitTypeId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Visits>()
            .HasOne(v => v.Status)
            .WithMany()
            .HasForeignKey(v => v.StatusId)
            .OnDelete(DeleteBehavior.NoAction);

        // ── VisitTypePolicy ─────────────────────────────────────
        builder.Entity<VisitTypePolicy>()
            .HasOne(vtp => vtp.VisitType)
            .WithMany()
            .HasForeignKey(vtp => vtp.VisitTypeId)
            .OnDelete(DeleteBehavior.NoAction);

        // ── Indexes and Constraints ─────────────────────────────
        builder.Entity<StudentRoom>()
            .HasIndex(s => new { s.UserId, s.VacatedAt })
            .IsUnique()
            .HasFilter("VacatedAt IS NULL");

        builder.Entity<ResidenceAccessPolicy>()
            .HasIndex(r => new { r.ResidenceId, r.DayOfWeek })
            .IsUnique();

        builder.Entity<ResidenceAccessPolicy>()
            .HasIndex(x => new { x.ResidenceId, x.DayOfWeek, x.ValidFrom })
            .IsUnique();

        builder.Entity<Visitor>()
            .ToTable(t => t.HasCheckConstraint(
                "CK_Visitor_Identification",
                "IdNumber IS NOT NULL OR StudentNumber IS NOT NULL"
            ));

        builder.Entity<VisitTypePolicy>()
            .HasIndex(x => new { x.PolicyId, x.VisitTypeId })
            .IsUnique();

        builder.Entity<BaseUser>()
            .HasIndex(x => new { x.Email, x.InstitutionId })
            .IsUnique();

        builder.Entity<Residence>()
            .HasIndex(x => new { x.Code, x.InstitutionId })
            .IsUnique();

        builder.Entity<Room>()
            .HasIndex(x => new { x.RoomNumber, x.ResidenceId })
            .IsUnique();
        
        builder.Entity<RefreshToken>()
            .HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<RefreshToken>()
            .HasIndex(r => r.UserId)
            .IsUnique()
            .HasFilter("IsUsed = 0 AND IsRevoked = 0");
    }
}