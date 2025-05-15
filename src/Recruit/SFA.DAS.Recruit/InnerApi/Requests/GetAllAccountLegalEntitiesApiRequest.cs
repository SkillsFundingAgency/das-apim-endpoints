using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Requests
{
    public record GetAllAccountLegalEntitiesApiRequest(long AccountId, int PageNumber, int PageSize, string SortColumn, bool IsAscending) : IGetApiRequest
    {
        public string GetUrl => $"api/accounts/{AccountId}/legalentities/getall?pageNumber={PageNumber}&pageSize={PageSize}&sortColumn={SortColumn}&isAscending={IsAscending}";
    }
}