using MediatR;

namespace SFA.DAS.EmployerFinanceJobs.Commands.PostJob;

public sealed record PostJobCommand(long Id, PostJobCommandData Data) : IRequest<Unit>;

public sealed record PostJobCommandData(
    long AccountId,
    string JobName,
    string JobState,
    DateTime LastSuccessfulImportDate,
    DateTime LastAttemptedDate,
    bool LastAttemptSuccessful,
    int TotalRecordsLastRun,
    int FailedRecordsLastRun);
