using InvestimentosJwt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestimentosJwt.Infra.Data.Sql;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Simulacao> Simulacoes { get; set; }
    public DbSet<TelemetriaRegistro> TelemetriaRegistros { get; set; }
}
