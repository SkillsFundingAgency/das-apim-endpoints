﻿using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerPR.Application.Commands.PostNotifications;
using SFA.DAS.EmployerPR.Application.Commands.PostPermissions;
using SFA.DAS.EmployerPR.Application.Queries.GetPermissions;
using SFA.DAS.EmployerPR.Common;
using SFA.DAS.EmployerPR.Infrastructure;
using SFA.DAS.SharedOuterApi.Models.ProviderRelationships;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerPR.UnitTests.Application.Permissions.Commands.PostPermissions;

public class PostPermissionsCommandHandlerTests
{
    [Test]
    [MoqAutoData]
    public async Task PostPermissionsCommandHandler_Handle_Requires_No_Action(
        [Frozen] Mock<IProviderRelationshipsApiRestClient> providerRelationshipsApiRestClient,
        PostPermissionsCommandHandler sut,
        PostPermissionsCommandResult result,
        PostPermissionsCommand command
    )
    {
        command.Operations = [];

        providerRelationshipsApiRestClient.Setup(x =>
            x.GetPermissions(
                It.IsAny<long>(),
                It.IsAny<long>(),
                CancellationToken.None
            )
        ).ReturnsAsync(new GetPermissionsResponse());

        var actual = await sut.Handle(command, CancellationToken.None);
        actual.Should().Be(Unit.Value);

        providerRelationshipsApiRestClient.Verify(x => 
            x.RemovePermissions(
                It.IsAny<Guid>(), 
                It.IsAny<long>(), 
                It.IsAny<long>(),
                It.IsAny<CancellationToken>()
            ), 
        Times.Never);

        providerRelationshipsApiRestClient.Verify(x =>
            x.PostPermissions(
                It.IsAny<PostPermissionsCommand>(),
                It.IsAny<CancellationToken>()
            ),
        Times.Never);
    }

    [Test]
    [MoqAutoData]
    public async Task PostPermissionsCommandHandler_Handle_Requires_Permissions_Removed(
        [Frozen] Mock<IProviderRelationshipsApiRestClient> providerRelationshipsApiRestClient,
        PostPermissionsCommandHandler sut,
        PostPermissionsCommandResult result,
        PostPermissionsCommand command
    )
    {
        command.Operations = [];

        providerRelationshipsApiRestClient.Setup(x =>
            x.GetPermissions(
                It.IsAny<long>(),
                It.IsAny<long>(),
                CancellationToken.None
            )
        ).ReturnsAsync(new GetPermissionsResponse()
        {
            Operations = [Operation.Recruitment]
        });

        var actual = await sut.Handle(command, CancellationToken.None);
        actual.Should().Be(Unit.Value);

        providerRelationshipsApiRestClient.Verify(x =>
            x.RemovePermissions(
                It.IsAny<Guid>(),
                It.IsAny<long>(),
                It.IsAny<long>(),
                It.IsAny<CancellationToken>()
            ),
        Times.Once);

        providerRelationshipsApiRestClient.Verify(x =>
            x.PostPermissions(
                It.IsAny<PostPermissionsCommand>(),
                It.IsAny<CancellationToken>()
            ),
        Times.Never);

        providerRelationshipsApiRestClient.Verify(x =>
            x.PostNotifications(
                It.Is<PostNotificationsCommand>(cmd =>
                    cmd.Notifications.Count() == 1 &&
                    cmd.Notifications[0].NotificationType == nameof(PermissionEmailTemplateType.PermissionDeleted) &&
                    cmd.Notifications[0].TemplateName == "provider" &&
                    cmd.Notifications[0].Ukprn == command.Ukprn &&
                    cmd.Notifications[0].AccountLegalEntityId == command.AccountLegalEntityId &&
                    cmd.Notifications[0].PermitApprovals == 0 &&
                    cmd.Notifications[0].PermitRecruit == 0 &&
                    cmd.Notifications[0].CreatedBy == command.UserRef.ToString()
                ),
                It.IsAny<CancellationToken>()
            ),
        Times.Once);
    }

    [Test]
    [MoqAutoData]
    public async Task PostPermissionsCommandHandler_Handle_Permissions_Created_PermitRecruit(
        [Frozen] Mock<IProviderRelationshipsApiRestClient> providerRelationshipsApiRestClient,
        PostPermissionsCommandHandler sut,
        PostPermissionsCommandResult result,
        PostPermissionsCommand command
    )
    {
        command.Operations = [Operation.CreateCohort, Operation.Recruitment];

        providerRelationshipsApiRestClient.Setup(x =>
            x.GetPermissions(
                It.IsAny<long>(),
                It.IsAny<long>(),
                CancellationToken.None
            )
        ).ReturnsAsync(new GetPermissionsResponse()
        {
            Operations = []
        });

        var actual = await sut.Handle(command, CancellationToken.None);
        actual.Should().Be(Unit.Value);

        providerRelationshipsApiRestClient.Verify(x =>
            x.PostPermissions(
                It.IsAny<PostPermissionsCommand>(),
                It.IsAny<CancellationToken>()
            ),
        Times.Once);

        providerRelationshipsApiRestClient.Verify(x =>
            x.RemovePermissions(
                It.IsAny<Guid>(),
                It.IsAny<long>(),
                It.IsAny<long>(),
                It.IsAny<CancellationToken>()
            ),
        Times.Never);

        providerRelationshipsApiRestClient.Verify(x =>
            x.PostNotifications(
                It.Is<PostNotificationsCommand>(cmd =>
                    cmd.Notifications.Count() == 1 &&
                    cmd.Notifications[0].NotificationType == nameof(PermissionEmailTemplateType.PermissionsCreated) &&
                    cmd.Notifications[0].TemplateName == "provider" &&
                    cmd.Notifications[0].Ukprn == command.Ukprn &&
                    cmd.Notifications[0].AccountLegalEntityId == command.AccountLegalEntityId &&
                    cmd.Notifications[0].PermitApprovals == 1 &&
                    cmd.Notifications[0].PermitRecruit == 1 &&
                    cmd.Notifications[0].CreatedBy == command.UserRef.ToString()
                ),
                It.IsAny<CancellationToken>()
            ),
        Times.Once);
    }

    [Test]
    [MoqAutoData]
    public async Task PostPermissionsCommandHandler_Handle_Permissions_Created_PermitRecruitWithReview(
        [Frozen] Mock<IProviderRelationshipsApiRestClient> providerRelationshipsApiRestClient,
        PostPermissionsCommandHandler sut,
        PostPermissionsCommandResult result,
        PostPermissionsCommand command
    )
    {
        command.Operations = [Operation.CreateCohort, Operation.RecruitmentRequiresReview];

        providerRelationshipsApiRestClient.Setup(x =>
            x.GetPermissions(
                It.IsAny<long>(),
                It.IsAny<long>(),
                CancellationToken.None
            )
        ).ReturnsAsync(new GetPermissionsResponse()
        {
            Operations = []
        });

        var actual = await sut.Handle(command, CancellationToken.None);
        actual.Should().Be(Unit.Value);

        providerRelationshipsApiRestClient.Verify(x =>
            x.PostPermissions(
                It.IsAny<PostPermissionsCommand>(),
                It.IsAny<CancellationToken>()
            ),
        Times.Once);

        providerRelationshipsApiRestClient.Verify(x =>
            x.RemovePermissions(
                It.IsAny<Guid>(),
                It.IsAny<long>(),
                It.IsAny<long>(),
                It.IsAny<CancellationToken>()
            ),
        Times.Never);

        providerRelationshipsApiRestClient.Verify(x =>
            x.PostNotifications(
                It.Is<PostNotificationsCommand>(cmd =>
                    cmd.Notifications.Count() == 1 &&
                    cmd.Notifications[0].NotificationType == nameof(PermissionEmailTemplateType.PermissionsCreated) &&
                    cmd.Notifications[0].TemplateName == "provider" &&
                    cmd.Notifications[0].Ukprn == command.Ukprn &&
                    cmd.Notifications[0].AccountLegalEntityId == command.AccountLegalEntityId &&
                    cmd.Notifications[0].PermitApprovals == 1 &&
                    cmd.Notifications[0].PermitRecruit == 2 &&
                    cmd.Notifications[0].CreatedBy == command.UserRef.ToString()
                ),
                It.IsAny<CancellationToken>()
            ),
        Times.Once);
    }

    [Test]
    [MoqAutoData]
    public async Task PostPermissionsCommandHandler_Handle_Permissions_Updated(
       [Frozen] Mock<IProviderRelationshipsApiRestClient> providerRelationshipsApiRestClient,
       PostPermissionsCommandHandler sut,
       PostPermissionsCommandResult result,
       PostPermissionsCommand command
   )
    {
        command.Operations = [Operation.Recruitment];

        providerRelationshipsApiRestClient.Setup(x =>
            x.GetPermissions(
                It.IsAny<long>(),
                It.IsAny<long>(),
                CancellationToken.None
            )
        ).ReturnsAsync(new GetPermissionsResponse()
        {
            Operations = [Operation.RecruitmentRequiresReview]
        });

        var actual = await sut.Handle(command, CancellationToken.None);
        actual.Should().Be(Unit.Value);

        providerRelationshipsApiRestClient.Verify(x =>
            x.PostPermissions(
                It.IsAny<PostPermissionsCommand>(),
                It.IsAny<CancellationToken>()
            ),
        Times.Once);

        providerRelationshipsApiRestClient.Verify(x =>
            x.RemovePermissions(
                It.IsAny<Guid>(),
                It.IsAny<long>(),
                It.IsAny<long>(),
                It.IsAny<CancellationToken>()
            ),
        Times.Never);

        providerRelationshipsApiRestClient.Verify(x =>
            x.PostNotifications(
                It.Is<PostNotificationsCommand>(cmd =>
                    cmd.Notifications.Count() == 1 &&
                    cmd.Notifications[0].NotificationType == nameof(PermissionEmailTemplateType.PermissionsUpdated) &&
                    cmd.Notifications[0].TemplateName == "provider" &&
                    cmd.Notifications[0].Ukprn == command.Ukprn &&
                    cmd.Notifications[0].AccountLegalEntityId == command.AccountLegalEntityId &&
                    cmd.Notifications[0].PermitApprovals == 0 &&
                    cmd.Notifications[0].PermitRecruit == 1 &&
                    cmd.Notifications[0].CreatedBy == command.UserRef.ToString()
                ),
                It.IsAny<CancellationToken>()
            ),
        Times.Once);
    }
}