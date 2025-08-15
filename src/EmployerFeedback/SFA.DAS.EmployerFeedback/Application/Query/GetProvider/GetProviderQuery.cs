using MediatR;

namespace SFA.DAS.EmployerFeedback.Application.Queries.GetProvider
{
    public class GetProviderQuery : IRequest<GetProviderQueryResult>
    {
        public int ProviderId { get; set; }
    }
}