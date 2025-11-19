using InvestimentosJwt.Domain.Entities;

namespace InvestimentosJwt.Infra.Data.Sql.TelemetriaRepository;
public class TelemetriaRepository : ITelemetriaRepository
{
    private readonly AppDbContext _context;

    public TelemetriaRepository(AppDbContext context)
    {
        _context = context;
    }

    public void AdicionarRegistro(TelemetriaRegistro registro)
    {
        _context.TelemetriaRegistros.Add(registro);
        _context.SaveChanges();
    }

    public IEnumerable<TelemetriaRegistro> ObterDadosPorPeriodo(DateTime inicio, DateTime fim)
    {
        return _context.TelemetriaRegistros
            .Where(r => r.DataRegistro >= inicio && r.DataRegistro <= fim)
            .ToList();
    }
}