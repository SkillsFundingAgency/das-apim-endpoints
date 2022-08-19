using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.TrackProgress.Apis.CommitmentsV2InnerApi;
using SFA.DAS.TrackProgress.Application.Services;

namespace SFA.DAS.TrackProgress.Application.Commands;

public class TrackProgressCommandHandler : IRequestHandler<TrackProgressCommand, TrackProgressResponse>
{
    private readonly CommitmentsV2Service _commitmentsService;
    private readonly CoursesService _coursesService;
    private readonly ILogger<TrackProgressCommandHandler> _logger;

    public TrackProgressCommandHandler(CommitmentsV2Service commitmentsV2Service, CoursesService coursesService, ILogger<TrackProgressCommandHandler> logger)
    {
        _commitmentsService = commitmentsV2Service;
        _coursesService = coursesService;
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

        var app = apprenticeshipsReponse.Apprenticeships!.First();
        ValidateApprenticeshipStateCanAcceptTrackProgressUpdates(app);
        

        if (request.ProviderContext.InSandboxMode)
        {
            // Any calls to add the progress record should be avoided when in Sandbox Mode
            return new TrackProgressResponse();
        }

        return new TrackProgressResponse();
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

    private void ValidateApprenticeshipStateCanAcceptTrackProgressUpdates(GetApprenticeshipsResponse.ApprenticeshipDetails apprenticeship)
    {
        var errors = new List<ErrorDetail>();

        if (apprenticeship?.DeliveryModel != DeliveryModel.PortableFlexiJob)
            errors.Add(new ErrorDetail("DeliveryModel", "Must be a portable flexi-job"));

        if (apprenticeship?.ApprenticeshipStatus == ApprenticeshipStatus.WaitingToStart)
            errors.Add(new ErrorDetail("ApprenticeshipStatus", "Apprenticeship cannot be updated if not started"));

        if (errors.Any())
            throw new InvalidTaxonomyRequestException("This apprenticeship cannot accept the taxonomy as it's in an incorrect state", errors);
    }

    private void CheckTheApprenticeshipCanAcceptTrackProgressUpdates(GetApprenticeshipsResponse apprenticeships)
    {
        if (apprenticeships.TotalApprenticeshipsFound == 0)
            throw new ApprenticeshipNotFoundException();

        if (apprenticeships.TotalApprenticeshipsFound > 1)
            apprenticeships.Apprenticeships?.RemoveAll(x => x.StartDate == x.StopDate);

        if (apprenticeships.Apprenticeships?.Count == 0)
            throw new ApprenticeshipNotFoundException();
    }
}

public class ApprenticeshipNotFoundException : Exception
{
}

public class InvalidTaxonomyRequestException : Exception
{
    public List<ErrorDetail> Errors { get; } = new ();

    public InvalidTaxonomyRequestException(string errorTitle) : base(errorTitle)
    {
        
    }

    public InvalidTaxonomyRequestException(string errorTitle, List<ErrorDetail> errors) : this(errorTitle)
    {
        Errors = errors;
    }

}

public record ErrorDetail(
    string error,
    string description
);

