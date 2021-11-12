using System;
using Microsoft.EntityFrameworkCore;
#nullable disable

namespace Backend.Models.Database
{
    public partial class Context : DbContext
    {
        public string DbPath { get; private set; }
        public Context(DbContextOptions<Context> options)
            : base(options)
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = $"{path}{System.IO.Path.DirectorySeparatorChar}Recommendation.db";
        }

        public virtual DbSet<Movie> Movies { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Rating> Ratings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}