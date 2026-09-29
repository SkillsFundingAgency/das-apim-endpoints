using SFA.DAS.RecruitJobs.InnerApi.Requests.Reports;

namespace SFA.DAS.RecruitJobs.UnitTests.InnerApi;

public class WhenBuildingGetApplicationsByVacancyReferenceApiRequest
{
    [Test, AutoData]
    public void Then_The_Request_Is_Built_With_Id(long vacancyReference)
    {
        var actual = new GetApplicationsByVacancyReferenceApiRequest(vacancyReference);
        
        actual.GetUrl.Should().Be($"api/vacancies/{vacancyReference}/applications");
    }
}