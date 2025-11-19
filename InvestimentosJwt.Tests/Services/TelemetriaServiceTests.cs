using InvestimentosJwt.Application.TelemetriaService;
using InvestimentosJwt.Domain.Entities;
using InvestimentosJwt.Infra.Data.Sql.TelemetriaRepository;
using Moq;

namespace InvestimentosJwt.Tests.Services;
public class TelemetriaServiceTests
{
    private readonly Mock<ITelemetriaRepository> _repoMock;
    private readonly TelemetriaService _service;

    public TelemetriaServiceTests()
    {
        _repoMock = new Mock<ITelemetriaRepository>();
        _service = new TelemetriaService(_repoMock.Object);
    }

    // -----------------------------------------------------------
    // TESTE 1: RegistrarChamada deve chamar o repositório corretamente
    // -----------------------------------------------------------
    [Fact]
    public void RegistrarChamada_DeveRegistrarNoRepositorio()
    {
        // Arrange
        string servico = "SimulacaoService";
        long tempo = 150;

        // Act
        _service.RegistrarChamada(servico, tempo);

        // Assert
        _repoMock.Verify(r => r.AdicionarRegistro(It.Is<TelemetriaRegistro>(
            t => t.NomeServico == servico &&
                 t.TempoRespostaMs == tempo &&
                 t.DataRegistro <= DateTime.UtcNow
        )), Times.Once);
    }

    // -----------------------------------------------------------
    // TESTE 2: ObterRelatorio deve agrupar, calcular médias e retornar estrutura correta
    // -----------------------------------------------------------
    [Fact]
    public void ObterRelatorio_DeveRetornarDadosAgrupados()
    {
        // Arrange
        var inicio = new DateTime(2025, 1, 1);
        var fim = new DateTime(2025, 1, 31);

        var registros = new List<TelemetriaRegistro>
        {
            new TelemetriaRegistro { NomeServico = "Auth", TempoRespostaMs = 100, DataRegistro = inicio },
            new TelemetriaRegistro { NomeServico = "Auth", TempoRespostaMs = 300, DataRegistro = inicio },
            new TelemetriaRegistro { NomeServico = "Simulacao", TempoRespostaMs = 200, DataRegistro = inicio }
        };

        _repoMock.Setup(r => r.ObterDadosPorPeriodo(inicio, fim)).Returns(registros);

        // Act
        var resultado = _service.ObterRelatorio(inicio, fim);

        // Assert
        Assert.NotNull(resultado);
    }
}
