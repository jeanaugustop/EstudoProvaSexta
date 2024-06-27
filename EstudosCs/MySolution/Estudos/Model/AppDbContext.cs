namespace Estudos.Model;
using Microsoft.EntityFrameworkCore;
public class AppDbContext : DbContext
{
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Empresa> Empresas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=bancoTeste.db");
    }

}

