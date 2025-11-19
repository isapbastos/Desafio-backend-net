
namespace InvestimentosJwt.Domain.Entities;

public class SimulacaoPorProdutoDiaDto
{
    public string Produto { get; set; }
    public DateTime Data { get; set; }
    public int QuantidadeSimulacoes { get; set; }
    public decimal MediaValorFinal { get; set; }
}

