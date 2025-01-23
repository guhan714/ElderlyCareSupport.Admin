using Microsoft.AspNetCore.SignalR;

namespace ElderlyCareSupport.Admin.Infrastructure.Persistence.RealTime;

internal sealed class UserHub : Hub<IUserUpdateHandler>
{
    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        await base.OnDisconnectedAsync(exception);
    }

    
}