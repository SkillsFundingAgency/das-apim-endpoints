using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses.Queries.GetTrainingCourseProviders;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipTraining.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Application.TrainingCourses.Queries
{
    public class WhenGettingProvidersByTrainingCourse
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_The_Standard_And_The_List_Of_Providers_For_That_Course_From_Course_Delivery_Api_Client_With_No_Location_And_ShortlistItem_Count(
            GetTrainingCourseProvidersQuery query,
            GetProvidersListResponse apiResponse,
            GetStandardsListItem apiCourseResponse,
            GetShowEmployerDemandResponse showEmployerDemandResponse,
            int shortlistItemCount,
            [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> mockLocationApiClient,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<ICourseDeliveryApiClient<CourseDeliveryApiConfiguration>> mockApiClient,
            [Frozen] Mock<IEmployerDemandApiClient<EmployerDemandApiConfiguration>> mockEmployerDemandApiClient,
            [Frozen] Mock<IShortlistService> shortlistService,
            GetTrainingCourseProvidersQueryHandler handler)
        {
            apiCourseResponse.Level = 1;
            query.Location = "";
            query.Lat = 0;
            query.Lon = 0;
            mockApiClient
                .Setup(client => client.Get<GetProvidersListResponse>(It.Is<GetProvidersByCourseRequest>(c=>
                    c.GetUrl.Contains(query.Id.ToString()) 
                    && c.GetUrl.Contains($"sectorSubjectArea={apiCourseResponse.SectorSubjectAreaTier2Description}&level={apiCourseResponse.Level}")
                )))
                .ReturnsAsync(apiResponse);
            mockCoursesApiClient
                .Setup(client => client.Get<GetStandardsListItem>(It.Is<GetStandardRequest>(c=>c.GetUrl.Contains(query.Id.ToString()))))
                .ReturnsAsync(apiCourseResponse);
            mockEmployerDemandApiClient
                .Setup(client => client.GetResponseCode(It.IsAny<GetShowEmployerDemandRequest>()))
                .ReturnsAsync(HttpStatusCode.OK);
            shortlistService.Setup(x => x.GetShortlistItemCount(query.ShortlistUserId))
                .ReturnsAsync(shortlistItemCount);
            
            var result = await handler.Handle(query, CancellationToken.None);

            result.Providers.Should().BeEquivalentTo(apiResponse.Providers);
            result.Total.Should().Be(apiResponse.TotalResults);
            result.Course.Should().BeEquivalentTo(apiCourseResponse);
            result.ShowEmployerDemand.Should().BeTrue();
            result.ShortlistItemCount.Should().Be(shortlistItemCount);
            mockLocationApiClient.Verify(x=>x.Get<GetLocationsListItem>(It.IsAny<GetLocationByLocationAndAuthorityName>()), Times.Never);
        }

        [Test, MoqAutoData]
        public async Task Then_If_There_Is_A_Location_Supplied_It_Is_Searched_And_Passed_To_The_Provider_Search(
            string locationName,
            string authorityName,
            GetTrainingCourseProvidersQuery query,
            GetProvidersListResponse apiResponse,
            GetStandardsListItem apiCourseResponse,
            GetLocationsListItem apiLocationResponse,
            [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> mockLocationApiClient,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<ICourseDeliveryApiClient<CourseDeliveryApiConfiguration>> mockApiClient,
            GetTrainingCourseProvidersQueryHandler handler)
        {
            query.Location = $"{locationName}, {authorityName} ";
            query.Lat = 0;
            query.Lon = 0;
            mockLocationApiClient
                .Setup(client =>
                    client.Get<GetLocationsListItem>(
                        It.Is<GetLocationByLocationAndAuthorityName>(c => c.GetUrl.Contains(locationName.Trim()) && c.GetUrl.Contains(authorityName.Trim()))))
                .ReturnsAsync(apiLocationResponse);
            mockApiClient
                .Setup(client => client.Get<GetProvidersListResponse>(It.Is<GetProvidersByCourseRequest>(c=>
                    c.GetUrl.Contains(query.Id.ToString())
                    && c.GetUrl.Contains(apiLocationResponse.Location.GeoPoint.First().ToString())
                    && c.GetUrl.Contains(apiLocationResponse.Location.GeoPoint.Last().ToString())
                    && c.GetUrl.Contains($"&sortOrder={query.SortOrder}")
                )))
                .ReturnsAsync(apiResponse);
            mockCoursesApiClient
                .Setup(client => client.Get<GetStandardsListItem>(It.Is<GetStandardRequest>(c=>c.GetUrl.Contains(query.Id.ToString()))))
                .ReturnsAsync(apiCourseResponse);
            
            
            var result = await handler.Handle(query, CancellationToken.None);

            result.Providers.Should().BeEquivalentTo(apiResponse.Providers);
            result.Total.Should().Be(apiResponse.TotalResults);
            result.Course.Should().BeEquivalentTo(apiCourseResponse);
            result.Location.Name.Should().Be(query.Location);
            result.Location.GeoPoint.Should().BeEquivalentTo(apiLocationResponse.Location.GeoPoint);
        }

        [Test, MoqAutoData]
        public async Task Then_If_There_Is_A_Location_Name_With_A_Comma_Is_Is_Searched_And_Passed_To_The_Provider_Search(
            string locationName,
            string authorityName,
            GetTrainingCourseProvidersQuery query,
            GetProvidersListResponse apiResponse,
            GetStandardsListItem apiCourseResponse,
            GetLocationsListItem apiLocationResponse,
            [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> mockLocationApiClient,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<ICourseDeliveryApiClient<CourseDeliveryApiConfiguration>> mockApiClient,
            GetTrainingCourseProvidersQueryHandler handler)
        {
            query.Location = $"{locationName}, {locationName}, {authorityName} ";
            query.Lat = 0;
            query.Lon = 0;
            var encodedLocation = "?locationName=" + HttpUtility.UrlEncode($"{locationName}, {locationName}");
            mockLocationApiClient
                .Setup(client =>
                    client.Get<GetLocationsListItem>(
                        It.Is<GetLocationByLocationAndAuthorityName>(c => c.GetUrl.Contains(encodedLocation) && c.GetUrl.Contains(authorityName.Trim()))))
                .ReturnsAsync(apiLocationResponse);
            mockApiClient
                .Setup(client => client.Get<GetProvidersListResponse>(It.Is<GetProvidersByCourseRequest>(c=>
                    c.GetUrl.Contains(query.Id.ToString())
                    && c.GetUrl.Contains(apiLocationResponse.Location.GeoPoint.First().ToString())
                    && c.GetUrl.Contains(apiLocationResponse.Location.GeoPoint.Last().ToString())
                    && c.GetUrl.Contains($"&sortOrder={query.SortOrder}")
                )))
                .ReturnsAsync(apiResponse);
            mockCoursesApiClient
                .Setup(client => client.Get<GetStandardsListItem>(It.Is<GetStandardRequest>(c=>c.GetUrl.Contains(query.Id.ToString()))))
                .ReturnsAsync(apiCourseResponse);
            
            
            var result = await handler.Handle(query, CancellationToken.None);

            result.Providers.Should().BeEquivalentTo(apiResponse.Providers);
            result.Total.Should().Be(apiResponse.TotalResults);
            result.Course.Should().BeEquivalentTo(apiCourseResponse);
            result.Location.Name.Should().Be(query.Location);
            result.Location.GeoPoint.Should().BeEquivalentTo(apiLocationResponse.Location.GeoPoint);
        }
        
        [Test, MoqAutoData]
        public async Task Then_If_There_Is_A_Location_But_It_Does_Not_Have_Location_And_Authority_Supplied_It_Is_Not_Passed_To_The_Provider_Search(
            string locationName,
            GetTrainingCourseProvidersQuery query,
            GetProvidersListResponse apiResponse,
            GetStandardsListItem apiCourseResponse,
            GetLocationsListItem apiLocationResponse,
            [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> mockLocationApiClient,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<ICourseDeliveryApiClient<CourseDeliveryApiConfiguration>> mockApiClient,
            GetTrainingCourseProvidersQueryHandler handler)
        {
            query.Location = $"{locationName} ";
            query.Lat = 0;
            query.Lon = 0;
            mockApiClient
                .Setup(client => client.Get<GetProvidersListResponse>(It.Is<GetProvidersByCourseRequest>(c=>
                    c.GetUrl.Contains(query.Id.ToString())
                )))
                .ReturnsAsync(apiResponse);
            mockCoursesApiClient
                .Setup(client => client.Get<GetStandardsListItem>(It.Is<GetStandardRequest>(c=>c.GetUrl.Contains(query.Id.ToString()))))
                .ReturnsAsync(apiCourseResponse);
            
            
            var result = await handler.Handle(query, CancellationToken.None);

            result.Providers.Should().BeEquivalentTo(apiResponse.Providers);
            result.Total.Should().Be(apiResponse.TotalResults);
            result.Course.Should().BeEquivalentTo(apiCourseResponse);
            mockLocationApiClient.Verify(x=>x.Get<GetLocationsListItem>(It.IsAny<GetLocationByLocationAndAuthorityName>()), Times.Never);
            result.Location.Should().BeNull();
        }

        [Test, MoqAutoData]
        public async Task Then_If_There_Is_An_Outcode_Supplied_It_Is_Searched_And_Passed_To_The_Provider_Search(
            GetTrainingCourseProvidersQuery query,
            GetProvidersListResponse apiResponse,
            GetStandardsListItem apiCourseResponse,
            GetLocationsListItem apiLocationResponse,
            [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> mockLocationApiClient,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<ICourseDeliveryApiClient<CourseDeliveryApiConfiguration>> mockApiClient,
            GetTrainingCourseProvidersQueryHandler handler)
        {
            var outcode = "CV1";

            query.Location = $"{outcode}";
            query.Lat = 0;
            query.Lon = 0;
            mockLocationApiClient
                .Setup(client =>
                    client.Get<GetLocationsListItem>(
                        It.Is<GetLocationByOutcodeRequest>(c => c.GetUrl.Contains(outcode))))
                .ReturnsAsync(apiLocationResponse);
            mockApiClient
                .Setup(client => client.Get<GetProvidersListResponse>(It.Is<GetProvidersByCourseRequest>(c =>
                    c.GetUrl.Contains(query.Id.ToString())
                    && c.GetUrl.Contains(apiLocationResponse.Location.GeoPoint.First().ToString())
                    && c.GetUrl.Contains(apiLocationResponse.Location.GeoPoint.Last().ToString())
                    && c.GetUrl.Contains($"&sortOrder={query.SortOrder}")
                )))
                .ReturnsAsync(apiResponse);
            mockCoursesApiClient
                .Setup(client => client.Get<GetStandardsListItem>(It.Is<GetStandardRequest>(c => c.GetUrl.Contains(query.Id.ToString()))))
                .ReturnsAsync(apiCourseResponse);


            var result = await handler.Handle(query, CancellationToken.None);

            result.Providers.Should().BeEquivalentTo(apiResponse.Providers);
            result.Total.Should().Be(apiResponse.TotalResults);
            result.Course.Should().BeEquivalentTo(apiCourseResponse);
            result.Location.Name.Should().Be(query.Location);
            result.Location.GeoPoint.Should().BeEquivalentTo(apiLocationResponse.Location.GeoPoint);
        }

        [Test, MoqAutoData]
        public async Task Then_If_The_Outcode_Returns_No_Results_Then_No_Location_Is_Returned(
            GetTrainingCourseProvidersQuery query,
            GetProvidersListResponse apiResponse,
            GetStandardsListItem apiCourseResponse,
            GetLocationsListItem apiLocationResponse,
            [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> mockLocationApiClient,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<ICourseDeliveryApiClient<CourseDeliveryApiConfiguration>> mockApiClient,
            GetTrainingCourseProvidersQueryHandler handler)
        {
            var outcode = "NO1SE";

            query.Location = $"{outcode}";
            query.Lat = 0;
            query.Lon = 0;
            mockLocationApiClient
                .Setup(client =>
                    client.Get<GetLocationsListItem>(
                        It.Is<GetLocationByOutcodeRequest>(c => c.GetUrl.Contains(outcode))))
                .ReturnsAsync(new GetLocationsListItem
                {
                    Location = null
                });
            mockApiClient
                .Setup(client => client.Get<GetProvidersListResponse>(It.Is<GetProvidersByCourseRequest>(c =>
                    c.GetUrl.Contains($"courses/{query.Id}/providers?lat=&lon=&sortOrder={query.SortOrder}")
                )))
                .ReturnsAsync(apiResponse);
            mockCoursesApiClient
                .Setup(client => client.Get<GetStandardsListItem>(It.Is<GetStandardRequest>(c => c.GetUrl.Contains(query.Id.ToString()))))
                .ReturnsAsync(apiCourseResponse);


            var result = await handler.Handle(query, CancellationToken.None);

            result.Providers.Should().BeEquivalentTo(apiResponse.Providers);
            result.Total.Should().Be(apiResponse.TotalResults);
            result.Course.Should().BeEquivalentTo(apiCourseResponse);
            result.Location.Should().BeNull();
        }

        [Test, MoqAutoData]
        public async Task Then_If_There_Is_An_Postcode_Supplied_It_Is_Searched_And_Passed_To_The_Provider_Search(
            GetTrainingCourseProvidersQuery query,
            GetProvidersListResponse apiResponse,
            GetStandardsListItem apiCourseResponse,
            GetLocationsListItem apiLocationResponse,
            [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> mockLocationApiClient,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<ICourseDeliveryApiClient<CourseDeliveryApiConfiguration>> mockApiClient,
            GetTrainingCourseProvidersQueryHandler handler)
        {
            var postcode = "CV1 1AA";

            query.Location = $"{postcode}";
            query.Lat = 0;
            query.Lon = 0;
            mockLocationApiClient
                .Setup(client =>
                    client.Get<GetLocationsListItem>(
                        It.Is<GetLocationByFullPostcodeRequest>(c => c.GetUrl.Contains(postcode.Split().FirstOrDefault())
                                                                     && c.GetUrl.Contains(postcode.Split().LastOrDefault()))))
                .ReturnsAsync(apiLocationResponse);
            mockApiClient
                .Setup(client => client.Get<GetProvidersListResponse>(It.Is<GetProvidersByCourseRequest>(c =>
                    c.GetUrl.Contains(query.Id.ToString())
                    && c.GetUrl.Contains(apiLocationResponse.Location.GeoPoint.First().ToString())
                    && c.GetUrl.Contains(apiLocationResponse.Location.GeoPoint.Last().ToString())
                    && c.GetUrl.Contains($"&sortOrder={query.SortOrder}")
                )))
                .ReturnsAsync(apiResponse);
            mockCoursesApiClient
                .Setup(client => client.Get<GetStandardsListItem>(It.Is<GetStandardRequest>(c => c.GetUrl.Contains(query.Id.ToString()))))
                .ReturnsAsync(apiCourseResponse);


            var result = await handler.Handle(query, CancellationToken.None);

            result.Providers.Should().BeEquivalentTo(apiResponse.Providers);
            result.Total.Should().Be(apiResponse.TotalResults);
            result.Course.Should().BeEquivalentTo(apiCourseResponse);
            result.Location.Name.Should().Be(query.Location);
            result.Location.GeoPoint.Should().BeEquivalentTo(apiLocationResponse.Location.GeoPoint);
        }

        [Test, MoqAutoData]
        public async Task Then_If_There_Is_An_Outcode_And_District_Supplied_It_Is_Searched_And_Passed_To_The_Provider_Search(
            GetTrainingCourseProvidersQuery query,
            GetProvidersListResponse apiResponse,
            GetStandardsListItem apiCourseResponse,
            GetLocationsListItem apiLocationResponse,
            [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> mockLocationApiClient,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<ICourseDeliveryApiClient<CourseDeliveryApiConfiguration>> mockApiClient,
            GetTrainingCourseProvidersQueryHandler handler)
        {
            var outcode = "CV1";
            var location = $"{outcode} Birmingham, West Midlands";

            query.Location = $"{location}";
            query.Lat = 0;
            query.Lon = 0;
            mockLocationApiClient
                .Setup(client =>
                    client.Get<GetLocationsListItem>(
                        It.Is<GetLocationByOutcodeAndDistrictRequest>(c => c.GetUrl.Contains(outcode))))
                .ReturnsAsync(apiLocationResponse);
            mockApiClient
                .Setup(client => client.Get<GetProvidersListResponse>(It.Is<GetProvidersByCourseRequest>(c =>
                    c.GetUrl.Contains(query.Id.ToString())
                    && c.GetUrl.Contains(apiLocationResponse.Location.GeoPoint.First().ToString())
                    && c.GetUrl.Contains(apiLocationResponse.Location.GeoPoint.Last().ToString())
                    && c.GetUrl.Contains($"&sortOrder={query.SortOrder}")
                )))
                .ReturnsAsync(apiResponse);
            mockCoursesApiClient
                .Setup(client => client.Get<GetStandardsListItem>(It.Is<GetStandardRequest>(c => c.GetUrl.Contains(query.Id.ToString()))))
                .ReturnsAsync(apiCourseResponse);


            var result = await handler.Handle(query, CancellationToken.None);

            result.Providers.Should().BeEquivalentTo(apiResponse.Providers);
            result.Total.Should().Be(apiResponse.TotalResults);
            result.Course.Should().BeEquivalentTo(apiCourseResponse);
            result.Location.Name.Should().Be($"{apiLocationResponse.Outcode} {apiLocationResponse.DistrictName}");
            result.Location.GeoPoint.Should().BeEquivalentTo(apiLocationResponse.Location.GeoPoint);
        }

        [Test, MoqAutoData]
        public async Task Then_If_There_Is_A_Lat_Lon_In_The_Request_Then_The_Location_Is_Not_Searched(
            GetTrainingCourseProvidersQuery query,
            GetProvidersListResponse apiResponse,
            GetStandardsListItem apiCourseResponse,
            [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> mockLocationApiClient,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<ICourseDeliveryApiClient<CourseDeliveryApiConfiguration>> mockApiClient,
            GetTrainingCourseProvidersQueryHandler handler
        )
        {
            mockApiClient
                .Setup(client => client.Get<GetProvidersListResponse>(It.Is<GetProvidersByCourseRequest>(c =>
                    c.GetUrl.Contains(query.Id.ToString())
                    && c.GetUrl.Contains(query.Lat.ToString())
                    && c.GetUrl.Contains(query.Lon.ToString())
                    && c.GetUrl.Contains($"&sortOrder={query.SortOrder}")
                )))
                .ReturnsAsync(apiResponse);
            mockCoursesApiClient
                .Setup(client => client.Get<GetStandardsListItem>(It.Is<GetStandardRequest>(c => c.GetUrl.Contains(query.Id.ToString()))))
                .ReturnsAsync(apiCourseResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Providers.Should().BeEquivalentTo(apiResponse.Providers);
            mockLocationApiClient.Verify(x=>x.Get<GetLocationsListItem>(It.IsAny<IGetApiRequest>()), Times.Never);
        }

        [Test, MoqAutoData]
        public async Task Then_If_There_Is_A_Partial_Location_Name_Then_This_Is_Searched_And_Matched_On_The_First_Result(
            GetTrainingCourseProvidersQuery query,
            GetProvidersListResponse apiResponse,
            GetStandardsListItem apiCourseResponse,
            GetLocationsListItem apiLocationResponse,
            [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> mockLocationApiClient,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<ICourseDeliveryApiClient<CourseDeliveryApiConfiguration>> mockApiClient,
            GetTrainingCourseProvidersQueryHandler handler)
        {
            apiLocationResponse.Postcode = "";
            apiLocationResponse.DistrictName = "";
            query.Lat = 0;
            query.Lon = 0;
            mockLocationApiClient
                .Setup(client =>
                    client.Get<GetLocationsListResponse>(
                        It.Is<GetLocationsQueryRequest>(c => c.GetUrl.Contains(query.Location))))
                .ReturnsAsync(new GetLocationsListResponse{Locations = new List<GetLocationsListItem>{apiLocationResponse}});
            mockApiClient
                .Setup(client => client.Get<GetProvidersListResponse>(It.Is<GetProvidersByCourseRequest>(c =>
                    c.GetUrl.Contains(query.Id.ToString())
                    && c.GetUrl.Contains($"lat={apiLocationResponse.Location.GeoPoint.First().ToString()}")
                    && c.GetUrl.Contains($"lon={apiLocationResponse.Location.GeoPoint.Last().ToString()}")
                    && c.GetUrl.Contains($"&sortOrder={query.SortOrder}")
                )))
                .ReturnsAsync(apiResponse);
            mockCoursesApiClient
                .Setup(client => client.Get<GetStandardsListItem>(It.Is<GetStandardRequest>(c => c.GetUrl.Contains(query.Id.ToString()))))
                .ReturnsAsync(apiCourseResponse);
            
            var result = await handler.Handle(query, CancellationToken.None);

            result.Providers.Should().BeEquivalentTo(apiResponse.Providers);
            result.Total.Should().Be(apiResponse.TotalResults);
            result.Course.Should().BeEquivalentTo(apiCourseResponse);
            result.Location.Name.Should().Be($"{apiLocationResponse.LocationName}, {apiLocationResponse.LocalAuthorityName}");
            result.Location.GeoPoint.Should().BeEquivalentTo(apiLocationResponse.Location.GeoPoint);
        }

        [Test, MoqAutoData]
        public async Task Then_If_Doing_A_Partial_Search_And_No_Matches_Then_Null_Location_Returned(
            GetTrainingCourseProvidersQuery query,
            GetProvidersListResponse apiResponse,
            GetStandardsListItem apiCourseResponse,
            [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> mockLocationApiClient,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<ICourseDeliveryApiClient<CourseDeliveryApiConfiguration>> mockApiClient,
            GetTrainingCourseProvidersQueryHandler handler)
        {
            query.Lat = 0;
            query.Lon = 0;
            mockLocationApiClient
                .Setup(client =>
                    client.Get<GetLocationsListResponse>(
                        It.Is<GetLocationsQueryRequest>(c => c.GetUrl.Contains(query.Location))))
                .ReturnsAsync(new GetLocationsListResponse{Locations = new List<GetLocationsListItem>()});
            mockApiClient
                .Setup(client => client.Get<GetProvidersListResponse>(It.Is<GetProvidersByCourseRequest>(c =>
                    c.GetUrl.Contains(query.Id.ToString())
                    && c.GetUrl.Contains($"&sortOrder={query.SortOrder}")
                )))
                .ReturnsAsync(apiResponse);
            mockCoursesApiClient
                .Setup(client => client.Get<GetStandardsListItem>(It.Is<GetStandardRequest>(c => c.GetUrl.Contains(query.Id.ToString()))))
                .ReturnsAsync(apiCourseResponse);
            
            var result = await handler.Handle(query, CancellationToken.None);

            result.Providers.Should().BeEquivalentTo(apiResponse.Providers);
            result.Total.Should().Be(apiResponse.TotalResults);
            result.Course.Should().BeEquivalentTo(apiCourseResponse);
            result.Location.Should().BeNull();
        }
        
        [Test, MoqAutoData]
        public async Task Then_If_There_Is_A_Partial_Location_Name_Which_Is_Less_Than_Two_Characters_Then_Location_Is_Set_To_Null(
            GetTrainingCourseProvidersQuery query,
            GetProvidersListResponse apiResponse,
            GetStandardsListItem apiCourseResponse,
            GetLocationsListResponse apiLocationResponse,
            [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> mockLocationApiClient,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<ICourseDeliveryApiClient<CourseDeliveryApiConfiguration>> mockApiClient,
            GetTrainingCourseProvidersQueryHandler handler)
        {
            query.Location = "C";
            query.Lat = 0;
            query.Lon = 0;
            mockApiClient
                .Setup(client => client.Get<GetProvidersListResponse>(It.Is<GetProvidersByCourseRequest>(c =>
                    c.GetUrl.Contains(query.Id.ToString())
                    && c.GetUrl.Contains($"&sortOrder={query.SortOrder}")
                )))
                .ReturnsAsync(apiResponse);
            mockCoursesApiClient
                .Setup(client => client.Get<GetStandardsListItem>(It.Is<GetStandardRequest>(c => c.GetUrl.Contains(query.Id.ToString()))))
                .ReturnsAsync(apiCourseResponse);


            var result = await handler.Handle(query, CancellationToken.None);

            result.Providers.Should().BeEquivalentTo(apiResponse.Providers);
            result.Total.Should().Be(apiResponse.TotalResults);
            result.Course.Should().BeEquivalentTo(apiCourseResponse);
            result.Location.Should().BeNull();
        }

        [Test, MoqAutoData]
        public async Task Then_If_The_Location_Lookup_Returns_Empty_And_There_Is_A_Location_It_Is_Searched_And_First_Returned(
            GetTrainingCourseProvidersQuery query,
            GetProvidersListResponse apiResponse,
            GetStandardsListItem apiCourseResponse,
            GetLocationsListItem apiLocationResponse,
            [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> mockLocationApiClient,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<ICourseDeliveryApiClient<CourseDeliveryApiConfiguration>> mockApiClient,
            GetTrainingCourseProvidersQueryHandler handler)
        {
            apiLocationResponse.Postcode = "";
            apiLocationResponse.DistrictName = "";
            query.Location = "LE1";
            query.Lat = 0;
            query.Lon = 0;
            mockLocationApiClient
                .Setup(client =>
                    client.Get<GetLocationsListItem>(
                        It.Is<GetLocationByOutcodeRequest>(c => c.GetUrl.Contains(query.Location))))
                .ReturnsAsync(new GetLocationsListItem
                {
                    Location = null
                });
            mockLocationApiClient
                .Setup(client =>
                    client.Get<GetLocationsListResponse>(
                        It.Is<GetLocationsQueryRequest>(c => c.GetUrl.Contains(query.Location))))
                .ReturnsAsync(new GetLocationsListResponse{Locations = new List<GetLocationsListItem>{apiLocationResponse}});
            mockApiClient
                .Setup(client => client.Get<GetProvidersListResponse>(It.Is<GetProvidersByCourseRequest>(c =>
                    c.GetUrl.Contains(query.Id.ToString())
                    && c.GetUrl.Contains($"lat={apiLocationResponse.Location.GeoPoint.First().ToString()}")
                    && c.GetUrl.Contains($"lon={apiLocationResponse.Location.GeoPoint.Last().ToString()}")
                    && c.GetUrl.Contains($"&sortOrder={query.SortOrder}")
                )))
                .ReturnsAsync(apiResponse);
            mockCoursesApiClient
                .Setup(client => client.Get<GetStandardsListItem>(It.Is<GetStandardRequest>(c => c.GetUrl.Contains(query.Id.ToString()))))
                .ReturnsAsync(apiCourseResponse);
            
            var result = await handler.Handle(query, CancellationToken.None);

            result.Providers.Should().BeEquivalentTo(apiResponse.Providers);
            result.Total.Should().Be(apiResponse.TotalResults);
            result.Course.Should().BeEquivalentTo(apiCourseResponse);
            result.Location.Should().NotBeNull();
            result.Location.Name.Should().Be($"{apiLocationResponse.LocationName}, {apiLocationResponse.LocalAuthorityName}");
            result.Location.GeoPoint.Should().BeEquivalentTo(apiLocationResponse.Location.GeoPoint);
        }

        [Test, MoqAutoData]
        public async Task And_ShowDemandResponse_Not_OK_Then_ShowDemand_False(
            GetTrainingCourseProvidersQuery query,
            GetProvidersListResponse apiResponse,
            GetStandardsListItem apiCourseResponse,
            int shortlistItemCount,
            [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> mockLocationApiClient,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<ICourseDeliveryApiClient<CourseDeliveryApiConfiguration>> mockApiClient,
            [Frozen] Mock<IEmployerDemandApiClient<EmployerDemandApiConfiguration>> mockEmployerDemandApiClient,
            [Frozen] Mock<IShortlistService> shortlistService,
            GetTrainingCourseProvidersQueryHandler handler)
        {
            apiCourseResponse.Level = 1;
            query.Location = "";
            query.Lat = 0;
            query.Lon = 0;
            mockApiClient
                .Setup(client => client.Get<GetProvidersListResponse>(It.Is<GetProvidersByCourseRequest>(c=>
                    c.GetUrl.Contains(query.Id.ToString()) 
                    && c.GetUrl.Contains($"sectorSubjectArea={apiCourseResponse.SectorSubjectAreaTier2Description}&level={apiCourseResponse.Level}")
                )))
                .ReturnsAsync(apiResponse);
            mockCoursesApiClient
                .Setup(client => client.Get<GetStandardsListItem>(It.Is<GetStandardRequest>(c=>c.GetUrl.Contains(query.Id.ToString()))))
                .ReturnsAsync(apiCourseResponse);
            mockEmployerDemandApiClient
                .Setup(client => client.GetResponseCode(It.IsAny<GetShowEmployerDemandRequest>()))
                .ReturnsAsync(HttpStatusCode.Forbidden);
            shortlistService.Setup(x => x.GetShortlistItemCount(query.ShortlistUserId))
                .ReturnsAsync(shortlistItemCount);
            
            var result = await handler.Handle(query, CancellationToken.None);
            
            result.ShowEmployerDemand.Should().BeFalse();
        }
        
    }
}