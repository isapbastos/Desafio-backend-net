using InvestimentosJwt.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InvestimentosJwt.Infra.Data.Sql.SimulacaoRepository;

public class SimulacaoRepository : ISimulacaoRepository
{
    private readonly AppDbContext _context;

    public SimulacaoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task Adicionar(Simulacao simulacao)
    {
        _context.Simulacoes.Add(simulacao);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Simulacao>> ObterTodas()
    {
        return await _context.Simulacoes.ToListAsync();
    }

    public async Task<IEnumerable<SimulacaoPorProdutoDiaDto>> ObterSimulacoesPorProdutoDia()
    {
        return await _context.Simulacoes
        .GroupBy(s => new { s.Produto, Data = s.DataSimulacao.Date })
        .Select(g => new SimulacaoPorProdutoDiaDto
        {
            Produto = g.Key.Produto,
            Data = g.Key.Data,
            QuantidadeSimulacoes = g.Count(),
            MediaValorFinal = (decimal)g.Average(x => (double)x.ValorFinal)
        })
        .ToListAsync();
    }

    public async Task<List<Simulacao>> ObterSimulacoesByClientId(int clienteId)
    {
        var simulacoesClient = await _context.Simulacoes.Where(s => s.ClienteId == clienteId).ToListAsync();
        return simulacoesClient;
    }
}
