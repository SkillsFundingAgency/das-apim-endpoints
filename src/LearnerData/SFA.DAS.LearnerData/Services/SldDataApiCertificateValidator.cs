using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Logging;
using SFA.DAS.LearnerData.Configuration;

namespace SFA.DAS.LearnerData.Services;

public class SldDataApiCertificateValidator(SLDDataApiConfiguration configuration, ILogger<SldDataApiCertificateValidator> logger)
{
    private readonly HashSet<string> _trustedThumbprints = configuration.CertificateThumbprints
        .Select(Normalise)
        .Where(thumbprint => thumbprint.Length > 0)
        .ToHashSet(StringComparer.OrdinalIgnoreCase);

    private readonly string? _sldHost = Uri.TryCreate(configuration.Url, UriKind.Absolute, out var url) ? url.Host : null;

    public bool IsTrusted(X509Certificate2? certificate)
    {
        return certificate != null && _trustedThumbprints.Contains(Normalise(certificate.Thumbprint));
    }

    // The default HttpClient is shared with the Azure AD token call, so only requests to the SLD host are pinned
    public bool IsServerCertificateValid(HttpRequestMessage request, X509Certificate2? certificate, X509Chain? chain, SslPolicyErrors sslPolicyErrors)
    {
        var isSldRequest = _sldHost != null && string.Equals(request.RequestUri?.Host, _sldHost, StringComparison.OrdinalIgnoreCase);

        if (!isSldRequest)
        {
            return sslPolicyErrors == SslPolicyErrors.None;
        }

        var trusted = IsTrusted(certificate);

        // Temporary diagnostics: only the first 5 characters of each thumbprint are logged
        logger.LogInformation(
            "SLD server certificate pinning for {Host}: {Result}. Presented thumbprint starts {PresentedPrefix}; {ConfiguredCount} configured thumbprint(s) start {ConfiguredPrefixes}. SslPolicyErrors {SslPolicyErrors}, subject {Subject}, issuer {Issuer}, expires {NotAfter:u}",
            request.RequestUri?.Host,
            trusted ? "trusted" : "REJECTED",
            Prefix(certificate?.Thumbprint),
            _trustedThumbprints.Count,
            string.Join(", ", _trustedThumbprints.Select(Prefix)),
            sslPolicyErrors,
            certificate?.Subject,
            certificate?.Issuer,
            certificate?.NotAfter);

        return trusted;
    }

    private static string Prefix(string? thumbprint)
    {
        var normalised = Normalise(thumbprint);
        return normalised.Length > 5 ? normalised[..5] : normalised;
    }

    // Keep hex digits only, so spaces, colons and the hidden characters Windows adds when copying from the certificate dialog are ignored
    private static string Normalise(string? thumbprint)
    {
        return new string((thumbprint ?? string.Empty).Where(Uri.IsHexDigit).ToArray());
    }
}
