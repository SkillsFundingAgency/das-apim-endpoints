using System.Collections.Generic;
using MediatR;
using SFA.DAS.Recruit.Enums;

namespace SFA.DAS.Recruit.Application.Queries.GetDashboardVacanciesCountByUkprn;

public record GetDashboardVacanciesCountByUkprnQuery(int Ukprn,
    int PageNumber = 1,
    int PageSize = 25,
    string SortColumn = "CreatedDate",
    bool IsAscending = false,
    List<ApplicationReviewStatus>? Status = null) : IRequest<GetDashboardVacanciesCountByUkprnQueryResult>;
