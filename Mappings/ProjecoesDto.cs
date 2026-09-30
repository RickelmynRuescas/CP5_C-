using System.Linq.Expressions;
using PraiaStore.Api.Dtos;
using PraiaStore.Api.Models;

namespace PraiaStore.Api.Mappings;

/// <summary>
/// Projeções de entidade para DTO. Por serem Expressions, o EF Core
/// traduz direto para SQL e busca só as colunas necessárias.
/// </summary>
public static class ProjecoesDto
{
    public static readonly Expression<Func<Categoria, CategoriaResponse>> ParaCategoriaResponse =
        c => new CategoriaResponse(c.Id, c.Nome, c.Descricao, c.Produtos.Count);

    public static readonly Expression<Func<Produto, ProdutoResponse>> ParaProdutoResponse =
        p => new ProdutoResponse(
            p.Id, p.Nome, p.Descricao, p.Preco, p.Tamanho, p.Cor, p.Estoque,
            p.Ativo, p.DataCadastro, p.CategoriaId, p.Categoria!.Nome);

    public static readonly Expression<Func<Cliente, ClienteResponse>> ParaClienteResponse =
        c => new ClienteResponse(c.Id, c.Nome, c.Email, c.Telefone, c.DataCadastro);
}
