using System.ComponentModel.DataAnnotations;

namespace InvestimentosJwt.Domain.Entities;
public class TelemetriaRegistro
{
    [Key]
    public int Id { get; set; }
    public string NomeServico { get; set; } = string.Empty;
    public long TempoRespostaMs { get; set; }
    public DateTime DataRegistro { get; set; }
}