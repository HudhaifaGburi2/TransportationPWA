using System.Net;
using System.Text.Json;
using TransportationAttendance.Application.DTOs.Common;
using TransportationAttendance.Domain.Exceptions;

namespace TransportationAttendance.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, response) = exception switch
        {
            EntityNotFoundException e => (
                HttpStatusCode.NotFound,
                ApiResponse.FailureResponse(e.Message)
            ),
            DuplicateEntityException e => (
                HttpStatusCode.Conflict,
                ApiResponse.FailureResponse(e.Message)
            ),
            ValidationException e => (
                HttpStatusCode.BadRequest,
                ApiResponse.FailureResponse(e.Message, e.Errors)
            ),
            UnauthorizedException e => (
                HttpStatusCode.Unauthorized,
                ApiResponse.FailureResponse(e.Message)
            ),
            ForbiddenException e => (
                HttpStatusCode.Forbidden,
                ApiResponse.FailureResponse(e.Message)
            ),
            DomainException e => (
                HttpStatusCode.BadRequest,
                ApiResponse.FailureResponse(e.Message)
            ),
            _ => (
                HttpStatusCode.InternalServerError,
                ApiResponse.FailureResponse("An unexpected error occurred.")
            )
        };

        _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var json = JsonSerializer.Serialize(response, options);
        await context.Response.WriteAsync(json);
    }
}

public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
