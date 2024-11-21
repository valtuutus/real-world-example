using System.Diagnostics.CodeAnalysis;

namespace Valtuutus.RealWorld.Api.Results;

/// <summary>
/// Representa um erro que aconteceu durante a execução de uma operação.
/// </summary>
public record AppError
{
    public AppError()
    {
    }

    [SetsRequiredMembers]
    public AppError(string errorMessage, string errorCode = "")
    {
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
    }

    public required string ErrorMessage { get; init; }
    public string ErrorCode { get; init; } = null!;
}