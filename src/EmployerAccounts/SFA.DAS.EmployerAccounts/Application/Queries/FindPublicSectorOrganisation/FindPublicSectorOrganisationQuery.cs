using MediatR;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.EmployerAccounts.Application.Queries.FindPublicSectorOrganisation
{
    public class FindPublicSectorOrganisationQuery : IRequest<PagedResponse<FindPublicSectorOrganisationResult>>
    {
        public string SearchTerm { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}