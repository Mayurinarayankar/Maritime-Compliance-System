using MaritimeApp.Domain.Entities;
using MaritimeApp.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
namespace MaritimeApp.Infrastructure.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (await context.Ships.AnyAsync()) return;

        // Ships
        var ship1 = new Ship
        {
            Id = Guid.NewGuid(),
            Name = "MV Atlantic Pioneer",
            IMONumber = "IMO9234567",
            Type = "Bulk Carrier",
            Flag = "Panama",
            YearBuilt = 2010,
            IsActive = true
        };
        var ship2 = new Ship
        {
            Id = Guid.NewGuid(),
            Name = "MV Pacific Star",
            IMONumber = "IMO9345678",
            Type = "Container Ship",
            Flag = "Liberia",
            YearBuilt = 2015,
            IsActive = true
        };
        var ship3 = new Ship
        {
            Id = Guid.NewGuid(),
            Name = "MV Indian Voyager",
            IMONumber = "IMO9456789",
            Type = "Tanker",
            Flag = "Marshall Islands",
            YearBuilt = 2018,
            IsActive = true
        };

        await context.Ships.AddRangeAsync(ship1, ship2, ship3);

        // Admin user
        var adminId = Guid.NewGuid();
        var admin = new User
        {
            Id = adminId,
            FirstName = "John",
            LastName = "Smith",
            Email = "admin@maritime.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
            Role = UserRole.Admin,
            Rank = "Fleet Manager",
            IsActive = true
        };

        // Crew users
        var crew1Id = Guid.NewGuid();
        var crew1 = new User
        {
            Id = crew1Id,
            FirstName = "Sarah",
            LastName = "Johnson",
            Email = "crew1@maritime.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Crew@123"),
            Role = UserRole.Crew,
            Rank = "Chief Engineer",
            ShipId = ship1.Id,
            IsActive = true
        };
        var crew2Id = Guid.NewGuid();
        var crew2 = new User
        {
            Id = crew2Id,
            FirstName = "Mike",
            LastName = "Williams",
            Email = "crew2@maritime.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Crew@123"),
            Role = UserRole.Officer,
            Rank = "First Officer",
            ShipId = ship1.Id,
            IsActive = true
        };
        var crew3Id = Guid.NewGuid();
        var crew3 = new User
        {
            Id = crew3Id,
            FirstName = "Emily",
            LastName = "Chen",
            Email = "crew3@maritime.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Crew@123"),
            Role = UserRole.Crew,
            Rank = "Second Engineer",
            ShipId = ship2.Id,
            IsActive = true
        };

        await context.Users.AddRangeAsync(admin, crew1, crew2, crew3);
        await context.SaveChangesAsync();

        // Maintenance Tasks
        var tasks = new List<MaintenanceTask>
        {
            new() { Title = "Engine Room Inspection", Description = "Full inspection of main engine and auxiliary systems", Status = MaintenanceTaskStatus.Pending, DueDate = DateTime.UtcNow.AddDays(5), Priority = "High", ShipId = ship1.Id, AssignedToId = crew1.Id, CreatedById = adminId },
            new() { Title = "Navigation Equipment Check", Description = "Calibrate GPS, radar, and communication systems", Status = MaintenanceTaskStatus.InProgress, DueDate = DateTime.UtcNow.AddDays(2), Priority = "Critical", ShipId = ship1.Id, AssignedToId = crew2.Id, CreatedById = adminId },
            new() { Title = "Hull Cleaning", Description = "Underwater hull cleaning and anti-fouling paint application", Status = MaintenanceTaskStatus.Completed, DueDate = DateTime.UtcNow.AddDays(-5), CompletedAt = DateTime.UtcNow.AddDays(-3), Priority = "Medium", ShipId = ship1.Id, AssignedToId = crew1.Id, CreatedById = adminId },
            new() { Title = "Fire Suppression System Test", Description = "Test all fire detection and suppression systems", Status = MaintenanceTaskStatus.Pending, DueDate = DateTime.UtcNow.AddDays(-2), Priority = "Critical", ShipId = ship1.Id, AssignedToId = crew2.Id, CreatedById = adminId },
            new() { Title = "Cargo Hold Inspection", Description = "Inspect all cargo holds for structural integrity", Status = MaintenanceTaskStatus.Pending, DueDate = DateTime.UtcNow.AddDays(10), Priority = "Medium", ShipId = ship2.Id, AssignedToId = crew3.Id, CreatedById = adminId },
            new() { Title = "Fuel System Maintenance", Description = "Check fuel tanks, pumps and filters", Status = MaintenanceTaskStatus.InProgress, DueDate = DateTime.UtcNow.AddDays(-1), Priority = "High", ShipId = ship2.Id, AssignedToId = crew3.Id, CreatedById = adminId },
            new() { Title = "Life Raft Inspection", Description = "Inspect and service all life rafts and survival equipment", Status = MaintenanceTaskStatus.Completed, DueDate = DateTime.UtcNow.AddDays(-10), CompletedAt = DateTime.UtcNow.AddDays(-8), Priority = "High", ShipId = ship3.Id, CreatedById = adminId },
            new() { Title = "Anchor Chain Inspection", Description = "Inspect anchor chains for wear and corrosion", Status = MaintenanceTaskStatus.Pending, DueDate = DateTime.UtcNow.AddDays(15), Priority = "Low", ShipId = ship3.Id, CreatedById = adminId },
        };
        await context.MaintenanceTasks.AddRangeAsync(tasks);

        // Safety Drills
        var drills = new List<SafetyDrill>
        {
            new() { Title = "Monthly Fire Drill", Description = "Mandatory monthly fire evacuation drill", DrillType = DrillType.FireDrill, ScheduledDate = DateTime.UtcNow.AddDays(3), ShipId = ship1.Id, CreatedById = adminId },
            new() { Title = "Abandon Ship Drill", Description = "Quarterly abandon ship drill with lifeboat lowering", DrillType = DrillType.AbandonShipDrill, ScheduledDate = DateTime.UtcNow.AddDays(-5), IsCompleted = true, CompletedAt = DateTime.UtcNow.AddDays(-5), ShipId = ship1.Id, CreatedById = adminId },
            new() { Title = "Man Overboard Drill", Description = "Practice MOB rescue procedures", DrillType = DrillType.ManOverboardDrill, ScheduledDate = DateTime.UtcNow.AddDays(-3), ShipId = ship1.Id, CreatedById = adminId },
            new() { Title = "Oil Spill Response Drill", Description = "Emergency oil spill containment procedures", DrillType = DrillType.OilSpillDrill, ScheduledDate = DateTime.UtcNow.AddDays(7), ShipId = ship2.Id, CreatedById = adminId },
            new() { Title = "Evacuation Drill", Description = "Full ship evacuation exercise", DrillType = DrillType.EvacuationDrill, ScheduledDate = DateTime.UtcNow.AddDays(-1), ShipId = ship2.Id, CreatedById = adminId },
            new() { Title = "Security Drill", Description = "ISPS Code security exercise", DrillType = DrillType.SecurityDrill, ScheduledDate = DateTime.UtcNow.AddDays(14), IsCompleted = false, ShipId = ship3.Id, CreatedById = adminId },
        };
        await context.SafetyDrills.AddRangeAsync(drills);
        await context.SaveChangesAsync();

        // Add attendance for completed drill
        var completedDrill = drills[1];
        var attendances = new List<DrillAttendance>
        {
            new() { DrillId = completedDrill.Id, UserId = crew1.Id, Attended = true, MarkedAt = completedDrill.CompletedAt },
            new() { DrillId = completedDrill.Id, UserId = crew2.Id, Attended = true, MarkedAt = completedDrill.CompletedAt },
        };
        await context.DrillAttendances.AddRangeAsync(attendances);
        await context.SaveChangesAsync();
    }
}