using MediatR;
using SFA.DAS.TrackProgress.Application.Services;
using System.Net;

namespace SFA.DAS.TrackProgress.Application.Commands
{
    public class TrackProgressCommandHandler : IRequestHandler<TrackProgressCommand, TrackProgressResponse>
    {
        private readonly CommitmentsV2Service _commitmentsService;

        public TrackProgressCommandHandler(CommitmentsV2Service commitmentsV2Service)
            => _commitmentsService = commitmentsV2Service;  

        public async Task<TrackProgressResponse> Handle(TrackProgressCommand request, CancellationToken cancellationToken)
        {
            var apprenticeshipResult = await _commitmentsService.GetApprenticeship(request.Ukprn.Value, request.Uln, request.PlannedStartDate);
            if (apprenticeshipResult.StatusCode != HttpStatusCode.OK)
                return new TrackProgressResponse(apprenticeshipResult.StatusCode, apprenticeshipResult.ErrorContent);

            if (apprenticeshipResult.Body.TotalApprenticeshipsFound == 0)
            {
                var providerResult = await _commitmentsService.GetProvider(request.Ukprn.Value);

                if (providerResult.StatusCode == HttpStatusCode.NotFound)
                    return new TrackProgressResponse(HttpStatusCode.NotFound, "Provider not found");
                else return new TrackProgressResponse(HttpStatusCode.NotFound, "Apprenticeship not found");
            }

            if (apprenticeshipResult.Body.Apprenticeships?.Where(x => x.StartDate == request.PlannedStartDate).Count() > 1)
                return new TrackProgressResponse(HttpStatusCode.NotFound, "Multiple results for start date");

            return new TrackProgressResponse(HttpStatusCode.Created);
        }
    }
}
