namespace Valtuutus.RealWorld.Api.Results;

/// <summary>
/// Representa os tipos de erros possíveis durante uma operação.
/// </summary>
public enum ErrorType
{
    Error,
    Forbidden,
    Unauthorized,
    Invalid,
    NotFound,
    Conflict,
}