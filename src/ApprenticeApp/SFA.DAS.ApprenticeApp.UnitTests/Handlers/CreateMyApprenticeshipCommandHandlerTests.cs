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
    public class CreateMyApprenticeshipCommandHandlerTests
    {
        [Test, MoqAutoData]
        public async Task Handle_Should_Post_CreateMyApprenticeshipRequest_With_Command_Data(
            Mock<IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration>> clientMock,
            CreateMyApprenticeshipCommand command,
            CancellationToken cancellationToken)
        {
            // arrange - deterministic values so assertions are stable
            var apprenticeId = Guid.NewGuid();
            var uln = 1234567890L;
            var apprenticeshipId = 1234L;
            var employerName = "Employer Ltd";
            var startDate = new DateTime(2020, 1, 1);
            var endDate = new DateTime(2021, 1, 1);
            var trainingProviderId = 567;
            var trainingProviderName = "Provider Ltd";
            var trainingCode = "T123";
            var standardUId = "STD-001";

            command.ApprenticeId = apprenticeId;
            command.Data = new CreateMyApprenticeshipData
            {
                Uln = uln,
                ApprenticeshipId = apprenticeshipId,
                EmployerName = employerName,
                StartDate = startDate,
                EndDate = endDate,
                TrainingProviderId = trainingProviderId,
                TrainingProviderName = trainingProviderName,
                TrainingCode = trainingCode,
                StandardUId = standardUId
            };

            clientMock
                .Setup(c => c.PostWithResponseCode<object>(It.IsAny<CreateMyApprenticeshipRequest>(), false))
                .Returns(Task.FromResult<ApiResponse<object>>(null));

            var sut = new CreateMyApprenticeshipCommandHandler(clientMock.Object);

            // act
            var result = await sut.Handle(command, cancellationToken);

            // assert
            result.Should().Be(Unit.Value);

            clientMock.Verify(
                c => c.PostWithResponseCode<object>(
                    It.Is<CreateMyApprenticeshipRequest>(req =>
                        req.ApprenticeId == apprenticeId
                        && (req.Data as CreateMyApprenticeshipData) != null
                        && ((CreateMyApprenticeshipData)req.Data).Uln == uln
                        && ((CreateMyApprenticeshipData)req.Data).ApprenticeshipId == apprenticeshipId
                        && ((CreateMyApprenticeshipData)req.Data).EmployerName == employerName
                        && ((CreateMyApprenticeshipData)req.Data).StartDate == startDate
                        && ((CreateMyApprenticeshipData)req.Data).EndDate == endDate
                        && ((CreateMyApprenticeshipData)req.Data).TrainingProviderId == trainingProviderId
                        && ((CreateMyApprenticeshipData)req.Data).TrainingProviderName == trainingProviderName
                        && ((CreateMyApprenticeshipData)req.Data).TrainingCode == trainingCode
                        && ((CreateMyApprenticeshipData)req.Data).StandardUId == standardUId
                    ),
                    false
                ),
                Times.Once);
        }
    }
}