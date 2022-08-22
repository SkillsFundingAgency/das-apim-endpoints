using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Application.Locations.Queries.GetAllProviderLocations;
using SFA.DAS.RoatpCourseManagement.Application.Locations.Queries.GetAvailableProviderLocations;
using SFA.DAS.RoatpCourseManagement.InnerApi.Models;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.RoatpCourseManagement.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.Application.Locations.Queries.GetAvailableProviderLocations
{
    [TestFixture]
    public class GetAvailableProviderLocationsQueryHandlerTests
    {
        private GetAvailableProviderLocationsQueryHandler _sut;
        private Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> _apiClientMock;

        [SetUp]
        public void Before_Each_Test()
        {
            _apiClientMock = new Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>>();
            _sut = new GetAvailableProviderLocationsQueryHandler(Mock.Of<ILogger<GetAvailableProviderLocationsQueryHandler>>(), _apiClientMock.Object);
        }


        [Test, AutoData]
        public async Task Handle_EmptyProviderCourseLocations_ReturnsAllProviderLocations(List<ProviderLocationModel> getAllProviderLocations, GetAvailableProviderLocationsQuery request)
        {
             var providerLocations = getAllProviderLocations.Where(a => a.LocationType == LocationType.Provider);
            _apiClientMock.Setup(a => a.Get<List<ProviderLocationModel>>(It.IsAny<GetAllProviderLocationsQuery>())).ReturnsAsync(getAllProviderLocations);
            _apiClientMock.Setup(a => a.Get<List<GetProviderCourseLocationsResponse>>(It.IsAny<GetProviderCourseLocationsRequest>())).ReturnsAsync(new List<GetProviderCourseLocationsResponse>());

            var result = await _sut.Handle(request, new CancellationToken());

            result.AvailableProviderLocations.Count.Should().Be(providerLocations.Count());
        }

        [Test, AutoData]
        public async Task Handle_HasProviderCourseLocations_ReturnsFilteredList(List<ProviderLocationModel> getAllProviderLocations, GetAvailableProviderLocationsQuery request)
        {
            var providerLocations = getAllProviderLocations.Where(a => a.LocationType == LocationType.Provider);
            var locationName = providerLocations.First().LocationName;
            _apiClientMock.Setup(a => a.Get<List<ProviderLocationModel>>(It.IsAny<GetAllProviderLocationsQuery>())).ReturnsAsync(getAllProviderLocations);
            _apiClientMock.Setup(a => a.Get<List<GetProviderCourseLocationsResponse>>(It.IsAny<GetProviderCourseLocationsRequest>())).ReturnsAsync(new List<GetProviderCourseLocationsResponse>() { new GetProviderCourseLocationsResponse {LocationName = locationName } });

            var result = await _sut.Handle(request, new CancellationToken());

            result.AvailableProviderLocations.Count.Should().Be(providerLocations.Count() - 1);
            result.AvailableProviderLocations.Any(c => c.LocationName == locationName).Should().BeFalse();
        }
    }
}
