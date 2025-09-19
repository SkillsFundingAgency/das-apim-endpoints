using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.LearnerDataJobs.Application.Queries;
using SFA.DAS.LearnerDataJobs.InnerApi;
using SFA.DAS.LearnerDataJobs.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.LearnerDataJobs.UnitTests.Application.Queries;

public class GetAllLearnersQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Then_Returns_Successful_Response_When_Api_Call_Succeeds(
        GetAllLearnersQuery query,
        GetAllLearnersResponse apiResponse,
        [Frozen] Mock<IInternalApiClient<LearnerDataInnerApiConfiguration>> client,
        [Greedy] GetAllLearnersQueryHandler handler)
    {
        // Arrange
        client.Setup(x => x.Get<GetAllLearnersResponse>(It.Is<GetAllLearnersRequest>(r => 
            r.GetUrl.Contains($"page={query.Page}") &&
            r.GetUrl.Contains($"pageSize={query.PageSize}") &&
            r.GetUrl.Contains($"excludeApproved={query.ExcludeApproved.ToString().ToLower()}"))))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(apiResponse);
        client.Verify(x => x.Get<GetAllLearnersResponse>(It.Is<GetAllLearnersRequest>(r => 
            r.GetUrl.Contains($"page={query.Page}") &&
            r.GetUrl.Contains($"pageSize={query.PageSize}") &&
            r.GetUrl.Contains($"excludeApproved={query.ExcludeApproved.ToString().ToLower()}"))), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Then_Returns_Empty_Response_When_Api_Returns_Null(
        GetAllLearnersQuery query,
        [Frozen] Mock<IInternalApiClient<LearnerDataInnerApiConfiguration>> client,
        [Greedy] GetAllLearnersQueryHandler handler)
    {
        // Arrange
        client.Setup(x => x.Get<GetAllLearnersResponse>(It.IsAny<GetAllLearnersRequest>()))
            .ReturnsAsync((GetAllLearnersResponse)null);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Page.Should().Be(query.Page);
        result.PageSize.Should().Be(query.PageSize);
        result.TotalItems.Should().Be(0);
        result.TotalPages.Should().Be(0);
        result.Data.Should().BeEmpty();
    }

    [Test, MoqAutoData]
    public async Task Then_Returns_Empty_Response_When_Api_Throws_Exception(
        GetAllLearnersQuery query,
        Exception exception,
        [Frozen] Mock<IInternalApiClient<LearnerDataInnerApiConfiguration>> client,
        [Greedy] GetAllLearnersQueryHandler handler)
    {
        // Arrange
        client.Setup(x => x.Get<GetAllLearnersResponse>(It.IsAny<GetAllLearnersRequest>()))
            .ThrowsAsync(exception);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Page.Should().Be(query.Page);
        result.PageSize.Should().Be(query.PageSize);
        result.TotalItems.Should().Be(0);
        result.TotalPages.Should().Be(0);
        result.Data.Should().BeEmpty();
    }


    [Test, MoqAutoData]
    public async Task Then_Handles_Query_With_ExcludeApproved_False_Correctly(
        GetAllLearnersQuery query,
        GetAllLearnersResponse apiResponse,
        [Frozen] Mock<IInternalApiClient<LearnerDataInnerApiConfiguration>> client,
        [Greedy] GetAllLearnersQueryHandler handler)
    {
        // Arrange
        query.ExcludeApproved = false;
        client.Setup(x => x.Get<GetAllLearnersResponse>(It.Is<GetAllLearnersRequest>(r => 
            r.GetUrl.Contains($"page={query.Page}") &&
            r.GetUrl.Contains($"pageSize={query.PageSize}") &&
            r.GetUrl.Contains("excludeApproved=false"))))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(apiResponse);
        client.Verify(x => x.Get<GetAllLearnersResponse>(It.Is<GetAllLearnersRequest>(r => 
            r.GetUrl.Contains($"page={query.Page}") &&
            r.GetUrl.Contains($"pageSize={query.PageSize}") &&
            r.GetUrl.Contains("excludeApproved=false"))), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Then_Handles_Query_With_ExcludeApproved_True_Correctly(
        GetAllLearnersQuery query,
        GetAllLearnersResponse apiResponse,
        [Frozen] Mock<IInternalApiClient<LearnerDataInnerApiConfiguration>> client,
        [Greedy] GetAllLearnersQueryHandler handler)
    {
        // Arrange
        query.ExcludeApproved = true;
        client.Setup(x => x.Get<GetAllLearnersResponse>(It.Is<GetAllLearnersRequest>(r => 
            r.GetUrl.Contains($"page={query.Page}") &&
            r.GetUrl.Contains($"pageSize={query.PageSize}") &&
            r.GetUrl.Contains("excludeApproved=true"))))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(apiResponse);
        client.Verify(x => x.Get<GetAllLearnersResponse>(It.Is<GetAllLearnersRequest>(r => 
            r.GetUrl.Contains($"page={query.Page}") &&
            r.GetUrl.Contains($"pageSize={query.PageSize}") &&
            r.GetUrl.Contains("excludeApproved=true"))), Times.Once);
    }

}
