using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using StrawberryShake;

namespace SFA.DAS.RecruitJobs.GraphQL.RecruitInner.Mappers;

public static class OperationResultExtensions
{
    private static string FormatErrors(this IReadOnlyList<IClientError> clientErrors)
    {
        var sb = new StringBuilder();
        foreach (var clientError in clientErrors)
        {
            var path = clientError.Path is { Count: > 0 }
                ? $" (path: {string.Join("/", clientError.Path.Select(x => $"{x}"))})"
                : null;
            sb.AppendLine($"{clientError.Code}{path}: {clientError.Message}");
        }
        return sb.ToString();
    }
    
    public static string FormatErrors<T>(this IOperationResult<T> operationResult) where T : class
    {
        return operationResult is not { Errors: { Count: > 0 } errors }
            ? string.Empty
            : $"The following errors were returned in the GQL response:{Environment.NewLine}{errors.FormatErrors()}";
    }

    public static ProblemDetails ToProblemDetails<T>(this IOperationResult<T> operationResult) where T : class
    {
        return operationResult is not { Errors: { Count: > 0 } errors }
            ? null
            : new ProblemDetails
            {
                Title = "The following errors occurred",
                Detail = errors.FormatErrors()
            };
    }
}