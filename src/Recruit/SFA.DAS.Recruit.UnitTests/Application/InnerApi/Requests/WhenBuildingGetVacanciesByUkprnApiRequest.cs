using SFA.DAS.Recruit.Enums;
using SFA.DAS.Recruit.InnerApi.Requests;

namespace SFA.DAS.Recruit.UnitTests.Application.InnerApi.Requests;
[TestFixture]
internal class WhenBuildingGetVacanciesByUkprnApiRequest
{
    [Test, MoqAutoData]
    public void Then_The_Correct_Uri_Is_Built(int ukprn,
        int page = 1,
        int pageSize = 25,
        string sortColumn = "",
        string sortOrder = "Desc",
        FilteringOptions filterBy = FilteringOptions.All,
        string searchTerm = "")
    {
        //Act
        var actual = new GetVacanciesByUkprnApiRequest(ukprn, page, pageSize, sortColumn, sortOrder, filterBy, searchTerm);
        //Assert
        actual.GetUrl.Should().Be($"api/provider/{ukprn}/vacancies?page={page}&pageSize={pageSize}&sortColumn={sortColumn}&sortOrder={sortOrder}&filterBy={filterBy}&searchTerm={searchTerm}");
    }
}