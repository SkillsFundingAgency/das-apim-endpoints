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
            GetProviderCourseInformation courseProviderResponse,
            [Frozen] Mock<ILocationLookupService> locationLookupService,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClient,
            [Frozen] Mock<IEmployerDemandApiClient<EmployerDemandApiConfiguration>> employerDemandApiClient,
            [Frozen] Mock<ICourseDeliveryApiClient<CourseDeliveryApiConfiguration>> courseDeliveryApiClient,
            GetEmployerCourseProviderDemandQueryHandler handler)
        {
            //Arrange
            locationLookupService.Setup(x => x.GetLocationInformation(query.LocationName, 0, 0, true))
                .ReturnsAsync(locationResult);
            coursesApiClient
                .Setup(x => x.Get<GetStandardsListItem>(It.Is<GetStandardRequest>(c =>
                    c.StandardId.Equals(query.CourseId)))).ReturnsAsync(standardResult);
            employerDemandApiClient
                .Setup(x => x.Get<GetEmployerCourseProviderListResponse>(
                    It.Is<GetCourseProviderDemandsRequest>(c => c.GetUrl.Contains($"providers/{query.Ukprn}/courses/{query.CourseId}?lat={locationResult.GeoPoint.First()}&lon={locationResult.GeoPoint.Last()}&radius={query.LocationRadius}"))))
                .ReturnsAsync(demandResponse);
            courseDeliveryApiClient.Setup(x =>
                x.Get<GetProviderCourseInformation>(
                    It.Is<GetProviderCourseInformationRequest>(c => c.GetUrl.Contains($"courses/{query.CourseId}/providers/{query.Ukprn}"))))
                .ReturnsAsync(courseProviderResponse);
            
            //Act
            var actual = await handler.Handle(query, CancellationToken.None);
            
            //Assert
            actual.Course.Should().BeEquivalentTo(standardResult);
            actual.Location.Should().BeEquivalentTo(locationResult);
            actual.EmployerCourseDemands.Should().BeEquivalentTo(demandResponse.EmployerCourseDemands);
            actual.ProviderDetail.Should().BeEquivalentTo(courseProviderResponse);
            actual.Total.Should().Be(demandResponse.Total);
            actual.TotalFiltered.Should().Be(demandResponse.TotalFiltered);
        }

        [Test, MoqAutoData]
        public async Task Then_If_No_Location_Found_Then_Returns_Data(
            GetEmployerCourseProviderDemandQuery query,
            GetStandardsListItem standardResult,
            GetEmployerCourseProviderListResponse demandResponse,
            [Frozen] Mock<ILocationLookupService> locationLookupService,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClient,
            [Frozen] Mock<IEmployerDemandApiClient<EmployerDemandApiConfiguration>> employerDemandApiClient,
            GetEmployerCourseProviderDemandQueryHandler handler)
        {
            //Arrange
            locationLookupService.Setup(x => x.GetLocationInformation(query.LocationName, 0, 0, true))
                .ReturnsAsync((LocationItem)null);
            coursesApiClient
                .Setup(x => x.Get<GetStandardsListItem>(It.Is<GetStandardRequest>(c =>
                    c.StandardId.Equals(query.CourseId)))).ReturnsAsync(standardResult);
            employerDemandApiClient
                .Setup(x => x.Get<GetEmployerCourseProviderListResponse>(
                    It.Is<GetCourseProviderDemandsRequest>(c => c.GetUrl.Contains($"providers/{query.Ukprn}/courses/{query.CourseId}?lat=&lon=&radius="))))
                .ReturnsAsync(demandResponse);
            
            //Act
            var actual = await handler.Handle(query, CancellationToken.None);
            
            //Assert
            actual.Course.Should().BeEquivalentTo(standardResult);
            actual.Location.Should().BeNull();
            actual.EmployerCourseDemands.Should().BeEquivalentTo(demandResponse.EmployerCourseDemands);
            actual.Total.Should().Be(demandResponse.Total);
            actual.TotalFiltered.Should().Be(demandResponse.TotalFiltered);
        }
    }
}