using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.InnerApi.Requests
{
    public class WhenBuildGetExpiredShortlistsRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed(uint expiryInDays)
        {
            //Act
            var actual = new GetExpiredShortlistsRequest(expiryInDays);
            
            //Assert
            actual.GetUrl.Should().Be($"api/Shortlist/users/expired?expiryInDays={expiryInDays}");
        }
    }
}