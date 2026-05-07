// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Design;

// namespace MaritimeApp.Infrastructure.Data
// {
//     public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
//     {
//         public AppDbContext CreateDbContext(string[] args)
//         {
//             var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

//             optionsBuilder.UseNpgsql(
//                 "Host=localhost;Port=5432;Database=maritime_db;Username=postgres;Password=Mayu@9049"
//             );

//             return new AppDbContext(optionsBuilder.Options);
//         }
//     }
// }