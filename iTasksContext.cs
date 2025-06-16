using iTasks.models.Tarefas;
using iTasks.models.Usuarios;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTasks
{
    public class iTasksContext : DbContext
    {
        public iTasksContext() : base("name=iTasksContext")
        {
            // Inicialização do banco se não existir
            Database.SetInitializer(new CreateDatabaseIfNotExists<iTasksContext>());
        }

        public DbSet<Utilizador> Utilizadores { get; set; }
        public DbSet<Programador> Programadores { get; set; }
        public DbSet<Gestor> Gestores { get; set; }
        public DbSet<TipoTarefa> TiposTarefa { get; set; }
        public DbSet<Tarefa> Tarefas { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Remove cascade delete
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            // Configura Table-Per-Type para herança
            modelBuilder.Entity<Utilizador>().ToTable("Utilizadores");
            modelBuilder.Entity<Gestor>().ToTable("Gestores");
            modelBuilder.Entity<Programador>().ToTable("Programadores");

            // Configurações específicas de relacionamentos
            modelBuilder.Entity<Programador>()
                .HasRequired(p => p.Gestor)
                .WithMany(g => g.Programadores)
                .HasForeignKey(p => p.GestorId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Tarefa>()
                .HasRequired(t => t.Gestor)
                .WithMany(g => g.Tarefas)
                .HasForeignKey(t => t.GestorId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Tarefa>()
                .HasRequired(t => t.Programador)
                .WithMany(p => p.TarefasAtribuidas)
                .HasForeignKey(t => t.ProgramadorId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Tarefa>()
                .HasRequired(t => t.TipoTarefa)
                .WithMany(tt => tt.Tarefas)
                .HasForeignKey(t => t.TipoTarefaId)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}
