using InvestimentosJwt.Application.PerfilService;
using InvestimentosJwt.Domain.Entities;
using InvestimentosJwt.Infra.Data.Sql.ProdutoRepository;
using InvestimentosJwt.Infra.Data.Sql.SimulacaoRepository;
using Moq;

namespace InvestimentosJwt.Tests.Services;
public class PerfilServiceTests
{
    private readonly Mock<ISimulacaoRepository> _simulacaoRepoMock;
    private readonly Mock<IProdutoRepository> _produtoRepoMock;
    private readonly PerfilService _service;

    public PerfilServiceTests()
    {
        _simulacaoRepoMock = new Mock<ISimulacaoRepository>();
        _produtoRepoMock = new Mock<IProdutoRepository>();
        _service = new PerfilService(_simulacaoRepoMock.Object, _produtoRepoMock.Object);
    }

    // --------------------------
    //      PERFIL CONSERVADOR
    // --------------------------
    [Fact]
    public async Task ObterPerfilRisco_DeveRetornarConservador()
    {
        var simulacoes = new List<Simulacao>
        {
            new Simulacao { Produto = "Tesouro Selic" },
            new Simulacao { Produto = "CDB Conservador" }
        };

        _simulacaoRepoMock
            .Setup(r => r.ObterSimulacoesByClientId(1))
            .ReturnsAsync(simulacoes);

        _produtoRepoMock.Setup(p => p.ObterPorNome("Tesouro Selic"))
            .ReturnsAsync(new Produto { Nome = "Tesouro Selic", Risco = "Baixo" });

        _produtoRepoMock.Setup(p => p.ObterPorNome("CDB Conservador"))
            .ReturnsAsync(new Produto { Nome = "CDB Conservador", Risco = "Baixo" });

        var resultado = await _service.ObterPerfilRisco(1);

        Assert.Equal("Conservador", resultado.perfil);
        Assert.Equal("Perfil com foco em liquidez e segurança.", resultado.descricao);
    }

    // --------------------------
    //      PERFIL MODERADO
    // --------------------------
    [Fact]
    public async Task ObterPerfilRisco_DeveRetornarModerado()
    {
        var simulacoes = new List<Simulacao>
        {
            new Simulacao { Produto = "CDB Médio" },
            new Simulacao { Produto = "Tesouro Selic" },
            new Simulacao { Produto = "CDB Médio" }
        };

        _simulacaoRepoMock
            .Setup(r => r.ObterSimulacoesByClientId(2))
            .ReturnsAsync(simulacoes);

        _produtoRepoMock.Setup(p => p.ObterPorNome("CDB Médio"))
            .ReturnsAsync(new Produto { Nome = "CDB Médio", Risco = "Médio" });

        _produtoRepoMock.Setup(p => p.ObterPorNome("Tesouro Selic"))
            .ReturnsAsync(new Produto { Nome = "Tesouro Selic", Risco = "Baixo" });

        var resultado = await _service.ObterPerfilRisco(2);

        Assert.Equal("Moderado", resultado.perfil);
        Assert.Equal("Perfil equilibrado entre segurança e rentabilidade.", resultado.descricao);
    }

    // --------------------------
    //      PERFIL AGRESSIVO
    // --------------------------
    [Fact]
    public async Task ObterPerfilRisco_DeveRetornarAgressivo()
    {
        var simulacoes = new List<Simulacao>
        {
            new Simulacao { Produto = "Ações X" },
            new Simulacao { Produto = "Ações Y" }
        };

        _simulacaoRepoMock
            .Setup(r => r.ObterSimulacoesByClientId(3))
            .ReturnsAsync(simulacoes);

        _produtoRepoMock.Setup(p => p.ObterPorNome(It.IsAny<string>()))
            .ReturnsAsync(new Produto { Nome = "Ações X", Risco = "Alto" });

        var resultado = await _service.ObterPerfilRisco(3);

        Assert.Equal("Agressivo", resultado.perfil);
        Assert.Equal("Perfil com maior apetite ao risco e busca por rentabilidade.", resultado.descricao);
    }

    // --------------------------
    //   TESTE PRODUTOS RECOMENDADOS
    // --------------------------

    [Fact]
    public async Task ObterProdutosRecomendados_Conservador()
    {
        var produtos = new List<Produto>
        {
            new Produto { Nome = "Tesouro", Risco = "Baixo" },
            new Produto { Nome = "CDB Médio", Risco = "Médio" },
            new Produto { Nome = "Ações", Risco = "Alto" }
        };

        _produtoRepoMock.Setup(p => p.ObterTodos())
            .ReturnsAsync(produtos);

        var recomendados = await _service.ObterProdutosRecomendados("conservador");

        Assert.Single(recomendados);
        Assert.Equal("Baixo", recomendados.First().Risco);
    }

    [Fact]
    public async Task ObterProdutosRecomendados_Moderado()
    {
        var produtos = new List<Produto>
        {
            new Produto { Nome = "Tesouro", Risco = "Baixo" },
            new Produto { Nome = "CDB Médio", Risco = "Médio" },
            new Produto { Nome = "Ações", Risco = "Alto" }
        };

        _produtoRepoMock.Setup(p => p.ObterTodos())
            .ReturnsAsync(produtos);

        var recomendados = await _service.ObterProdutosRecomendados("moderado");

        Assert.Equal(2, recomendados.Count());
    }

    [Fact]
    public async Task ObterProdutosRecomendados_Agressivo()
    {
        var produtos = new List<Produto>
        {
            new Produto { Nome = "Tesouro", Risco = "Baixo" },
            new Produto { Nome = "CDB Médio", Risco = "Médio" },
            new Produto { Nome = "Ações", Risco = "Alto" }
        };

        _produtoRepoMock.Setup(p => p.ObterTodos())
            .ReturnsAsync(produtos);

        var recomendados = await _service.ObterProdutosRecomendados("agressivo");

        Assert.Single(recomendados);
        Assert.Equal("Alto", recomendados.First().Risco);
    }
}
