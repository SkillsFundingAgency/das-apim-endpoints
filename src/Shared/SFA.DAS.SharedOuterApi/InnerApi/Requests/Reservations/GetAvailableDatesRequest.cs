using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Reservations
{
    public class GetAvailableDatesRequest : IGetApiRequest
    {
        public long AccountLegalEntityId { get; }
        public string GetUrl => $"api/rules/available-dates/{AccountLegalEntityId}";

        public GetAvailableDatesRequest(long accountLegalEntityId)
        {
            AccountLegalEntityId = accountLegalEntityId;
        }
    }
}