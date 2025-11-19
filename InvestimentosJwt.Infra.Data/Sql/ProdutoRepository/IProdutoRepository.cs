using InvestimentosJwt.Domain.Entities;

namespace InvestimentosJwt.Infra.Data.Sql.ProdutoRepository;
public interface IProdutoRepository
{
    Task<IEnumerable<Produto>> ObterTodos();
    Task<Produto?> ObterPorId(int id);
    Task<Produto?> ObterPorTipo(string tipoProduto);
    Task<Produto?> ObterPorNome(string nome);
}
