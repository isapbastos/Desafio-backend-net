using InvestimentosJwt.Application.PerfilService;
using InvestimentosJwt.Domain.Entities;
using InvestimentosJwtApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace InvestimentosJwt.Tests.Controllers;

public class PerfilControllerTests
{
    private readonly Mock<IPerfilService> _perfilServiceMock;
    private readonly PerfilController _controller;

    public PerfilControllerTests()
    {
        _perfilServiceMock = new Mock<IPerfilService>();
        _controller = new PerfilController(_perfilServiceMock.Object);
    }


    // ---------------------------------------------------------
    // TESTE: GET Produtos Recomendados
    // ---------------------------------------------------------
    [Fact]
    public async Task GetProdutosRecomendados_DeveRetornar200_ComLista()
    {
        // Arrange
        string perfil = "Moderado";

        var produtos = new List<Produto>
        {
            new Produto { Id = 1, Nome = "CDB", Risco = "médio" },
            new Produto { Id = 2, Nome = "Tesouro Selic", Risco = "baixo" }
        };

        _perfilServiceMock
            .Setup(s => s.ObterProdutosRecomendados(perfil))
            .ReturnsAsync(produtos);

        // Act
        var result = await _controller.GetProdutosRecomendados(perfil) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);

        var lista = Assert.IsAssignableFrom<IEnumerable<Produto>>(result.Value);
        Assert.Equal(2, lista.Count());
    }

    // ---------------------------------------------------------
    // TESTE: Perfil inexistente → deve retornar lista vazia (mas 200 OK)
    // ---------------------------------------------------------
    [Fact]
    public async Task GetProdutosRecomendados_PerfilInvalido_DeveRetornarListaVazia()
    {
        // Arrange
        _perfilServiceMock
            .Setup(s => s.ObterProdutosRecomendados("qualquer"))
            .ReturnsAsync(new List<Produto>());

        // Act
        var result = await _controller.GetProdutosRecomendados("qualquer") as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);

        var lista = Assert.IsAssignableFrom<IEnumerable<Produto>>(result.Value);
        Assert.Empty(lista);
    }
}
