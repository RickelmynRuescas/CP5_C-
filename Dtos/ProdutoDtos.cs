using System.ComponentModel.DataAnnotations;

namespace PraiaStore.Api.Dtos;

/// <summary>Dados para criar ou atualizar um produto.</summary>
public sealed class ProdutoRequest
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(120, MinimumLength = 2, ErrorMessage = "O nome deve ter entre 2 e 120 caracteres.")]
    public string Nome { get; init; } = string.Empty;

    [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres.")]
    public string? Descricao { get; init; }

    [Required(ErrorMessage = "O preço é obrigatório.")]
    [Range(typeof(decimal), "0.01", "99999.99", ErrorMessage = "O preço deve estar entre 0,01 e 99.999,99.")]
    public decimal? Preco { get; init; }

    [Required(ErrorMessage = "O tamanho é obrigatório.")]
    [RegularExpression("^(?i)(PP|P|M|G|GG|U)$", ErrorMessage = "Tamanho inválido. Use PP, P, M, G, GG ou U (único).")]
    public string Tamanho { get; init; } = string.Empty;

    [Required(ErrorMessage = "A cor é obrigatória.")]
    [StringLength(40, ErrorMessage = "A cor deve ter no máximo 40 caracteres.")]
    public string Cor { get; init; } = string.Empty;

    [Required(ErrorMessage = "O estoque é obrigatório.")]
    [Range(0, 100000, ErrorMessage = "O estoque deve estar entre 0 e 100000.")]
    public int? Estoque { get; init; }

    /// <summary>Se o produto está disponível para venda. Padrão: true.</summary>
    public bool Ativo { get; init; } = true;

    [Required(ErrorMessage = "A categoria é obrigatória.")]
    [Range(1, int.MaxValue, ErrorMessage = "Informe um id de categoria válido.")]
    public int? CategoriaId { get; init; }
}

/// <summary>Filtros opcionais da listagem de produtos (query string).</summary>
public sealed class ProdutoFiltro
{
    /// <summary>Filtra pelo id da categoria.</summary>
    public int? CategoriaId { get; init; }

    /// <summary>Busca por parte do nome.</summary>
    public string? Nome { get; init; }

    /// <summary>Filtra por produtos ativos (true) ou inativos (false).</summary>
    public bool? Ativo { get; init; }
}

/// <summary>Produto retornado pela API.</summary>
public sealed record ProdutoResponse(
    int Id,
    string Nome,
    string? Descricao,
    decimal Preco,
    string Tamanho,
    string Cor,
    int Estoque,
    bool Ativo,
    DateTime DataCadastro,
    int CategoriaId,
    string CategoriaNome);
