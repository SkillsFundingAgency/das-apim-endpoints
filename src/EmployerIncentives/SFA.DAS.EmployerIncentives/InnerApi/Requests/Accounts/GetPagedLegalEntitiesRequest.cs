using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests.Accounts
{
    public class GetPagedLegalEntitiesRequest : IGetPagedApiRequest
    {
        public int PageNumber { get; }
        public int PageSize { get; }

        public GetPagedLegalEntitiesRequest(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public string GetPagedUrl => $"api/accountlegalentities?pageNumber={PageNumber}&pageSize={PageSize}";
    }
}
