using InvestimentosJwt.Domain.Entities;
using InvestimentosJwt.Infra.Data.Sql;
using InvestimentosJwt.Infra.Data.Sql.SimulacaoRepository;
using Microsoft.EntityFrameworkCore;

namespace InvestimentosJwt.Tests.Sql.Repository;
public class SimulacaoRepositoryTests
{
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) // BD isolado por teste
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task Adicionar_DeveSalvarSimulacao()
    {
        var context = GetDbContext();
        var repo = new SimulacaoRepository(context);

        var simulacao = new Simulacao
        {
            ClienteId = 1,
            Produto = "CDB",
            ValorFinal = 100,
            DataSimulacao = DateTime.UtcNow
        };

        await repo.Adicionar(simulacao);
        var result = await context.Simulacoes.ToListAsync();

        Assert.Single(result);
        Assert.Equal("CDB", result[0].Produto);
    }

    [Fact]
    public async Task ObterTodas_DeveRetornarTodasSimulacoes()
    {
        var context = GetDbContext();
        var repo = new SimulacaoRepository(context);

        context.Simulacoes.AddRange(
            new Simulacao { ClienteId = 1, Produto = "CDB", ValorFinal = 100, DataSimulacao = DateTime.UtcNow },
            new Simulacao { ClienteId = 2, Produto = "LCI", ValorFinal = 200, DataSimulacao = DateTime.UtcNow }
        );
        await context.SaveChangesAsync();

        var result = await repo.ObterTodas();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task ObterSimulacoesPorProdutoDia_DeveAgruparCorretamente()
    {
        var context = GetDbContext();
        var repo = new SimulacaoRepository(context);

        var data = DateTime.UtcNow.Date;

        context.Simulacoes.AddRange(
            new Simulacao { ClienteId = 1, Produto = "CDB", ValorFinal = 100, DataSimulacao = data },
            new Simulacao { ClienteId = 2, Produto = "CDB", ValorFinal = 200, DataSimulacao = data },
            new Simulacao { ClienteId = 3, Produto = "LCI", ValorFinal = 300, DataSimulacao = data }
        );
        await context.SaveChangesAsync();

        var result = await repo.ObterSimulacoesPorProdutoDia();
        var list = result.ToList();

        Assert.Equal(2, list.Count); // dois produtos agrupados

        var cdb = list.First(g => g.Produto == "CDB");
        Assert.Equal(2, cdb.QuantidadeSimulacoes);
        Assert.Equal(150, cdb.MediaValorFinal);

        var lci = list.First(g => g.Produto == "LCI");
        Assert.Equal(1, lci.QuantidadeSimulacoes);
        Assert.Equal(300, lci.MediaValorFinal);
    }

    [Fact]
    public async Task ObterSimulacoesByClientId_DeveRetornarSomenteDoCliente()
    {
        var context = GetDbContext();
        var repo = new SimulacaoRepository(context);

        context.Simulacoes.AddRange(
            new Simulacao { ClienteId = 1, Produto = "CDB", ValorFinal = 100, DataSimulacao = DateTime.UtcNow },
            new Simulacao { ClienteId = 1, Produto = "LCI", ValorFinal = 200, DataSimulacao = DateTime.UtcNow },
            new Simulacao { ClienteId = 2, Produto = "CDB", ValorFinal = 300, DataSimulacao = DateTime.UtcNow }
        );
        await context.SaveChangesAsync();

        var result = await repo.ObterSimulacoesByClientId(1);

        Assert.Equal(2, result.Count);
        Assert.All(result, s => Assert.Equal(1, s.ClienteId));
    }
}
