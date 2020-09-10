using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheLocationQueryRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Is_Correctly_Built(string query)
        {
            var actual = new GetLocationsQueryRequest(query);

            actual.GetUrl.Should().Be($"api/locations/search?query={query}");   
        }
    }
}