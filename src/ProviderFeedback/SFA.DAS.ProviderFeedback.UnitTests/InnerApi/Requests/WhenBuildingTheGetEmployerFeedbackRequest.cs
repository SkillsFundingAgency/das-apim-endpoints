using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderFeedback.Application.InnerApi.Requests;

namespace SFA.DAS.ProviderFeedback.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetEmployerFeedbackRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed(long ukprn)
        {
            var actual = new GetEmployerFeedbackRequest(ukprn);

            actual.GetUrl.Should().Be($"api/employerfeedbackresult/{ukprn}");
        }
    }
}