using Microsoft.EntityFrameworkCore;
using PraiaStore.Api.Data;
using PraiaStore.Api.Dtos;
using PraiaStore.Api.Exceptions;
using PraiaStore.Api.Mappings;
using PraiaStore.Api.Models;

namespace PraiaStore.Api.Services;

public sealed class CategoriaService(AppDbContext context) : ICategoriaService
{
    public async Task<IReadOnlyList<CategoriaResponse>> ListarAsync(CancellationToken ct) =>
        await context.Categorias
            .AsNoTracking()
            .OrderBy(c => c.Nome)
            .Select(ProjecoesDto.ParaCategoriaResponse)
            .ToListAsync(ct);

    public async Task<CategoriaResponse> ObterPorIdAsync(int id, CancellationToken ct) =>
        await context.Categorias
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(ProjecoesDto.ParaCategoriaResponse)
            .FirstOrDefaultAsync(ct)
        ?? throw NaoEncontrada(id);

    public async Task<IReadOnlyList<ProdutoResponse>> ListarProdutosAsync(int categoriaId, CancellationToken ct)
    {
        if (!await context.Categorias.AnyAsync(c => c.Id == categoriaId, ct))
        {
            throw NaoEncontrada(categoriaId);
        }

        return await context.Produtos
            .AsNoTracking()
            .Where(p => p.CategoriaId == categoriaId)
            .OrderBy(p => p.Nome)
            .Select(ProjecoesDto.ParaProdutoResponse)
            .ToListAsync(ct);
    }

    public async Task<CategoriaResponse> CriarAsync(CategoriaRequest request, CancellationToken ct)
    {
        var nome = request.Nome.Trim();
        await GarantirNomeDisponivelAsync(nome, idAtual: null, ct);

        var categoria = new Categoria
        {
            Nome = nome,
            Descricao = request.Descricao?.Trim()
        };

        context.Categorias.Add(categoria);
        await context.SaveChangesAsync(ct);

        return new CategoriaResponse(categoria.Id, categoria.Nome, categoria.Descricao, QuantidadeProdutos: 0);
    }

    public async Task AtualizarAsync(int id, CategoriaRequest request, CancellationToken ct)
    {
        var categoria = await context.Categorias.FindAsync([id], ct) ?? throw NaoEncontrada(id);

        var nome = request.Nome.Trim();
        await GarantirNomeDisponivelAsync(nome, idAtual: id, ct);

        categoria.Nome = nome;
        categoria.Descricao = request.Descricao?.Trim();

        await context.SaveChangesAsync(ct);
    }

    public async Task RemoverAsync(int id, CancellationToken ct)
    {
        var categoria = await context.Categorias.FindAsync([id], ct) ?? throw NaoEncontrada(id);

        if (await context.Produtos.AnyAsync(p => p.CategoriaId == id, ct))
        {
            throw new ConflictException(
                $"A categoria '{categoria.Nome}' possui produtos vinculados e não pode ser removida.");
        }

        context.Categorias.Remove(categoria);
        await context.SaveChangesAsync(ct);
    }

    private async Task GarantirNomeDisponivelAsync(string nome, int? idAtual, CancellationToken ct)
    {
        var nomeNormalizado = nome.ToLower();
        var emUso = await context.Categorias.AnyAsync(
            c => c.Nome.ToLower() == nomeNormalizado && c.Id != idAtual, ct);

        if (emUso)
        {
            throw new ConflictException($"Já existe uma categoria com o nome '{nome}'.");
        }
    }

    private static NotFoundException NaoEncontrada(int id) =>
        new($"Categoria com id {id} não encontrada.");
}
