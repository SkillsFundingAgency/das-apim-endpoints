using SFA.DAS.EmployerFinance.Application.Queries.GetStandards;
using SFA.DAS.EmployerFinance.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.EmployerFinance.InnerApi.Requests;

namespace SFA.DAS.EmployerFinance.UnitTests.Application.Queries.GetStandards
{
    public class WhenHandlingTheGetStandardsQuery
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Standards_From_Courses_Api(
            GetStandardsQuery query,
            GetStandardsListResponse apiResponse,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockApiClient,
            GetStandardsQueryHandler handler)
        {
            mockApiClient
                .Setup(client => client.Get<GetStandardsListResponse>(It.IsAny<GetCoursesRequest>()))
                .ReturnsAsync(apiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Standards.Should().BeEquivalentTo(apiResponse.Courses);
        }
    }
}