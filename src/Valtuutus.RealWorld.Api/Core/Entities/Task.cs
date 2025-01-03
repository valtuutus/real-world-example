﻿
using StronglyTypedIds;

namespace Valtuutus.RealWorld.Api.Core.Entities;

[StronglyTypedId]
public partial struct TaskId;

public class Task
{
    public required ProjectStatusId ProjectStatusId { get; init; }
    public ProjectStatus ProjectStatus { get; init; } = null!;
    public required ProjectId ProjectId { get; init; }
    public Project Project { get; init; } = null!;
    public required TaskId Id { get; init; }
    public required string Name { get; init; }
    public required float Order { get; init; }
    public List<TaskAssignee> Assignees { get; init; } = null!;
}