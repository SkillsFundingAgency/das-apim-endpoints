using MediatR;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Vacancies.Application.Vacancies.Queries
{
    public class GetVacanciesQuery: IRequest<GetVacanciesQueryResult>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int? Ukprn { get; set; }
        public string EncodedAccountId { get; set; }
        public string AccountPublicHashedId { get; set; }
        public string AccountLegalEntityPublicHashedId { get; set; }
        public AccountType? AccountType { get; set; }

    }
}
