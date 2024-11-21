namespace Valtuutus.RealWorld.Api.Results;


/// <summary>
/// Representa um resultado de sucesso de uma operação que retorna valor.
/// </summary>
/// <typeparam name="T"></typeparam>
public record SuccessResult<T>
{
    internal SuccessResult() {}
    public required T Value { get; init; }
    
    public required SuccessType Type { get; init; }


    public static implicit operator T(SuccessResult<T> result) => result.Value;
    public static implicit operator SuccessResult<T>(T value) => new() { Value = value, Type = SuccessType.Ok };
}

/// <summary>
/// Representa um resultado de sucesso de uma operação que não retorna valor.
/// </summary>
public sealed record SuccessResult : SuccessResult<Unit>
{
    
}