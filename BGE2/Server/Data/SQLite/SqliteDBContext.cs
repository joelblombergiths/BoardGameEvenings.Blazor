using BGE2.Server.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BGE2.Server.Data.SQLite;

public class SqliteDBContext : DbContext
{
    public DbSet<UserModel> UserRights { get; set; } = null!;

    public SqliteDBContext(DbContextOptions options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserModel>().HasData(
            new UserModel
            {
                Email = "joel.blomberg@iths.se",
                Role = "admin"
            }
        );
    }
}