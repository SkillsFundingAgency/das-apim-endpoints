using System.Collections.Generic;
using SFA.DAS.Recruit.Enums;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests;
using SFA.DAS.Recruit.InnerApi.Requests;

namespace SFA.DAS.Recruit.UnitTests.Application.InnerApi.Requests;
[TestFixture]
public class WhenBuildingGetDashboardVacanciesCountByAccountIdApiRequest
{
    [Test, MoqAutoData]
    public void Then_The_Correct_Url_Is_Built(long accountId,
        int pageNumber,
        int pageSize,
        string sortColumn,
        bool isAscending,
        List<ApplicationReviewStatus> status)
    {
        // Act
        var request = new GetDashboardVacanciesCountByAccountIdApiRequest(accountId, pageNumber, pageSize, sortColumn, isAscending, status);
        // Assert
        request.GetUrl.Should()
            .Be($"api/employer/{accountId}/applicationReviews/dashboard/vacancies?pageNumber={pageNumber}&pageSize={pageSize}&sortColumn={sortColumn}&isAscending={isAscending}&status={string.Join("&status=", status)}");
    }
}