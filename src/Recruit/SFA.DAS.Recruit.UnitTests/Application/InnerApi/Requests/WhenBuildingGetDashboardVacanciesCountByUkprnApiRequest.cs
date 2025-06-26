using SFA.DAS.Recruit.Enums;
using SFA.DAS.Recruit.InnerApi.Requests;

namespace SFA.DAS.Recruit.UnitTests.Application.InnerApi.Requests;
[TestFixture]
public class WhenBuildingGetDashboardVacanciesCountByUkprnApiRequest
{
    [Test, MoqAutoData]
    public void Then_The_Correct_Url_Is_Built(int ukprn,
        int pageNumber = 1,
        int pageSize = 25,
        string sortColumn = "CreatedDate",
        bool isAscending = false,
        ApplicationReviewStatus status = ApplicationReviewStatus.New)
    {
        // Act
        var request = new GetDashboardVacanciesCountByUkprnApiRequest(ukprn, pageNumber, pageSize, sortColumn, isAscending, status);
        // Assert
        request.GetUrl.Should()
            .Be(
                $"api/provider/{ukprn}/applicationReviews/dashboard/vacancies?pageNumber={pageNumber}&pageSize={pageSize}&sortColumn={sortColumn}&isAscending={isAscending}&status={status}");
    }
}