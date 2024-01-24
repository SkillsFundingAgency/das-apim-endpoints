using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Application.Queries.GetTasks;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Commitments;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Commitments;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAccounts.UnitTests.Application.Queries.GetTasks
{
    [TestFixture]
    public class WhenCallingHandler
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Tasks_Returns_GetTasksQueryResult(
          [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> mockCommitmentsApi,
          GetTransferRequestSummaryResponse transferRequestResponse,
          GetTasksQuery request,
          GetTasksQueryHandler handler)
        {
            // Arrange
            foreach (var tr in transferRequestResponse.TransferRequestSummaryResponse)
            {
                tr.Status = TransferApprovalStatus.Pending;
            }

            mockCommitmentsApi
                .Setup(m => m.Get<GetTransferRequestSummaryResponse>(It.Is<GetTransferRequestsRequest>(r => r.AccountId == request.AccountId)))
                .ReturnsAsync(transferRequestResponse);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            result.NumberOfTransferRequestToReview.Should().Be(3);
        }

        [Test, MoqAutoData]
        public async Task Then_Only_Returns_Pending_TransferRequests(
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> mockCommitmentsApi,
        GetTransferRequestSummaryResponse transferRequestResponse,
        GetTasksQuery request,
        GetTasksQueryHandler handler)
        {
            transferRequestResponse.TransferRequestSummaryResponse.Select((cohort, index) =>
            {
                cohort.Status = index == 0 ? TransferApprovalStatus.Approved : TransferApprovalStatus.Pending;
                return cohort;
            }).ToArray();

            mockCommitmentsApi
                .Setup(m => m.Get<GetTransferRequestSummaryResponse>(It.Is<GetTransferRequestsRequest>(r => r.AccountId == request.AccountId)))
                .ReturnsAsync(transferRequestResponse);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);
            result.NumberOfTransferRequestToReview.Should().Be(2);

        }

        [Test, MoqAutoData]
        public async Task When_TransferRequests_Are_Null_Return_Zero(
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> mockCommitmentsApi,
        GetTasksQuery request,
        GetTasksQueryHandler handler)
        {
            mockCommitmentsApi
                .Setup(m => m.Get<GetTransferRequestSummaryResponse>(It.Is<GetTransferRequestsRequest>(r => r.AccountId == request.AccountId)))
                .ReturnsAsync(new GetTransferRequestSummaryResponse());

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            result.NumberOfTransferRequestToReview.Should().Be(0);
        }
    }
}