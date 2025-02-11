using ElderlyCareSupport.Admin.Contracts.Request;
using ElderlyCareSupport.Admin.Contracts.Response;
using ElderlyCareSupport.Admin.Domain.Models;

namespace ElderlyCareSupport.Admin.Application.IService;

public interface IUserService
{
    Task<PagedResponse<User>> GetAllUsersAsync(PageQueryParameters? userQueryParameters);
    
    Task<User> GetUserByIdAsync(string userId);
    
    Task<Tuple<User, bool>> AddUserAsync(User user);
    
    Task<Tuple<User, bool>> UpdateUserAsync(User user);
    
    Task<Tuple<string, bool>> DeleteUserAsync(string userId);
}