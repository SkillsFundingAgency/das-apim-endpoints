using System.Net.Security;
using Microsoft.Extensions.Logging.Abstractions;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using SFA.DAS.LearnerData.Configuration;
using SFA.DAS.LearnerData.Services;

namespace SFA.DAS.LearnerData.UnitTests.Services;

[TestFixture]
public class SldDataApiCertificateValidatorTests
{
    private const string OtherThumbprint = "0123456789ABCDEF0123456789ABCDEF01234567";

    private X509Certificate2 _certificate = null!;

    [SetUp]
    public void SetUp() => _certificate = CreateSelfSignedCertificate();

    [TearDown]
    public void TearDown() => _certificate.Dispose();

    [Test]
    public void Then_a_certificate_matching_the_configured_thumbprint_is_trusted()
    {
        var sut = CreateValidator(_certificate.Thumbprint);

        sut.IsTrusted(_certificate).Should().BeTrue();
    }

    [Test]
    public void Then_a_certificate_matching_any_of_the_configured_thumbprints_is_trusted()
    {
        var sut = CreateValidator(OtherThumbprint, _certificate.Thumbprint);

        sut.IsTrusted(_certificate).Should().BeTrue();
    }

    [Test]
    public void Then_a_certificate_not_matching_any_configured_thumbprint_is_not_trusted()
    {
        var sut = CreateValidator(OtherThumbprint);

        sut.IsTrusted(_certificate).Should().BeFalse();
    }

    [Test]
    public void Then_the_thumbprint_comparison_ignores_case()
    {
        var sut = CreateValidator(_certificate.Thumbprint.ToLowerInvariant());

        sut.IsTrusted(_certificate).Should().BeTrue();
    }

    [Test]
    public void Then_the_thumbprint_comparison_ignores_spaces_and_colons()
    {
        var pairs = Enumerable.Range(0, _certificate.Thumbprint.Length / 2)
            .Select(i => _certificate.Thumbprint.Substring(i * 2, 2))
            .ToArray();

        CreateValidator(string.Join(" ", pairs)).IsTrusted(_certificate).Should().BeTrue();
        CreateValidator(string.Join(":", pairs)).IsTrusted(_certificate).Should().BeTrue();
    }

    [Test]
    public void Then_a_certificate_is_not_trusted_when_no_thumbprints_are_configured()
    {
        var sut = CreateValidator();

        sut.IsTrusted(_certificate).Should().BeFalse();
    }

    [Test]
    public void Then_a_certificate_is_not_trusted_when_the_only_configured_thumbprints_are_blank()
    {
        var sut = CreateValidator("", "  ");

        sut.IsTrusted(_certificate).Should().BeFalse();
    }

    [Test]
    public void Then_a_missing_certificate_is_not_trusted()
    {
        var sut = CreateValidator(_certificate.Thumbprint);

        sut.IsTrusted(null).Should().BeFalse();
    }

    [Test]
    public void Then_a_request_to_the_sld_host_is_valid_when_the_thumbprint_matches_even_with_policy_errors()
    {
        var sut = CreateValidatorForUrl("https://sld.example.test/", _certificate.Thumbprint);
        var request = new HttpRequestMessage(HttpMethod.Get, "https://sld.example.test/api/lars/get-em-value/ABC/2025-08-01");

        sut.IsServerCertificateValid(request, _certificate, null, SslPolicyErrors.RemoteCertificateChainErrors)
            .Should().BeTrue();
    }

    [Test]
    public void Then_a_request_to_the_sld_host_is_not_valid_when_the_thumbprint_does_not_match_even_with_no_policy_errors()
    {
        var sut = CreateValidatorForUrl("https://sld.example.test/", OtherThumbprint);
        var request = new HttpRequestMessage(HttpMethod.Get, "https://sld.example.test/api/lars/get-em-value/ABC/2025-08-01");

        sut.IsServerCertificateValid(request, _certificate, null, SslPolicyErrors.None)
            .Should().BeFalse();
    }

    [Test]
    public void Then_a_request_to_another_host_is_valid_only_when_there_are_no_policy_errors()
    {
        var sut = CreateValidatorForUrl("https://sld.example.test/", _certificate.Thumbprint);
        var request = new HttpRequestMessage(HttpMethod.Post, "https://login.example.test/tenant/oauth2/v2.0/token");

        sut.IsServerCertificateValid(request, _certificate, null, SslPolicyErrors.None).Should().BeTrue();
        sut.IsServerCertificateValid(request, _certificate, null, SslPolicyErrors.RemoteCertificateNameMismatch).Should().BeFalse();
    }

    [Test]
    public void Then_a_request_with_no_uri_is_valid_only_when_there_are_no_policy_errors()
    {
        var sut = CreateValidatorForUrl("https://sld.example.test/", _certificate.Thumbprint);
        var request = new HttpRequestMessage();

        sut.IsServerCertificateValid(request, _certificate, null, SslPolicyErrors.None).Should().BeTrue();
        sut.IsServerCertificateValid(request, _certificate, null, SslPolicyErrors.RemoteCertificateChainErrors).Should().BeFalse();
    }

    private static SldDataApiCertificateValidator CreateValidatorForUrl(string url, params string[] thumbprints) =>
        new(new SLDDataApiConfiguration { Url = url, CertificateThumbprints = thumbprints }, NullLogger<SldDataApiCertificateValidator>.Instance);

    private static SldDataApiCertificateValidator CreateValidator(params string[] thumbprints) =>
        new(new SLDDataApiConfiguration { CertificateThumbprints = thumbprints }, NullLogger<SldDataApiCertificateValidator>.Instance);

    private static X509Certificate2 CreateSelfSignedCertificate()
    {
        using var key = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        var request = new CertificateRequest("CN=sld-data-api-test", key, HashAlgorithmName.SHA256);
        return request.CreateSelfSigned(DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.UtcNow.AddDays(1));
    }
}
