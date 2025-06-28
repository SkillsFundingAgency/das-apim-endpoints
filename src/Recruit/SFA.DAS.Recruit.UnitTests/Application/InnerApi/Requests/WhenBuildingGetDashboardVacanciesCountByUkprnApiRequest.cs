using System.Collections.Generic;
using SFA.DAS.Recruit.Enums;
using SFA.DAS.Recruit.InnerApi.Requests;

namespace SFA.DAS.Recruit.UnitTests.Application.InnerApi.Requests;
[TestFixture]
public class WhenBuildingGetDashboardVacanciesCountByUkprnApiRequest
{
    [Test, MoqAutoData]
    public void Then_The_Correct_Url_Is_Built(int ukprn,
        int pageNumber,
        int pageSize,
        string sortColumn,
        bool isAscending,
        List<ApplicationReviewStatus> status)
    {
        // Act
        var request = new GetDashboardVacanciesCountByUkprnApiRequest(ukprn, pageNumber, pageSize, sortColumn, isAscending, status);
        // Assert
        request.GetUrl.Should()
            .Be(
                $"api/provider/{ukprn}/applicationReviews/dashboard/vacancies?pageNumber={pageNumber}&pageSize={pageSize}&sortColumn={sortColumn}&isAscending={isAscending}&status={string.Join("&status=", status)}");
    }
}