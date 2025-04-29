using Microsoft.EntityFrameworkCore;
using Models;

namespace Context{
    public class MyDbContext : DbContext{
        public DbSet<User> Users {get;set;}
        public DbSet<Note> Notes {get;set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
            .HasIndex(u => u.Nickname)
            .IsUnique();
            modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
        }
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options){}
    }

}