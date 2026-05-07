using MaritimeApp.Application.Interfaces;
using MaritimeApp.Application.IRepositories;
using MaritimeApp.Infrastructure.Data;
using MaritimeApp.Infrastructure.Repositories;
using MaritimeApp.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MaritimeApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
       var connectionString =
            config.GetConnectionString("DefaultConnection");

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));


        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IShipRepository, ShipRepository>();
        services.AddScoped<IMaintenanceTaskRepository, MaintenanceTaskRepository>();
        services.AddScoped<ISafetyDrillRepository, SafetyDrillRepository>();
        services.AddScoped<IDrillAttendanceRepository, DrillAttendanceRepository>();
        services.AddScoped<ITaskCommentRepository, TaskCommentRepository>();

        // Services
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IShipService, ShipService>();
        services.AddScoped<IMaintenanceService, MaintenanceService>();
        services.AddScoped<ISafetyDrillService, SafetyDrillService>();
        services.AddScoped<IComplianceService, ComplianceService>();

        return services;
    }
}