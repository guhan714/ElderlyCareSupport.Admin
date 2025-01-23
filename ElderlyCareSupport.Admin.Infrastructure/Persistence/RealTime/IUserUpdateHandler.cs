using ElderlyCareSupport.Admin.Domain.Models;

namespace ElderlyCareSupport.Admin.Infrastructure.Persistence.RealTime;

public interface IUserUpdateHandler
{
    Task ReceiveUserUpdates(User user);
}