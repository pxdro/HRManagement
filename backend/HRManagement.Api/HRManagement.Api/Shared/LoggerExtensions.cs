using HRManagement.Application.DTOs;
using System.Net;

namespace HRManagement.Api.Shared
{
    public static class LoggerExtensions
    {
        public static void LogResult<T>(this ILogger logger, string method, string action, ResultDto<T> result)
        {
            if (result.StatusCode == HttpStatusCode.InternalServerError)
                logger.LogError("{Method} {Action} internal error with message {ErrorMessage}", method, action, result.ErrorMessage);
            else if (result.IsSuccess)
                logger.LogInformation("{Method} {Action} succeeded with code {StatusCode} and data {@Data}", method, action, (int)result.StatusCode, result.Data);
            else
                logger.LogInformation("{Method} {Action} failed with code {StatusCode} and message {ErrorMessage}", method, action, (int)result.StatusCode, result.ErrorMessage);
        }
    }
}
