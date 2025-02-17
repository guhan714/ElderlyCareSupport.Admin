using System.Net;
using Asp.Versioning;
using ElderlyCareSupport.Admin.Application.IService;
using ElderlyCareSupport.Admin.Contracts.Request;
using ElderlyCareSupport.Admin.Contracts.Response;
using ElderlyCareSupport.Admin.WebApi.Abstractions;
using ElderlyCareSupport.Admin.WebApi.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElderlyCareSupport.Admin.WebApi.Controllers;

[ApiController]
[Route("v{v:apiVersion}/tasks")]
[ApiVersion(1)]
[Produces("application/json")]
[Authorize(Roles = "Admin")]
[MacFilter]
public class TaskController : BaseController
{
    private readonly ITaskService _service;

    public TaskController(ITaskService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTasks([FromBody] PageQueryParameters? parameters, CancellationToken cancellationToken)
    {
        var tasks = await _service.GetAllTasksAsync(parameters);
        return SuccessResult(HttpStatusCode.OK, tasks);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTaskById(int id)
    {
        var result = await _service.GetTaskByIdAsync(id);
        if (string.IsNullOrEmpty(result.TaskName))
            return FailureResult(HttpStatusCode.NotFound, [new Error("Task Not Found")]);

        return SuccessResult(HttpStatusCode.OK, result);
    }

    [HttpPut("cancel/{taskId}")]
    public async Task<IActionResult> CancelTaskById(long taskId)
    {
        var (isTaskCancelled, idTask) = await _service.CancelTaskAsync(taskId);

        if (!isTaskCancelled)
            return FailureResult( HttpStatusCode.Conflict,
                [new Error($"Task {taskId} was not Cancelled")]);
        
        return SuccessResult(HttpStatusCode.NoContent, idTask);
    }
    
}