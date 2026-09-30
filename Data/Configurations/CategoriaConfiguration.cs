using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PraiaStore.Api.Models;

namespace PraiaStore.Api.Data.Configurations;

public class CategoriaConfiguration : IEntityTypeConfiguration<Categoria>
{
    public void Configure(EntityTypeBuilder<Categoria> builder)
    {
        builder.ToTable("Categorias");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Nome).IsRequired().HasMaxLength(80);
        builder.Property(c => c.Descricao).HasMaxLength(300);

        builder.HasIndex(c => c.Nome).IsUnique();
    }
}
