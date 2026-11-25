using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.InnerApi.Requests;

public class GetShortlistForUserIdRequestTests
{
    [Test, AutoData]
    public void WhenBuildingGetShortlistForUserIdRequest_ReturnsExpectedUrl(Guid shortlistUserId)
    {
        var actual = new GetShortlistForUserIdRequest(shortlistUserId);

        actual.GetAllUrl.Should().Be($"api/shortlist/{shortlistUserId}");
    }
}