using MediatR;

namespace SFA.DAS.EmployerAccounts.Application.Queries.GetEnglishFractionHistory
{
    public class GetEnglishFractionHistoryQuery : IRequest<GetEnglishFractionHistoryQueryResult>
    {
        public string HashedAccountId { get; set; }
        public string EmpRef { get; set; }
    }
}