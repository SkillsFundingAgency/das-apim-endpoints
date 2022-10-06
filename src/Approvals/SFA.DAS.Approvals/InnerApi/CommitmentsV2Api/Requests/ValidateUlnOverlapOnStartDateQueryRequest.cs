using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests
{
    public class ValidateUlnOverlapOnStartDateQueryRequest : IGetApiRequest
    {
        public readonly long ProviderId;
        public string Uln { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }

        public ValidateUlnOverlapOnStartDateQueryRequest(long providerId, string uln, string startDate, string endDate)
        {
            ProviderId = providerId;
            Uln = uln;
            StartDate = startDate;
            EndDate = endDate;
        }

        public string GetUrl => $"api/overlapping-training-date-request/{ProviderId}/validateUlnOverlap?uln={Uln}&startDate={StartDate}&endDate={EndDate}";
    }
}