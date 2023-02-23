using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.InnerApi.Requests;

public class WhenBuildingTheGetShortlistForUserIdRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Correctly_Built(Guid shortlistUserId)
    {
        var actual = new GetShortlistForUserIdRequest(shortlistUserId);

        actual.GetAllUrl.Should().Be($"api/shortlist/{shortlistUserId}");
    }
}