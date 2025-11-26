using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipJobs.UnitTests.InnerApi;

[TestFixture]
public class WhenBuildingGetClosedVacancyApiRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Correctly_Constructed(int vacancyReference)
    {
        var actual = new GetClosedVacancyApiRequest(vacancyReference);

        actual.GetUrl.Should().Be($"api/vacancies/{vacancyReference}/closed");
    }
}