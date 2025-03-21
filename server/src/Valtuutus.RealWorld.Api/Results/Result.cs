namespace Valtuutus.RealWorld.Api.Results;


/// <summary>
/// Representa um resultado de uma operação que retorna um valor.
/// </summary>
/// <typeparam name="T"></typeparam>
public record Result<T>: IResult
{
    private readonly SuccessResult<T>? _successResult;
    private readonly ErrorResult? _errorResult;
    public bool IsSuccess { get; }

    protected Result(SuccessResult<T> successResult)
    {
        _successResult = successResult;
        IsSuccess = true;
    }

    protected Result(ErrorResult errorResult)
    {
        _errorResult = errorResult;
        IsSuccess = false;
    }
    
    
    public static implicit operator Result<T>(T value) => new(Result.Ok(value));

    public static implicit operator Result<T>(SuccessResult<T> value)
    {
        return new(value);
    }


    public static implicit operator Result<T>(ErrorResult value)
    {
        return new(value);
    }

    public static implicit operator Result(Result<T> value)
    {
        return value.Match(success => new Result(Result.Success(success.Type)),
            
            error => new Result(error));
    }

    
    /// <summary>
    /// Executa a função <paramref name="successHandler"/> se o resultado for sucesso, caso contrário executa a função <paramref name="errorHandler"/>.
    /// </summary>
    /// <param name="successHandler"></param>
    /// <param name="errorHandler"></param>
    /// <typeparam name="TE"></typeparam>
    /// <returns></returns>
    public TE Match<TE>(Func<SuccessResult<T>, TE> successHandler, Func<ErrorResult, TE> errorHandler)
    {
        return IsSuccess
            ? successHandler(_successResult!)
            : errorHandler(_errorResult!);
    }
    
    
    /// <summary>
    /// Executa a função <paramref name="successHandler"/> se o resultado for sucesso, caso contrário executa a função <paramref name="errorHandler"/>.
    /// </summary>
    /// <param name="successHandler"></param>
    /// <param name="errorHandler"></param>
    /// <typeparam name="TE"></typeparam>
    /// <returns></returns>
    public TE MatchObject<TE>(Func<SuccessResult<object>, TE> successHandler, Func<ErrorResult, TE> errorHandler)
    {
        return IsSuccess
            ? successHandler(Result.Success<object>(_successResult!.Value!, _successResult.Type))
            : errorHandler(_errorResult!);
    }


    /// <summary>
    /// Transforma o result atual num result novo através de uma função de mapping, que será executada caso ele seja sucesso.
    /// </summary>
    /// <param name="mapper"></param>
    /// <typeparam name="TE"></typeparam>
    /// <returns></returns>
    public Result<TE> Map<TE>(Func<T, TE> mapper)
    {
        return IsSuccess
            ? Result.Ok(mapper(_successResult!))
            : _errorResult!;
    }

    
    /// <summary>
    /// Retorna o erro interno do resultado, caso contrário retorna null.
    /// </summary>
    /// <returns></returns>
    public ErrorResult? AsError()
    {
        return _errorResult;
    }

    /// <summary>
    /// Retorna true se e seta o valor da variável value caso o resultado for sucesso e false caso contrário.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool TryGetValue(out T value)
    {
        if (IsSuccess)
        {
            value = _successResult!.Value;
            return true;
        }

        value = default!;
        return false;
    }
    
    #region Errors
    
    /// <summary>
    /// Cria um resultado de erro com o tipo <see cref="ErrorType.Error"/>;
    /// </summary>
    /// <param name="appErrors"></param>
    /// <returns>Resultado de erro do tipo <see cref="ErrorType.Error"/></returns>
    public static ErrorResult Error(params AppError[] appErrors)
    {
        return new ErrorResult { Errors = appErrors.ToList(), Type = ErrorType.Error };
    }

    
    /// <summary>
    /// Cria um resultado de erro com o tipo <see cref="ErrorType.Error"/>;
    /// </summary>
    /// <param name="errorMessage"></param>
    /// <returns>Resultado de erro do tipo <see cref="ErrorType.Error"/></returns>
    public static ErrorResult Error(string errorMessage)
    {
        return new ErrorResult {Errors = new List<AppError> {new AppError(errorMessage)}, Type = ErrorType.Error};
    }

    
    /// <summary>
    /// Cria um resultado de erro com o tipo <see cref="ErrorType.Invalid"/>;
    /// </summary>
    /// <param name="appError"></param>
    /// <returns>Resultado de erro do tipo <see cref="ErrorType.Invalid"/></returns>
    public static ErrorResult Invalid(AppError appError)
    {
        return new ErrorResult
            { Errors = new List<AppError>(new[] { appError }), Type = ErrorType.Invalid };
    }
    
    /// <summary>
    /// Cria um resultado de erro com o tipo <see cref="ErrorType.Invalid"/>;
    /// </summary>
    /// <returns>Resultado de erro do tipo <see cref="ErrorType.Invalid"/></returns>
    public static ErrorResult Invalid()
    {
        return new ErrorResult
            { Errors = new List<AppError>(), Type = ErrorType.Invalid };
    }

    /// <summary>
    /// Cria um resultado de erro com o tipo <see cref="ErrorType.Invalid"/>;
    /// </summary>
    /// <param name="appErrors"></param>
    /// <returns>Resultado de erro do tipo <see cref="ErrorType.Invalid"/></returns>
    public static ErrorResult Invalid(IEnumerable<AppError> appErrors)
    {
        return new ErrorResult { Errors = appErrors.ToList(), Type = ErrorType.Invalid };
    }

    
    /// <summary>
    /// Cria um resultado de erro com o tipo <see cref="ErrorType.NotFound"/>;
    /// </summary>
    /// <returns>Resultado de erro do tipo <see cref="ErrorType.NotFound"/></returns>
    public static ErrorResult NotFound()
    {
        return new ErrorResult { Errors = Array.Empty<AppError>(), Type = ErrorType.NotFound };
    }

    
    /// <summary>
    /// Cria um resultado de erro com o tipo <see cref="ErrorType.Forbidden"/>;
    /// </summary>
    /// <returns>Resultado de erro do tipo <see cref="ErrorType.Forbidden"/></returns>
    public static ErrorResult Forbidden()
    {
        return new ErrorResult { Errors = Array.Empty<AppError>(), Type = ErrorType.Forbidden };
    }

    
    /// <summary>
    /// Cria um resultado de erro com o tipo <see cref="ErrorType.Unauthorized"/>;
    /// </summary>
    /// <returns>Resultado de erro do tipo <see cref="ErrorType.Unauthorized"/></returns>
    public static ErrorResult Unauthorized()
    {
        return new ErrorResult { Errors = Array.Empty<AppError>(), Type = ErrorType.Unauthorized };
    }

    
    
    /// <summary>
    /// Cria um resultado de erro com o tipo <see cref="ErrorType.Conflict"/>;
    /// </summary>
    /// <returns>Resultado de erro do tipo <see cref="ErrorType.Conflict"/></returns>
    public static ErrorResult Conflict()
    {
        return new ErrorResult { Errors = Array.Empty<AppError>(), Type = ErrorType.Conflict };
    }
    #endregion

    #region Success

    /// <summary>
    /// Cria um resultado de sucesso com o tipo especificado.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    internal static SuccessResult Success(SuccessType type)
    {
        return new SuccessResult { Value = Unit.Value, Type = type};
    }
    

    /// <summary>
    /// Cria um resultado de sucesso com o tipo e valor especificado.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    internal static SuccessResult<T> Success<T>(T value, SuccessType type)
    {
        return new SuccessResult<T> { Value = value, Type = type};
    }
    
    
    /// <summary>
    /// Cria um resultado de sucesso com o tipo <see cref="SuccessType.Ok"/>.
    /// </summary>
    /// <returns></returns>
    public static SuccessResult Ok()
    {
        return Success(SuccessType.Ok);
    }
    
    
    /// <summary>
    /// Cria um resultado de sucesso com valor e com tipo <see cref="SuccessType.Ok"/>.
    /// </summary>
    /// <returns></returns>
    public static SuccessResult<T> Ok<T>(T value)
    {
        return Success(value, SuccessType.Ok);
    }
    
    
    /// <summary>
    /// Cria um resultado de sucesso com o tipo <see cref="SuccessType.Accepted"/>.
    /// </summary>
    /// <returns></returns>
    public static SuccessResult Accepted()
    {
        return Success(SuccessType.Accepted);
    }

    
    /// <summary>
    /// Cria um resultado de sucesso com o tipo <see cref="SuccessType.NoContent"/>.
    /// </summary>
    /// <returns></returns>
    public static SuccessResult NoContent()
    {
        return Success(SuccessType.NoContent);
    }

    #endregion

}

/// <summary>
/// Representa um resultado de uma operação que não retorna um valor.
/// </summary>
public sealed record Result : Result<Unit>
{
    internal Result(SuccessResult successResult) : base(successResult)
    {
    }

    internal Result(ErrorResult errorResult) : base(errorResult)
    {
    }

    public static implicit operator Result(ErrorResult value)
    {
        return new(value);
    }

    public static implicit operator Result(SuccessResult value)
    {
        return new(value);
    }
}