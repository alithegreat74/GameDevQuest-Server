using GamedevQuest.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace GamedevQuest.Helpers
{
    public class IpAddressHelper
    {
        public OperationResult<string> GetIpAddress(HttpContext context, HttpRequest request)
        {
            if (context.Connection.RemoteIpAddress == null)
                return new OperationResult<string>(new UnprocessableEntityObjectResult("user ip is null"));
            string? ipAddress = request.Headers["X-Forwarded-For"].FirstOrDefault()?.Split(',').FirstOrDefault()?.Trim();
            ipAddress ??= context.Connection.RemoteIpAddress.MapToIPv4().ToString();
            return new OperationResult<string>(ipAddress);
        }
    }
}
