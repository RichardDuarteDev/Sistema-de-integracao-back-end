using API.Model;
using Microsoft.EntityFrameworkCore;

namespace API.DAL
{
    public class Contexto : DbContext
    {

        public Contexto(DbContextOptions<Contexto> options) : base(options)
        {
        }

        public virtual DbSet<Cliente> Clientes { get; set; }

        public virtual DbSet<SituacaoCliente> SituacaoClientes { get; set; }

        public List<Cliente> ConsultarClientes()
        {
            return Clientes.FromSqlRaw("EXEC ConsultarClientes").ToList();
        }

        public void InserirCliente(Cliente cliente)
        {
            Database.ExecuteSqlRaw("EXEC InserirCliente @p0, @p1, @p2, @p3, @p4",
                cliente.Nome, cliente.Cpf, cliente.DataNascimento, cliente.Sexo, cliente.SituacaoClienteId);
        }

        public void AtualizarCliente(Cliente cliente)
        {
            Database.ExecuteSqlRaw("EXEC AtualizarCliente @p0, @p1, @p2, @p3, @p4, @p5",
                cliente.Id, cliente.Nome, cliente.Cpf, cliente.DataNascimento, cliente.Sexo, cliente.SituacaoClienteId);
        }

        public void DeletarCliente(int id)
        {
            Database.ExecuteSqlRaw("EXEC DeletarCliente @p0", id);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Clientes__3214EC074263A239");

                entity.HasIndex(e => e.Cpf, "UQ__Clientes__C1F89731157D81F1").IsUnique();

                entity.Property(e => e.Cpf)
                    .HasMaxLength(11)
                    .HasColumnName("CPF");
                entity.Property(e => e.DataNascimento).HasColumnType("date");
                entity.Property(e => e.Nome).HasMaxLength(100);
                entity.Property(e => e.Sexo)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.SituacaoCliente).WithMany(p => p.Clientes)
                    .HasForeignKey(d => d.SituacaoClienteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Clientes__Situac__4CA06362");
            });

            modelBuilder.Entity<SituacaoCliente>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Situacao__3214EC07EE0A4D63");

                entity.ToTable("SituacaoCliente");

                entity.Property(e => e.Descricao).HasMaxLength(50);
            });

           
        }

    

    }
}
