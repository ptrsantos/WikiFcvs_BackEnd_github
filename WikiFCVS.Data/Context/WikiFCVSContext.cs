using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WikiFCVS.Domain.Models;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace WikiFCVS.Data.Context
{
    public class WikiFCVSContext : DbContext
    {
        public WikiFCVSContext(DbContextOptions<WikiFCVSContext> options) : base(options)
        {
        }

     

        public DbSet<Tema> Temas { get; set; }
        public DbSet<Artigo> Artigos { get; set; }
        public DbSet<EdicaoTema> EdicoesTemas { get; set; }
        public DbSet<EdicaoArtigo> EdicoesArtigos { get; set; }
        public DbSet<Protocolo> Protocolos { get; set; }
        public DbSet<RegistroUsuario> RegistroUsuarios { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetProperties()
                    .Where(p => p.ClrType == typeof(string))))
                property.Relational().ColumnType = "varchar(100)";

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(WikiFCVSContext).Assembly);

            
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.Cascade;



            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("DataCadastro") != null))
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("DataCadastro").CurrentValue = DateTime.Now;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property("DataCadastro").IsModified = false;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }


   
    }
}

