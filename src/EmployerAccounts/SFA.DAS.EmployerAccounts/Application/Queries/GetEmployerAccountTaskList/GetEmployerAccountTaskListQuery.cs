using MediatR;

namespace SFA.DAS.EmployerAccounts.Application.Queries.GetEmployerAccountTaskList
{
    public class GetEmployerAccountTaskListQuery : IRequest<GetEmployerAccountTaskListQueryResult>
    {
        public string HashedAccountId { get; set; }
    }
}