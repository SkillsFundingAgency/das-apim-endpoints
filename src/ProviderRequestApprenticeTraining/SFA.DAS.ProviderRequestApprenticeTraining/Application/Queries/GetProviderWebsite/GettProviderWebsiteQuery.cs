using MediatR;
using System;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetProviderWebsite
{
    public class GetProviderWebsiteQuery : IRequest<GetProviderWebsiteResult>
    {
        public long Ukprn { get; set; }
    }
}
