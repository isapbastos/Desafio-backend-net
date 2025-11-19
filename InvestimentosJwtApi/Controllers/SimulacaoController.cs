using InvestimentosJwt.Application.SimulacaoService;
using InvestimentosJwt.Application.SimulacaoService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvestimentosJwtApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class SimulacaoController : ControllerBase
{
    private readonly ISimulacaoService _simulacaoService;

    public SimulacaoController(ISimulacaoService simulacaoService)
    {
        _simulacaoService = simulacaoService;
    }

    /// <summary>
    /// Realiza a simulação de um investimento com base nos parâmetros informados. 
    /// </summary> 
    /// <param name="request">Objeto contendo ClienteId, Valor, PrazoMeses e TipoProduto.</param> 
    /// <returns>Retorna o produto validado e o resultado da simulação.</returns> 
    /// <response code="200">Simulação realizada com sucesso.</response> 
    /// <response code="400">Dados inválidos para simulação.</response> 
    /// <response code="404">Produto não encontrado.</response>
    [HttpPost("simular-investimento")]
    public async Task<IActionResult> SimularInvestimento([FromBody]SimulacaoRequest request)
    {
        var resultado = await _simulacaoService.RealizarSimulacao(request);

        if (!resultado.Sucesso)
            return BadRequest(resultado.Mensagem);

        return Ok(resultado.Dados);
    }

    /// <summary>
    /// Retorna todas as simulações realizadas.
    /// </summary>
    [HttpGet("simulacoes")]
    public async Task<IActionResult> GetSimulacoes()
    {
        var simulacoes = await _simulacaoService.ObterTodasSimulacoes();
        return Ok(simulacoes);
    }

    /// <summary>
    /// Retorna agregação de simulações por produto e dia.
    /// </summary>
    [HttpGet("simulacoes/por-produto-dia")]
    public async Task<IActionResult> GetSimulacoesPorProdutoDia()
    {
        var agregacao = await _simulacaoService.ObterSimulacoesPorProdutoDia();
        return Ok(agregacao);
    }
}
