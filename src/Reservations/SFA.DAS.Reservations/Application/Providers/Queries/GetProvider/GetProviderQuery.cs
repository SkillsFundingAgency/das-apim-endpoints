using MediatR;

namespace SFA.DAS.Reservations.Application.Providers.Queries.GetProvider
{
    public class GetProviderQuery : IRequest<GetProviderResult>
    {
        public int Ukprn { get; set; }
    }
}