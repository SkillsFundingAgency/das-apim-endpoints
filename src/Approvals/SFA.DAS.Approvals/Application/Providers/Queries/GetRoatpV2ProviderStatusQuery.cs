using MediatR;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.RoatpV2;

namespace SFA.DAS.Approvals.Application.Providers.Queries;

    public class GetRoatpV2ProviderStatusQuery :  IRequest<GetRoatpV2ProviderStatusQueryResult>
    {
        public int Ukprn { get; set; }
    }

