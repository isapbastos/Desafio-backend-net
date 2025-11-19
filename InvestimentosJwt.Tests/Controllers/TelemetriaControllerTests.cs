using InvestimentosJwt.Application.TelemetriaService;
using InvestimentosJwtApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace InvestimentosJwt.Tests.Controllers;
public class TelemetriaControllerTests
{
    private readonly Mock<ITelemetriaService> _mockService;
    private readonly TelemetriaController _controller;

    public TelemetriaControllerTests()
    {
        _mockService = new Mock<ITelemetriaService>();
        _controller = new TelemetriaController(_mockService.Object);
    }

    [Fact]
    public void GetTelemetria_DeveRetornarOk_ComRelatorio()
    {
        // Arrange
        var inicio = new DateTime(2025, 01, 01);
        var fim = new DateTime(2025, 01, 31);

        var relatorioMock = new
        {
            servicos = new[]
            {
                new { nome = "SimulacaoService", quantidadeChamadas = 10, mediaTempoRespostaMs = 120.5 },
                new { nome = "AuthService", quantidadeChamadas = 5, mediaTempoRespostaMs = 90.2 }
            },
            periodo = new { inicio = "2025-01-01", fim = "2025-01-31" }
        };

        _mockService
            .Setup(s => s.ObterRelatorio(inicio, fim))
            .Returns(relatorioMock);

        // Act
        var result = _controller.GetTelemetria(inicio, fim);

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, ok.StatusCode);
        Assert.Equal(relatorioMock, ok.Value);

        _mockService.Verify(s => s.ObterRelatorio(inicio, fim), Times.Once);
    }

    [Fact]
    public void GetTelemetria_DeveRetornarOk_MesmoSeRelatorioVazio()
    {
        // Arrange
        var inicio = DateTime.Today.AddDays(-7);
        var fim = DateTime.Today;

        var relatorioVazio = new
        {
            servicos = new object[] { },
            periodo = new { inicio = inicio.ToString("yyyy-MM-dd"), fim = fim.ToString("yyyy-MM-dd") }
        };

        _mockService
            .Setup(s => s.ObterRelatorio(inicio, fim))
            .Returns(relatorioVazio);

        // Act
        var result = _controller.GetTelemetria(inicio, fim);

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(relatorioVazio, ok.Value);
    }
}
