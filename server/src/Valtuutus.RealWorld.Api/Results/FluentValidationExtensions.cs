using FluentValidation.Results;

namespace Valtuutus.RealWorld.Api.Results;

public static class FluentValidationExtensions
{
    
    /// <summary>
    /// Converte um <see cref="ValidationResult"/> para <see cref="ErrorResult"/>
    /// </summary>
    /// <param name="validationResult">Resultado de uma validação pelo FluentValidation</param>
    /// <returns></returns>
    public static ErrorResult ToErrorResult(this ValidationResult validationResult)
    {
        var errors = validationResult.Errors.Select(x =>
            new AppError(errorCode: x.ErrorCode, errorMessage: x.ErrorMessage)).ToArray();

        return Result.Invalid(errors);
    }
}