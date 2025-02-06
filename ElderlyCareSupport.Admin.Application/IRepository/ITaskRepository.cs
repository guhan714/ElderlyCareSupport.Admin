using ElderlyCareSupport.Admin.Contracts.Request;
using ElderlyCareSupport.Admin.Contracts.Response;
using ElderlyCareSupport.Admin.Domain.Models;

namespace ElderlyCareSupport.Admin.Application.IRepository;

public interface ITaskRepository
{
    Task<PagedResponse<TaskDetails>> GetAllTaskDetails(PageQueryParameters taskQueryParameters);
    Task<TaskDetails?> GetTaskDetails(long id);
    
    Task<Tuple<bool, long>> CancelTaskAsync(long id);
}