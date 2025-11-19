
namespace InvestimentosJwt.Application.SimulacaoService;
public interface ISimulacaoCalculator
{
    decimal CalcularValorFinal(decimal valorInicial, decimal rentabilidadeAnual, int prazoMeses);
    decimal CalcularRentabilidadeEfetiva(decimal rentabilidadeAnual, int prazoMeses);
}
