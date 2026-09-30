namespace PraiaStore.Api.Exceptions;

/// <summary>Base para os erros de negócio tratados pelo GlobalExceptionHandler.</summary>
public abstract class AppException(string message) : Exception(message);

/// <summary>Recurso não encontrado (HTTP 404).</summary>
public sealed class NotFoundException(string message) : AppException(message);

/// <summary>Dados válidos no formato, mas que violam uma regra de negócio (HTTP 400).</summary>
public sealed class BusinessRuleException(string message) : AppException(message);

/// <summary>Conflito com o estado atual do recurso, ex.: e-mail duplicado (HTTP 409).</summary>
public sealed class ConflictException(string message) : AppException(message);
