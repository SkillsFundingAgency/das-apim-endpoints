using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.InnerApi.Requests;

public class GetShortlistsForUserRequestTests
{
    [Test, AutoData]
    public void WhenBuildingShortlistsForUserRequest_ReturnsExpectedUrl(Guid shortlistUserId)
    {
        var actual = new GetShortlistsForUserRequest(shortlistUserId);

        actual.GetUrl.Should().Be($"shortlists/users/{shortlistUserId}");
    }
}