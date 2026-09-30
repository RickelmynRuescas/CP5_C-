using Microsoft.EntityFrameworkCore;
using PraiaStore.Api.Data;
using PraiaStore.Api.Dtos;
using PraiaStore.Api.Exceptions;
using PraiaStore.Api.Mappings;
using PraiaStore.Api.Models;

namespace PraiaStore.Api.Services;

public sealed class ClienteService(AppDbContext context) : IClienteService
{
    public async Task<IReadOnlyList<ClienteResponse>> ListarAsync(CancellationToken ct) =>
        await context.Clientes
            .AsNoTracking()
            .OrderBy(c => c.Nome)
            .Select(ProjecoesDto.ParaClienteResponse)
            .ToListAsync(ct);

    public async Task<ClienteResponse> ObterPorIdAsync(int id, CancellationToken ct) =>
        await context.Clientes
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(ProjecoesDto.ParaClienteResponse)
            .FirstOrDefaultAsync(ct)
        ?? throw NaoEncontrado(id);

    public async Task<ClienteResponse> CriarAsync(ClienteRequest request, CancellationToken ct)
    {
        var email = NormalizarEmail(request.Email);
        await GarantirEmailDisponivelAsync(email, idAtual: null, ct);

        var cliente = new Cliente
        {
            Nome = request.Nome.Trim(),
            Email = email,
            Telefone = request.Telefone?.Trim(),
            DataCadastro = DateTime.UtcNow
        };

        context.Clientes.Add(cliente);
        await context.SaveChangesAsync(ct);

        return new ClienteResponse(cliente.Id, cliente.Nome, cliente.Email, cliente.Telefone, cliente.DataCadastro);
    }

    public async Task AtualizarAsync(int id, ClienteRequest request, CancellationToken ct)
    {
        var cliente = await context.Clientes.FindAsync([id], ct) ?? throw NaoEncontrado(id);

        var email = NormalizarEmail(request.Email);
        await GarantirEmailDisponivelAsync(email, idAtual: id, ct);

        cliente.Nome = request.Nome.Trim();
        cliente.Email = email;
        cliente.Telefone = request.Telefone?.Trim();

        await context.SaveChangesAsync(ct);
    }

    public async Task RemoverAsync(int id, CancellationToken ct)
    {
        var cliente = await context.Clientes.FindAsync([id], ct) ?? throw NaoEncontrado(id);

        context.Clientes.Remove(cliente);
        await context.SaveChangesAsync(ct);
    }

    private async Task GarantirEmailDisponivelAsync(string email, int? idAtual, CancellationToken ct)
    {
        if (await context.Clientes.AnyAsync(c => c.Email == email && c.Id != idAtual, ct))
        {
            throw new ConflictException($"Já existe um cliente cadastrado com o e-mail '{email}'.");
        }
    }

    private static string NormalizarEmail(string email) => email.Trim().ToLowerInvariant();

    private static NotFoundException NaoEncontrado(int id) =>
        new($"Cliente com id {id} não encontrado.");
}
