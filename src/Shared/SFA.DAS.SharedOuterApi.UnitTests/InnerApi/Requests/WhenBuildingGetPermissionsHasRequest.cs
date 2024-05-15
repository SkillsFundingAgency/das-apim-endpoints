using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderRelationships;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests;
public class WhenBuildingGetPermissionsRequest
{
    [TestCase(12345678, "hash1")]
    [TestCase(12345678, null)]
    [TestCase(null, "hash2")]
    public void Then_The_Url_Is_Correctly_Constructed(long? ukprn, string? hashedId)
    {
        var url = $"permissions";

        var expectedQueryString = $"Ukprn={ukprn}&PublicHashedId={hashedId}";

        var actual = new GetPermissionsRequest(ukprn, hashedId);

        var composedUrl = $"{url}?{expectedQueryString}";

        actual.GetUrl.Should().Be(composedUrl);
    }
}
