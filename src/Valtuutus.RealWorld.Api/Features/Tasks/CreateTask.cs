﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Valtuutus.Core;
using Valtuutus.Data.Db;
using Valtuutus.Lang;
using Valtuutus.RealWorld.Api.Core;
using Valtuutus.RealWorld.Api.Core.Auth;
using Valtuutus.RealWorld.Api.Core.Entities;
using Valtuutus.RealWorld.Api.Results;
using Task = Valtuutus.RealWorld.Api.Core.Entities.Task;

namespace Valtuutus.RealWorld.Api.Features.Tasks;

public record CreateTaskReqBody
{
    public required string Name { get; init; }
    public required ProjectStatusId ProjectStatusId { get; init; }
}

public record CreateTask
{
    public required ProjectId ProjectId { get; init; }
    public required CreateTaskReqBody Body { get; init; }
}

public class CreateTaskHandler(Context context, ISessaoManager manager, IDbDataWriterProvider dataWriterProvider) : IUseCase<CreateTask, TaskId>
{
    public async Task<Result<TaskId>> Handle(CreateTask req, CancellationToken ct)
    {
        var task = new Task()
        {
            Id = TaskId.New(),
            Name = req.Body.Name,
            ProjectId = req.ProjectId,
            ProjectStatusId = req.Body.ProjectStatusId,
            Order = 0
        };
        
        context.Tasks.Add(task);

        // we shouldn't auto add the current user to the task
        var taskAssignee = new TaskAssignee()
            { TaskId = task.Id, UserId = manager.UsuarioId};
        
        context.TaskAssignees.Add(taskAssignee);
        
        var transaction = await context.Database.BeginTransactionAsync(ct);

        await dataWriterProvider.Write(context.Database.GetDbConnection(), transaction.GetDbTransaction(), [
            new RelationTuple(SchemaConstsGen.Task.Name, task.Id.ToString(), SchemaConstsGen.Task.Relations.Assignee, SchemaConstsGen.User.Name, manager.UsuarioId.ToString())
        ], [], ct);

        
        await context.SaveChangesAsync(ct);
        
        return task.Id;
    }
}