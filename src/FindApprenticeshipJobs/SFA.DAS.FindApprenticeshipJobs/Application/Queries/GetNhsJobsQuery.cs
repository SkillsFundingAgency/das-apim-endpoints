using MediatR;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Queries;

public record GetNhsJobsQuery : IRequest<GetNhsJobsQueryResult>;