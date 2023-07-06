using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using movieManiaAppBackend.Models;

namespace movieManiaAppBackend.Models
{
    public class myDBContext: DbContext
    {
        public myDBContext() : base("Data Source=localhost\\MSSQLSERVER03;Initial Catalog=movieManiaDatabase;Integrated Security=True")
        {
            Configuration.ProxyCreationEnabled = false; // Disable proxy creation
        }

        public DbSet<Rentals> Rentals { get; set; }
        public DbSet<Movies> Movies { get; set; }
        public DbSet<Customers> Customers { get; set; }
        public DbSet<RentalMovies> RentalMovies { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Configure table names
            modelBuilder.Entity<Rentals>().ToTable("Rentals");
            modelBuilder.Entity<Movies>().ToTable("Movies");
            modelBuilder.Entity<Customers>().ToTable("Customers");
            modelBuilder.Entity<RentalMovies>().ToTable("RentalMovies");

            // Configure primary keys
            modelBuilder.Entity<Rentals>().HasKey(r => r.rental_id);
            modelBuilder.Entity<Movies>().HasKey(m => m.movie_id);
            modelBuilder.Entity<Customers>().HasKey(c => c.customer_id);
            modelBuilder.Entity<RentalMovies>().HasKey(rm => new { rm.rental_id, rm.movie_id });

            // Configure relationships
            modelBuilder.Entity<Rentals>()
                .HasRequired(r => r.Customer)
                .WithMany(c => c.Rentals)
                .HasForeignKey(r => r.customer_id)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<RentalMovies>()
                .HasRequired(rm => rm.Rental)
                .WithMany(r => r.RentalMovies)
                .HasForeignKey(rm => rm.rental_id)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<RentalMovies>()
                .HasRequired(rm => rm.Movie)
                .WithMany(m => m.RentalMovies)
                .HasForeignKey(rm => rm.movie_id)
                .WillCascadeOnDelete(true);


            base.OnModelCreating(modelBuilder);
        }
    }
}