using Microsoft.AspNetCore.Http;
using SFA.DAS.VacanciesManage.Api.Models;
using System;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using FluentValidation;

namespace SFA.DAS.VacanciesManage.Api.Middlewares;

public class ExceptionMiddleware(RequestDelegate next)
{
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
        catch (Exception ex)
        {
            await HandleGenericException(context, ex);
        }
    }

    private static async Task HandleValidationException(HttpContext context,
        ValidationException ex)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        var traceId = context.TraceIdentifier;

        var messages = ex.Errors
            .SelectMany(x => x.ErrorMessage)
            .ToList();

        var response = new CreateVacancyExampleBadRequestResponse
        {
            Type = new Uri("https://tools.ietf.org/html/rfc7231#section-6.5.1"),
            Title = $"Validation failed: {string.Join("; ", messages)}",
            Status = 400,
            TraceId = traceId
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private static async Task HandleGenericException(HttpContext context,
        Exception _)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = 500;

        var response = new CreateVacancyExampleBadRequestResponse
        {
            Type = new Uri("https://tools.ietf.org/html/rfc7231#section-6.6.1"),
            Title = "Internal server error",
            Status = 500,
            TraceId = context.TraceIdentifier
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}