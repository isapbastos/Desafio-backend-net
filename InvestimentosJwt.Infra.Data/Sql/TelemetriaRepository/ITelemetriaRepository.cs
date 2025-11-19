using InvestimentosJwt.Domain.Entities;

namespace InvestimentosJwt.Infra.Data.Sql.TelemetriaRepository;

public interface ITelemetriaRepository
{
    void AdicionarRegistro(TelemetriaRegistro registro);
    IEnumerable<TelemetriaRegistro> ObterDadosPorPeriodo(DateTime inicio, DateTime fim);
}