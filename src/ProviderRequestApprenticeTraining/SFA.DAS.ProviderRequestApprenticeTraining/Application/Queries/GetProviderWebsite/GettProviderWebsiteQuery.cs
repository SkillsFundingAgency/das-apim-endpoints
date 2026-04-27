using MediatR;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetProviderWebsite
{
    public class GetProviderWebsiteQuery : IRequest<GetProviderWebsiteResult>
    {
        public long Ukprn { get; set; }
    }
}
