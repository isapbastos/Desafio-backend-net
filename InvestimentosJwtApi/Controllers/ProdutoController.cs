using InvestimentosJwt.Application.ProdutoService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvestimentosJwtApi.Controllers;

/// <summary>
/// Controlador responsável por operações relacionadas aos produtos de investimento.
/// </summary>
[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ProdutoController : ControllerBase
{
    private readonly IProdutoService _service;

    public ProdutoController(IProdutoService service)
    {
        _service = service;
    }

    /// <summary>
    /// Retorna a lista de todos os produtos de investimento disponíveis.
    /// </summary>
    /// <returns>Lista de produtos.</returns>
    /// <response code="200">Lista retornada com sucesso.</response>
    [HttpGet("produtos")]
    public async Task<IActionResult> GetProdutos()
    {
        var produtos = await _service.ListarProdutos();
        return Ok(produtos);
    }

    /// <summary>
    /// Retorna as informações de um produto específico.
    /// </summary>
    /// <param name="id">ID do produto desejado.</param>
    /// <returns>Objeto contendo os dados do produto.</returns>
    /// <response code="200">Produto encontrado.</response>
    /// <response code="404">Produto não encontrado.</response>
    [HttpGet("produtos/{id}")]
    public async Task<IActionResult> GetProduto(int id)
    {
        var produto = await _service.ObterProduto(id);
        if (produto == null)
            return NotFound("Produto não encontrado.");

        return Ok(produto);
    }
}
