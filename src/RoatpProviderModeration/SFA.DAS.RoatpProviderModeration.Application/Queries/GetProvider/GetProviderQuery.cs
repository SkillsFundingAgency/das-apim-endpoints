using MediatR;

namespace SFA.DAS.RoatpProviderModeration.Application.Queries.GetProvider
{
    public class GetProviderQuery : IRequest<GetProviderResult>
    {
        public int Ukprn { get; }
      
        public GetProviderQuery(int ukprn)
        {
            Ukprn = ukprn;
        }
    }
}
