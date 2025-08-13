using Microsoft.EntityFrameworkCore;
using minimal_api.domain.entities;

namespace minimal_api.infraestructure.DBContext
{
    public class DBContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public DBContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public DbSet<Admin> Admins { get; set; } = default!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection").ToString();
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
                }
                optionsBuilder.UseMySql(
                    connectionString,
                    ServerVersion.AutoDetect(connectionString)
                );
            }
        }
    }
}