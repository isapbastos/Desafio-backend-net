using InvestimentosJwt.Domain.Entities;
using InvestimentosJwt.Infra.Data.Sql.ProdutoRepository;
using InvestimentosJwt.Infra.Data.Sql.SimulacaoRepository;

namespace InvestimentosJwt.Application.PerfilService;
public class PerfilService : IPerfilService
{
    private readonly ISimulacaoRepository _simulacaoRepository;
    private readonly IProdutoRepository _produtoRepository;

    public PerfilService(ISimulacaoRepository simulacaoRepository, IProdutoRepository produtoRepository)
    {
        _simulacaoRepository = simulacaoRepository;
        _produtoRepository = produtoRepository;
    }

    /// <summary>
    /// Calcula o perfil de risco do cliente com base no histórico de simulações.
    /// </summary>
    public async Task<(string perfil, double pontuacao, string descricao)> ObterPerfilRisco(int clienteId)
    {
        var simulacoesCliente = await _simulacaoRepository.ObterSimulacoesByClientId(clienteId);
        int produtosBaixo = 0;
        int produtosMedio = 0;
        int produtosAltos = 0;
        int produtosTotal = simulacoesCliente.Count;
        foreach (var simulacao in simulacoesCliente)
        {
            var produto = await _produtoRepository.ObterPorNome(simulacao.Produto);
            var risco = produto.Risco;
            if (risco.ToLower() == "baixo")
            {
                produtosBaixo++;
            }
            else if (risco.ToLower() == "médio")
            {
                produtosMedio++;
            }
            else
            {
                produtosAltos++;
            }
        }
        var pontuacao = ((produtosBaixo * 0.15 + produtosMedio * 0.35 + produtosAltos * 0.5) * 100) / produtosTotal;//media ponderada onde o produto de mais risco tem maior peso
        string perfil = pontuacao <= 15 ? "Conservador" : pontuacao < 50 ? "Moderado" : "Agressivo";
        string descricao = perfil == "Conservador" ? "Perfil com foco em liquidez e segurança." :
                           perfil == "Moderado" ? "Perfil equilibrado entre segurança e rentabilidade." :
                           "Perfil com maior apetite ao risco e busca por rentabilidade.";

        return (perfil, pontuacao, descricao);
    }

    /// <summary>
    /// Retorna produtos recomendados com base no perfil informado.
    /// </summary>
    public async Task<IEnumerable<Produto>> ObterProdutosRecomendados(string perfil)
    {
        var produtos = await _produtoRepository.ObterTodos();

        return perfil.ToLower() switch
        {
            "conservador" => produtos.Where(p => p.Risco.ToLower() == "baixo"),
            "moderado" => produtos.Where(p => p.Risco.ToLower() == "médio" || p.Risco.ToLower() == "baixo"),
            "agressivo" => produtos.Where(p => p.Risco.ToLower() == "alto"),
            _ => produtos
        };
    }
}