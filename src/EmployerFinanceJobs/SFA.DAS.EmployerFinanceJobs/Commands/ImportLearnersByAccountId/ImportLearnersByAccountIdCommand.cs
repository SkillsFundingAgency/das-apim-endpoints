using MediatR;

namespace SFA.DAS.EmployerFinanceJobs.Commands.ImportLearnersByAccountId;

public sealed record ImportLearnersByAccountIdCommand(long AccountId) : IRequest<ImportLearnersByAccountIdResult>;