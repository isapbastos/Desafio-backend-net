using FluentAssertions;
using InvestimentosJwt.Application.SimulacaoService;
using Xunit;

namespace InvestimentosJwt.Tests.Services;

public class SimulacaoCalculatorTests
{
    private readonly SimulacaoCalculator _calculator;

    public SimulacaoCalculatorTests()
    {
        _calculator = new SimulacaoCalculator();
    }

    // ---------------------------
    // CalcularValorFinal
    // ---------------------------
    [Theory]
    [InlineData(0, 0.10, 10)]
    [InlineData(1000, 0, 10)]
    [InlineData(1000, 0.10, 0)]
    [InlineData(-100, 0.10, 10)]
    public void CalcularValorFinal_DeveRetornarZero_QuandoParametrosInvalidos(
        decimal valorInicial, decimal rentabilidadeAnual, int prazo)
    {
        var result = _calculator.CalcularValorFinal(valorInicial, rentabilidadeAnual, prazo);

        result.Should().Be(0);
    }

    // ---------------------------
    // CalcularRentabilidadeEfetiva
    // ---------------------------
    [Theory]
    [InlineData(0, 10)]
    [InlineData(0.10, 0)]
    [InlineData(-1, 10)]
    public void CalcularRentabilidadeEfetiva_DeveRetornarZero_QuandoParametrosInvalidos(
        decimal rentabilidade, int prazo)
    {
        var result = _calculator.CalcularRentabilidadeEfetiva(rentabilidade, prazo);
        result.Should().Be(0);
    }

    [Fact]
    public void CalcularRentabilidadeEfetiva_DeveCalcularCorretamente()
    {
        // Arrange
        decimal rentabilidade = 0.12m; // 12% ao ano
        int prazoMeses = 12;

        // Act
        var result = _calculator.CalcularRentabilidadeEfetiva(rentabilidade, prazoMeses);

        // Assert
        result.Should().BeApproximately(0.1132m, 0.0068m);
    }
}
