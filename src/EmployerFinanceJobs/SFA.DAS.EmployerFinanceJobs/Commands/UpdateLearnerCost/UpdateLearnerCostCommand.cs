using MediatR;

namespace SFA.DAS.EmployerFinanceJobs.Commands.UpdateLearnerCost;

public sealed record UpdateLearnerCostCommand : IRequest<UpdateLearnerCostCommandResult>;