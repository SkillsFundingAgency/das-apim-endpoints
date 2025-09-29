using MediatR;
using SFA.DAS.Recruit.Enums;

namespace SFA.DAS.Recruit.Application.Queries.GetVacanciesByUkprn;
public record GetVacanciesByUkprnQuery(
    int Ukprn,
    int Page = 1,
    int PageSize = 25,
    string SortColumn = "",
    string SortOrder = "Desc",
    FilteringOptions FilterBy = FilteringOptions.All,
    string SearchTerm = "") : IRequest<GetVacanciesByUkprnQueryResult>;