namespace InvestimentosJwt.Application.TelemetriaService;
public interface ITelemetriaService
{
    void RegistrarChamada(string nomeServico, long tempoRespostaMs);
    object ObterRelatorio(System.DateTime inicio, System.DateTime fim);
}