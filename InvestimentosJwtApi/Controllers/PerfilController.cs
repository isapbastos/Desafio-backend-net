using InvestimentosJwt.Application.PerfilService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvestimentosJwtApi.Controllers;
/// <summary>
/// Controlador responsável por operações relacionadas ao perfil de risco e produtos recomendados.
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PerfilController : ControllerBase
{
    private readonly IPerfilService _service;

    public PerfilController(IPerfilService service)
    {
        _service = service;
    }

    /// <summary>
    /// Retorna o perfil de risco do cliente com base em dados financeiros simulados.
    /// </summary>
    /// <param name="clienteId">ID do cliente.</param>
    /// <returns>Perfil de risco com pontuação e descrição.</returns>
    /// <response code="200">Perfil encontrado com sucesso.</response>
    [HttpGet("perfil-risco/{clienteId}")]
    public async Task<IActionResult> GetPerfilRiscoAsync(int clienteId)
    {
        var (perfil, pontuacao, descricao) = await _service.ObterPerfilRisco(clienteId);
        return Ok(new { clienteId, perfil, pontuacao, descricao });
    }

    /// <summary>
    /// Retorna produtos recomendados com base no perfil informado.
    /// </summary>
    /// <param name="perfil">Perfil do cliente (Conservador, Moderado, Agressivo).</param>
    /// <returns>Lista de produtos recomendados.</returns>
    /// <response code="200">Produtos recomendados retornados com sucesso.</response>
    [HttpGet("produtos-recomendados/{perfil}")]
    public async Task<IActionResult> GetProdutosRecomendados(string perfil)
    {
        var recomendados = await _service.ObterProdutosRecomendados(perfil);
        return Ok(recomendados);
    }
}

