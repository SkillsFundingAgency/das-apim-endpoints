using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;

namespace SFA.DAS.ProviderFeedback.Domain.UnitTests.ProviderAccounts.Api;

public class WhenBuildingGetProviderAccountRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Correctly_Constructed(int ukprn)
    {
        var actual = new GetAccountProvidersRequest(ukprn);

        actual.GetUrl.Should().Be($"provideraccounts/{ukprn}");
    }
}