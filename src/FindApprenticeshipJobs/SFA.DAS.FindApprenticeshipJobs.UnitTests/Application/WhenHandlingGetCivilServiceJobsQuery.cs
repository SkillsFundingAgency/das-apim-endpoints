using SFA.DAS.FindApprenticeshipJobs.Application.Queries.CivilServiceJobs;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipJobs.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Location;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Location;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using static SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses.GetCivilServiceJobsApiResponse;

namespace SFA.DAS.FindApprenticeshipJobs.UnitTests.Application;
[TestFixture]
public class WhenHandlingGetCivilServiceJobsQuery
{
    [Test, MoqAutoData]
    public async Task Handle_ReturnsEmptyList_WhenJobsListIsEmpty(
        [Frozen] Mock<ICivilServiceJobsApiClient<CivilServiceJobsApiConfiguration>> civilServiceJobsApiClient,
        [Greedy] GetCivilServiceJobsQueryHandler handler)
    {
        var apiResponse = new GetCivilServiceJobsApiResponse { Jobs = []};
        civilServiceJobsApiClient.Setup(x => x.Get<GetCivilServiceJobsApiResponse>(It.IsAny<GetCivilServiceJobsApiRequest>()))
            .ReturnsAsync(apiResponse);

        var result = await handler.Handle(new GetCivilServiceJobsQuery(), CancellationToken.None);

        result.CivilServiceVacancies.Should().BeEmpty();
    }

    [Test, MoqAutoData]
    public async Task Handle_WhenJobsReturned_ShouldMapAndReturnVacancies(
        GetRoutesListItem routeListItem,
        [Frozen] Mock<ICivilServiceJobsApiClient<CivilServiceJobsApiConfiguration>> mockCivilServiceJobsApiClient,
        [Frozen] Mock<ICourseService> mockCourseService,
        [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> mockLocationApiClient,
        [Frozen] Mock<ILiveVacancyMapper> mockMapper,
        [Greedy] GetCivilServiceJobsQueryHandler handler,
        CancellationToken cancellationToken)
    {
        // Arrange
        var query = new GetCivilServiceJobsQuery();

        var apiResponse = new GetCivilServiceJobsApiResponse
        {
            Jobs = [new Job {JobCode = "J001", Profession = new Profession { En = "Other" } }]
        };
        routeListItem.Name = "Business and administration";

        mockCivilServiceJobsApiClient
            .Setup(c => c.Get<GetCivilServiceJobsApiResponse>(It.IsAny<GetCivilServiceJobsApiRequest>()))
            .ReturnsAsync(apiResponse);

        var routes = new GetRoutesListResponse
        {
            Routes = new List<GetRoutesListItem>
            {
                routeListItem
            }
        };
        mockCourseService
            .Setup(x => x.GetRoutes())
            .ReturnsAsync(routes);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.CivilServiceVacancies.Should().HaveCount(1);
        mockCourseService.Verify(x => x.GetRoutes(), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Handle_WhenJobHasValidCoordinates_ShouldCallLocationApiAndUpdateAddress(
        GetRoutesListItem routeListItem,
        [Frozen] Mock<ICivilServiceJobsApiClient<CivilServiceJobsApiConfiguration>> mockCivilServiceJobsApiClient,
        [Frozen] Mock<ICourseService> mockCourseService,
        [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> mockLocationApiClient,
        [Frozen] Mock<ILiveVacancyMapper> mockMapper,
        [Greedy] GetCivilServiceJobsQueryHandler handler,
        CancellationToken cancellationToken)
    {
        // Arrange
        var query = new GetCivilServiceJobsQuery();

        var job = new Job { JobCode = "J002", Profession = new Profession { En = "Other" } };
        var apiResponse = new GetCivilServiceJobsApiResponse { Jobs = [job]};
        routeListItem.Name = "Business and administration";

        mockCivilServiceJobsApiClient
            .Setup(c => c.Get<GetCivilServiceJobsApiResponse>(It.IsAny<GetCivilServiceJobsApiRequest>()))
            .ReturnsAsync(apiResponse);

        var routes = new GetRoutesListResponse
        {
            Routes = new List<GetRoutesListItem>
            {
                routeListItem
            }
        };
        mockCourseService
            .Setup(x => x.GetRoutes())
            .ReturnsAsync(routes);

        var liveVacancy = new SFA.DAS.FindApprenticeshipJobs.Application.Shared.LiveVacancy
        {
            Address = new Address
            {
                Latitude = 51.5,
                Longitude = -0.1
            },
            Title = "Geo Vacancy",
            VacancyReference = "VAC12345",
            OtherAddresses = []
        };

        mockMapper
            .Setup(x => x.Map(It.IsAny<Job>(), routeListItem))
            .Returns(liveVacancy);

        var locationResponse = new GetAddressByCoordinatesApiResponse
        {
            AddressLine1 = "10 Downing Street",
            AddressLine2 = "Westminster",
            AddressLine3 = "London",
            Postcode = "SW1A 2AA",
            Country = "United Kingdom"
        };

        mockLocationApiClient
            .Setup(x => x.Get<GetAddressByCoordinatesApiResponse>(It.IsAny<GetAddressByCoordinatesApiRequest>()))
            .ReturnsAsync(locationResponse);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.CivilServiceVacancies.Should().HaveCount(1);
        var updatedAddress = result.CivilServiceVacancies.First().Address;
        updatedAddress?.AddressLine1.Should().Be("10 Downing Street");
        updatedAddress?.Postcode.Should().Be("SW1A 2AA");

        mockLocationApiClient.Verify(
            x => x.Get<GetAddressByCoordinatesApiResponse>(
                It.IsAny<GetAddressByCoordinatesApiRequest>()),
            Times.Exactly(1)); //Primary address + Other addresses
    }

    [Test, MoqAutoData]
    public async Task Handle_WhenCoordinatesAreInvalid_ShouldSkipLocationLookup(
        GetRoutesListItem routeListItem,
        [Frozen] Mock<ICivilServiceJobsApiClient<CivilServiceJobsApiConfiguration>> mockCivilServiceJobsApiClient,
        [Frozen] Mock<ICourseService> mockCourseService,
        [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> mockLocationApiClient,
        [Frozen] Mock<ILiveVacancyMapper> mockMapper,
        [Greedy] GetCivilServiceJobsQueryHandler handler,
        CancellationToken cancellationToken)
    {
        // Arrange
        var query = new GetCivilServiceJobsQuery();

        var job = new Job { JobCode = "J003", Profession = new Profession{ En = "Other" } };
        var apiResponse = new GetCivilServiceJobsApiResponse { Jobs = [job]};
        routeListItem.Name = "Business and administration";

        mockCivilServiceJobsApiClient
            .Setup(c => c.Get<GetCivilServiceJobsApiResponse>(It.IsAny<GetCivilServiceJobsApiRequest>()))
            .ReturnsAsync(apiResponse);

        var routes = new GetRoutesListResponse
        {
            Routes = new List<GetRoutesListItem>
            {
                routeListItem
            }
        };
        mockCourseService
            .Setup(x => x.GetRoutes())
            .ReturnsAsync(routes);

        var liveVacancy = new SFA.DAS.FindApprenticeshipJobs.Application.Shared.LiveVacancy
        {
            Title = "Invalid Geo Vacancy",
            VacancyReference = "VAC1234",
            Address = new Address()
        };

        mockMapper
            .Setup(x => x.Map(It.IsAny<Job>(), It.IsAny<GetRoutesListItem>()))
            .Returns(liveVacancy);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.CivilServiceVacancies.Should().HaveCount(1);
        mockLocationApiClient.Verify(
            x => x.Get<GetAddressByCoordinatesApiResponse>(
                It.IsAny<GetAddressByCoordinatesApiRequest>()),
            Times.Never);
    }
}