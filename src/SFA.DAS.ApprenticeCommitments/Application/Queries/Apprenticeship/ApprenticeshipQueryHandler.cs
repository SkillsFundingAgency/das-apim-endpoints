using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApprenticeCommitments.Apis.InnerApi;
using SFA.DAS.ApprenticeCommitments.Application.Services;

namespace SFA.DAS.ApprenticeCommitments.Application.Queries.Apprenticeship
{
    public class ApprenticeshipQueryHandler : IRequestHandler<ApprenticeshipQuery, ApprenticeshipResponse>
    {
        private readonly ApprenticeCommitmentsService _apprenticeCommitmentsService;

        public ApprenticeshipQueryHandler(ApprenticeCommitmentsService apprenticeCommitmentsService)
        {
            _apprenticeCommitmentsService = apprenticeCommitmentsService;
        }

        public Task<ApprenticeshipResponse> Handle(
            ApprenticeshipQuery command,
            CancellationToken cancellationToken)
        {
            return _apprenticeCommitmentsService.GetApprenticeship(command.ApprenticeId, command.ApprenticeshipId);
        }
    }

    public class ApprenticeshipQuery : IRequest<ApprenticeshipResponse>
    {
        public ApprenticeshipQuery(Guid apprenticeId, long apprenticeshipId)
            => (ApprenticeId, ApprenticeshipId) = (apprenticeId, apprenticeshipId);

        public Guid ApprenticeId { get; }
        public long ApprenticeshipId { get; }
    }
}