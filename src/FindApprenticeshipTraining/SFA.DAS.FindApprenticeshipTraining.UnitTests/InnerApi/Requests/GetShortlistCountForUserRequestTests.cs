using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.InnerApi.Requests;

public class GetShortlistCountForUserRequestTests
{
    [Test, AutoData]
    public void WhenBuildingShortlistCountRequest_ReturnsExpectedUrl(Guid shortlistUserId)
    {
        var actual = new GetShortlistCountForUserRequest(shortlistUserId);

        actual.GetUrl.Should().Be($"shortlists/users/{shortlistUserId}/count");
    }
}
