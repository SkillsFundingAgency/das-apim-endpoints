using MediatR;

namespace SFA.DAS.Forecasting.Application.Approvals.Queries.GetApprenticeships
{
    public class GetApprenticeshipsQuery : IRequest<GetApprenticeshipsQueryResult>
    {
        public long AccountId { get; set; }
        public string Status { get; set; }
        public int PageNumber { get; set; }
        public int PageItemCount { get; set; }
    }
}
