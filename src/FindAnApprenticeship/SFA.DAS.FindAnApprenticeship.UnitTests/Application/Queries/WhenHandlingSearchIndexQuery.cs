using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.SearchIndex;
using SFA.DAS.FindAnApprenticeship.InnerApi.RecruitApi.Requests;
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
        long totalApprenticeshipsAvailable,
        LocationItem locationItem,
        [Frozen] Mock<ILocationLookupService> locationService,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> apiClient,
        SearchIndexQueryHandler handler)
    {
        // Arrange
        var expectedRequest = new GetTotalPositionsAvailableRequest();
        apiClient
            .Setup(client => client.Get<long>(It.Is<GetTotalPositionsAvailableRequest>(r => r.GetUrl == expectedRequest.GetUrl)))
            .ReturnsAsync(totalApprenticeshipsAvailable);
        locationService.Setup(x => x.GetLocationInformation(query.LocationSearchTerm,0,0,false)).ReturnsAsync(locationItem);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(totalApprenticeshipsAvailable, Is.EqualTo(result.TotalApprenticeshipCount));
        Assert.That(result.LocationSearched, Is.True);
        Assert.That(locationItem, Is.EqualTo(result.LocationItem));
    }
}