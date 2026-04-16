using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeApp.Application.Queries.Cmad.GetCommitmentsApprenticeshipById;
using SFA.DAS.ApprenticeApp.InnerApi.ApprenticeAccounts.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Commitments;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeApp.UnitTests.Handlers
{
    public class GetCommitmentsApprenticeshipByIdQueryHandlerTests
    {
        [Test, MoqAutoData]
        public async Task Handle_Should_Call_CommitmentsApiClient_With_Correct_Id_And_Return_Response(
            Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsApiClientMock,
            GetCommitmentsApprenticeshipByIdQuery query,
            GetApprenticeshipResponse expectedResponse,
            CancellationToken cancellationToken)
        {
            // Arrange – use deterministic id
            query.ApprenticeshipId = 98765L;

            commitmentsApiClientMock
                .Setup(c => c.Get<GetApprenticeshipResponse>(
                    It.Is<GetCommitmentsApprenticeshipByIdRequest>(
                        r => r.GetUrl == $"/api/apprenticeships/{query.ApprenticeshipId}"
                    )))
                .ReturnsAsync(expectedResponse);

            var sut = new GetCommitmentsApprenticeshipByIdQueryHandler(commitmentsApiClientMock.Object);

            // Act
            var result = await sut.Handle(query, cancellationToken);

            // Assert
            result.Should().BeSameAs(expectedResponse);

            commitmentsApiClientMock.Verify(
                c => c.Get<GetApprenticeshipResponse>(
                    It.Is<GetCommitmentsApprenticeshipByIdRequest>(
                        r => r.GetUrl == $"/api/apprenticeships/{query.ApprenticeshipId}"
                    )),
                Times.Once);
        }
    }
}