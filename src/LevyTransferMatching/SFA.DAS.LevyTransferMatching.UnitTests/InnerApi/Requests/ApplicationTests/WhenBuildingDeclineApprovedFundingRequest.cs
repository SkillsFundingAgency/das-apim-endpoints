using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;

namespace SFA.DAS.LevyTransferMatching.UnitTests.InnerApi.Requests.ApplicationTests;

public class WhenBuildingDeclineApprovedFundingRequest
{
    [Test, AutoData]
    public void Then_The_PostUrl_Is_Correctly_Built(int applicationId)
    {
        var actual = new DeclineApprovedFundingRequest(applicationId, new DeclineApprovedFundingRequestData());
        var expectedPostUrl = $"/applications/{applicationId}/decline-approved-funding";
        actual.PostUrl.Should().Be(expectedPostUrl);
    }
}
