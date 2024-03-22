using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.RoatpCourseManagement.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetProviderCourseLocation;
using System.Linq;
using SFA.DAS.RoatpCourseManagement.InnerApi.Models;
using System;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.Application.Standards.Queries
{
    [TestFixture]
     public class GetProviderCourseLocationQueryHandlerTests
    {
        [Test, RecursiveMoqAutoData]
        public async Task Handle_CallsInnerApi_ReturnsValidResponse(
            List<GetProviderCourseLocationsResponse> apiResponseProviderCourseLocation,
            GetProviderCourseLocationQuery query,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
            GetProviderCourseLocationQueryHandler sut)
        {
            apiClientMock.Setup(c => c.GetWithResponseCode<List<GetProviderCourseLocationsResponse>>(It.Is<GetProviderCourseLocationsRequest>(c =>
                        c.GetUrl.Equals(new GetProviderCourseLocationsRequest(query.Ukprn, query.LarsCode).GetUrl)))).
                        ReturnsAsync(new ApiResponse<List<GetProviderCourseLocationsResponse>>(apiResponseProviderCourseLocation, HttpStatusCode.OK, ""));

            var providerCount = apiResponseProviderCourseLocation.Where(l => l.LocationType == LocationType.Provider).Count();
            var result = await sut.Handle(query, new CancellationToken());
            result.Should().NotBeNull();
            result.ProviderCourseLocations.Count.Should().Be(providerCount);
            result.ProviderCourseLocations.Count.Should().NotBe(apiResponseProviderCourseLocation.Count());
        }

        [Test, RecursiveMoqAutoData]
        public void Handle_CallsInnerApi_ReturnsExceptionWhenProviderCourseGetsBadRequest(
            GetProviderCourseLocationQuery query,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
            GetProviderCourseLocationQueryHandler sut)
        {
              apiClientMock.Setup(c => c.GetWithResponseCode<List<GetProviderCourseLocationsResponse>>(It.Is<GetProviderCourseLocationsRequest>(c =>
                        c.GetUrl.Equals(new GetProviderCourseLocationsRequest(query.Ukprn, query.LarsCode).GetUrl))))
                       .ReturnsAsync(new ApiResponse<List<GetProviderCourseLocationsResponse>>(new List<GetProviderCourseLocationsResponse>(), HttpStatusCode.BadRequest, "Error"));

            Assert.ThrowsAsync<InvalidOperationException>(() => sut.Handle(query, new CancellationToken()));
        }
    }
}
