using ElderlyCareSupport.Admin.Application.IRepository;
using ElderlyCareSupport.Admin.Application.IService;
using ElderlyCareSupport.Admin.Contracts.Request;
using ElderlyCareSupport.Admin.Contracts.Response;
using ElderlyCareSupport.Admin.Domain.Models;
using ElderlyCareSupport.Admin.Logging.ILogger;

namespace ElderlyCareSupport.Admin.Application.Service;

public class TaskService : ITaskService
{
    private readonly ILoggerFactory _logger;
    private readonly ITaskRepository _taskRepository;

    public TaskService(ILoggerFactory logger, ITaskRepository taskRepository)
    {
        _logger = logger;
        _taskRepository = taskRepository;
    }

    public async Task<PagedResponse<TaskDetails>> GetAllTasksAsync(PageQueryParameters pageQueryParameters)
    {
        var taskResults = await _taskRepository.GetAllTaskDetails(pageQueryParameters);
        return taskResults;
    }

    public async Task<TaskDetails> GetTaskByIdAsync(long taskId)
    {
        var result = await _taskRepository.GetTaskDetails(taskId);
        return result ?? new TaskDetails();
    }

    public async Task<Tuple<bool, long>> CancelTaskAsync(long taskId)
    {
        var result = await _taskRepository.CancelTaskAsync(taskId);
        return result;
    }
}