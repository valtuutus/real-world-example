using Valtuutus.RealWorld.Api.Core.Entities;
using Vogen;

namespace Valtuutus.RealWorld.Api.Config.Vogen;

[EfCoreConverter<ProjectId>]
[EfCoreConverter<ProjectStatusId>]
[EfCoreConverter<TaskId>]
[EfCoreConverter<TeamId>]
[EfCoreConverter<UserId>]
[EfCoreConverter<WorkspaceId>]
internal partial class VogenEfCoreConverters;