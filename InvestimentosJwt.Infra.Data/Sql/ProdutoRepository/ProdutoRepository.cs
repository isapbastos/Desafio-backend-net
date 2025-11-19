using InvestimentosJwt.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InvestimentosJwt.Infra.Data.Sql.ProdutoRepository;
public class ProdutoRepository : IProdutoRepository
{
    private readonly AppDbContext _context;

    public ProdutoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Produto>> ObterTodos()
    {
        return await _context.Produtos.ToListAsync();
    }

    public async Task<Produto?> ObterPorId(int id)
    {
        return await _context.Produtos
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Produto?> ObterPorTipo(string tipoProduto)
    {
        return await _context.Produtos
            .FirstOrDefaultAsync(p => p.Tipo == tipoProduto);
    }

    public async Task<Produto?> ObterPorNome(string nome)
    {
        return await _context.Produtos
            .FirstOrDefaultAsync(p => p.Nome == nome);
    }
}
