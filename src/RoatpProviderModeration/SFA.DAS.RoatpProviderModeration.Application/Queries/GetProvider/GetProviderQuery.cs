using MediatR;

namespace SFA.DAS.RoatpProviderModeration.Application.Queries.GetProvider
{
    public class GetProviderQuery : IRequest<GetProviderQueryResponse>
    {
        public int Ukprn { get; set; }
    }

    public class GetProviderQueryResponse
    {

    }
}
