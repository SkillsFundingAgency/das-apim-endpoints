using MediatR;

namespace SFA.DAS.RecruitQa.Application.Dashboard.Queries.GetQaDashboard;

public record GetQaDashboardQuery : IRequest<GetQaDashboardQueryResult>;