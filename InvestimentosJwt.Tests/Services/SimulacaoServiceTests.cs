using InvestimentosJwt.Application.SimulacaoService;
using InvestimentosJwt.Application.SimulacaoService.Models;
using InvestimentosJwt.Domain.Entities;
using InvestimentosJwt.Infra.Data.Sql.ProdutoRepository;
using InvestimentosJwt.Infra.Data.Sql.SimulacaoRepository;
using Moq;

namespace InvestimentosJwt.Tests.Services;
public class SimulacaoServiceTests
{
    private readonly Mock<ISimulacaoRepository> _repo = new();
    private readonly Mock<IProdutoRepository> _produtoRepo = new();
    private readonly Mock<ISimulacaoCalculator> _calc = new();

    private SimulacaoService CriarService() =>
        new SimulacaoService(_repo.Object, _produtoRepo.Object, _calc.Object);

    [Fact]
    public async Task RealizarSimulacao_DeveRetornarErro_QuandoRequestInvalido()
    {
        var service = CriarService();

        var result = await service.RealizarSimulacao(new SimulacaoRequest
        {
            ClienteId = 1,
            Valor = 0,
            PrazoMeses = 0
        });

        Assert.False(result.Sucesso);
    }

    [Fact]
    public async Task RealizarSimulacao_DeveRetornarErro_QuandoProdutoNaoExiste()
    {
        _produtoRepo.Setup(r => r.ObterPorTipo("CDB"))
            .ReturnsAsync((Produto?)null);

        var service = CriarService();

        var result = await service.RealizarSimulacao(new SimulacaoRequest
        {
            ClienteId = 1,
            Valor = 1000,
            PrazoMeses = 12,
            TipoProduto = "CDB"
        });

        Assert.False(result.Sucesso);
        Assert.Equal("Produto não encontrado.", result.Mensagem);
    }

    [Fact]
    public async Task RealizarSimulacao_DeveSalvarSimulacaoEDevolverSucesso()
    {
        var produto = new Produto { Nome = "CDB", Rentabilidade = 0.10m };

        _produtoRepo.Setup(r => r.ObterPorTipo("CDB"))
            .ReturnsAsync(produto);

        _calc.Setup(c => c.CalcularValorFinal(1000, 0.10m, 12))
            .Returns(1120m);

        _calc.Setup(c => c.CalcularRentabilidadeEfetiva(0.10m, 12))
            .Returns(0.11m);

        var service = CriarService();

        var result = await service.RealizarSimulacao(new SimulacaoRequest
        {
            ClienteId = 1,
            Valor = 1000,
            PrazoMeses = 12,
            TipoProduto = "CDB"
        });

        Assert.True(result.Sucesso);

        _repo.Verify(r => r.Adicionar(It.IsAny<Simulacao>()), Times.Once);
    }

    [Fact]
    public async Task ObterTodasSimulacoes_DeveChamarRepositorio()
    {
        _repo.Setup(r => r.ObterTodas())
            .ReturnsAsync(new List<Simulacao> { new Simulacao() });

        var service = CriarService();
        var result = await service.ObterTodasSimulacoes();

        Assert.Single(result);
    }
}
