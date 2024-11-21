namespace Valtuutus.RealWorld.Api.Results;

/// <summary>
/// Representa o resultado de uma operação.
/// </summary>
public interface IResult 
{
    /// <summary>
    /// Executa a função <paramref name="successHandler"/> se o resultado for sucesso, caso contrário executa a função <paramref name="errorHandler"/>.
    /// </summary>
    /// <param name="successHandler"></param>
    /// <param name="errorHandler"></param>
    /// <typeparam name="TE"></typeparam>
    /// <returns></returns>
    public TE MatchObject<TE>(Func<SuccessResult<object>, TE> successHandler, Func<ErrorResult, TE> errorHandler);
}