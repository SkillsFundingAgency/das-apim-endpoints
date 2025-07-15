using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Commitments;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Commitments;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.Apprentices.Commands.ConfirmEditApprenticeship;

public class ConfirmEditApprenticeshipCommandHandler(
    ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsV2ApiClient,
    ICourseTypeRulesService courseTypeRulesService)
    : IRequestHandler<ConfirmEditApprenticeshipCommand, ConfirmEditApprenticeshipResult>
{
    public async Task<ConfirmEditApprenticeshipResult> Handle(ConfirmEditApprenticeshipCommand command, CancellationToken cancellationToken)
    {
        var apprenticeshipRequest = new GetApprenticeshipRequest(command.ApprenticeshipId);
        var apprenticeship = await commitmentsV2ApiClient.Get<GetApprenticeshipResponse>(apprenticeshipRequest);

        if (apprenticeship == null)
        {
            throw new InvalidOperationException($"Apprenticeship {command.ApprenticeshipId} not found");
        }
        
        var courseTypeRules = await courseTypeRulesService.GetCourseTypeRulesAsync(apprenticeship.CourseCode);
        
        var editRequestData = new EditApprenticeshipApiRequestData
        {
            ApprenticeshipId = command.ApprenticeshipId,
            ProviderId = command.ProviderId,
            FirstName = command.FirstName,
            LastName = command.LastName,
            Email = command.Email,
            DateOfBirth = command.DateOfBirth,
            Cost = command.Cost,
            ProviderReference = command.ProviderReference,
            StartDate = command.StartDate,
            EndDate = command.EndDate,
            DeliveryModel = command.DeliveryModel,
            EmploymentEndDate = command.EmploymentEndDate,
            EmploymentPrice = command.EmploymentPrice,
            CourseCode = command.CourseCode,
            Version = command.Version,
            Option = command.Option == "TBC" ? string.Empty : command.Option,
            MinimumAgeAtApprenticeshipStart = courseTypeRules.LearnerAgeRules.MinimumAge,
            MaximumAgeAtApprenticeshipStart = courseTypeRules.LearnerAgeRules.MaximumAge
        };

        var editApiRequest = new EditApprenticeshipApiRequest(editRequestData);
        var result = await commitmentsV2ApiClient.PostWithResponseCode<EditApprenticeshipResponse>(editApiRequest);

        return new ConfirmEditApprenticeshipResult
        {
            ApprenticeshipId = result.Body.ApprenticeshipId,
            NeedReapproval = result.Body.NeedReapproval
        };
    }
} 