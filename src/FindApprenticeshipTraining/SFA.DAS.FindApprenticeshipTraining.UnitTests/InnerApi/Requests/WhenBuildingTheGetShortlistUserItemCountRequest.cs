using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetShortlistUserItemCountRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed(Guid userId)
        {
            //Act
            var actual = new GetShortlistUserItemCountRequest(userId);
            
            //Assert
            actual.GetUrl.Should().Be($"api/Shortlist/count/users/{userId}");
        }
    }
}