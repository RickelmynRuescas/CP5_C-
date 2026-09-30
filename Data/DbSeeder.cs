using PraiaStore.Api.Models;

namespace PraiaStore.Api.Data;

/// <summary>
/// Popula o banco com alguns dados iniciais para facilitar os testes.
/// Só insere quando as tabelas estão vazias.
/// </summary>
public static class DbSeeder
{
    public static void Seed(AppDbContext context)
    {
        if (context.Categorias.Any())
        {
            return;
        }

        var agora = DateTime.UtcNow;

        var biquinis = new Categoria { Nome = "Biquínis", Descricao = "Conjuntos de duas peças" };
        var maios = new Categoria { Nome = "Maiôs", Descricao = "Peças únicas para praia e piscina" };
        var saidas = new Categoria { Nome = "Saídas de Praia", Descricao = "Kimonos, cangas e vestidos leves" };

        context.Categorias.AddRange(biquinis, maios, saidas);

        context.Produtos.AddRange(
            new Produto
            {
                Nome = "Biquíni Cortininha Tropical",
                Descricao = "Top cortininha com calcinha de amarrar, estampa de folhagens",
                Preco = 149.90m, Tamanho = "M", Cor = "Verde", Estoque = 12,
                DataCadastro = agora, Categoria = biquinis
            },
            new Produto
            {
                Nome = "Biquíni Top Faixa Canelado",
                Descricao = "Tecido canelado com proteção UV 50+",
                Preco = 169.90m, Tamanho = "P", Cor = "Terracota", Estoque = 8,
                DataCadastro = agora, Categoria = biquinis
            },
            new Produto
            {
                Nome = "Maiô Decote Costas",
                Descricao = "Maiô com decote profundo nas costas",
                Preco = 219.90m, Tamanho = "G", Cor = "Preto", Estoque = 5,
                DataCadastro = agora, Categoria = maios
            },
            new Produto
            {
                Nome = "Kimono Linho Areia",
                Descricao = "Saída de praia em linho, modelagem ampla",
                Preco = 189.00m, Tamanho = "U", Cor = "Areia", Estoque = 10,
                DataCadastro = agora, Categoria = saidas
            });

        context.Clientes.AddRange(
            new Cliente { Nome = "Ana Souza", Email = "ana.souza@email.com", Telefone = "11988887777", DataCadastro = agora },
            new Cliente { Nome = "Bruna Lima", Email = "bruna.lima@email.com", Telefone = "11977776666", DataCadastro = agora });

        context.SaveChanges();
    }
}
