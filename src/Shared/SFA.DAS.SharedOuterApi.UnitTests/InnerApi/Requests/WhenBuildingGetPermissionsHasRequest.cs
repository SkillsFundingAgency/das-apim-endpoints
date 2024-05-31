using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderRelationships;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests;
public class WhenBuildingGetPermissionsRequest
{
    [TestCase(12345678, 1)]
    [TestCase(12345678, null)]
    [TestCase(null, 2)]
    public void Then_The_Url_Is_Correctly_Constructed(long? ukprn, int? accountLegalEntityId)
    {
        var url = $"permissions";

        var expectedQueryString = $"Ukprn={ukprn}&accountLegalEntityId={accountLegalEntityId}";

        var actual = new GetPermissionsRequest(ukprn, accountLegalEntityId);

        var composedUrl = $"{url}?{expectedQueryString}";

        actual.GetUrl.Should().Be(composedUrl);
    }
}
