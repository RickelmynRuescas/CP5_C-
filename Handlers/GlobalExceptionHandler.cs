using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PraiaStore.Api.Exceptions;

namespace PraiaStore.Api.Handlers;

/// <summary>
/// Converte exceções em respostas HTTP padronizadas (ProblemDetails - RFC 9457).
/// Assim os controllers ficam limpos, sem try/catch repetido.
/// </summary>
public sealed class GlobalExceptionHandler(
    IProblemDetailsService problemDetailsService,
    ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var (status, titulo) = exception switch
        {
            NotFoundException => (StatusCodes.Status404NotFound, "Recurso não encontrado"),
            BusinessRuleException => (StatusCodes.Status400BadRequest, "Requisição inválida"),
            ConflictException => (StatusCodes.Status409Conflict, "Conflito"),
            BadHttpRequestException => (StatusCodes.Status400BadRequest, "Requisição inválida"),
            _ => (StatusCodes.Status500InternalServerError, "Erro interno no servidor")
        };

        if (status == StatusCodes.Status500InternalServerError)
        {
            logger.LogError(exception, "Erro não tratado ao processar {Metodo} {Rota}",
                httpContext.Request.Method, httpContext.Request.Path);
        }

        // Em erros 500 não expomos detalhes internos para o cliente.
        var detalhe = exception is AppException or BadHttpRequestException
            ? exception.Message
            : "Ocorreu um erro inesperado. Tente novamente mais tarde.";

        httpContext.Response.StatusCode = status;

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Status = status,
                Title = titulo,
                Detail = detalhe,
                Instance = httpContext.Request.Path
            }
        });
    }
}
