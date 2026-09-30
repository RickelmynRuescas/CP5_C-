using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using PraiaStore.Api.Dtos;
using PraiaStore.Api.Services;

namespace PraiaStore.Api.Controllers.V1;

/// <summary>Gerencia os clientes da loja.</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/clientes")]
[Produces("application/json")]
public class ClientesController(IClienteService service) : ControllerBase
{
    /// <summary>Lista todos os clientes.</summary>
    [HttpGet]
    [ProducesResponseType<IReadOnlyList<ClienteResponse>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> Listar(CancellationToken ct) =>
        Ok(await service.ListarAsync(ct));

    /// <summary>Busca um cliente pelo id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType<ClienteResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(int id, CancellationToken ct) =>
        Ok(await service.ObterPorIdAsync(id, ct));

    /// <summary>Cadastra um novo cliente.</summary>
    [HttpPost]
    [ProducesResponseType<ClienteResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Criar(ClienteRequest request, CancellationToken ct)
    {
        var criado = await service.CriarAsync(request, ct);
        return CreatedAtAction(nameof(ObterPorId), new { id = criado.Id, version = "1" }, criado);
    }

    /// <summary>Atualiza um cliente existente.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Atualizar(int id, ClienteRequest request, CancellationToken ct)
    {
        await service.AtualizarAsync(id, request, ct);
        return NoContent();
    }

    /// <summary>Remove um cliente.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remover(int id, CancellationToken ct)
    {
        await service.RemoverAsync(id, ct);
        return NoContent();
    }
}
