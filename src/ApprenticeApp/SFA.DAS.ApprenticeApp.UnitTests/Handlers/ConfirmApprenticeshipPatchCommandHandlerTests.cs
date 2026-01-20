using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeApp.Application.Commands.Commitments;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.UnitTests.Handlers
{
    public class ConfirmApprenticeshipPatchCommandHandlerTests
    {
        [Test, MoqAutoData]
        public async Task Handle_Calls_Patch_With_Correct_Url_And_Data_And_Returns_Unit(
            [Frozen] Mock<IApprenticeCommitmentsApiClient<ApprenticeCommitmentsApiConfiguration>> clientMock,
            ConfirmApprenticeshipPatchCommandHandler handler,
            Confirmations confirmations)
        {
            // Arrange
            var apprenticeId = Guid.NewGuid();
            var apprenticeshipId = 12345L;
            var revisionId = 67890L;

            var command = new ConfirmApprenticeshipPatchCommand
            {
                ApprenticeId = apprenticeId,
                ApprenticeshipId = apprenticeshipId,
                RevisionId = revisionId,
                Patch = confirmations
            };

            var expectedUrl =
                $"apprentices/{apprenticeId}/apprenticeships/{apprenticeshipId}/revisions/{revisionId}/confirmations";

            clientMock
                .Setup(c => c.Patch(
                    It.Is<IPatchApiRequest<Confirmations>>(r =>
                        r.PatchUrl == expectedUrl &&
                        r.Data == confirmations)))
                .Returns(Task.FromResult(HttpStatusCode.OK));

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);            
        }
    }
}
