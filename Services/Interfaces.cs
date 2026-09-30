using PraiaStore.Api.Dtos;

namespace PraiaStore.Api.Services;

public interface ICategoriaService
{
    Task<IReadOnlyList<CategoriaResponse>> ListarAsync(CancellationToken ct);
    Task<CategoriaResponse> ObterPorIdAsync(int id, CancellationToken ct);
    Task<IReadOnlyList<ProdutoResponse>> ListarProdutosAsync(int categoriaId, CancellationToken ct);
    Task<CategoriaResponse> CriarAsync(CategoriaRequest request, CancellationToken ct);
    Task AtualizarAsync(int id, CategoriaRequest request, CancellationToken ct);
    Task RemoverAsync(int id, CancellationToken ct);
}

public interface IProdutoService
{
    Task<IReadOnlyList<ProdutoResponse>> ListarAsync(ProdutoFiltro filtro, CancellationToken ct);
    Task<ProdutoResponse> ObterPorIdAsync(int id, CancellationToken ct);
    Task<ProdutoResponse> CriarAsync(ProdutoRequest request, CancellationToken ct);
    Task AtualizarAsync(int id, ProdutoRequest request, CancellationToken ct);
    Task RemoverAsync(int id, CancellationToken ct);
}

public interface IClienteService
{
    Task<IReadOnlyList<ClienteResponse>> ListarAsync(CancellationToken ct);
    Task<ClienteResponse> ObterPorIdAsync(int id, CancellationToken ct);
    Task<ClienteResponse> CriarAsync(ClienteRequest request, CancellationToken ct);
    Task AtualizarAsync(int id, ClienteRequest request, CancellationToken ct);
    Task RemoverAsync(int id, CancellationToken ct);
}
