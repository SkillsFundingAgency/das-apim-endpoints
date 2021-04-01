using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetAggregatedCourseDemandList;
using SFA.DAS.EmployerDemand.InnerApi.Requests;
using SFA.DAS.EmployerDemand.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.UnitTests.Application.Demand.Queries.GetRegisterDemand
{
    public class WhenHandlingGetAggregatedCourseDemandListQuery
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Demands_And_Courses_From_Apis(
            GetAggregatedCourseDemandListQuery query,
            GetStandardsListResponse coursesApiResponse,
            GetAggregatedCourseDemandListResponse demandApiResponse,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApi,
            [Frozen] Mock<IEmployerDemandApiClient<EmployerDemandApiConfiguration>> mockDemandApi,
            GetAggregatedCourseDemandListQueryHandler handler)
        {
            mockCoursesApi
                .Setup(client => client.Get<GetStandardsListResponse>(It.IsAny<GetAllStandardsListRequest>()))
                .ReturnsAsync(coursesApiResponse);
            mockDemandApi
                .Setup(client => client.Get<GetAggregatedCourseDemandListResponse>(It.Is<GetAggregatedCourseDemandListRequest>(request => 
                    request.Ukprn == query.Ukprn &&
                    request.CourseId == query.CourseId)))
                .ReturnsAsync(demandApiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Courses.Should().BeEquivalentTo(coursesApiResponse.Courses);
            result.AggregatedCourseDemands.Should().BeEquivalentTo(demandApiResponse.AggregatedCourseDemandList);
            result.Total.Should().Be(demandApiResponse.Total);
            result.TotalFiltered.Should().Be(demandApiResponse.TotalFiltered);
        }
    }
}
