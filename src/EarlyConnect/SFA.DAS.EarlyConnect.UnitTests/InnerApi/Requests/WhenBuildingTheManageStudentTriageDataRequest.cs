using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EarlyConnect.InnerApi.Requests;
using SFA.DAS.EarlyConnect.Models;

namespace SFA.DAS.EarlyConnect.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheManageStudentTriageDataRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed(StudentTriageData studentTriageData, string surveyGuid)
        {
            var actual = new ManageStudentTriageDataRequest(studentTriageData, surveyGuid);

            actual.PostUrl.Should().Be($"/api/student-triage-data/{surveyGuid}");
        }
    }
}