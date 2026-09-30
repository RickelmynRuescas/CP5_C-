namespace PraiaStore.Api.Models;

/// <summary>
/// Peça de moda praia vendida pela loja.
/// </summary>
public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public decimal Preco { get; set; }
    public string Tamanho { get; set; } = string.Empty;
    public string Cor { get; set; } = string.Empty;
    public int Estoque { get; set; }
    public bool Ativo { get; set; } = true;
    public DateTime DataCadastro { get; set; }

    public int CategoriaId { get; set; }
    public Categoria? Categoria { get; set; }
}
