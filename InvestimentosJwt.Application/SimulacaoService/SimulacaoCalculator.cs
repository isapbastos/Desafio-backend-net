
namespace InvestimentosJwt.Application.SimulacaoService;

public class SimulacaoCalculator : ISimulacaoCalculator
{
    public decimal CalcularValorFinal(decimal valorInicial, decimal rentabilidadeAnual, int prazoMeses)
    {
        if (valorInicial <= 0 || rentabilidadeAnual <= 0 || prazoMeses <= 0)
            return 0;

        decimal taxaMensal = (decimal)Math.Pow((double)(1 + rentabilidadeAnual), 1.0 / 12.0) - 1;

        decimal valorFinal = valorInicial * (decimal)Math.Pow((double)(1 + taxaMensal), prazoMeses);

        return Math.Round(valorFinal, 2);
    }

    public decimal CalcularRentabilidadeEfetiva(decimal rentabilidadeAnual, int prazoMeses)
    {
        if (rentabilidadeAnual <= 0 || prazoMeses <= 0)
            return 0;

        decimal taxaMensal = (decimal)Math.Pow((double)(1 + rentabilidadeAnual), 1.0 / 12.0) - 1;

        decimal fator = (decimal)Math.Pow((double)(1 + taxaMensal), prazoMeses);

        return Math.Round(fator - 1, 4);
    }
}

