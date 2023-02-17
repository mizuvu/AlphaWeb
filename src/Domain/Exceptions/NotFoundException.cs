using System.Net;

namespace Domain.Exceptions
{
    public class NotFoundException : AppException
    {
        public NotFoundException(string message)
            : base(message, null, HttpStatusCode.NotFound)
        {
        }

        public NotFoundException(string key, string objectName)
            : base($"Queried object {objectName} by {key} was not found", null, HttpStatusCode.NotFound)
        {
        }
    }
}