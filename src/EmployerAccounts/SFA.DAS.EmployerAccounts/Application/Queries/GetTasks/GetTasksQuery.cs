using MediatR;

namespace SFA.DAS.EmployerAccounts.Application.Queries.GetTasks
{
    public class GetTasksQuery : IRequest<GetTasksQueryResult>
    {
        public long AccountId { get; set; }
    }
}