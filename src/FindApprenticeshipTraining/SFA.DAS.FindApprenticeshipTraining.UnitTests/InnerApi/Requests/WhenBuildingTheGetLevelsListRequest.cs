using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetLevelsListRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Build()
        {
            var actual = new GetLevelsListRequest();

            actual.GetUrl.Should().Be("api/courses/levels");

        }
        
    }
}