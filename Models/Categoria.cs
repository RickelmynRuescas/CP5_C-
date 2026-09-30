namespace PraiaStore.Api.Models;

/// <summary>
/// Agrupa os produtos da loja (ex.: Biquínis, Maiôs, Saídas de Praia).
/// </summary>
public class Categoria
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }

    public ICollection<Produto> Produtos { get; set; } = new List<Produto>();
}
