using MediatR;

namespace SFA.DAS.ProviderFeedback.Application.Queries.GetProvider
{
    public class GetRoatpV2ProviderQuery : IRequest<bool>
    {
        public int Ukprn { get; set; }
    }
}