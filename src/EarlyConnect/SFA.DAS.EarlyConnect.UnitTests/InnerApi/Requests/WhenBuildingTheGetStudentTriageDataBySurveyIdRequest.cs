using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EarlyConnect.InnerApi.Requests;

namespace SFA.DAS.EarlyConnect.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetStudentTriageDataBySurveyIdRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed(string SurveyGuid)
        {
            var actual = new GetStudentTriageDataBySurveyIdRequest(SurveyGuid);

            actual.GetUrl.Should().Be($"/api/student-triage-data/{SurveyGuid}");
        }
    }
}