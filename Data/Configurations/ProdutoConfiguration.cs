using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PraiaStore.Api.Models;

namespace PraiaStore.Api.Data.Configurations;

public class ProdutoConfiguration : IEntityTypeConfiguration<Produto>
{
    public void Configure(EntityTypeBuilder<Produto> builder)
    {
        builder.ToTable("Produtos");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Nome).IsRequired().HasMaxLength(120);
        builder.Property(p => p.Descricao).HasMaxLength(500);
        builder.Property(p => p.Preco).HasPrecision(10, 2);
        builder.Property(p => p.Tamanho).IsRequired().HasMaxLength(3);
        builder.Property(p => p.Cor).IsRequired().HasMaxLength(40);

        // Uma categoria possui vários produtos. Restrict impede apagar
        // uma categoria que ainda tenha produtos vinculados.
        builder.HasOne(p => p.Categoria)
            .WithMany(c => c.Produtos)
            .HasForeignKey(p => p.CategoriaId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
