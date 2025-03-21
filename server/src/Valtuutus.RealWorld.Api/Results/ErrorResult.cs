namespace Valtuutus.RealWorld.Api.Results;

/// <summary>
/// Representa um resultado de erro de uma operação.
/// </summary>
public sealed record ErrorResult
{
    public required ErrorType Type { get; init; }
    public required IList<AppError> Errors { get; init; }

    
}