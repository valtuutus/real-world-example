using Valtuutus.RealWorld.Api.Results;

namespace Valtuutus.RealWorld.Api.Core;

public interface IUseCase<in TRequest, TResult>
{
    Task<Result<TResult>> Handle(TRequest req, CancellationToken ct);
}