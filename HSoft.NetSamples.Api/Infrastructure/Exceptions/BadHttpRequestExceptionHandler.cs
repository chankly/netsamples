using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HSoft.NetSamples.Api.Infrastructure.Exceptions
{
    public class BadHttpRequestExceptionHandler : IExceptionHandler
    {
        ILogger<BadHttpRequestExceptionHandler> _logger;
        
        public BadHttpRequestExceptionHandler(ILogger<BadHttpRequestExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is not BadHttpRequestException) 
            {
                return false;
            }

            _logger.LogTrace("TRACE");
            _logger.LogDebug("DEBUG");
            _logger.LogInformation("INFO");
            _logger.LogWarning("WARNINNG");
            _logger.LogError("ERROR");
            _logger.LogCritical("CRITICAL");


            _logger.LogError(exception.Message, exception);

            var result = new ProblemDetails();
            result = new ProblemDetails
            {
                Status = (int)HttpStatusCode.NotFound,
                Type = exception.GetType().Name,
                Title = "An bad request error occurred",
                Detail = exception.Message,
                Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"
            };

            await httpContext.Response.WriteAsJsonAsync(result, cancellationToken: cancellationToken);
            return true;
        }
    }
}
