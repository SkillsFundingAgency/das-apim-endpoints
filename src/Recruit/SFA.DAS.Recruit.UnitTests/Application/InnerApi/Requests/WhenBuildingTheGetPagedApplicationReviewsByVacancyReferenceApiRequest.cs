using SFA.DAS.Recruit.InnerApi.Requests;

namespace SFA.DAS.Recruit.UnitTests.Application.InnerApi.Requests;
[TestFixture]
internal class WhenBuildingTheGetPagedApplicationReviewsByVacancyReferenceApiRequest
{
    [Test, MoqAutoData]
    public void Then_The_Request_Is_Built_Correctly(long vacancyReference,
        int pageNumber,
        int pageSize,
        string sortColumn,
        bool isAscending)
    {
        // Arrange
        var expectedUrl = $"api/applicationreviews/paginated/{vacancyReference}?pageNumber={pageNumber}&pageSize={pageSize}&sortColumn={sortColumn}&isAscending={isAscending}";
        // Act
        var request = new GetPagedApplicationReviewsByVacancyReferenceApiRequest(vacancyReference, pageNumber, pageSize, sortColumn, isAscending);

        // Assert
        request.GetUrl.Should().Be(expectedUrl);
    }
}