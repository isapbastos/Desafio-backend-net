
using InvestimentosJwt.Application.SimulacaoService.Models;
using InvestimentosJwt.Domain.Entities;
using InvestimentosJwt.Infra.Data.Sql.ProdutoRepository;
using InvestimentosJwt.Infra.Data.Sql.SimulacaoRepository;

namespace InvestimentosJwt.Application.SimulacaoService;
public class SimulacaoService : ISimulacaoService
{
    private readonly ISimulacaoRepository _simulacaoRepository;
    private readonly IProdutoRepository _produtoRepository;
    private readonly ISimulacaoCalculator _calculator;

    public SimulacaoService(
        ISimulacaoRepository simulacaoRepository,
        IProdutoRepository produtoRepository,
        ISimulacaoCalculator calculator)
    {
        _simulacaoRepository = simulacaoRepository;
        _produtoRepository = produtoRepository;
        _calculator = calculator;
    }

    /// <summary>
    /// Realiza a simulação de um investimento aplicando juros compostos mensais
    /// com base no produto escolhido pelo cliente.
    /// </summary>
    /// <param name="request">
    /// Objeto contendo ClienteId, Valor inicial, Prazo em meses
    /// e o Tipo do Produto a ser utilizado na simulação.
    /// </param>
    /// <returns>
    /// Retorna um objeto <see cref="RetornoSimulacao"/> contendo:
    /// - Dados do produto utilizado  
    /// - Valor final do investimento (juros compostos)  
    /// - Rentabilidade efetiva no período  
    /// - Data da simulação  
    /// Em caso de falha, retorna mensagem de erro.
    /// </returns>
    public async Task<RetornoSimulacao> RealizarSimulacao(SimulacaoRequest request)
    {
        if (request == null || request.Valor <= 0 || request.PrazoMeses <= 0)
            return RetornoSimulacao.Erro("Dados inválidos para simulação.");

        var produto = await _produtoRepository.ObterPorTipo(request.TipoProduto);

        if (produto == null)
            return RetornoSimulacao.Erro("Produto não encontrado.");

        var valorFinal = _calculator.CalcularValorFinal(
            request.Valor,
            produto.Rentabilidade,
            request.PrazoMeses
        );

        var rentabilidadeEfetiva = _calculator.CalcularRentabilidadeEfetiva(
            produto.Rentabilidade,
            request.PrazoMeses
        );

        var simulacao = new Simulacao
        {
            ClienteId = request.ClienteId,
            Produto = produto.Nome,
            ValorInvestido = request.Valor,
            ValorFinal = valorFinal,
            PrazoMeses = request.PrazoMeses,
            DataSimulacao = DateTime.UtcNow
        };

        await _simulacaoRepository.Adicionar(simulacao);

        var resposta = new
        {
            produtoValidado = produto,
            resultadoSimulacao = new
            {
                valorFinal,
                rentabilidadeEfetiva,
                prazoMeses = request.PrazoMeses
            },
            dataSimulacao = simulacao.DataSimulacao
        };

        return RetornoSimulacao.SucessoRetorno(resposta);
    }

    /// <summary>
    /// Retorna todas as simulações registradas no banco de dados.
    /// </summary>
    /// <returns>
    /// Uma lista de objetos <see cref="Simulacao"/> contendo
    /// todas as simulações realizadas pelos usuários.
    /// </returns>
    public async Task<IEnumerable<Simulacao>> ObterTodasSimulacoes()
    {
        return await _simulacaoRepository.ObterTodas();
    }

    /// <summary>
    /// Retorna estatísticas agregadas das simulações,
    /// agrupando por produto e data da simulação.
    /// </summary>
    /// <returns>
    /// Uma lista de <see cref="SimulacaoPorProdutoDiaDto"/> contendo:
    /// - Nome do produto  
    /// - Data  
    /// - Quantidade de simulações realizadas  
    /// - Média do valor final  
    /// </returns>
    public async Task<IEnumerable<SimulacaoPorProdutoDiaDto>> ObterSimulacoesPorProdutoDia()
    {
        return await _simulacaoRepository.ObterSimulacoesPorProdutoDia();
    }
}

