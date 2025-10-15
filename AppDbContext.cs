using Microsoft.EntityFrameworkCore;
using app2.Models;
using app2.Configurations;

namespace app2
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 应用所有配置
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            
            // 或者自动应用当前程序集中的所有配置
            // modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries<User>();

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.create_time = DateTime.Now;
                        entry.Entity.update_time = DateTime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.update_time = DateTime.Now;
                        break;
                }
            }
        }
    }
}