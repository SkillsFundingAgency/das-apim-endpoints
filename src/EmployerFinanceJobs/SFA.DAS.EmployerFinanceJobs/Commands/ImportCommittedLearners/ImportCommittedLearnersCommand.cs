using MediatR;

namespace SFA.DAS.EmployerFinanceJobs.Commands.ImportCommittedLearners;

public sealed record ImportCommittedLearnersCommand(DateTime CutOffDateTime)
    : IRequest<ImportCommittedLearnersCommandResult>;