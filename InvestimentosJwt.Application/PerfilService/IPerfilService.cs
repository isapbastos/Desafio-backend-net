using InvestimentosJwt.Domain.Entities;

namespace InvestimentosJwt.Application.PerfilService;

public interface IPerfilService
{
    Task<(string perfil, double pontuacao, string descricao)> ObterPerfilRisco(int clienteId);
    Task<IEnumerable<Produto>> ObterProdutosRecomendados(string perfil);
}
