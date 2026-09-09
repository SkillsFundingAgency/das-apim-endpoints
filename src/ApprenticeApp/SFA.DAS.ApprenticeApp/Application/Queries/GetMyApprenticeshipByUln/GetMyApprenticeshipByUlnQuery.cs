using MediatR;

namespace SFA.DAS.ApprenticeApp.Application.Queries.GetMyApprenticeshipByUln
{
    public class GetMyApprenticeshipByUlnQuery : IRequest<GetMyApprenticeshipByUlnQueryResult>
    {
        public long Uln { get; set; }
    }
}
