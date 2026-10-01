using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.LearnerData.Services;

// Temporary diagnostics for FLP-1281: records what we sent to the SLD host and what came back (never the query string, headers sent or token)
public class SldDataApiLoggingHandler(ILogger<SldDataApiLoggingHandler> logger) : DelegatingHandler
{
    private const int MaxBodyCharacters = 200;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var uri = request.RequestUri;
        var target = uri == null ? "(no uri)" : $"{uri.Scheme}://{uri.Authority}{uri.AbsolutePath}";
        var hasAuthorization = request.Headers.Authorization != null;
        var tokenClaims = DescribeToken(request.Headers.Authorization?.Parameter);

        HttpResponseMessage response;
        try
        {
            response = await base.SendAsync(request, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "SLD request {Method} {Target} failed before a response was received", request.Method, target);
            throw;
        }

        response.Headers.TryGetValues("x-correlation-id", out var correlationIds);
        var body = string.Empty;
        if (!response.IsSuccessStatusCode && response.Content != null)
        {
            await response.Content.LoadIntoBufferAsync();
            var text = await response.Content.ReadAsStringAsync(cancellationToken);
            body = text.Length > MaxBodyCharacters ? text[..MaxBodyCharacters] : text;
        }

        logger.LogInformation(
            "SLD request {Method} {Target} (authorization header sent: {HasAuthorization}, token claims: {TokenClaims}) returned {StatusCode}. Correlation id {CorrelationId}, content type {ContentType}, content length {ContentLength}{Body}",
            request.Method,
            target,
            hasAuthorization,
            tokenClaims,
            (int)response.StatusCode,
            correlationIds == null ? null : string.Join(",", correlationIds),
            response.Content?.Headers.ContentType?.ToString(),
            response.Content?.Headers.ContentLength,
            body.Length == 0 ? string.Empty : $", body starts: {body}");

        return response;
    }

    // Only the non-secret identifying claims are read from the token payload; the token itself is never logged
    private static string DescribeToken(string? token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return "none";
        }

        try
        {
            var parts = token.Split('.');
            if (parts.Length < 2)
            {
                return "not a JWT";
            }

            var payload = parts[1].Replace('-', '+').Replace('_', '/');
            payload = payload.PadRight(payload.Length + (4 - payload.Length % 4) % 4, '=');
            using var document = JsonDocument.Parse(Encoding.UTF8.GetString(Convert.FromBase64String(payload)));

            var claims = new[] { "aud", "iss", "appid", "azp", "tid", "roles", "scp", "exp" }
                .Where(name => document.RootElement.TryGetProperty(name, out _))
                .Select(name => $"{name}={document.RootElement.GetProperty(name)}");

            return string.Join("; ", claims);
        }
        catch (Exception ex)
        {
            return $"unreadable ({ex.GetType().Name})";
        }
    }
}
