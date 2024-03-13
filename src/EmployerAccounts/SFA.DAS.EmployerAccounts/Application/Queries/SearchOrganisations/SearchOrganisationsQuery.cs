using MediatR;

namespace SFA.DAS.EmployerAccounts.Application.Queries.SearchOrganisations
{
    public class SearchOrganisationsQuery : IRequest<SearchOrganisationsResult>
    {
        public string SearchTerm { get; set; }
        public int MaximumResults { get; set; }
        public int Version { get; set; } = 0;
    }
}

