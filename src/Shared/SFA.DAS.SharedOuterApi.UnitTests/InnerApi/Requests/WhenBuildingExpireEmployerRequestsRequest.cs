using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests
{
    public class WhenBuildingExpireEmployerRequestsRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Is_Correctly_Build()
        {
            var actual = new ExpireEmployerRequestsRequest();

            actual.PutUrl.Should().Be("api/employer-requests/expire");
        }
    }
}
