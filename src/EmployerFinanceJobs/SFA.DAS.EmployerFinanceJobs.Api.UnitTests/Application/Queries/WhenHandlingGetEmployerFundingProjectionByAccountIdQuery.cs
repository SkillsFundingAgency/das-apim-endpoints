using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.EmployerFinanceJobs.InnerApi.Requests;
using SFA.DAS.EmployerFinanceJobs.InnerApi.Responses;
using SFA.DAS.EmployerFinanceJobs.Queries.GetEmployerFundingProjectionByAccountId;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerFinanceJobs.UnitTests.Application.Queries;

[TestFixture]
internal class WhenHandlingGetEmployerFundingProjectionByAccountIdQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Data_Returned(
            GetEmployerFundingProjectionByAccountIdQuery query,
            GetEmployerFundingProjectionByAccountIdResponse apiResponse,
            [Frozen] Mock<IFundingProjectionApiClient<FundingProjectionApiConfiguration>> fundingProjectionApiClient,
            GetEmployerFundingProjectionByAccountIdQueryHandler handler)
    {
        //Arrange
        var expectedGetUrl = new GetEmployerFundingProjectionByAccountIdRequest(query.AccountId);
        fundingProjectionApiClient
            .Setup(x => x.Get<GetEmployerFundingProjectionByAccountIdResponse>(
                It.Is<GetEmployerFundingProjectionByAccountIdRequest>(c => c.GetUrl.Equals(expectedGetUrl.GetUrl))))
            .ReturnsAsync(apiResponse);

        //Act
        var actual = await handler.Handle(query, CancellationToken.None);

        //Assert
        actual.Should().BeEquivalentTo(apiResponse);
    }

    [Test, MoqAutoData]
    public async Task Then_If_NotFound_Response_Then_Null_Returned(
        GetEmployerFundingProjectionByAccountIdQuery query,
        [Frozen] Mock<IFundingProjectionApiClient<FundingProjectionApiConfiguration>> fundingProjectionApiClient,
        GetEmployerFundingProjectionByAccountIdQueryHandler handler)
    {
        //Arrange
        fundingProjectionApiClient.Setup(x =>
                x.Get<GetEmployerFundingProjectionByAccountIdResponse>(
                    It.IsAny<GetEmployerFundingProjectionByAccountIdRequest>()))
            .ReturnsAsync((GetEmployerFundingProjectionByAccountIdResponse)null!);

        //Act
        var actual = await handler.Handle(query, CancellationToken.None);

        //Assert
        actual.Should().NotBeNull();
        actual.EmployerAccountId.Should().Be(0);
        actual.CommittedLearnerCostTotal.Should().Be(0);
        actual.CommittedTransferOutTotal.Should().Be(0);
    }
}