using ElderlyCareSupport.Admin.Contracts.Response;
using ElderlyCareSupport.Admin.Domain.Models;

namespace ElderlyCareSupport.Admin.Application.IRepository;

public interface IUserRepository
{
    Task<PagedResoonse<User>> GetAllUsersAsync(UserQueryParameters userQueryParameters);
    
    Task<User?> GetUserByIdAsync(string userId);
    
    Task<User> AddUserAsync(User user);
    
    Task<Tuple<User, bool>> UpdateUserAsync(User user);
    
    Task<Tuple<User, bool>> DeleteUserAsync(string userId);
}