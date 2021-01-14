using SFA.DAS.Approvals.InnerApi.Responses;

namespace SFA.DAS.Approvals.Api.Models
{
    public class GetProvidersResponse
    {
        public string Name { get ; set ; }
        public int Ukprn { get ; set ; }

        public static implicit operator GetProvidersResponse(GetProvidersListItem source)
        {
            return new GetProvidersResponse
            {
                Ukprn = source.Ukprn,
                Name = source.Name
            };
        }
    }
}