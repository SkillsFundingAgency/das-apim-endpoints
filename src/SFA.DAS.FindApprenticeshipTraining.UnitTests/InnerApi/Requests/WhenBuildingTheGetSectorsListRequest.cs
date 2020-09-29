using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetSectorsListRequest
    {
        [Test]
        public void Then_The_Url_Is_Correctly_Constructed()
        {
            var actual = new GetSectorsListRequest();

            actual.GetUrl.Should().Be($"api/courses/sectors");
        }
    }
}