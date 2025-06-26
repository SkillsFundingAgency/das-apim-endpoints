using MediatR;
using SFA.DAS.Recruit.Enums;

namespace SFA.DAS.Recruit.Application.Queries.GetDashboardVacanciesCountByAccountId;

public record GetDashboardVacanciesCountByAccountIdQuery(long AccountId,
    int PageNumber = 1,
    int PageSize = 25,
    string SortColumn = "CreatedDate",
    bool IsAscending = false,
    ApplicationReviewStatus Status = ApplicationReviewStatus.New) : IRequest<GetDashboardVacanciesCountByAccountIdQueryResult>;
