using ElderlyCareSupport.Admin.Contracts.Request;
using ElderlyCareSupport.Admin.Contracts.Response;
using ElderlyCareSupport.Admin.Domain.Models;

namespace ElderlyCareSupport.Admin.Application.IRepository;

public interface IUserRepository
{
    Task<PagedResponse<User>> GetAllUsersAsync(PageQueryParameters? userQueryParameters);
    
    Task<User?> GetUserByIdAsync(string userId);
    
    Task<User> AddUserAsync(User user);
    
    Task<Tuple<User, bool>> UpdateUserAsync(User user);
    
    Task<Tuple<string, bool>> DeleteUserAsync(string userId);
}