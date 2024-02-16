using System.Collections.Generic;
using System.Linq;
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
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.UnitTests.Application.Demand.Queries
{
    public class WhenHandlingGetAggregatedCourseDemandListQuery
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Demands_And_Courses_From_Apis_And_Adds_Courses_And_Routes_To_Cache(
            GetAggregatedCourseDemandListQuery query,
            GetStandardsListResponse coursesApiStandardsResponse,
            GetRoutesListResponse coursesApiRoutesResponse,
            GetAggregatedCourseDemandListResponse demandApiResponse,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApi,
            [Frozen] Mock<IEmployerDemandApiClient<EmployerDemandApiConfiguration>> mockDemandApi,
            [Frozen] Mock<ILocationLookupService> mockLocationLookup,
            [Frozen] Mock<ICacheStorageService> cacheStorageService,
            GetAggregatedCourseDemandListQueryHandler handler)
        {
            query.LocationName = null;
            query.LocationRadius = null;
            cacheStorageService
                .Setup(client => client.RetrieveFromCache<GetStandardsListResponse>(nameof(GetStandardsListResponse)))
                .ReturnsAsync((GetStandardsListResponse)null);
            cacheStorageService
                .Setup(client => client.RetrieveFromCache<GetRoutesListResponse>(nameof(GetRoutesListResponse)))
                .ReturnsAsync((GetRoutesListResponse) null);
            mockLocationLookup
                .Setup(service => service.GetLocationInformation(null, default, default, false))
                .ReturnsAsync((LocationItem)null);
            mockCoursesApi
                .Setup(client => client.Get<GetStandardsListResponse>(It.IsAny<GetAvailableToStartStandardsListRequest>()))
                .ReturnsAsync(coursesApiStandardsResponse);
            mockCoursesApi
                .Setup(client => client.Get<GetRoutesListResponse>(It.IsAny<GetRoutesListRequest>()))
                .ReturnsAsync(coursesApiRoutesResponse);
            mockDemandApi
                .Setup(client => client.Get<GetAggregatedCourseDemandListResponse>(It.Is<GetAggregatedCourseDemandListRequest>(request => 
                    request.Ukprn == query.Ukprn &&
                    request.CourseId == query.CourseId &&
                    !request.Lat.HasValue &&
                    !request.Lon.HasValue &&
                    !request.Radius.HasValue &&
                    request.Routes == query.Routes)))
                .ReturnsAsync(demandApiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Courses.Should().BeEquivalentTo(coursesApiStandardsResponse.Standards);
            result.AggregatedCourseDemands.Should().BeEquivalentTo(demandApiResponse.AggregatedCourseDemandList);
            result.Total.Should().Be(demandApiResponse.Total);
            result.TotalFiltered.Should().Be(demandApiResponse.TotalFiltered);
            result.LocationItem.Should().BeNull();
            result.Routes.Should().BeEquivalentTo(coursesApiRoutesResponse.Routes.ToList());
            cacheStorageService.Verify(x=>x.SaveToCache(nameof(GetStandardsListResponse),coursesApiStandardsResponse, 1));
            cacheStorageService.Verify(x => x.SaveToCache(nameof(GetRoutesListResponse), coursesApiRoutesResponse, 1));
        }

        [Test, MoqAutoData]
        public async Task Then_Converts_LocationName_To_Lat_Long(
            GetAggregatedCourseDemandListQuery query,
            GetStandardsListResponse coursesApiStandardsResponse,
            GetRoutesListResponse coursesApiRoutesResponse,
            GetAggregatedCourseDemandListResponse demandApiResponse,
            LocationItem locationFromService,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApi,
            [Frozen] Mock<IEmployerDemandApiClient<EmployerDemandApiConfiguration>> mockDemandApi,
            [Frozen] Mock<ILocationLookupService> mockLocationLookup,
            [Frozen] Mock<ICacheStorageService> cacheStorageService,
            GetAggregatedCourseDemandListQueryHandler handler)
        {
            cacheStorageService
                .Setup(client => client.RetrieveFromCache<GetStandardsListResponse>(nameof(GetStandardsListResponse)))
                .ReturnsAsync((GetStandardsListResponse)null);
            cacheStorageService
                .Setup(client => client.RetrieveFromCache<GetRoutesListResponse>(nameof(GetRoutesListResponse)))
                .ReturnsAsync((GetRoutesListResponse)null);
            mockLocationLookup
                .Setup(service => service.GetLocationInformation(query.LocationName, default, default, false))
                .ReturnsAsync(locationFromService);
            mockCoursesApi
                .Setup(client => client.Get<GetStandardsListResponse>(It.IsAny<GetAvailableToStartStandardsListRequest>()))
                .ReturnsAsync(coursesApiStandardsResponse);
            mockCoursesApi
                .Setup(client => client.Get<GetRoutesListResponse>(It.IsAny<GetRoutesListRequest>()))
                .ReturnsAsync(coursesApiRoutesResponse);
            mockDemandApi
                .Setup(client => client.Get<GetAggregatedCourseDemandListResponse>(It.Is<GetAggregatedCourseDemandListRequest>(request => 
                    request.Ukprn == query.Ukprn &&
                    request.CourseId == query.CourseId &&
                    request.Lat == locationFromService.GeoPoint.First() &&
                    request.Lon == locationFromService.GeoPoint.Last() &&
                    request.Radius == query.LocationRadius)))
                .ReturnsAsync(demandApiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Courses.Should().BeEquivalentTo(coursesApiStandardsResponse.Standards);
            result.AggregatedCourseDemands.Should().BeEquivalentTo(demandApiResponse.AggregatedCourseDemandList);
            result.Total.Should().Be(demandApiResponse.Total);
            result.TotalFiltered.Should().Be(demandApiResponse.TotalFiltered);
            result.LocationItem.Should().BeEquivalentTo(locationFromService);
            result.Routes.Should().BeEquivalentTo(coursesApiRoutesResponse.Routes.ToList());
        }

        [Test, MoqAutoData]
        public async Task Then_Gets_Standards_From_Cache(
            GetAggregatedCourseDemandListQuery query,
            GetStandardsListResponse cachedCourses,
            GetRoutesListResponse coursesApiRoutesResponse,
            GetAggregatedCourseDemandListResponse demandApiResponse,
            LocationItem locationFromService,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApi,
            [Frozen] Mock<IEmployerDemandApiClient<EmployerDemandApiConfiguration>> mockDemandApi,
            [Frozen] Mock<ILocationLookupService> mockLocationLookup,
            [Frozen] Mock<ICacheStorageService> cacheStorageService,
            GetAggregatedCourseDemandListQueryHandler handler)
        {
            mockLocationLookup
                .Setup(service => service.GetLocationInformation(query.LocationName, default, default, false))
                .ReturnsAsync(locationFromService);
            cacheStorageService
                .Setup(client => client.RetrieveFromCache<GetStandardsListResponse>(nameof(GetStandardsListResponse)))
                .ReturnsAsync(cachedCourses);
            cacheStorageService
                .Setup(client => client.RetrieveFromCache<GetRoutesListResponse>(nameof(GetRoutesListResponse)))
                .ReturnsAsync(coursesApiRoutesResponse);
            mockDemandApi
                .Setup(client => client.Get<GetAggregatedCourseDemandListResponse>(It.Is<GetAggregatedCourseDemandListRequest>(request => 
                    request.Ukprn == query.Ukprn &&
                    request.CourseId == query.CourseId &&
                    request.Lat == locationFromService.GeoPoint.First() &&
                    request.Lon == locationFromService.GeoPoint.Last() &&
                    request.Radius == query.LocationRadius)))
                .ReturnsAsync(demandApiResponse);

            var result = await handler.Handle(query, CancellationToken.None);
            
            result.Courses.Should().BeEquivalentTo(cachedCourses.Standards);
            mockCoursesApi.Verify(client => client.Get<GetStandardsListResponse>(It.IsAny<GetAvailableToStartStandardsListRequest>()), Times.Never);
        }

        [Test, MoqAutoData]
        public async Task Then_Gets_Routes_From_Cache(
            GetAggregatedCourseDemandListQuery query,
            GetStandardsListResponse coursesApiStandardsResponse,
            GetRoutesListResponse cachedRoutes,
            GetAggregatedCourseDemandListResponse demandApiResponse,
            LocationItem locationFromService,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApi,
            [Frozen] Mock<IEmployerDemandApiClient<EmployerDemandApiConfiguration>> mockDemandApi,
            [Frozen] Mock<ILocationLookupService> mockLocationLookup,
            [Frozen] Mock<ICacheStorageService> cacheStorageService,
            GetAggregatedCourseDemandListQueryHandler handler)
        {
            mockLocationLookup
                .Setup(service => service.GetLocationInformation(query.LocationName, default, default, false))
                .ReturnsAsync(locationFromService);
            mockCoursesApi
                .Setup(client => client.Get<GetStandardsListResponse>(It.IsAny<GetAvailableToStartStandardsListRequest>()))
                .ReturnsAsync(coursesApiStandardsResponse);
            cacheStorageService
                .Setup(client => client.RetrieveFromCache<GetRoutesListResponse>(nameof(GetRoutesListResponse)))
                .ReturnsAsync(cachedRoutes);
            mockDemandApi
                .Setup(client => client.Get<GetAggregatedCourseDemandListResponse>(It.Is<GetAggregatedCourseDemandListRequest>(request =>
                    request.Ukprn == query.Ukprn &&
                    request.CourseId == query.CourseId &&
                    request.Lat == locationFromService.GeoPoint.First() &&
                    request.Lon == locationFromService.GeoPoint.Last() &&
                    request.Radius == query.LocationRadius)))
                .ReturnsAsync(demandApiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Routes.Should().BeEquivalentTo(cachedRoutes.Routes.ToList());
            mockCoursesApi.Verify(client => client.Get<GetRoutesListResponse>(It.IsAny<GetRoutesListRequest>()), Times.Never);
        }
    }
}
