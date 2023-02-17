using System.Net;

namespace Domain.Exceptions
{
    public class UnauthorizedException : AppException
    {
        public UnauthorizedException(string message = "Unauthorized")
            : base(message, null, HttpStatusCode.Unauthorized)
        {
        }
    }
}