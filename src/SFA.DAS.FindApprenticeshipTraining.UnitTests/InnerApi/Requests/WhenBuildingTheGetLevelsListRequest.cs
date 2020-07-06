using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Application.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetLevelsListRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Build(string baseUrl)
        {
            var actual = new GetLevelsListRequest {BaseUrl = baseUrl};

            actual.GetUrl.Should().Be($"{baseUrl}api/courses/levels");

        }
        
    }
}