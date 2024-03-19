using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application.Apprentices.Commands.StopApprenticeship;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Approvals.UnitTests.Application.Apprentices.Commands
{
    public class StopApprenticeshipCommandHandlerTests
    {
        [Test, MoqAutoData]
        public async Task Handler_should_send_StopApprenticeshipRequest(
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> mockCommitmentsApi,
        StopApprenticeshipCommand command,
        StopApprenticeshipCommandHandler handler)
        {
            mockCommitmentsApi
                .Setup(m => m.PostWithResponseCode<StopApprenticeshipRequestResponse>(It.IsAny<StopApprenticeshipRequest>(), false))
                .ReturnsAsync(new ApiResponse<StopApprenticeshipRequestResponse>(null, HttpStatusCode.OK, string.Empty));

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            result.Should().Be(Unit.Value);
            mockCommitmentsApi.Verify(m => m.PostWithResponseCode<StopApprenticeshipRequestResponse>(It.IsAny<StopApprenticeshipRequest>(), false)
            , Times.Once);
        }
    }
}
