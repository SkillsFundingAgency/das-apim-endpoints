using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.InnerApi.Requests
{
    public class GetShortlistUserItemCountRequestTests
    {
        [Test, AutoData]
        public void WhenBuildingShortlistUserItemCountRequest_ReturnsExpectedUrl(Guid userId)
        {
            //Act
            var actual = new GetShortlistUserItemCountRequest(userId);

            //Assert
            actual.GetUrl.Should().Be($"api/Shortlist/users/{userId}/count");
        }
    }
}