using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Application.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetSectorsListRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed(
            string baseUrl)
        {
            var actual = new GetSectorsListRequest {BaseUrl = baseUrl};

            actual.BaseUrl.Should().Be(baseUrl);
            actual.GetUrl.Should().Be($"{baseUrl}api/courses/sectors");
        }
    }
}