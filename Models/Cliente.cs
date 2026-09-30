namespace PraiaStore.Api.Models;

/// <summary>
/// Cliente cadastrado na loja.
/// </summary>
public class Cliente
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Telefone { get; set; }
    public DateTime DataCadastro { get; set; }
}
