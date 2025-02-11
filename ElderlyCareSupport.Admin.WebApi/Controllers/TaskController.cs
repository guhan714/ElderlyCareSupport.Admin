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
[MacAddressFilter]
public class TaskController : BaseController
{
    private readonly ITaskService _service;

    public TaskController(ITaskService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTasks([FromBody] PageQueryParameters parameters)
    {
        var tasks = await _service.GetAllTasksAsync(parameters);
        return ApiResponse(true, HttpStatusCode.OK, tasks);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTaskById(int id)
    {
        var result = await _service.GetTaskByIdAsync(id);
        if (string.IsNullOrEmpty(result.TaskName))
            return ApiResponse(false, HttpStatusCode.NotFound, result, [new Error("Task Not Found")]);

        return ApiResponse(true, HttpStatusCode.OK, result);
    }

    [HttpPut("cancel/{taskId}")]
    public async Task<IActionResult> CancelTaskById(long taskId)
    {
        var (isTaskCancelled, idTask) = await _service.CancelTaskAsync(taskId);

        if (!isTaskCancelled)
            return ApiResponse(isTaskCancelled, HttpStatusCode.Conflict, idTask,
                [new Error("Task was not Cancelled")]);
        return ApiResponse(isTaskCancelled, HttpStatusCode.NoContent, idTask);
    }
    
}