using ControleConta.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace BancoSA.Models
{
    public class BancoContext : DbContext
    {

        public DbSet<PessoaJuridica> PessoaJuridica { get; set; }
        public DbSet<PessoaFisica> PessoaFisica { get; set; }
        public DbSet<Pessoa> Pessoa { get; set; }
        public DbSet<Conta> Conta { get; set; }
        public DbSet<ContaMatriz> ContaMatriz { get; set; }
        public DbSet<ContaFilial> ContaFilial { get; set; }
        public DbSet<Aporte> Aporte { get; set; }
        public DbSet<Historico> Historico { get; set; }


        public BancoContext() : base("DbBancoSA")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public System.Data.Entity.DbSet<ControleConta.Models.Deposito> Depositoes { get; set; }

        public System.Data.Entity.DbSet<ControleConta.Models.Transferencia> Transferencias { get; set; }

        public System.Data.Entity.DbSet<ControleConta.Models.Estorno> Estornoes { get; set; }
    }
}