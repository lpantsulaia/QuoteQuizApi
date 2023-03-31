using DataModels.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace QuoteQuizApi
{
    public class DataContext : DbContext
    {
        public IConfiguration Configuration { get; }
        public DataContext(DbContextOptions<DataContext> options, IConfiguration configuration) : base(options)
        {
            Configuration = configuration;
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<QuoteAnswer> QuoteAnswers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            IConfigurationSection appSettingsSection = Configuration.GetSection("AppSettings");
            AppSettings appSettings = appSettingsSection.Get<AppSettings>();
            byte[] passwordHash, passwordSalt;
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(appSettings.AdminPassword));
            }

            modelBuilder.Entity<User>().HasData(
              new User { Id = 1, PasswordHash = passwordHash, PasswordSalt = passwordSalt, UserName = "admin" });

        }
    }
}
