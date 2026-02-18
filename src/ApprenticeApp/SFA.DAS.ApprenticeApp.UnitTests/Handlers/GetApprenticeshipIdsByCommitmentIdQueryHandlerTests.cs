using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeApp.Application.Queries.Cmad.GetApprenticeshipIdsByCommitmentId;
using SFA.DAS.ApprenticeApp.InnerApi.ApprenticeAccounts.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeApp.UnitTests.Handlers
{
    public class GetApprenticeshipIdsByCommitmentIdQueryHandlerTests
    {
        [Test, MoqAutoData]
        public async Task Handle_Should_Call_CommitmentsApiClient_And_Return_Result(
            Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsApiClientMock,
            GetApprenticeshipIdsByCommitmentIdQuery query,
            GetApprenticeshipIdsByCommitmentIdQueryResult expectedResult,
            CancellationToken cancellationToken)
        {
            // Arrange - use a deterministic commitment id so assertions are stable
            query.CommitmentId = 12345L;

            // Setup the mock: when Get<T> is called with a request whose GetUrl contains the commitment id,
            // return the expectedResult.
            commitmentsApiClientMock
                .Setup(c => c.Get<GetApprenticeshipIdsByCommitmentIdQueryResult>(
                    It.Is<GetApprenticeshipIdsByCommitmentIdRequest>(r => r.GetUrl.Contains(query.CommitmentId.ToString()))
                ))
                .ReturnsAsync(expectedResult);

            var sut = new GetApprenticeshipIdsByCommitmentIdQueryHandler(commitmentsApiClientMock.Object);

            // Act
            var result = await sut.Handle(query, cancellationToken);

            // Assert
            result.Should().BeSameAs(expectedResult);

            commitmentsApiClientMock.Verify(
                c => c.Get<GetApprenticeshipIdsByCommitmentIdQueryResult>(
                    It.Is<GetApprenticeshipIdsByCommitmentIdRequest>(r => r.GetUrl.Contains(query.CommitmentId.ToString()))
                ),
                Times.Once);
        }
    }
}