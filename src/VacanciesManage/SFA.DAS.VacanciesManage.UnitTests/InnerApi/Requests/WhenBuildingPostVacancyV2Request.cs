using SFA.DAS.VacanciesManage.InnerApi.Requests;

namespace SFA.DAS.VacanciesManage.UnitTests.InnerApi.Requests;

public class WhenBuildingPostVacancyV2Request
{
    [Test, AutoData]
    public void Then_The_Url_Is_Correctly_Built(PostVacancyV2RequestData requestData)
    {
        var actual = new PostVacancyV2Request(requestData);
            
        actual.PostUrl.Should().Be("api/vacancies?validateOnly=true&ruleset=All");
        actual.Data.Should().BeEquivalentTo(requestData);
    }
}