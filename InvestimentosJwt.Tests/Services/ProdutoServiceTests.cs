using InvestimentosJwt.Application.ProdutoService;
using InvestimentosJwt.Domain.Entities;
using InvestimentosJwt.Infra.Data.Sql.ProdutoRepository;
using Moq;

namespace InvestimentosJwt.Tests.Services;
public class ProdutoServiceTests
{
    private readonly Mock<IProdutoRepository> _repositoryMock;
    private readonly ProdutoService _service;

    public ProdutoServiceTests()
    {
        _repositoryMock = new Mock<IProdutoRepository>();
        _service = new ProdutoService(_repositoryMock.Object);
    }

    private Produto CriarProduto(int id, string tipo)
    {
        return new Produto
        {
            Id = id,
            Nome = $"Produto {id}",
            Tipo = tipo,
            Rentabilidade = 0.12m,
            Risco = "Baixo"
        };
    }

    // ---------------------------------------------------------------
    // LISTAR PRODUTOS
    // ---------------------------------------------------------------
    [Fact]
    public async Task ListarProdutos_DeveRetornarListaDeProdutos()
    {
        // Arrange
        var produtos = new List<Produto>
        {
            CriarProduto(1, "CDB"),
            CriarProduto(2, "LCI")
        };

        _repositoryMock
            .Setup(r => r.ObterTodos())
            .ReturnsAsync(produtos);

        // Act
        var resultado = await _service.ListarProdutos();

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(2, resultado.Count());
    }

    [Fact]
    public async Task ListarProdutos_DeveRetornarListaVazia_QuandoNaoExistirProdutos()
    {
        _repositoryMock
            .Setup(r => r.ObterTodos())
            .ReturnsAsync(new List<Produto>());

        var resultado = await _service.ListarProdutos();

        Assert.NotNull(resultado);
        Assert.Empty(resultado);
    }

    // ---------------------------------------------------------------
    // OBTER POR ID
    // ---------------------------------------------------------------
    [Fact]
    public async Task ObterProduto_DeveRetornarProduto_QuandoExistir()
    {
        var produto = CriarProduto(10, "CDB");

        _repositoryMock
            .Setup(r => r.ObterPorId(10))
            .ReturnsAsync(produto);

        var resultado = await _service.ObterProduto(10);

        Assert.NotNull(resultado);
        Assert.Equal(10, resultado!.Id);
        Assert.Equal("CDB", resultado.Tipo);
    }

    [Fact]
    public async Task ObterProduto_DeveRetornarNull_QuandoNaoExistir()
    {
        _repositoryMock
            .Setup(r => r.ObterPorId(100))
            .ReturnsAsync((Produto?)null);

        var resultado = await _service.ObterProduto(100);

        Assert.Null(resultado);
    }

    // ---------------------------------------------------------------
    // OBTER POR TIPO
    // ---------------------------------------------------------------
    [Fact]
    public async Task ObterPorTipo_DeveRetornarProduto_QuandoTipoExistir()
    {
        var produto = CriarProduto(5, "LCI");

        _repositoryMock
            .Setup(r => r.ObterPorTipo("LCI"))
            .ReturnsAsync(produto);

        var resultado = await _service.ObterPorTipo("LCI");

        Assert.NotNull(resultado);
        Assert.Equal("LCI", resultado!.Tipo);
    }

    [Fact]
    public async Task ObterPorTipo_DeveRetornarNull_QuandoTipoNaoExistir()
    {
        _repositoryMock
            .Setup(r => r.ObterPorTipo("TESOURO"))
            .ReturnsAsync((Produto?)null);

        var resultado = await _service.ObterPorTipo("TESOURO");

        Assert.Null(resultado);
    }
}
