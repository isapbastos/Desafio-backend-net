using InvestimentosJwt.Application.SimulacaoService.Models;
using InvestimentosJwt.Domain.Entities;

namespace InvestimentosJwt.Application.SimulacaoService;
public interface ISimulacaoService
{
    Task<RetornoSimulacao> RealizarSimulacao(SimulacaoRequest request);
    Task<IEnumerable<Simulacao>> ObterTodasSimulacoes();
    Task<IEnumerable<SimulacaoPorProdutoDiaDto>> ObterSimulacoesPorProdutoDia();
}
