using MediatR;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Queries.NhsJobs;

public record GetNhsJobsQuery : IRequest<GetNhsJobsQueryResult>;