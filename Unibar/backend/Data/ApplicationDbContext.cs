using Microsoft.EntityFrameworkCore;
using Backend.Models;

namespace Backend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ReferencePrint>().ToTable("reference_prints");
            modelBuilder.Entity<MachinePriceLog>().ToTable("machine_price_log");
            modelBuilder.Entity<MaterialPriceLog>().ToTable("material_price_log");

        }


        public DbSet<User> Users { get; set; }
        public DbSet<Calculation> Calculations { get; set; }
        public DbSet<Machine> Machines { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Process> Processes { get; set; }
        public DbSet<ReferencePrint> ReferencePrints { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<MaterialPriceLog> MaterialPriceLogs { get; set; }
        public DbSet<MachinePriceLog> MachinePriceLogs { get; set; }
        public DbSet<UploadedFile> UploadedFiles { get; set; }
    }
}
