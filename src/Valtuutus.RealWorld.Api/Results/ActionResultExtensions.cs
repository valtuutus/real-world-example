using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Valtuutus.RealWorld.Api.Results;

/// <summary>
/// Extensões para transformar um <see cref="Result"/> em um resultado de aspnetcore.
/// </summary>
public static partial class ResultExtensions
{
    /// <summary>
    /// Converte um <see cref="Result{T}"/> em um <see cref="ActionResult"/>
    /// </summary>
    /// <typeparam name="T">O tipo do valor sendo retornado</typeparam>
    /// <param name="controller">O controller de onde está sendo chamado</param>
    /// <param name="result">O result para converter em ActionResult</param>
    /// <returns></returns>
    public static ActionResult<T> ToActionResult<T>(this Result<T> result, ControllerBase controller)
    {
        return controller.ToActionResult((IResult)result);
    }

    /// <summary>
    /// Converte um <see cref="Result"/> em um <see cref="ActionResult"/>
    /// </summary>
    /// <param name="controller">O controller de onde está sendo chamado</param>
    /// <param name="result">O result para converter em ActionResult</param>
    /// <returns></returns>
    public static ActionResult ToActionResult(this Result result, ControllerBase controller)
    {
        return controller.ToActionResult((IResult)result);
    }

    /// <summary>
    /// Converte um <see cref="Result{T}"/> em um <see cref="ActionResult{T}"/>
    /// </summary>
    /// <typeparam name="T">O tipo do valor sendo retornado</typeparam>
    /// <param name="controller">O controller de onde está sendo chamado</param>
    /// <param name="result">O result para converter em ActionResult</param>
    /// <returns></returns>
    public static ActionResult<T> ToActionResult<T>(this ControllerBase controller,
        Result<T> result)
    {
        return controller.ToActionResult((IResult)result);
    }

    /// <summary>
    /// Converte um <see cref="Result"/> em um <see cref="ActionResult"/>
    /// </summary>
    /// <param name="controller">O controller de onde está sendo chamado</param>
    /// <param name="result">O result para converter em ActionResult</param>
    /// <returns></returns>
    public static ActionResult ToActionResult(this ControllerBase controller,
        Result result)
    {
        return controller.ToActionResult((IResult)result);
    }

    internal static ActionResult ToActionResult(this ControllerBase controller, IResult result)
    {
        return result.MatchObject<ActionResult>(
            successHandler: value => value.Type switch
            {
                SuccessType.Accepted => controller.Accepted(),
                SuccessType.Ok => controller.Ok(value.Value),
                SuccessType.NoContent => controller.NoContent(),
                _ => throw new ArgumentOutOfRangeException()
            },
            errorHandler: error => error.Type switch
            {
                ErrorType.Error => controller.StatusCode((int)HttpStatusCode.InternalServerError, error.Errors),
                ErrorType.Invalid => controller.StatusCode((int)HttpStatusCode.BadRequest, error.Errors),
                ErrorType.NotFound => controller.NotFound(),
                ErrorType.Unauthorized => controller.Unauthorized(),
                ErrorType.Conflict => controller.Conflict(),
                ErrorType.Forbidden => controller.Forbid(),
                _ => throw new ArgumentOutOfRangeException()
            });
    }
}