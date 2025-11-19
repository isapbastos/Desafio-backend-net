using System.ComponentModel.DataAnnotations;

namespace InvestimentosJwt.Domain.Entities;
public class Produto
{
    [Key]
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public decimal Rentabilidade { get; set; }
    public string Risco { get; set; } = string.Empty;
}