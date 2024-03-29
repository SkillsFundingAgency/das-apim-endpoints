using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.SearchIndex;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries;

public class WhenHandlingSearchIndexQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Data_Returned(
        SearchIndexQuery query,
        GetApprenticeshipCountResponse apiResponse,
        LocationItem locationItem,
        [Frozen] Mock<ILocationLookupService> locationService,
        [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> apiClient,
        SearchIndexQueryHandler handler)
    {
        // Arrange
        var expectedRequest = new GetApprenticeshipCountRequest();
        apiClient
            .Setup(client => client.Get<GetApprenticeshipCountResponse>(It.Is<GetApprenticeshipCountRequest>(r => r.GetUrl == expectedRequest.GetUrl)))
            .ReturnsAsync(apiResponse);
        locationService.Setup(x => x.GetLocationInformation(query.LocationSearchTerm,0,0,false)).ReturnsAsync(locationItem);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(apiResponse.TotalVacancies, Is.EqualTo(result.TotalApprenticeshipCount));
        Assert.That(result.LocationSearched, Is.True);
        Assert.That(locationItem, Is.EqualTo(result.LocationItem));
    }
}