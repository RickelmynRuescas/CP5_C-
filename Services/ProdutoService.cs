using Microsoft.EntityFrameworkCore;
using PraiaStore.Api.Data;
using PraiaStore.Api.Dtos;
using PraiaStore.Api.Exceptions;
using PraiaStore.Api.Mappings;
using PraiaStore.Api.Models;

namespace PraiaStore.Api.Services;

public sealed class ProdutoService(AppDbContext context) : IProdutoService
{
    public async Task<IReadOnlyList<ProdutoResponse>> ListarAsync(ProdutoFiltro filtro, CancellationToken ct)
    {
        var query = context.Produtos.AsNoTracking();

        if (filtro.CategoriaId is not null)
        {
            query = query.Where(p => p.CategoriaId == filtro.CategoriaId);
        }

        if (!string.IsNullOrWhiteSpace(filtro.Nome))
        {
            var termo = filtro.Nome.Trim().ToLower();
            query = query.Where(p => p.Nome.ToLower().Contains(termo));
        }

        if (filtro.Ativo is not null)
        {
            query = query.Where(p => p.Ativo == filtro.Ativo);
        }

        return await query
            .OrderBy(p => p.Nome)
            .Select(ProjecoesDto.ParaProdutoResponse)
            .ToListAsync(ct);
    }

    public async Task<ProdutoResponse> ObterPorIdAsync(int id, CancellationToken ct) =>
        await context.Produtos
            .AsNoTracking()
            .Where(p => p.Id == id)
            .Select(ProjecoesDto.ParaProdutoResponse)
            .FirstOrDefaultAsync(ct)
        ?? throw NaoEncontrado(id);

    public async Task<ProdutoResponse> CriarAsync(ProdutoRequest request, CancellationToken ct)
    {
        await GarantirCategoriaExisteAsync(request.CategoriaId!.Value, ct);

        var produto = new Produto { DataCadastro = DateTime.UtcNow };
        AplicarDados(produto, request);

        context.Produtos.Add(produto);
        await context.SaveChangesAsync(ct);

        return await ObterPorIdAsync(produto.Id, ct);
    }

    public async Task AtualizarAsync(int id, ProdutoRequest request, CancellationToken ct)
    {
        var produto = await context.Produtos.FindAsync([id], ct) ?? throw NaoEncontrado(id);

        await GarantirCategoriaExisteAsync(request.CategoriaId!.Value, ct);
        AplicarDados(produto, request);

        await context.SaveChangesAsync(ct);
    }

    public async Task RemoverAsync(int id, CancellationToken ct)
    {
        var produto = await context.Produtos.FindAsync([id], ct) ?? throw NaoEncontrado(id);

        context.Produtos.Remove(produto);
        await context.SaveChangesAsync(ct);
    }

    private static void AplicarDados(Produto produto, ProdutoRequest request)
    {
        // Os campos obrigatórios já foram validados pelo [ApiController] antes de chegar aqui.
        produto.Nome = request.Nome.Trim();
        produto.Descricao = request.Descricao?.Trim();
        produto.Preco = request.Preco!.Value;
        produto.Tamanho = request.Tamanho.Trim().ToUpperInvariant();
        produto.Cor = request.Cor.Trim();
        produto.Estoque = request.Estoque!.Value;
        produto.Ativo = request.Ativo;
        produto.CategoriaId = request.CategoriaId!.Value;
    }

    private async Task GarantirCategoriaExisteAsync(int categoriaId, CancellationToken ct)
    {
        if (!await context.Categorias.AnyAsync(c => c.Id == categoriaId, ct))
        {
            throw new BusinessRuleException($"A categoria com id {categoriaId} não existe.");
        }
    }

    private static NotFoundException NaoEncontrado(int id) =>
        new($"Produto com id {id} não encontrado.");
}
