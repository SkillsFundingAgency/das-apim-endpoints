using MediatR;

namespace SFA.DAS.ApprenticeApp.Application.Queries.KsbProgress
{
    public class GetKsbsByApprenticeshipIdQuery : IRequest<GetKsbsByApprenticeshipIdQueryResult>
    {
        public long ApprenticeshipId { get; set; }
    }
}
