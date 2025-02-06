using ElderlyCareSupport.Admin.Contracts.Request;
using ElderlyCareSupport.Admin.Contracts.Response;
using ElderlyCareSupport.Admin.Domain.Models;

namespace ElderlyCareSupport.Admin.Application.IService;

public interface ITaskService
{
    Task<PagedResponse<TaskDetails>> GetAllTasksAsync(PageQueryParameters pageQueryParameters);
    Task<TaskDetails> GetTaskByIdAsync(long taskId);
    Task<Tuple<bool, long>> CancelTaskAsync(long taskId);
}