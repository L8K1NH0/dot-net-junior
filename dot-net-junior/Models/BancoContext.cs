using Microsoft.EntityFrameworkCore;
using dot_net_junior.Models;

namespace dot_net_junior.Models
{
    public class BancoContext : DbContext
    {
        public BancoContext(DbContextOptions<BancoContext> options) : base(options)
        {

        }
        public DbSet<dot_net_junior.Models.Cliente> Cliente { get; set; } = default!;
        public DbSet<dot_net_junior.Models.Endereco> Endereco { get; set; } = default!;
        public DbSet<dot_net_junior.Models.ContatoModel> Contato { get; set; } = default!;


    }
}
