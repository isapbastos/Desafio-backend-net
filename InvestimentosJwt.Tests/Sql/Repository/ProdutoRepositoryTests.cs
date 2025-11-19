using InvestimentosJwt.Domain.Entities;
using InvestimentosJwt.Infra.Data.Sql;
using InvestimentosJwt.Infra.Data.Sql.ProdutoRepository;
using Microsoft.EntityFrameworkCore;

namespace InvestimentosJwt.Tests.Sql.Repository;

public class ProdutoRepositoryTests
{
    private AppDbContext CriarContextoEmMemoria()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
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

    [Fact]
    public async Task ObterTodos_DeveRetornarTodosOsProdutos()
    {
        // Arrange
        var context = CriarContextoEmMemoria();

        context.Produtos.Add(CriarProduto(1, "CDB"));
        context.Produtos.Add(CriarProduto(2, "LCI"));
        await context.SaveChangesAsync();

        var repository = new ProdutoRepository(context);

        // Act
        var resultado = await repository.ObterTodos();

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(2, resultado.Count());
    }

    [Fact]
    public async Task ObterPorId_DeveRetornarProduto_QuandoExistir()
    {
        var context = CriarContextoEmMemoria();

        var produto = CriarProduto(10, "CDB");
        context.Produtos.Add(produto);
        await context.SaveChangesAsync();

        var repository = new ProdutoRepository(context);

        var resultado = await repository.ObterPorId(10);

        Assert.NotNull(resultado);
        Assert.Equal("CDB", resultado!.Tipo);
        Assert.Equal("Produto 10", resultado.Nome);
    }

    [Fact]
    public async Task ObterPorId_DeveRetornarNull_QuandoNaoExistir()
    {
        var context = CriarContextoEmMemoria();
        var repository = new ProdutoRepository(context);

        var resultado = await repository.ObterPorId(999);

        Assert.Null(resultado);
    }

    [Fact]
    public async Task ObterPorTipo_DeveRetornarProduto_QuandoTipoExistir()
    {
        var context = CriarContextoEmMemoria();

        var produto = CriarProduto(1, "CDB");
        context.Produtos.Add(produto);
        await context.SaveChangesAsync();

        var repository = new ProdutoRepository(context);

        var resultado = await repository.ObterPorTipo("CDB");

        Assert.NotNull(resultado);
        Assert.Equal(1, resultado!.Id);
    }

    [Fact]
    public async Task ObterPorTipo_DeveRetornarNull_QuandoTipoNaoExistir()
    {
        var context = CriarContextoEmMemoria();
        var repository = new ProdutoRepository(context);

        var resultado = await repository.ObterPorTipo("TESOURO");

        Assert.Null(resultado);
    }
}
