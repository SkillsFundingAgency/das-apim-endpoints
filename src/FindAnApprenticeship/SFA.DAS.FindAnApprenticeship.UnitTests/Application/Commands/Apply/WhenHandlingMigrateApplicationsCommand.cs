using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.MigrateLegacyApplications;
using SFA.DAS.FindAnApprenticeship.Services;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Commands.Apply
{
    public class WhenHandlingMigrateApplicationsCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_CommandResult_Is_Returned_As_Expected(
            MigrateApplicationsCommand command,
            [Frozen] Mock<ILegacyApplicationMigrationService> vacancyMigrationService,
            MigrateApplicationsCommandHandler handler)
        {
            var result = await handler.Handle(command, CancellationToken.None);

            result.Should().Be(Unit.Value);
            vacancyMigrationService
                .Verify(client => client.MigrateLegacyApplications(command.CandidateId, command.EmailAddress), Times.Once);
        }
    }
}
