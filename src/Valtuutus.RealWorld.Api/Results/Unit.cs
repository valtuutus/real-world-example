namespace Valtuutus.RealWorld.Api.Results;

/// <summary>
/// Representa um tipo padrão para o caso de operações sem resultado.
/// </summary>
public struct Unit
{
    public static Unit Value { get; } = new();
}