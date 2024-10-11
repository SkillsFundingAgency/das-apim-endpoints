using MediatR;

namespace SFA.DAS.EarlyConnect.Application.Queries.GetEducationalOrganisationsByLepCode
{
    public class GetEducationalOrganisationsByLepCodeQuery : IRequest<GetEducationalOrganisationsByLepCodeResult>
    {
        public string LepCode { get; set; } = string.Empty;
        public string? SearchTerm { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}