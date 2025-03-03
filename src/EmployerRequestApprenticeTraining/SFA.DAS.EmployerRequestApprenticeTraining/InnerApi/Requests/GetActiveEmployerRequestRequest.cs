using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Requests
{
    public class GetActiveEmployerRequestRequest : IGetApiRequest
    {
        public long? AccountId { get; set; }
        public string StandardReference { get; set; }

        public GetActiveEmployerRequestRequest(long? accountId, string standardReference)
        {
            AccountId = accountId;
            StandardReference = standardReference;
        }

        public string GetUrl => $"api/accounts/{AccountId}/standard/{StandardReference}/employer-request/active";
    }
}
