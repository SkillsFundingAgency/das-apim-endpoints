using MediatR;

namespace SFA.DAS.EmployerAccounts.Application.Queries.GetEnglishFractionCurrent
{
    public class GetEnglishFractionCurrentQuery : IRequest<GetEnglishFractionCurrentQueryResult>
    {
        public string HashedAccountId { get; set; }
        public string[] EmpRefs { get; set; }
    }
}