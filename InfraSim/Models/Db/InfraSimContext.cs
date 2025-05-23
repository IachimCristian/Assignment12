using Microsoft.EntityFrameworkCore;
using System.IO;
using System;

namespace InfraSim.Models.Db
{
    public class InfraSimContext : DbContext
    {
        private static readonly string DbFileName = "InfraSim.db";
        private static readonly string DbPath;
        
        static InfraSimContext()
        {
            try
            {
                string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string infraSimFolder = Path.Combine(appDataPath, "InfraSim");
                
                if (!Directory.Exists(infraSimFolder))
                {
                    Directory.CreateDirectory(infraSimFolder);
                }
                
                DbPath = Path.Combine(infraSimFolder, DbFileName);
                Console.WriteLine($"Database initialized at fixed path: {DbPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing database path: {ex.Message}");
                DbPath = Path.Combine(Directory.GetCurrentDirectory(), DbFileName);
            }
        }
        
        public DbSet<DbServer> DbServers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            try
            {
                bool exists = File.Exists(DbPath);
                
                Console.WriteLine($"=== DATABASE INFO ===");
                Console.WriteLine($"Database path: {DbPath}");
                Console.WriteLine($"Database file exists: {exists}");
                
                options.UseSqlite($"Data Source={DbPath}");
                Console.WriteLine("Database configured successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR configuring database: {ex.Message}");
            }
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            try
            {
                modelBuilder.Entity<DbServer>()
                    .HasKey(s => s.Id);
                
                modelBuilder.Entity<DbServer>()
                    .Property(s => s.ParentId)
                    .IsRequired(false);
                
                modelBuilder.Entity<DbServer>()
                    .HasIndex(s => s.ParentId);
                
                Console.WriteLine("Model configured successfully");
                base.OnModelCreating(modelBuilder);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR configuring model: {ex.Message}");
                throw;
            }
        }
    }
} 