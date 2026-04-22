using SFA.DAS.RecruitQa.InnerApi.Requests;

namespace SFA.DAS.RecruitQa.UnitTests.InnerApi;

public class WhenBuildingPutVacancyRequest
{
    [Test, AutoData]
    public void Then_Builds_Correct_Url(Guid vacancyId, PutVacancyRequestData data)
    {
        var actual = new PutVacancyRequest(vacancyId, data);

        actual.PutUrl.Should().Be($"api/vacancies/{vacancyId}");
    }

    [Test, AutoData]
    public void Then_Data_Is_Set(Guid vacancyId, PutVacancyRequestData data)
    {
        var actual = new PutVacancyRequest(vacancyId, data);

        actual.Data.Should().Be(data);
    }
}
