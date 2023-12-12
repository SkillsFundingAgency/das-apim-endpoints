using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.SearchIndex;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries;

public class WhenHandlingSearchIndexQuery
{
    
    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Data_Returned(
        SearchIndexQuery query,
        GetApprenticeshipCountResponse apiResponse,
        [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> apiClient,
        SearchIndexQueryHandler handler)
    {
        // Arrange
        var expectedRequest = new GetApprenticeshipCountRequest(
            null,
            null,
            null,
            null,
            null
        );

        apiClient
            .Setup(client => client.Get<GetApprenticeshipCountResponse>(It.Is<GetApprenticeshipCountRequest>(r => r.GetUrl == expectedRequest.GetUrl)))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.AreEqual(apiResponse.TotalVacancies, result.TotalApprenticeshipCount);

    }
}