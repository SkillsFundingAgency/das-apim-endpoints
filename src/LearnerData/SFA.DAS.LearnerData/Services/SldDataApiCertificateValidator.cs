using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using SFA.DAS.LearnerData.Configuration;

namespace SFA.DAS.LearnerData.Services;

public class SldDataApiCertificateValidator(SLDDataApiConfiguration configuration)
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

        return isSldRequest
            ? IsTrusted(certificate)
            : sslPolicyErrors == SslPolicyErrors.None;
    }

    // Keep hex digits only, so spaces, colons and the hidden characters Windows adds when copying from the certificate dialog are ignored
    private static string Normalise(string? thumbprint)
    {
        return new string((thumbprint ?? string.Empty).Where(Uri.IsHexDigit).ToArray());
    }
}
