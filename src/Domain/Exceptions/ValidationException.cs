using System.Net;

namespace Domain.Exceptions;

public class ValidationException : AppException
{
    public ValidationException(string message = "One or more validation errors occurred.")
        : base(message, null, HttpStatusCode.BadRequest)
    {
    }

    public ValidationException(List<string> messages)
        : base("", messages, HttpStatusCode.BadRequest)
    {
    }
}
