using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerPR.Application.Commands.PostPermissions;
using SFA.DAS.EmployerPR.Infrastructure;
using SFA.DAS.SharedOuterApi.Models.ProviderRelationships;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerPR.UnitTests.Application.Permissions.Commands.PostPermissions;

public class PostPermissionsCommandHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_OneOperation_CallsPostPermissions(
        [Frozen] Mock<IProviderRelationshipsApiRestClient> providerRelationshipsApiRestClient,
        PostPermissionsCommandHandler sut,
        PostPermissionsCommand command,
        PostPermissionsCommandResult result
    )
    {
        command.Operations = new List<Operation> { Operation.CreateCohort };

        providerRelationshipsApiRestClient.Setup(x =>
            x.PostPermissions(
                command,
                CancellationToken.None
            )
        );

        var actual = await sut.Handle(command, CancellationToken.None);
        providerRelationshipsApiRestClient.Verify(s => s.PostPermissions(It.IsAny<PostPermissionsCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        providerRelationshipsApiRestClient.Verify(s => s.RemovePermissions(It.IsAny<Guid>(), It.IsAny<long>(), It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test, MoqAutoData]
    public async Task Handle_NoOperations_CallsRemovePermissions(
        [Frozen] Mock<IProviderRelationshipsApiRestClient> providerRelationshipsApiRestClient,
        PostPermissionsCommandHandler sut,
        PostPermissionsCommand command
    )
    {
        command.Operations = new List<Operation>();

        providerRelationshipsApiRestClient.Setup(x =>
            x.RemovePermissions(
                command.UserRef,
                command.Ukprn!.Value,
                command.AccountLegalEntityId,
                CancellationToken.None
            )
        );

        await sut.Handle(command, CancellationToken.None);
        providerRelationshipsApiRestClient.Verify(s => s.PostPermissions(It.IsAny<PostPermissionsCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        providerRelationshipsApiRestClient.Verify(s => s.RemovePermissions(command.UserRef, command.Ukprn!.Value, command.AccountLegalEntityId, It.IsAny<CancellationToken>()), Times.Once);
    }
}