using System.ComponentModel.DataAnnotations;

namespace PraiaStore.Api.Dtos;

/// <summary>Dados para criar ou atualizar um cliente.</summary>
public sealed class ClienteRequest
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(120, MinimumLength = 2, ErrorMessage = "O nome deve ter entre 2 e 120 caracteres.")]
    public string Nome { get; init; } = string.Empty;

    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "E-mail em formato inválido.")]
    [StringLength(150, ErrorMessage = "O e-mail deve ter no máximo 150 caracteres.")]
    public string Email { get; init; } = string.Empty;

    [StringLength(20, ErrorMessage = "O telefone deve ter no máximo 20 caracteres.")]
    public string? Telefone { get; init; }
}

/// <summary>Cliente retornado pela API.</summary>
public sealed record ClienteResponse(int Id, string Nome, string Email, string? Telefone, DateTime DataCadastro);
