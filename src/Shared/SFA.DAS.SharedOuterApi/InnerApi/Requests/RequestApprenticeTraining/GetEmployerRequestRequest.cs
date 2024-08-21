using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining
{
    public class GetEmployerRequestRequest : IGetApiRequest
    {
        public Guid? EmployerRequestId { get; set; }
        public long? AccountId { get; set; }
        public string StandardReference { get; set; }

        public GetEmployerRequestRequest(Guid employerRequestId)
        {
            EmployerRequestId = employerRequestId;
        }

        public GetEmployerRequestRequest(long? accountId, string standardReference)
        {
            AccountId = accountId;
            StandardReference = standardReference;
        }

        public string GetUrl
        { 
            get
            {
                if (EmployerRequestId.HasValue)
                {
                    return $"api/employerrequest/{EmployerRequestId}";
                }
                else if (AccountId.HasValue && !string.IsNullOrEmpty(StandardReference))
                {
                    return $"api/employerrequest/account/{AccountId}/standard/{StandardReference}";
                }

                return string.Empty;
            }
        }
    }
}
