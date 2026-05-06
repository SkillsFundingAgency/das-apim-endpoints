using MediatR;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Queries.CivilServiceJobs;

public record GetCivilServiceJobsQuery : IRequest<GetCivilServiceJobsQueryResult>;