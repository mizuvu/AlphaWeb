using Application.Contracts.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Infrastructure.SignalR
{
    public class CustomIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            return connection.User?.FindFirst(AppClaimType.UserId)?.Value;
        }
    }
}
