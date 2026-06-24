using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace SFA.DAS.VacanciesManage.Api.Middlewares;

public class ExceptionMiddleware(RequestDelegate next)
{
    private const string ValidationUri = "https://tools.ietf.org/html/rfc7231#section-6.5.1";

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            await HandleValidationException(context, ex);
        }
        catch
        {
            await HandleGenericException(context);
        }
    }

    private static async Task HandleValidationException(HttpContext context,
        ValidationException ex)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        var errors = ex.Errors
            .GroupBy(x => x.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => x.ErrorMessage).ToArray());

        var problemDetails = new ValidationProblemDetails(errors)
        {
            Type = ValidationUri,
            Title = "One or more validation errors occurred.",
            Status = StatusCodes.Status400BadRequest,
            Extensions =
            {
                ["traceId"] = context.TraceIdentifier
            }
        };

        await context.Response.WriteAsJsonAsync(problemDetails);
    }

    private static async Task HandleGenericException(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var response = new ValidationProblemDetails
        {
            Type = ValidationUri,
            Title = "Internal server error",
            Status = StatusCodes.Status500InternalServerError,
            Extensions =
            {
                ["traceId"] = context.TraceIdentifier
            }
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}