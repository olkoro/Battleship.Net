using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL
{
    public class AppDbContext : DbContext
    {

        public static readonly LoggerFactory MyLoggerFactory
            = new LoggerFactory(new[] {
                new ConsoleLoggerProvider((category, level)
                    => category == DbLoggerCategory.Database.Command.Name
                        && level == LogLevel.Information, true)
            });

        public DbSet<Save> Saves { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder
                .UseLoggerFactory(MyLoggerFactory)
                .UseMySQL(
                    "server=alpha.akaver.com;" +
                    "database=student2018_olkoro_Db;" +
                    "user=student2018;" +
                    "password=student2018");
//                .UseSqlServer(
//                "Server=(localdb)\\mssqllocaldb;" + // server to use
//                "Database=MyDatabase;" + // database to use or create
//                "Trusted_Connection=True;" + // no credentials needed, this is local sql instance
//                "MultipleActiveResultSets=true" // allow multiple  parallel queries
//                );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // configure entities
            modelBuilder.Entity<Save>()
                .HasIndex(i => i.SaveId);
            modelBuilder.Entity<State>().HasIndex(i => i.StateID);


            // remove Cascade delete from all the entities <- this has to be at the end
            foreach (var mutableForeignKey in 
                modelBuilder.Model.GetEntityTypes()
                    .Where(e => e.IsOwned() == false)
                    .SelectMany(e => e.GetForeignKeys()))
            {
                mutableForeignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}
