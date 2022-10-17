using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SFA.DAS.TrackProgressInternal.Application.Services;

public class HttpResponseMessageResult : IActionResult
{
    private readonly HttpResponseMessage _responseMessage;

    public HttpResponseMessageResult(HttpResponseMessage responseMessage)
    {
        _responseMessage = responseMessage ??
            throw new ArgumentNullException(nameof(responseMessage));
    }

    public async Task ExecuteResultAsync(ActionContext context)
    {
        context.HttpContext.Response.StatusCode = (int)_responseMessage.StatusCode;

        if (_responseMessage.Content.Headers.TryGetValues("Content-Type", out var values))
            context.HttpContext.Response.Headers.TryAdd("Content-Type", new StringValues(values.ToArray()));

        using var stream = await _responseMessage.Content.ReadAsStreamAsync();
        await stream.CopyToAsync(context.HttpContext.Response.Body);
        await context.HttpContext.Response.Body.FlushAsync();
    }
}