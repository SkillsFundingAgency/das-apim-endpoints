using MediatR;

namespace SFA.DAS.EmployerFeedback.Application.Queries.GetProvider
{
    //TODO - REMOVE - TEMPORARY - FOR TESTING PURPOSES ONLY
    public class GetProviderQuery : IRequest<GetProviderQueryResult>
    {
        public int ProviderId { get; set; }
    }
}