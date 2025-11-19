
using InvestimentosJwt.Application.ProdutoService;
using InvestimentosJwt.Domain.Entities;
using InvestimentosJwtApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace InvestimentosJwt.Tests.Controllers;
public class ProdutoControllerTests
{
    private readonly Mock<IProdutoService> _serviceMock;
    private readonly ProdutoController _controller;

    public ProdutoControllerTests()
    {
        _serviceMock = new Mock<IProdutoService>();
        _controller = new ProdutoController(_serviceMock.Object);
    }

    private Produto CriarProduto(int id, string tipo)
    {
        return new Produto
        {
            Id = id,
            Nome = $"Produto {id}",
            Tipo = tipo,
            Rentabilidade = 0.1m,
            Risco = "Baixo"
        };
    }

    // -------------------------------------------------------------------------
    // GET /api/produto/produtos
    // -------------------------------------------------------------------------
    [Fact]
    public async Task GetProdutos_DeveRetornarOkComListaDeProdutos()
    {
        // Arrange
        var lista = new List<Produto>
        {
            CriarProduto(1, "CDB"),
            CriarProduto(2, "LCI")
        };

        _serviceMock
            .Setup(s => s.ListarProdutos())
            .ReturnsAsync(lista);

        // Act
        var result = await _controller.GetProdutos();

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(lista, ok.Value);
    }

    // -------------------------------------------------------------------------
    // GET /api/produto/produtos/{id}
    // -------------------------------------------------------------------------
    [Fact]
    public async Task GetProduto_DeveRetornarOk_QuandoEncontrado()
    {
        var produto = CriarProduto(10, "CDB");

        _serviceMock
            .Setup(s => s.ObterProduto(10))
            .ReturnsAsync(produto);

        var result = await _controller.GetProduto(10);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(produto, ok.Value);
    }

    [Fact]
    public async Task GetProduto_DeveRetornarNotFound_QuandoNaoExistir()
    {
        _serviceMock
            .Setup(s => s.ObterProduto(99))
            .ReturnsAsync((Produto)null);

        var result = await _controller.GetProduto(99);

        var n = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Produto não encontrado.", n.Value);
    }
}

