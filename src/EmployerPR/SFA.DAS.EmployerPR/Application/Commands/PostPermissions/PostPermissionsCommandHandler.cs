using MediatR;
using RestEase;
using SFA.DAS.EmployerPR.Application.Commands.PostNotifications;
using SFA.DAS.EmployerPR.Application.Queries.GetPermissions;
using SFA.DAS.EmployerPR.Common;
using SFA.DAS.EmployerPR.Infrastructure;
using SFA.DAS.SharedOuterApi.Models.ProviderRelationships;
using System.Net;

namespace SFA.DAS.EmployerPR.Application.Commands.PostPermissions;

public class PostPermissionsCommandHandler(IProviderRelationshipsApiRestClient _providerRelationshipsApiRestClient) : IRequestHandler<PostPermissionsCommand, Unit>
{
    private const int PERMITS_APPROVAL = 1;
    private const int PERMITS_RECRUITNENT = 1;
    private const int PERMITS_RECRUITNENT_WITH_REVIEW = 2;

    public async Task<Unit> Handle(PostPermissionsCommand command, CancellationToken cancellationToken)
    {
        Response<GetPermissionsResponse> permissionsResponse = 
            await _providerRelationshipsApiRestClient.GetPermissions(
                command.Ukprn, 
                command.AccountLegalEntityId, 
                cancellationToken
        );

        List<Operation> existingOperations = [];

        switch(permissionsResponse.ResponseMessage.StatusCode)
        {
            case HttpStatusCode.OK:
                {
                    existingOperations = permissionsResponse.GetContent().Operations;
                }
                break;
            case HttpStatusCode.NotFound:
                {
                    existingOperations = [];
                }
                break;
            default:
                {
                    throw new InvalidOperationException($"Invalid operation occurred trying to retrieve permissions for ukprn {command.Ukprn} and AccountLegalEntityId {command.AccountLegalEntityId}");
                }
        }

        if (NoUpdatesRequired(command.Operations, existingOperations))
        {
            return Unit.Value;
        }

        PermissionEmailTemplateType templateType = default;

        if (!command.Operations.Any())
        {
            await _providerRelationshipsApiRestClient.RemovePermissions(
                command.UserRef, 
                command.Ukprn!.Value, 
                command.AccountLegalEntityId, 
                cancellationToken
            );

            templateType = PermissionEmailTemplateType.PermissionDeleted;
        }
        else
        {
            await _providerRelationshipsApiRestClient.PostPermissions(
                command,
                cancellationToken
            );

            templateType = GetTemplateType(existingOperations);
        }

        await PostNotification(command, templateType, cancellationToken);

        return Unit.Value;
    }

    private bool NoUpdatesRequired(List<Operation> requestedPermissions, List<Operation> currentPermissions)
    {
        return (!requestedPermissions.Any() && !currentPermissions.Any()) || IdenticalPermissions(requestedPermissions, currentPermissions);
    }

    private bool IdenticalPermissions(List<Operation> requestedPermissions, List<Operation> currentPermissions)
    {
        return currentPermissions.OrderBy(a => (int)a).SequenceEqual(requestedPermissions.OrderBy(a => (int)a));
    }

    private PermissionEmailTemplateType GetTemplateType(List<Operation> operations)
    {
        return operations.Any() ?
            PermissionEmailTemplateType.PermissionsUpdated : 
            PermissionEmailTemplateType.PermissionsCreated;
    }

    private async Task PostNotification(PostPermissionsCommand command, PermissionEmailTemplateType templateType, CancellationToken cancellationToken)
    {
        NotificationModel notification = new NotificationModel()
        {
            NotificationType = nameof(NotificationType.Provider),
            TemplateName = templateType.ToString(),
            Ukprn = command.Ukprn,
            AccountLegalEntityId = command.AccountLegalEntityId,
            PermitApprovals = SetPermitsApproval(command.Operations),
            PermitRecruit = SetPermitsRecruit(command.Operations),
            CreatedBy = command.UserRef.ToString()
        };

        await _providerRelationshipsApiRestClient.PostNotifications(
            new PostNotificationsCommand([notification]),
            cancellationToken
        );
    }

    private int SetPermitsApproval(List<Operation> operations)
    {
        if(operations.Contains(Operation.CreateCohort))
        {
            return PERMITS_APPROVAL;
        }
        else
        {
            return default;
        }
    }

    private int SetPermitsRecruit(List<Operation> operations)
    {
        if(operations.Contains(Operation.Recruitment))
        {
            return PERMITS_RECRUITNENT;
        }

        if (operations.Contains(Operation.RecruitmentRequiresReview))
        {
            return PERMITS_RECRUITNENT_WITH_REVIEW;
        }

        return default;
    }
}