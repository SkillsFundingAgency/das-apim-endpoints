using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderRelationships;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests;
public class WhenBuildingGetEmployerRelationshipsRequest
{
    [Test]
    [AutoData]
    public void Then_The_Url_Is_Correctly_Constructed(string accountHashedId, long ukprn, string accountlegalentityPublicHashedId)
    {
        var actual = new GetEmployerRelationshipsRequest(accountHashedId, ukprn, accountlegalentityPublicHashedId);

        actual.GetUrl.Should().Be($"relationships/employeraccount/{accountHashedId}?Ukprn={ukprn}&AccountlegalentityPublicHashedId={accountlegalentityPublicHashedId}");
    }
}
