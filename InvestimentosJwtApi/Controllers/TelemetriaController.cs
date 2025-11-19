using InvestimentosJwt.Application.TelemetriaService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvestimentosJwtApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TelemetriaController : ControllerBase
{
    private readonly ITelemetriaService _service;

    public TelemetriaController(ITelemetriaService service)
    {
        _service = service;
    }

    /// <summary>
    /// Retorna dados de telemetria com volumes e tempos médios por serviço.
    /// </summary>
    /// <param name="inicio">Data inicial do período.</param>
    /// <param name="fim">Data final do período.</param>
    /// <returns>Relatório de telemetria.</returns>
    [HttpGet]
    public IActionResult GetTelemetria([FromQuery] DateTime inicio, [FromQuery] DateTime fim)
    {
        var relatorio = _service.ObterRelatorio(inicio, fim);
        return Ok(relatorio);
    }
}