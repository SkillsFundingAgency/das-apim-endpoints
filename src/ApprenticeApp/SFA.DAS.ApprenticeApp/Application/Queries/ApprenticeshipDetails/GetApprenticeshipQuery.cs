using MediatR;

namespace SFA.DAS.ApprenticeApp.Application.Queries.ApprenticeshipDetails
{
    public class GetApprenticeshipQuery : IRequest<GetApprenticeshipQueryResult>
    {
        public long ApprenticeshipId { get; set; }
    }
}