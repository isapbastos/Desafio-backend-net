using InvestimentosJwt.Domain.Entities;
using InvestimentosJwt.Infra.Data.Sql.ProdutoRepository;

namespace InvestimentosJwt.Application.ProdutoService;

public class ProdutoService : IProdutoService
{
    private readonly IProdutoRepository _repository;

    public ProdutoService(IProdutoRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Produto>> ListarProdutos()
    {
        return await _repository.ObterTodos();
    }

    public async Task<Produto?> ObterProduto(int id)
    {
        return await _repository.ObterPorId(id);
    }

    public async Task<Produto?> ObterPorTipo(string tipo)
    {
        return await _repository.ObterPorTipo(tipo);
    }
}
