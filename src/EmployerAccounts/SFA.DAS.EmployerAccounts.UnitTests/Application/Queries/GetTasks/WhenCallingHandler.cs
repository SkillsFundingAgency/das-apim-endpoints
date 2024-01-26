using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Application.Queries.GetTasks;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerFinance;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LevyTransferMatching;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerFinance;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LevyTransferMatching;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAccounts.UnitTests.Application.Queries.GetTasks
{
    [TestFixture]
    public class WhenCallingHandler
    {
        [Test, MoqAutoData]
        public async Task Then_NumberTransferPledgeApplicationsToReview_Should_Match_Api_Response(
        [Frozen] Mock<ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration>> mockLTMApi,
        GetApplicationsResponse ltmApplicationsResponse,
        GetTasksQuery request,
        GetTasksQueryHandler handler)
        {

            mockLTMApi
                .Setup(m => m.Get<GetApplicationsResponse>(It.Is<GetApplicationsRequest>(r =>
                    r.SenderAccountId == request.AccountId
                    && r.ApplicationStatusFilter == ApplicationStatus.Pending)))
                .ReturnsAsync(ltmApplicationsResponse);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            result.NumberTransferPledgeApplicationsToReview.Should().Be(ltmApplicationsResponse.TotalItems);
        }

        [Test, MoqAutoData]
        public async Task Then_Gets_Tasks_Returns_NumberOfPendingTransferConnections(
          [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> _financeApiClient,
          List<GetTransferConnectionsResponse.TransferConnection> transferConnectionsResponse,
          GetTasksQuery request,
          GetTasksQueryHandler handler)
        {
            _financeApiClient
                .Setup(m => m.Get<List<GetTransferConnectionsResponse.TransferConnection>>(
                    It.Is<GetTransferConnectionsRequest>(
                        r => r.AccountId == request.AccountId && r.Status == TransferConnectionInvitationStatus.Pending
                        )))
                .ReturnsAsync(transferConnectionsResponse);

            var result = await handler.Handle(request, CancellationToken.None);

            result.Should().NotBeNull();
            result.NumberOfPendingTransferConnections.Should().Be(transferConnectionsResponse.Count);
        }
    }
}