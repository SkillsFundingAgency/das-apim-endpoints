using SFA.DAS.EmployerAccounts.Application.Queries.GetLatestDetails;

namespace SFA.DAS.EmployerAccounts.Api.Models
{
    public class GetLatestDetailsResponse
    {
        public OrganisationResponse Organisation { get; set; }
        public static implicit operator GetLatestDetailsResponse(GetLatestDetailsResult source)
        {
            return new GetLatestDetailsResponse
            {
                Organisation = (OrganisationResponse)source.OrganisationDetail
            };
        }
    }
}
