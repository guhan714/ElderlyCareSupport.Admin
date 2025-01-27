using ElderlyCareSupport.Admin.Contracts.Response;
using ElderlyCareSupport.Admin.Domain.Models;

namespace ElderlyCareSupport.Admin.Application.IService;

public interface IUserService
{
    Task<PagedResponse<User>> GetAllUsersAsync(UserQueryParameters userQueryParameters);
    
    Task<User> GetUserByIdAsync(string userId);
    
    Task<User> AddUserAsync(User user);
    
    Task<Tuple<User, bool>> UpdateUserAsync(User user);
    
    Task<Tuple<User, bool>> DeleteUserAsync(string userId);
}