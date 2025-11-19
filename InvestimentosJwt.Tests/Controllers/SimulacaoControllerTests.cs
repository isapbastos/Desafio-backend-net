using InvestimentosJwt.Application.SimulacaoService;
using InvestimentosJwt.Application.SimulacaoService.Models;
using InvestimentosJwt.Domain.Entities;
using InvestimentosJwtApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

public class SimulacaoControllerTests
{
    private readonly Mock<ISimulacaoService> _simulacaoServiceMock;
    private readonly SimulacaoController _controller;

    public SimulacaoControllerTests()
    {
        _simulacaoServiceMock = new Mock<ISimulacaoService>();
        _controller = new SimulacaoController(_simulacaoServiceMock.Object);
    }

    // ---------------------------------------------------------------------
    // POST /simular-investimento
    // ---------------------------------------------------------------------
    [Fact]
    public async Task SimularInvestimento_DeveRetornarOk_QuandoSucesso()
    {
        // Arrange
        var request = new SimulacaoRequest
        {
            ClienteId = 1,
            Valor = 1000,
            PrazoMeses = 12,
            TipoProduto = "CDB"
        };

        var dadosResultado = new { ValorFinal = 1500m };

        var retorno = RetornoSimulacao.SucessoRetorno(dadosResultado);

        _simulacaoServiceMock
            .Setup(s => s.RealizarSimulacao(request))
            .ReturnsAsync(retorno);

        // Act
        var result = await _controller.SimularInvestimento(request);

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, ok.StatusCode);
        Assert.Equal(dadosResultado, ok.Value);
    }

    [Fact]
    public async Task SimularInvestimento_DeveRetornarBadRequest_QuandoFalha()
    {
        // Arrange
        var request = new SimulacaoRequest();

        var retorno = RetornoSimulacao.Erro("Falha ao simular");

        _simulacaoServiceMock
            .Setup(s => s.RealizarSimulacao(request))
            .ReturnsAsync(retorno);

        // Act
        var result = await _controller.SimularInvestimento(request);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequest.StatusCode);
        Assert.Equal("Falha ao simular", badRequest.Value);
    }

    // ---------------------------------------------------------------------
    // GET /simulacoes
    // ---------------------------------------------------------------------
    [Fact]
    public async Task GetSimulacoes_DeveRetornarOk_ComListaDeSimulacoes()
    {
        // Arrange
        var lista = new List<Simulacao>
    {
        new Simulacao
        {
            Id = 1,
            ClienteId = 10,
            Produto = "CDB",
            ValorInvestido = 1000,
            ValorFinal = 1100,
            PrazoMeses = 12,
            DataSimulacao = DateTime.Now
        },
        new Simulacao
        {
            Id = 2,
            ClienteId = 20,
            Produto = "LCI",
            ValorInvestido = 2000,
            ValorFinal = 2300,
            PrazoMeses = 24,
            DataSimulacao = DateTime.Now
        }
    };

        _simulacaoServiceMock
            .Setup(s => s.ObterTodasSimulacoes())
            .ReturnsAsync(lista);

        // Act
        var result = await _controller.GetSimulacoes();

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, ok.StatusCode);

        var retorno = Assert.IsAssignableFrom<IEnumerable<Simulacao>>(ok.Value);

        Assert.Equal(2, retorno.Count());
    }


    // ---------------------------------------------------------------------
    // GET /simulacoes/por-produto-dia
    // ---------------------------------------------------------------------
    [Fact]
    public async Task GetSimulacoesPorProdutoDia_DeveRetornarOk()
    {
        // Arrange
        var agregacao = new List<SimulacaoPorProdutoDiaDto>
        {
            new SimulacaoPorProdutoDiaDto { Produto = "CDB", QuantidadeSimulacoes = 3, MediaValorFinal = 1200 },
            new SimulacaoPorProdutoDiaDto { Produto = "LCI", QuantidadeSimulacoes = 1, MediaValorFinal = 1600 }
        };

        _simulacaoServiceMock
            .Setup(s => s.ObterSimulacoesPorProdutoDia())
            .ReturnsAsync(agregacao);

        // Act
        var result = await _controller.GetSimulacoesPorProdutoDia();

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(agregacao, ok.Value);
    }
}
