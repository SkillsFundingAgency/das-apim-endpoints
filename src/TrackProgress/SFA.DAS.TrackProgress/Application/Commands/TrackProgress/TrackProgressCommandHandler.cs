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
    private const string ErrorTitleForProgressBody = "Failed to record progress due to one or more validation errors";
    private readonly List<ErrorDetail> _errors = new();

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

        var apprenticeship = await _commitmentsService.GetApprenticeship(apprenticeshipsReponse.Apprenticeships!.First().Id);

        if (string.IsNullOrEmpty(apprenticeship.Option))
            await ValidateCourseOptions(apprenticeship.StandardUId);

        await CheckForPayloadErrors(apprenticeship, request, apprenticeshipsReponse.Apprenticeships!.First().ApprenticeshipStatus);

        if (request.ProviderContext.InSandboxMode)
        {
            return new(999);
        }

        var response = await _trackProgressService.SaveProgress(new KsbProgress
        {
            ProviderId = request.ProviderContext.ProviderId,
            Uln = request.Uln,
            StartDate = request.PlannedStartDate,
            CommitmentsApprenticeshipId = apprenticeship.Id,
            CommitmentsContinuationId = apprenticeship.ContinuationOfId,
            Ksbs = request!.Progress!.Progress!.Ksbs!.ToArray()
        });

        return response;
    }

    private async Task CheckForPayloadErrors(GetApprenticeshipResponse apprenticeship, TrackProgressCommand request, ApprenticeshipStatus apprenticeshipStatus)
    {
        RecordPayloadFormatErrors(request.Progress);
        RecordDuplicateIdErrors(request.Progress!.Progress!.Ksbs!);

        RecordApprenticeshipStateCanAcceptTrackProgressUpdateErrors(apprenticeship, apprenticeshipStatus);

        await ValidateKsbIdsAgainstCourseKsbs(apprenticeship.StandardUId, apprenticeship.Option, request.Progress!.Progress!.Ksbs!);

        if (_errors.Any())
            throw new InvalidTaxonomyRequestException(ErrorTitleForProgressBody, _errors);
    }

    private async Task ValidateKsbIdsAgainstCourseKsbs(string standardUId, string? option, List<ProgressDto.Ksb> ksbs)
    {
        option = string.IsNullOrWhiteSpace(option) ? "core" : option;
        var courseKsbsResponse = await _coursesService.GetKsbsForCourseOption(standardUId, option);

        var ksbMatches = from ksb in ksbs
             where ksb.Id != null                    
             join courseKsb in courseKsbsResponse.Ksbs on ksb.Id!.ToLower() equals courseKsb.Id.ToString().ToLower()
             into joinedKsbs
             from match in joinedKsbs.DefaultIfEmpty()
             select new
             {
                 ksb.Id,
                 Matched = match != null
             };

        foreach (var missingKsb in ksbMatches.Where(x => !x.Matched))
        {
            _errors.Add(new(missingKsb.Id, "This KSB does not match the course option"));
        }
    }

    private async Task ValidateCourseOptions(string standardUId)
    {
        var course = await _coursesService.GetOptionsForCourse(standardUId);

        if (course.Options.Any())
            throw new InvalidTaxonomyRequestException("This apprenticeship requires an option to be set to record progress against it");
    }

    private void RecordPayloadFormatErrors(ProgressDto? payload)
    {
        if (payload == null || payload.Progress == null)
            throw new InvalidTaxonomyRequestException("Progress must be present");

        if (payload.Progress.Ksbs == null || !payload.Progress.Ksbs.Any())
            throw new InvalidTaxonomyRequestException("KSBs are required");

        var progress = payload.Progress;

        if (progress.Ksbs.Any(x => x.Id == null))
            _errors.Add(new ErrorDetail("KSBs", "KSB Ids cannot be null"));

        var KsbStates = progress.Ksbs.Where(x => x.Id != null).Select(x => new
        { x.Id, IsValidId = Guid.TryParse(x.Id, out _), x.Value, IsValidValue = x.Value >= 0 && x.Value <= 100 });

        foreach (var ksbState in KsbStates)
        {
            if (ksbState.IsValidId && ksbState.IsValidValue)
                continue;
            if (!ksbState.IsValidId)
                _errors.Add(new ErrorDetail(ksbState.Id!, $"{ksbState.Id} is not a valid guid"));
            if (!ksbState.IsValidValue)
                _errors.Add(new ErrorDetail(ksbState.Id!, $"The progress value ({ksbState.Value}) associated with this KSB must be in the range of 0 to 100 (inclusive)"));
        }
    }

    private void RecordDuplicateIdErrors(List<ProgressDto.Ksb> ksbs)
    {
        var groupIds = ksbs.Where(x=>x.Id != null).GroupBy(x => x.Id).Where(g => g.Count() > 1);

        foreach (var group in groupIds)
        {
            _errors.Add(new ErrorDetail(group.Key!,
                $"Ensure that there are no duplicate GUIDs in the progress submission"));
        }
    }

    private void CheckTheApprenticeshipHasBeenFound(GetApprenticeshipsResponse apprenticeships)
    {
        var errors = new List<ErrorDetail>();

        if (apprenticeships.TotalApprenticeshipsFound == 0)
            throw new ApprenticeshipNotFoundException();

        apprenticeships.Apprenticeships?.RemoveAll(x => x.StartDate == x.StopDate);

        if (apprenticeships.Apprenticeships?.Count == 0)
        {
            _errors.Add(new ErrorDetail("ApprenticeshipStatus", "Apprentice status must be Live, Paused, Stopped (provided some delivery took place) or Complete."));
            throw new InvalidTaxonomyRequestException(ErrorTitleForProgressBody, _errors);
        }
    }

    private void RecordApprenticeshipStateCanAcceptTrackProgressUpdateErrors(GetApprenticeshipResponse apprenticeship, ApprenticeshipStatus apprenticeshipStatus)
    {
        if (apprenticeship?.DeliveryModel != DeliveryModel.PortableFlexiJob)
            _errors.Add(new ErrorDetail("DeliveryModel", "Must be a portable flexi-job"));

        if (apprenticeshipStatus == ApprenticeshipStatus.WaitingToStart)
            _errors.Add(new ErrorDetail("ApprenticeshipStatus", "Apprentice status must be Live, Paused, Stopped (provided some delivery took place) or Complete."));
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