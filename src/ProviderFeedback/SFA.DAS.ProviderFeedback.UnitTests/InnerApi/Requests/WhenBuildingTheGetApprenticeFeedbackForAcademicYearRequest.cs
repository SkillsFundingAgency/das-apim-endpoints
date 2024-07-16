using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderFeedback.Application.InnerApi.Requests;

namespace SFA.DAS.ProviderFeedback.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetApprenticeFeedbackForAcademicYearRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed(long ukprn,string year)
        {
            var actual = new GetApprenticeFeedbackForAcademicYearRequest(ukprn, year);

            actual.GetUrl.Should().Be($"api/apprenticefeedbackresult/{ukprn}/annual/{year}");
        }
    }
}