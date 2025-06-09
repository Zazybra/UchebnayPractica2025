using Garifullin.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garifullin.ContextDB
{
    public class Context:DbContext
    {
        public DbSet<Apartment> Apartments { get; set; }
        public DbSet<House> Houses { get; set; }
        public DbSet<Land> Lands { get; set; }
        public DbSet<RealEstate> RealEstates { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Supply> Supplies { get; set; }
        public DbSet<Deal> Deals { get; set; }
        public DbSet<Agent> Agents { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Demand> Demands { get; set; }
        public DbSet<RealEstateType> RealEstateTypes { get; set; }
        public DbSet<ApartmentDemand> ApartmentDemands { get; set; }
        public DbSet<HouseDemand> HouseDemands { get; set; }
        public DbSet<LandDemand> LandDemands { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=RealEstateDB;Trusted_Connection=True;TrustServerCertificate=True;");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Agent>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            // Конфигурация Apartment
            modelBuilder.Entity<Apartment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasOne(a => a.RealEstate)
                      .WithOne(r => r.Apartment)
                      .HasForeignKey<RealEstate>(r => r.ApartmentId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Конфигурация ApartmentDemand
            modelBuilder.Entity<ApartmentDemand>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            // Конфигурация Client
            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            // Конфигурация Deal (исправленная)
            modelBuilder.Entity<Deal>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasOne(d => d.Demand)
                      .WithOne(d => d.Deal)
                      .HasForeignKey<Deal>(d => d.DemandId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Supply)
                      .WithOne(s => s.Deal)
                      .HasForeignKey<Deal>(d => d.SupplyId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Конфигурация Demand (исправленная)
            modelBuilder.Entity<Demand>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasOne(d => d.Agent)
                      .WithMany(a => a.Demands)
                      .HasForeignKey(d => d.AgentId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Client)
                      .WithMany(c => c.Demands)
                      .HasForeignKey(d => d.ClientId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.RealEstateType)
                      .WithMany(t => t.Demands)
                      .HasForeignKey(d => d.TypeId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.ApartmentDemand)
                      .WithOne()
                      .HasForeignKey<Demand>(d => d.ApartmentDemandId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.HouseDemand)
                      .WithOne()
                      .HasForeignKey<Demand>(d => d.HouseDemandId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.LandDemand)
                      .WithOne()
                      .HasForeignKey<Demand>(d => d.LandDemandId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Конфигурация District
            modelBuilder.Entity<District>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            // Конфигурация House
            modelBuilder.Entity<House>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasOne(h => h.RealEstate)
                      .WithOne(r => r.House)
                      .HasForeignKey<RealEstate>(r => r.HouseId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Конфигурация HouseDemand
            modelBuilder.Entity<HouseDemand>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            // Конфигурация Land
            modelBuilder.Entity<Land>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasOne(l => l.RealEstate)
                      .WithOne(r => r.Land)
                      .HasForeignKey<RealEstate>(r => r.LandId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Конфигурация LandDemand
            modelBuilder.Entity<LandDemand>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            // Конфигурация RealEstateType
            modelBuilder.Entity<RealEstateType>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.HasIndex(e => e.TypeName).IsUnique();
            });

            // Конфигурация RealEstate (исправленная)
            modelBuilder.Entity<RealEstate>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasOne(r => r.District)
                      .WithMany(d => d.RealEstates)
                      .HasForeignKey(r => r.DistrictId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // Конфигурация Supply (исправленная)
            modelBuilder.Entity<Supply>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasOne(s => s.Agent)
                      .WithMany(a => a.Supplies)
                      .HasForeignKey(s => s.AgentId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(s => s.Client)
                      .WithMany(c => c.Supplies)
                      .HasForeignKey(s => s.ClientId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(s => s.RealEstate)
                      .WithMany(r => r.Supplies)
                      .HasForeignKey(s => s.RealEstateId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

        }
    }
}
