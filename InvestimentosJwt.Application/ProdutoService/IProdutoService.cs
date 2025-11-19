using InvestimentosJwt.Domain.Entities;

namespace InvestimentosJwt.Application.ProdutoService;
public interface IProdutoService
{
    Task<IEnumerable<Produto>> ListarProdutos();
    Task<Produto?> ObterProduto(int id);
    Task<Produto?> ObterPorTipo(string tipo);
}
