using MediatR;

namespace SFA.DAS.Funding.Application.Queries.GetApprenticeships
{
    public class GetApprenticeshipsQuery : IRequest<GetApprenticeshipsResult>
    {
        public long Ukprn { get; set; }
    }
}