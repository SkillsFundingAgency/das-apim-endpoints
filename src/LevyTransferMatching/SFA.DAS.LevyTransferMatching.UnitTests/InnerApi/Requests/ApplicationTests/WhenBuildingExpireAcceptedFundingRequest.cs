using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;

namespace SFA.DAS.LevyTransferMatching.UnitTests.InnerApi.Requests.ApplicationTests;

public class WhenBuildingExpireAcceptedFundingRequest
{
    [Test, AutoData]
    public void Then_The_PostUrl_Is_Correctly_Built(int applicationId)
    {
        var actual = new ExpireAcceptedFundingRequest(applicationId, new ExpireAcceptedFundingRequestData());
        var expectedPostUrl = $"/applications/{applicationId}/expire-accepted-funding";
        actual.PostUrl.Should().Be(expectedPostUrl);
    }
}
