using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.TrackProgress.Apis.CommitmentsV2InnerApi;
using SFA.DAS.TrackProgress.Apis.TrackProgressInnerApi;
using SFA.DAS.TrackProgress.Application.DTOs;
using SFA.DAS.TrackProgress.Application.Services;

namespace SFA.DAS.TrackProgress.Application.Commands.TrackProgress;

public class TrackProgressCommandHandler : IRequestHandler<TrackProgressCommand, TrackProgressResponse>
{
    private readonly CommitmentsV2Service _commitmentsService;
    private readonly CoursesService _coursesService;
    private readonly TrackProgressService _trackProgressService;
    private readonly ILogger<TrackProgressCommandHandler> _logger;

    public TrackProgressCommandHandler(CommitmentsV2Service commitmentsV2Service, CoursesService coursesService, TrackProgressService trackProgressService, ILogger<TrackProgressCommandHandler> logger)
    {
        _commitmentsService = commitmentsV2Service;
        _coursesService = coursesService;
        _trackProgressService = trackProgressService;
        _logger = logger;
    }

    public async Task<TrackProgressResponse> Handle(TrackProgressCommand request, CancellationToken cancellationToken)
    {
        if (request.PlannedStartDate.Day != 1)
            throw new InvalidTaxonomyRequestException("Invalid start date (must start on the 1st)");

        var apprenticeshipsReponse = await _commitmentsService.GetApprenticeships(request.ProviderContext.ProviderId, request.Uln, request.PlannedStartDate);

        CheckTheApprenticeshipHasBeenFound(apprenticeshipsReponse);

        if (apprenticeshipsReponse.Apprenticeships?.Count > 1)
            throw new InvalidTaxonomyRequestException("Multiple apprenticeship records exist");

        CheckPayloadFormatIsCorrect(request.Progress);
        CheckPayloadForDuplicateIds(request.Progress!.Progress!.Ksbs!);

        var apprenticeship = await _commitmentsService.GetApprenticeship(apprenticeshipsReponse.Apprenticeships!.First().Id);
        CheckApprenticeshipStateCanAcceptTrackProgressUpdates(apprenticeship);

        await ValidateKsbIdsAgainstCourseKsbs(apprenticeship.StandardUId, apprenticeship.Option, request.Progress!.Progress!.Ksbs!);

        if (request.ProviderContext.InSandboxMode)
        {
            return new TrackProgressResponse();
        }

        var data = new KsbProgress
        {
            ProviderApprenticeshipIdentifier = new ProviderApprenticeshipIdentifier(request.ProviderContext.ProviderId, request.Uln, request.PlannedStartDate.ToString("yyyy-MM")),
            ApprenticeshipContinuationId = apprenticeship.ContinuationOfId,
            Ksbs = request!.Progress!.Progress!.Ksbs!.ToArray()
        };

        await _trackProgressService.SaveProgress(apprenticeship.Id, data);
        
        return new TrackProgressResponse();
    }

    private async Task ValidateKsbIdsAgainstCourseKsbs(string standardUId, string? option, List<ProgressDto.Ksb> ksbs)
    {
        option = string.IsNullOrWhiteSpace(option) ? "core" : option;
        var courseKsbsResponse = await _coursesService.GetKsbsForCourseOption(standardUId, option);

        var ksbMatches = from ksb in ksbs
                         join courseKsb in courseKsbsResponse.Ksbs on ksb.Id!.ToLower() equals courseKsb.Id.ToString().ToLower()
                         into joinedKsbs
                         from match in joinedKsbs.DefaultIfEmpty()
                         select new
                         {
                             ksb.Id,
                             Matched = match != null
                         };

        var errors = new List<ErrorDetail>();
        foreach (var missingKsb in ksbMatches.Where(x => !x.Matched))
        {
            errors.Add(new(missingKsb.Id, "This KSB does not match the course option"));
        }

        if (errors.Any())
        {
            throw new InvalidTaxonomyRequestException("The KSB identifiers submitted  are not valid for the matched apprenticeship");
        }
    }

    private void CheckPayloadFormatIsCorrect(ProgressDto? payload)
    {
        if (payload == null || payload.Progress == null)
            throw new InvalidTaxonomyRequestException("Progress must be present");

        if (payload.Progress.Ksbs == null || !payload.Progress.Ksbs.Any())
            throw new InvalidTaxonomyRequestException("KSBs are required");

        var progress = payload.Progress;

        var errors = new List<ErrorDetail>();
        if (progress.Ksbs.Any(x => x.Id == null))
            errors.Add(new ErrorDetail("KSBs", "KSB Ids cannot be null"));

        var KsbStates = progress.Ksbs.Where(x => x.Id != null).Select(x => new
        { x.Id, IsValidId = Guid.TryParse(x.Id, out _), x.Value, IsValidValue = x.Value >= 1 && x.Value <= 100 });

        foreach (var ksbState in KsbStates)
        {
            if (ksbState.IsValidId && ksbState.IsValidValue)
                continue;
            if (!ksbState.IsValidId)
                errors.Add(new ErrorDetail(ksbState.Id!, $"{ksbState.Id} is not a valid guid"));
            if (!ksbState.IsValidValue)
                errors.Add(new ErrorDetail(ksbState.Id!, $"That the progress �{ksbState.Value}� presented against this KSB is between 1 and 100 (inclusive)"));
        }

        if (errors.Any())
            throw new InvalidTaxonomyRequestException("Format of the Progress body is invalid", errors);
    }

    private void CheckPayloadForDuplicateIds(List<ProgressDto.Ksb> ksbs)
    {

        var errors = new List<ErrorDetail>();

        var groupIds = ksbs.GroupBy(x => x.Id).Where(g => g.Count() > 1);

        foreach (var group in groupIds)
        {
            errors.Add(new ErrorDetail(group.Key!,
                $"Ensure that there are no duplicate GUIDs in the progress submission"));
        }

        if (errors.Any())
            throw new InvalidTaxonomyRequestException("Format of the Progress body is invalid", errors);
    }

    private void CheckTheApprenticeshipHasBeenFound(GetApprenticeshipsResponse apprenticeships)
    {
        if (apprenticeships.TotalApprenticeshipsFound == 0)
            throw new ApprenticeshipNotFoundException();

        if (apprenticeships.TotalApprenticeshipsFound > 1)
            apprenticeships.Apprenticeships?.RemoveAll(x => x.StartDate == x.StopDate);

        if (apprenticeships.Apprenticeships?.Count == 0)
            throw new ApprenticeshipNotFoundException();
    }

    private void CheckApprenticeshipStateCanAcceptTrackProgressUpdates(GetApprenticeshipResponse apprenticeship)
    {
        var errors = new List<ErrorDetail>();

        if (apprenticeship?.DeliveryModel != DeliveryModel.PortableFlexiJob)
            errors.Add(new ErrorDetail("DeliveryModel", "Must be a portable flexi-job"));

        if (apprenticeship?.ApprenticeshipStatus == ApprenticeshipStatus.WaitingToStart)
            errors.Add(new ErrorDetail("ApprenticeshipStatus", "Apprenticeship cannot be updated if not started"));

        if (errors.Any())
            throw new InvalidTaxonomyRequestException("This apprenticeship cannot accept the taxonomy as it's in an incorrect state", errors);
    }
}

public class ApprenticeshipNotFoundException : Exception
{
}

public class InvalidTaxonomyRequestException : Exception
{
    public List<ErrorDetail> Errors { get; } = new();

    public InvalidTaxonomyRequestException(string errorTitle) : base(errorTitle)
    {
    }

    public InvalidTaxonomyRequestException(string errorTitle, List<ErrorDetail> errors) : this(errorTitle)
    {
        Errors = errors;
    }
}

public record ErrorDetail(
    string Error,
    string Description
);