using MediatR;

namespace SFA.DAS.EmployerFinanceJobs.Commands.RecalculateFundingProjection;

public sealed record RecalculateFundingProjectionCommand : IRequest<RecalculateFundingProjectionCommandResult>;