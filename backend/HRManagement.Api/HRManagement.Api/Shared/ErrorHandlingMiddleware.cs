using FluentValidation;
using HRManagement.Application.DTOs;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;
    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    public async Task Invoke(HttpContext ctx)
    {
        try
        {
            await _next(ctx);
        }
        catch (ValidationException vex)
        {
            ctx.Response.StatusCode = StatusCodes.Status400BadRequest;
            var resp = new ErrorResponseDto
            {
                StatusCode = 400,
                Message = "Validation failed",
                Errors = vex.Errors
                                   .GroupBy(e => e.PropertyName)
                                   .ToDictionary(
                                       grp => grp.Key,
                                       grp => grp.Select(e => e.ErrorMessage).ToArray()
                                   )
            };
            await ctx.Response.WriteAsJsonAsync(resp);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            ctx.Response.StatusCode = StatusCodes.Status500InternalServerError;
            var resp = new ErrorResponseDto
            {
                StatusCode = 500,
                Message = "An unexpected error occurred"
            };
            await ctx.Response.WriteAsJsonAsync(resp);
        }
    }
}
