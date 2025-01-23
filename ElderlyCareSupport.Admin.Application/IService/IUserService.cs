using ElderlyCareSupport.Admin.Contracts.Response;
using ElderlyCareSupport.Admin.Domain.Models;

namespace ElderlyCareSupport.Admin.Application.IService;

public interface IUserService
{
    Task<PagedResoonse<User>> GetAllUsersAsync(UserQueryParameters userQueryParameters);
    
    Task<User> GetUserByIdAsync(string userId);
    
    Task<User> AddUserAsync(User user);
    
    Task<User> UpdateUserAsync(string userId,User user);
    
    Task<Tuple<User, bool>> DeleteUserAsync(string userId);
}