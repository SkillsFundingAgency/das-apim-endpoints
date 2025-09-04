using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests.Courses;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Commitments;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Commitments;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.Apprentices.Commands.EditApprenticeship;

public class EditApprenticeshipCommandHandler(
    ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsV2ApiClient,
    ICourseTypeRulesService courseTypeRulesService)
    : IRequestHandler<EditApprenticeshipCommand, EditApprenticeshipResult>
{
    public async Task<EditApprenticeshipResult> Handle(EditApprenticeshipCommand command, CancellationToken cancellationToken)
    {
        var apprenticeshipRequest = new GetApprenticeshipRequest(command.ApprenticeshipId);
        var apprenticeship = await commitmentsV2ApiClient.Get<GetApprenticeshipResponse>(apprenticeshipRequest);

        if (apprenticeship == null)
        {
            throw new InvalidOperationException($"Apprenticeship {command.ApprenticeshipId} not found");
        }
        
        var versionToValidate = command.Version;
        var hasOptions = false;
        
        var triggerCalculate = command.CourseCode != apprenticeship.CourseCode || apprenticeship.StartDate < command.StartDate.Value;
        
        if (triggerCalculate)
        {
            TrainingProgramme trainingProgramme;

            if (int.TryParse(command.CourseCode, out var standardId))
            {
                var calculatedTrainingProgrammeVersionRequest = new GetCalculatedTrainingProgrammeVersionRequest(
                    standardId,
                    command.StartDate.Value);
                
                var calculatedTrainingProgrammeVersion = await commitmentsV2ApiClient.Get<GetTrainingProgrammeResponse>(calculatedTrainingProgrammeVersionRequest);

                trainingProgramme = calculatedTrainingProgrammeVersion.TrainingProgramme;
            }
            else
            {
                var trainingProgrammeRequest = new GetTrainingProgrammeRequest(command.CourseCode);
                var trainingProgrammeResponse = await commitmentsV2ApiClient.Get<GetTrainingProgrammeResponse>(trainingProgrammeRequest);
                
                trainingProgramme = trainingProgrammeResponse.TrainingProgramme;
            }

            versionToValidate = trainingProgramme.Version;
            hasOptions = trainingProgramme.Options.Any();
        }
        
        var courseTypeRules = await courseTypeRulesService.GetCourseTypeRulesAsync(command.CourseCode);
        var validateRequest = new ValidateApprenticeshipForEditRequest
        {
            ApprenticeshipId = command.ApprenticeshipId,
            EmployerAccountId = command.EmployerAccountId,
            ProviderId = command.ProviderId,
            FirstName = command.FirstName,
            LastName = command.LastName,
            Cost = command.Cost,
            EmployerReference = command.EmployerReference,
            DateOfBirth = command.DateOfBirth,
            Email = command.Email,
            ULN = command.ULN,
            TrainingCode = command.CourseCode,
            StartDate = command.StartDate,
            EndDate = command.EndDate,
            EmploymentEndDate = command.EmploymentEndDate,
            DeliveryModel = command.DeliveryModel,
            ProviderReference = command.ProviderReference,
            Version = versionToValidate,
            Option = command.Option,
            EmploymentPrice = command.EmploymentPrice,
            MinimumAgeAtApprenticeshipStart = courseTypeRules.LearnerAgeRules.MinimumAge,
            MaximumAgeAtApprenticeshipStart = courseTypeRules.LearnerAgeRules.MaximumAge,
            Party = command.Party,
        };

        var validateApiRequest = new ValidateApprenticeshipForEditApiRequest(validateRequest);
        await commitmentsV2ApiClient.PostWithResponseCode<NullResponse>(validateApiRequest);

        return new EditApprenticeshipResult
        {
            ApprenticeshipId = command.ApprenticeshipId,
            HasOptions = hasOptions,
            Version = versionToValidate,
            CourseOrStartDateChanged = triggerCalculate
        };
    }
}