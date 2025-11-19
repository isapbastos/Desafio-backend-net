

namespace InvestimentosJwt.Application.SimulacaoService.Models;
public class SimulacaoRequest
{
    public int ClienteId { get; set; }
    public decimal Valor { get; set; }
    public int PrazoMeses { get; set; }
    public string TipoProduto { get; set; } = string.Empty;
}