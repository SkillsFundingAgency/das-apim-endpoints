using MediatR;
using SFA.DAS.Funding.Application.Queries.GetProviderEarningsSummary;

namespace SFA.DAS.Funding.Application.Queries.GetAllApprenticeships
{
    public class GetAllApprenticeshipsQuery : IRequest<GetAllApprenticeshipsResult>
    {
        public long Ukprn { get; set; }
    }
}