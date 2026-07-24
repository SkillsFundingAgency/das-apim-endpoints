using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.InnerApi.Requests;

public class GetProviderSummaryRequestTests
{
    [Test, AutoData]
    public void WhenBuildingGetProviderSummaryRequest_ReturnsExpectedUrl(int ukprn)
    {
        //Act
        var sut = new GetProviderSummaryRequest(ukprn);

        //Assert
        sut.GetUrl.Should().Be($"api/providers/{ukprn}/summary");
    }
}