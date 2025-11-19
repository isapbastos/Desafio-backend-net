using InvestimentosJwt.Domain.Entities;

namespace InvestimentosJwt.Infra.Data.Sql.SimulacaoRepository;

public interface ISimulacaoRepository
{
    Task Adicionar(Simulacao simulacao);
    Task<IEnumerable<Simulacao>> ObterTodas();
    Task<IEnumerable<SimulacaoPorProdutoDiaDto>> ObterSimulacoesPorProdutoDia();
    Task<List<Simulacao>> ObterSimulacoesByClientId(int clienteId);
}
