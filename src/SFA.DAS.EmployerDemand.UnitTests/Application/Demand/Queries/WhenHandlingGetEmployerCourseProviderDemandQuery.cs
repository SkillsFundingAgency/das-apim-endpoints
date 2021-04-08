using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetEmployerCourseProviderDemand;
using SFA.DAS.EmployerDemand.InnerApi.Requests;
using SFA.DAS.EmployerDemand.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.UnitTests.Application.Demand.Queries
{
    public class WhenHandlingGetEmployerCourseProviderDemandQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Data_Is_Returned_From_The_Apis(
            GetEmployerCourseProviderDemandQuery query,
            LocationItem locationResult,
            GetStandardsListItem standardResult,
            GetEmployerCourseProviderListResponse demandResponse,
            [Frozen] Mock<ILocationLookupService> locationLookupService,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClient,
            [Frozen] Mock<IEmployerDemandApiClient<EmployerDemandApiConfiguration>> employerDemandApiClient,
            GetEmployerCourseProviderDemandQueryHandler handler)
        {
            //Arrange
            locationLookupService.Setup(x => x.GetLocationInformation(query.LocationName, 0, 0))
                .ReturnsAsync(locationResult);
            coursesApiClient
                .Setup(x => x.Get<GetStandardsListItem>(It.Is<GetStandardRequest>(c =>
                    c.StandardId.Equals(query.CourseId)))).ReturnsAsync(standardResult);
            employerDemandApiClient
                .Setup(x => x.Get<GetEmployerCourseProviderListResponse>(
                    It.Is<GetCourseProviderDemandsRequest>(c => c.GetUrl.Contains($"providers/{query.Ukprn}/courses/{query.CourseId}?lat={locationResult.GeoPoint.First()}&lon={locationResult.GeoPoint.Last()}&radius={query.Radius}"))))
                .ReturnsAsync(demandResponse);
            
            //Act
            var actual = await handler.Handle(query, CancellationToken.None);
            
            //Assert
            actual.Course.Should().BeEquivalentTo(standardResult);
            actual.Location.Should().BeEquivalentTo(locationResult);
            actual.EmployerCourseDemands.Should().BeEquivalentTo(demandResponse);
        }
    }
}