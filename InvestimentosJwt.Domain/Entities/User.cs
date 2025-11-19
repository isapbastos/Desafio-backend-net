
using System.ComponentModel.DataAnnotations;

namespace InvestimentosJwt.Domain.Entities;
public class User
{
    [Key]
    public int Id { get; set; }
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty; // plain text as requested
}