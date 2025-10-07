using MediatR;

namespace SFA.DAS.Approvals.Application.Providers.Queries;

    public class GetRoatpV2ProviderStatusQuery :  IRequest<GetRoatpV2ProviderStatusQueryResult>
    {
        public int Ukprn { get; set; }
    }

