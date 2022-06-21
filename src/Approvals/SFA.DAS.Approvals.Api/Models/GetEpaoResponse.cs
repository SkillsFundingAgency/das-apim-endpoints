using SFA.DAS.Approvals.InnerApi.Responses;

namespace SFA.DAS.Approvals.Api.Models
{
    public class GetEpaoResponse
    {
        public string Name { get ; set ; }
        public string Id { get ; set ; }

        public static implicit operator GetEpaoResponse(GetEpaosListItem source)
        {
            return new GetEpaoResponse
            {
                Id = source.Id,
                Name = source.Name
            };
        }
    }
}