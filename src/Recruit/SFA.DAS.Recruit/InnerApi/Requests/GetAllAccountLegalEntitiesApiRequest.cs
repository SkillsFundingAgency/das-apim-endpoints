using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Interfaces;
using static SFA.DAS.Recruit.InnerApi.Requests.GetAllAccountLegalEntitiesApiRequest;

namespace SFA.DAS.Recruit.InnerApi.Requests
{
    public record GetAllAccountLegalEntitiesApiRequest(GetAllAccountLegalEntitiesApiRequestData Payload) : IPostApiRequest
    {
        public string PostUrl => "api/accountlegalentities/GetAll";
        public object Data { get; set; } = Payload;

        public record GetAllAccountLegalEntitiesApiRequestData
        {
            public string SearchTerm { get; set; } = string.Empty;
            public int PageNumber { get; set; } = 1;
            public int PageSize { get; set; } = 10;
            public string SortColumn { get; set; } = "Name";
            public bool IsAscending { get; set; } = false;
            public List<long> AccountIds { get; set; } = [];
        }
    }
}