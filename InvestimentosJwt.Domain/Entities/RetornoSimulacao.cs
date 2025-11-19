
namespace InvestimentosJwt.Domain.Entities;

public class RetornoSimulacao
{
    public bool Sucesso { get; private set; }
    public string Mensagem { get; private set; } = string.Empty;
    public object? Dados { get; private set; }

    private RetornoSimulacao(bool sucesso, string mensagem, object? dados)
    {
        Sucesso = sucesso;
        Mensagem = mensagem;
        Dados = dados;
    }

    public static RetornoSimulacao SucessoRetorno(object dados)
    {
        return new RetornoSimulacao(
            sucesso: true,
            mensagem: string.Empty,
            dados: dados
        );
    }

    public static RetornoSimulacao Erro(string mensagem)
    {
        return new RetornoSimulacao(
            sucesso: false,
            mensagem: mensagem,
            dados: null
        );
    }
}

