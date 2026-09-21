using FluentAssertions;
using SFA.DAS.EmployerFinanceJobs.InnerApi.Requests;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerFinanceJobs.UnitTests.InnerApi.Requests;

[TestFixture]
internal class WhenBuildingGetAllLearnersApiRequest
{
    [Test, MoqAutoData]
    public void Build_ReturnsCorrectRequest(DateTime date, int pageSize, int page)
    {
        // Act
        var request = new GetAllLearnersApiRequest(date, pageSize, page);

        // Assert
        request.Should().NotBeNull();
        request.GetUrl.Should().Be($"api/learners?sinceTime={date:O}&batch_Size={pageSize}&batch_Number={page}");
    }
}