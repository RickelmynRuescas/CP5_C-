using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using PraiaStore.Api.Dtos;
using PraiaStore.Api.Services;

namespace PraiaStore.Api.Controllers.V1;

/// <summary>Gerencia os produtos (peças de moda praia).</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/produtos")]
[Produces("application/json")]
public class ProdutosController(IProdutoService service) : ControllerBase
{
    /// <summary>Lista produtos, com filtros opcionais por categoria, nome e status.</summary>
    [HttpGet]
    [ProducesResponseType<IReadOnlyList<ProdutoResponse>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> Listar([FromQuery] ProdutoFiltro filtro, CancellationToken ct) =>
        Ok(await service.ListarAsync(filtro, ct));

    /// <summary>Busca um produto pelo id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType<ProdutoResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(int id, CancellationToken ct) =>
        Ok(await service.ObterPorIdAsync(id, ct));

    /// <summary>Cadastra um novo produto.</summary>
    [HttpPost]
    [ProducesResponseType<ProdutoResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Criar(ProdutoRequest request, CancellationToken ct)
    {
        var criado = await service.CriarAsync(request, ct);
        return CreatedAtAction(nameof(ObterPorId), new { id = criado.Id, version = "1" }, criado);
    }

    /// <summary>Atualiza um produto existente.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Atualizar(int id, ProdutoRequest request, CancellationToken ct)
    {
        await service.AtualizarAsync(id, request, ct);
        return NoContent();
    }

    /// <summary>Remove um produto.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remover(int id, CancellationToken ct)
    {
        await service.RemoverAsync(id, ct);
        return NoContent();
    }
}
