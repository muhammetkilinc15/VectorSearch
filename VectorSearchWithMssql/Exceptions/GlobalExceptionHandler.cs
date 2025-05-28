using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace VectorSearchWithMssql.Exceptions
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            int statusCode = httpContext.Response.StatusCode;
            string message = exception.Message;

            ProblemDetails details = new()
            {
                Status = statusCode,
                Type = "https://tools.ietf.org/html/rfc7231#section-6",
                Title = "An error occurred while processing your request.",
                Detail = message
            };
            await httpContext.Response.WriteAsJsonAsync(details);
            return true;
        }
    }
}
