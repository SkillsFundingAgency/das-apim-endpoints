using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApprenticeCommitments.Apis.InnerApi;
using SFA.DAS.ApprenticeCommitments.Application.Services;

namespace SFA.DAS.ApprenticeCommitments.Application.Queries.Apprenticeship
{
    public class CurrentApprenticeshipQueryHandler : IRequestHandler<CurrentApprenticeshipQuery, CurrentApprenticeshipResponse>
    {
        private readonly ApprenticeCommitmentsService _apprenticeCommitmentsService;

        public CurrentApprenticeshipQueryHandler(ApprenticeCommitmentsService apprenticeCommitmentsService)
        {
            _apprenticeCommitmentsService = apprenticeCommitmentsService;
        }

        public Task<CurrentApprenticeshipResponse> Handle(
            CurrentApprenticeshipQuery command,
            CancellationToken cancellationToken)
        {
            return _apprenticeCommitmentsService.GetCurrentApprenticeship(command.ApprenticeId);
        }
    }

    public class CurrentApprenticeshipQuery : IRequest<CurrentApprenticeshipResponse>
    {
        public CurrentApprenticeshipQuery(Guid apprenticeId) => ApprenticeId = apprenticeId;

        public Guid ApprenticeId { get; }
    }
}