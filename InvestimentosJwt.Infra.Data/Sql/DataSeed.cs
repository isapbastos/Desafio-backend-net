using InvestimentosJwt.Domain.Entities;
using System;

namespace InvestimentosJwt.Infra.Data.Sql;

public static class DataSeed
{
    public static void Seed(AppDbContext context)
    {
        if (!context.Produtos.Any())
        {
            context.Produtos.AddRange(
                new Produto { Nome = "CDB Caixa", Tipo = "CDB", Rentabilidade = 0.12m, Risco = "Baixo" },
                new Produto { Nome = "LCI Caixa", Tipo = "LCI", Rentabilidade = 0.10m, Risco = "Baixo" },
                new Produto { Nome = "LCA Caixa", Tipo = "LCA", Rentabilidade = 0.11m, Risco = "Baixo" },
                new Produto { Nome = "Tesouro Selic", Tipo = "Tesouro", Rentabilidade = 0.13m, Risco = "Baixo" },
                new Produto { Nome = "Fundos Renda Fixa", Tipo = "FundoRF", Rentabilidade = 0.09m, Risco = "Médio" },
                new Produto { Nome = "Ações Caixa", Tipo = "Acoes", Rentabilidade = 0.18m, Risco = "Alto" }
            );

            context.SaveChanges();
        }
    }
}