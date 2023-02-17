using System.Net;

namespace Domain.Exceptions
{
    public class InternalServerErrorException : AppException
    {
        public InternalServerErrorException(string message)
            : base(message, null, HttpStatusCode.InternalServerError)
        {
        }
    }
}