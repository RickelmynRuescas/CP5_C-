using System.ComponentModel.DataAnnotations;

namespace PraiaStore.Api.Dtos;

/// <summary>Dados para criar ou atualizar uma categoria.</summary>
public sealed class CategoriaRequest
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(80, MinimumLength = 2, ErrorMessage = "O nome deve ter entre 2 e 80 caracteres.")]
    public string Nome { get; init; } = string.Empty;

    [StringLength(300, ErrorMessage = "A descrição deve ter no máximo 300 caracteres.")]
    public string? Descricao { get; init; }
}

/// <summary>Categoria retornada pela API.</summary>
public sealed record CategoriaResponse(int Id, string Nome, string? Descricao, int QuantidadeProdutos);
