using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using PraiaStore.Api.Dtos;
using PraiaStore.Api.Services;

namespace PraiaStore.Api.Controllers.V1;

/// <summary>Gerencia as categorias de produtos da loja.</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/categorias")]
[Produces("application/json")]
public class CategoriasController(ICategoriaService service) : ControllerBase
{
    /// <summary>Lista todas as categorias.</summary>
    [HttpGet]
    [ProducesResponseType<IReadOnlyList<CategoriaResponse>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> Listar(CancellationToken ct) =>
        Ok(await service.ListarAsync(ct));

    /// <summary>Busca uma categoria pelo id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType<CategoriaResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(int id, CancellationToken ct) =>
        Ok(await service.ObterPorIdAsync(id, ct));

    /// <summary>Lista os produtos de uma categoria.</summary>
    [HttpGet("{id:int}/produtos")]
    [ProducesResponseType<IReadOnlyList<ProdutoResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ListarProdutos(int id, CancellationToken ct) =>
        Ok(await service.ListarProdutosAsync(id, ct));

    /// <summary>Cadastra uma nova categoria.</summary>
    [HttpPost]
    [ProducesResponseType<CategoriaResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Criar(CategoriaRequest request, CancellationToken ct)
    {
        var criada = await service.CriarAsync(request, ct);
        return CreatedAtAction(nameof(ObterPorId), new { id = criada.Id, version = "1" }, criada);
    }

    /// <summary>Atualiza uma categoria existente.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Atualizar(int id, CategoriaRequest request, CancellationToken ct)
    {
        await service.AtualizarAsync(id, request, ct);
        return NoContent();
    }

    /// <summary>Remove uma categoria (somente se não tiver produtos).</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Remover(int id, CancellationToken ct)
    {
        await service.RemoverAsync(id, ct);
        return NoContent();
    }
}
