using MediatR;

namespace SFA.DAS.EmployerAccounts.Application.Queries.GetCreateAccountTaskList;

public record GetCreateAccountTaskListQuery(long AccountId, string HashedAccountId, string UserRef) : IRequest<GetCreateAccountTaskListQueryResponse>;