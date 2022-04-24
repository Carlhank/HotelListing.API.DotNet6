using System.Net;
using HotelListing.API.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace HotelListing.API.Core.Middleware;

public class ExceptionMiddleware
{
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next,
        ILogger<ExceptionMiddleware> logger)
    {
        _logger = logger;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Something Went Wrong while processing {Path}", context.Request.Path);
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        var statusCode = HttpStatusCode.InternalServerError;
        var errorDetails = new ErrorDetails
        {
            ErrorType = "Fail",
            ErrorMessage = exception.Message
        };

        switch (exception)
        {
            case NotFoundException notFoundException:
                statusCode = HttpStatusCode.NotFound;
                errorDetails.ErrorType = "Not Found";
                break;
            
            default:
                break;
        }

        var response = JsonConvert.SerializeObject(errorDetails);
        context.Response.StatusCode = (int)statusCode;
        return context.Response.WriteAsync(response);
    }
}

public class ErrorDetails
{
    public string ErrorType { get; set; }
    public string ErrorMessage { get; set; }
}