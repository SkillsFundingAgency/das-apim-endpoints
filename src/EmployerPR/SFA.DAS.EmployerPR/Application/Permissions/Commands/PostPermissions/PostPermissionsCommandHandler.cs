﻿using System.Net;
using MediatR;
using RestEase;
using SFA.DAS.EmployerPR.Application.Permissions.Queries.GetPermissions;
using SFA.DAS.EmployerPR.Common;
using SFA.DAS.EmployerPR.Infrastructure;
using SFA.DAS.EmployerPR.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Models.ProviderRelationships;

namespace SFA.DAS.EmployerPR.Application.Permissions.Commands.PostPermissions;

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

        switch (permissionsResponse.ResponseMessage.StatusCode)
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
                    throw new InvalidOperationException($"Unexpected response code {permissionsResponse.ResponseMessage.StatusCode} when trying to retrieve permissions for ukprn {command.Ukprn} and AccountLegalEntityId {command.AccountLegalEntityId}");
                }
        }

        if (NoUpdatesRequired(command.Operations, existingOperations))
        {
            return Unit.Value;
        }

        PermissionEmailTemplateType templateType;

        if (command.Operations.Count == 0)
        {
            await _providerRelationshipsApiRestClient.RemovePermissions(
                command.UserRef,
                command.Ukprn!.Value,
                command.AccountLegalEntityId,
                cancellationToken
            );

            templateType = PermissionEmailTemplateType.PermissionsRemoved;
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

    private static bool NoUpdatesRequired(List<Operation> requestedPermissions, List<Operation> currentPermissions)
    {
        return requestedPermissions.Count == 0 && currentPermissions.Count == 0 || HasIdenticalPermissions(requestedPermissions, currentPermissions);
    }

    private static bool HasIdenticalPermissions(List<Operation> requestedPermissions, List<Operation> currentPermissions)
    {
        return currentPermissions.OrderBy(a => (int)a).SequenceEqual(requestedPermissions.OrderBy(a => (int)a));
    }

    private static PermissionEmailTemplateType GetTemplateType(List<Operation> existingOperations)
    {
        return existingOperations.Count == 0 ?
            PermissionEmailTemplateType.PermissionsCreated :
            PermissionEmailTemplateType.PermissionsUpdated;
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
            new PostNotificationsRequest([notification]),
            cancellationToken
        );
    }

    private static int SetPermitsApproval(List<Operation> operations)
    {
        if (operations.Contains(Operation.CreateCohort))
        {
            return PERMITS_APPROVAL;
        }
        else
        {
            return default;
        }
    }

    private static int SetPermitsRecruit(List<Operation> operations)
    {
        if (operations.Contains(Operation.Recruitment))
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