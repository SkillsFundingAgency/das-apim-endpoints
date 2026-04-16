using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using MediatR;
using SFA.DAS.ApprenticeApp.Application.Queries.Cmad.GetRevisionById;
using SFA.DAS.ApprenticeApp.InnerApi.Cmad.Requests;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeApp.UnitTests.Handlers
{
    public class GetRevisionsByIdQueryHandlerTests
    {
        [Test, MoqAutoData]
        public async Task Handle_Should_Call_Client_With_Correct_Url_And_Return_Revision(
            Mock<IApprenticeCommitmentsApiClient<ApprenticeCommitmentsApiConfiguration>> commitmentsApiClientMock,
            GetRevisionsByIdQuery query,
            Revision expectedRevision,
            CancellationToken cancellationToken)
        {
            // Arrange - deterministic ids so assertions are stable
            var apprenticeId = Guid.NewGuid();
            var apprenticeshipId = 11111L;
            var revisionId = 22222L;

            query.ApprenticeId = apprenticeId;
            query.ApprenticeshipId = apprenticeshipId;
            query.RevisionId = revisionId;

            var expectedUrl = $"apprentices/{apprenticeId}/apprenticeships/{apprenticeshipId}/revisions/{revisionId}";

            commitmentsApiClientMock
                .Setup(c => c.Get<Revision>(It.Is<GetRevisionsByIdRequest>(r => r.GetUrl == expectedUrl)))
                .ReturnsAsync(expectedRevision);

            var sut = new GetRevisionsByIdQueryHandler(commitmentsApiClientMock.Object);

            // Act
            var result = await sut.Handle(query, cancellationToken);

            // Assert
            result.Should().NotBeNull();
            result.Revision.Should().BeSameAs(expectedRevision);

            commitmentsApiClientMock.Verify(
                c => c.Get<Revision>(It.Is<GetRevisionsByIdRequest>(r => r.GetUrl == expectedUrl)),
                Times.Once);
        }
    }
}