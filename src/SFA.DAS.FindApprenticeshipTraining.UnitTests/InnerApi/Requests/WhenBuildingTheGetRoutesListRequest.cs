using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetRoutesListRequest
    {
        [Test]
        public void Then_The_Url_Is_Correctly_Constructed()
        {
            var actual = new GetRoutesListRequest();

            actual.GetUrl.Should().Be($"api/courses/routes");
        }
    }
}