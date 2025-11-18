using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipJobs.UnitTests.InnerApi;

public class WhenBuildingGetLiveVacanciesApiRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Correctly_Constructed(int pageSize, int pageNumber)
    {
        var actual = new GetLiveVacanciesApiRequest(pageNumber, pageSize, null);

        actual.GetUrl.Should().Be($"api/vacancies/live?pageSize={pageSize}&page={pageNumber}");
    }
    [Test, AutoData]
    public void Then_The_Url_Is_Correctly_Constructed(int pageSize, int pageNumber, DateTime closingDate)
    {
        var actual = new GetLiveVacanciesApiRequest(pageNumber, pageSize, closingDate);

        actual.GetUrl.Should().Be($"api/vacancies/live?pageSize={pageSize}&page={pageNumber}&closingDate={closingDate.Date}");
    }
}