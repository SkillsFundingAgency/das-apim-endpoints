using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using RestEase;
using SFA.DAS.EmployerPR.Application.Notifications.Commands.PostNotifications;
using SFA.DAS.EmployerPR.Application.Permissions.Commands.PostPermissions;
using SFA.DAS.EmployerPR.Application.Permissions.Queries.GetPermissions;
using SFA.DAS.EmployerPR.Common;
using SFA.DAS.EmployerPR.Infrastructure;
using SFA.DAS.SharedOuterApi.Models.ProviderRelationships;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using System.Text.Json;

namespace SFA.DAS.EmployerPR.UnitTests.Application.Permissions.Commands.PostPermissions;

public class PostPermissionsCommandHandlerTests
{
    [Test]
    [MoqAutoData]
    public async Task Handle_NoRequestedAndExistingOperations_NoAction(
        [Frozen] Mock<IProviderRelationshipsApiRestClient> providerRelationshipsApiRestClient,
        PostPermissionsCommandHandler sut,
        PostPermissionsCommandResult result,
        PostPermissionsCommand command
    )
    {
        command.Operations = [];

        var responseObject = new GetPermissionsResponse()
        {
            Operations = []
        };

        var responseString = JsonSerializer.Serialize(responseObject);

        providerRelationshipsApiRestClient.Setup(x =>
            x.GetPermissions(
                It.IsAny<long>(),
                It.IsAny<long>(),
                CancellationToken.None
            )
        ).ReturnsAsync(
            new Response<GetPermissionsResponse>(
                responseString,
                new(HttpStatusCode.OK),
                () => JsonSerializer.Deserialize<GetPermissionsResponse>(responseString)!
            )
        );

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
    public async Task Handle_IdenticalRequestedAndExistingOperations_NoAction(
        [Frozen] Mock<IProviderRelationshipsApiRestClient> providerRelationshipsApiRestClient,
        PostPermissionsCommandHandler sut,
        PostPermissionsCommandResult result,
        PostPermissionsCommand command
    )
    {
        command.Operations = [Operation.CreateCohort];

        var responseObject = new GetPermissionsResponse()
        {
            Operations = [Operation.CreateCohort]
        };

        var responseString = JsonSerializer.Serialize(responseObject);

        providerRelationshipsApiRestClient.Setup(x =>
            x.GetPermissions(
                It.IsAny<long>(),
                It.IsAny<long>(),
                CancellationToken.None
            )
        ).ReturnsAsync(
            new Response<GetPermissionsResponse>(
                responseString,
                new(HttpStatusCode.OK),
                () => JsonSerializer.Deserialize<GetPermissionsResponse>(responseString)!
            )
        );

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
    public void Handle_UnexpectedResponseOnExistingPermission_ThrowsException(
        [Frozen] Mock<IProviderRelationshipsApiRestClient> providerRelationshipsApiRestClient,
        PostPermissionsCommandHandler sut,
        PostPermissionsCommandResult result,
        PostPermissionsCommand command
    )
    {
        providerRelationshipsApiRestClient.Setup(x =>
            x.GetPermissions(
                It.IsAny<long>(),
                It.IsAny<long>(),
                CancellationToken.None
            )
        ).ReturnsAsync(
            new Response<GetPermissionsResponse>(
                string.Empty,
                new(HttpStatusCode.BadRequest),
                () => null!
            )
        );

        Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await sut.Handle(command, CancellationToken.None)
        );       

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
    public async Task Handle_NoOperationsRequestedWithExistingPermissions_RemovesExistingPermissions(
        [Frozen] Mock<IProviderRelationshipsApiRestClient> providerRelationshipsApiRestClient,
        PostPermissionsCommandHandler sut,
        PostPermissionsCommandResult result,
        PostPermissionsCommand command
    )
    {
        command.Operations = [];

        var responseObject = new GetPermissionsResponse()
        {
            Operations = new List<Operation> { Operation.Recruitment }
        };

        var responseString = JsonSerializer.Serialize(responseObject);

        providerRelationshipsApiRestClient.Setup(x =>
            x.GetPermissions(
                It.IsAny<long>(),
                It.IsAny<long>(),
                CancellationToken.None
            )
        ).ReturnsAsync(
            new Response<GetPermissionsResponse>(
                responseString, 
                new(HttpStatusCode.OK),
                () => JsonSerializer.Deserialize<GetPermissionsResponse>(responseString)!
            )
        );

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
                    cmd.Notifications[0].NotificationType == nameof(NotificationType.Provider) &&
                    cmd.Notifications[0].TemplateName == nameof(PermissionEmailTemplateType.PermissionDeleted) &&
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

        var responseObject = new GetPermissionsResponse()
        {
            Operations = []
        };

        var responseString = JsonSerializer.Serialize(responseObject);

        providerRelationshipsApiRestClient.Setup(x =>
            x.GetPermissions(
                It.IsAny<long>(),
                It.IsAny<long>(),
                CancellationToken.None
            )
        ).ReturnsAsync(
            new Response<GetPermissionsResponse>(
                responseString,
                new(HttpStatusCode.OK),
                () => JsonSerializer.Deserialize<GetPermissionsResponse>(responseString)!
            )
        );

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
                    cmd.Notifications[0].NotificationType == nameof(NotificationType.Provider) &&
                    cmd.Notifications[0].TemplateName == nameof(PermissionEmailTemplateType.PermissionsCreated) &&
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

        var responseObject = new GetPermissionsResponse()
        {
            Operations = []
        };

        var responseString = JsonSerializer.Serialize(responseObject);

        providerRelationshipsApiRestClient.Setup(x =>
            x.GetPermissions(
                It.IsAny<long>(),
                It.IsAny<long>(),
                CancellationToken.None
            )
        ).ReturnsAsync(
            new Response<GetPermissionsResponse>(
                responseString,
                new(HttpStatusCode.OK),
                () => JsonSerializer.Deserialize<GetPermissionsResponse>(responseString)!
            )
        );

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
                    cmd.Notifications[0].NotificationType == nameof(NotificationType.Provider) &&
                    cmd.Notifications[0].TemplateName == nameof(PermissionEmailTemplateType.PermissionsCreated) &&
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

        var responseObject = new GetPermissionsResponse()
        {
            Operations = new List<Operation> { Operation.RecruitmentRequiresReview }
        };

        var responseString = JsonSerializer.Serialize(responseObject);

        providerRelationshipsApiRestClient.Setup(x =>
            x.GetPermissions(
                It.IsAny<long>(),
                It.IsAny<long>(),
                CancellationToken.None
            )
        ).ReturnsAsync(
            new Response<GetPermissionsResponse>(
                responseString,
                new(HttpStatusCode.OK),
                () => JsonSerializer.Deserialize<GetPermissionsResponse>(responseString)!
            )
        );

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
                    cmd.Notifications[0].NotificationType == nameof(NotificationType.Provider) &&
                    cmd.Notifications[0].TemplateName == nameof(PermissionEmailTemplateType.PermissionsUpdated) &&
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