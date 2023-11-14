using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.SearchApprenticeships;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Assessors.UnitTests.Application.Queries
{
    public class WhenHandlingSearchApprenticeshipsQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Called_And_Count_Returned(
            GetApprenticeshipCountResponse response,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> findApprenticeshipApiClient,
            SearchApprenticeshipsQueryHandler handler)
        {
            findApprenticeshipApiClient
                .Setup(x => x.Get<GetApprenticeshipCountResponse>(It.IsAny<GetApprenticeshipCountRequest>()))
                .ReturnsAsync(response);

            var actual = await handler.Handle(new SearchApprenticeshipsQuery(), CancellationToken.None);

            actual.TotalApprenticeshipCount.Should().Be(response.TotalVacancies);
        }

        [Test, MoqAutoData]
        public async Task And_when_There_Is_Filter_Data_Then_The_Api_Called_And_Count_Returned(
            [Frozen] Mock<ILocationLookupService> locationLookupService,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> apiClient,
            SearchApprenticeshipsQueryHandler handler,
            SearchApprenticeshipsQuery query,
            LocationItem locationInfo,
            GetApprenticeshipCountResponse apiResponse)
        {
            // Arrange
            locationLookupService
                .Setup(service => service.GetLocationInformation(
                    query.Location, default, default, false))
                .ReturnsAsync(locationInfo);

            // Pass locationInfo to the request
            var expectedRequest = new GetApprenticeshipCountRequest(
                locationInfo.GeoPoint?.FirstOrDefault(),
                locationInfo.GeoPoint?.LastOrDefault(),
                query.SelectedRouteIds,
                query.Distance
            );

            //original (object equality issue)
            //apiClient
            //.Setup(client => client.Get<GetApprenticeshipCountResponse>(expectedRequest))
            //.ReturnsAsync(apiResponse);

            //fix 1 - care less about the call in setup and care more in verify step (see below)
            //apiClient
                //.Setup(client => client.Get<GetApprenticeshipCountResponse>(It.IsAny<GetApprenticeshipCountRequest>()))
                //.ReturnsAsync(apiResponse);

            //fix 2 - care about the call more in setup and forget it in verify (not required)
            //small downside is that a failing handler will generate a NRE rather than a failed assertion
            apiClient
                .Setup(client => client.Get<GetApprenticeshipCountResponse>(It.Is<GetApprenticeshipCountRequest>(r => r.GetUrl == expectedRequest.GetUrl)))
                .ReturnsAsync(apiResponse);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(apiResponse.TotalVacancies, result.TotalApprenticeshipCount);

            // Verify that the request is constructed with locationInfo

            //original - won't work due to object equality issue
            //apiClient.Verify(client => client.Get<GetApprenticeshipCountResponse>(
            //expectedRequest), Times.Once);
            
            //fix 1 - verify the call was made with correct properties
            //not required in fix 2, since the "match" is specified in the setup instead
            //apiClient.Verify(client => client.Get<GetApprenticeshipCountResponse>(It.Is<GetApprenticeshipCountRequest>(r => r.GetUrl == expectedRequest.GetUrl)
            //), Times.Once);
        }
    }

}