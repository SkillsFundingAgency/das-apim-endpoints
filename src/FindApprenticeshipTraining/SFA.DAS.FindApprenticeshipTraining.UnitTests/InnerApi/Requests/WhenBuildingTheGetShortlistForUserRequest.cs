using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetShortlistForUserRequest
    {
        [Test, AutoData]
        public void Then_Builds_Url(Guid shortlistUserId)
        {
            var actual = new GetShortlistForUserRequest(shortlistUserId);

            actual.GetUrl.Should().Be($"api/shortlist/users/{shortlistUserId}");
        }
    }
}