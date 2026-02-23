using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeApp.Application.Commands.Cmad;
using SFA.DAS.ApprenticeApp.InnerApi.ApprenticeProgress.Requests;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.UnitTests.Handlers
{
    public class CreateApprenticeshipFromRegistrationCommandHandlerTests
    {
        [Test, MoqAutoData]
        public async Task Handle_Should_Post_CreateApprenticeshipFromRegistrationRequest_With_Command_Data(
           Mock<IApprenticeCommitmentsApiClient<ApprenticeCommitmentsApiConfiguration>> clientMock,
           CreateApprenticeshipFromRegistrationCommand command,
           CancellationToken cancellationToken)
        {
            // arrange - deterministic values so assertions are stable
            var registrationId = Guid.NewGuid();
            var apprenticeId = Guid.NewGuid();
            var lastName = "Smith";
            var dob = new DateTime(1990, 1, 1);

            command.RegistrationId = registrationId;
            command.ApprenticeId = apprenticeId;
            command.LastName = lastName;
            command.DateOfBirth = dob;

            clientMock
                .Setup(c => c.PostWithResponseCode<object>(It.IsAny<CreateApprenticeshipFromRegistrationRequest>(), false))
                .Returns(Task.FromResult<ApiResponse<object>>(null));

            var sut = new CreateApprenticeshipFromRegistrationCommandHandler(clientMock.Object);

            // act
            var result = await sut.Handle(command, cancellationToken);

            // assert
            result.Should().Be(Unit.Value);

            clientMock.Verify(
                c => c.PostWithResponseCode<object>(
                    It.Is<CreateApprenticeshipFromRegistrationRequest>(req =>
                        // single-expression predicate (no braces, no local variables)
                        (req.Data as CreateApprenticeshipFromRegistrationData) != null
                        && ((CreateApprenticeshipFromRegistrationData)req.Data).RegistrationId == registrationId
                        && ((CreateApprenticeshipFromRegistrationData)req.Data).ApprenticeId == apprenticeId
                        && ((CreateApprenticeshipFromRegistrationData)req.Data).LastName == lastName
                        && ((CreateApprenticeshipFromRegistrationData)req.Data).DateOfBirth == dob
                    ),
                    false
                ),
                Times.Once);
        }
    }
}