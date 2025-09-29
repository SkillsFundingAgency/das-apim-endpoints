using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.EmployerFeedback.InnerApi.Requests
{
    public class GetUpdatedEmployerAccountsRequest : IGetApiRequest
    {
        public GetUpdatedEmployerAccountsRequest(DateTime? sinceDate, int page, int pageSize)
        {
            SinceDate = sinceDate;
            PageNumber = page;
            PageSize = pageSize;
        }
        public DateTime? SinceDate { get; }
        public int PageNumber { get; }
        public int PageSize { get; }
        public string GetUrl =>
            SinceDate.HasValue
                ? $"api/accounts/update?sinceDate={SinceDate.Value.ToUniversalTime():yyyy-MM-ddTHH:mm:ss.fffffffZ}&pageNumber={PageNumber}&pageSize={PageSize}"
                : $"api/accounts/update?pageNumber={PageNumber}&pageSize={PageSize}";

    }
}
