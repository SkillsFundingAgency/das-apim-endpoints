using MediatR;

namespace SFA.DAS.ApprenticeCommitments.Application.Queries.Apprenticeships
{
    public class GetApprenticeshipByUlnQuery : IRequest<GetApprenticeshipByUlnQueryResponse>
    {
        public string Uln { get; set; }
    }
}
