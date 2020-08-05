using System;
using SFA.DAS.EmployerIncentives.Models.EmployerIncentives;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests
{
    public class PostCreateIncentiveApplicationRequest : IPostApiRequest
    {
        public string BaseUrl { get; set; }
        public string PostUrl => $"{BaseUrl}applications";
        public object Data { get; set; }
    }

    public class CreateIncentiveApplication
    {
        public Guid IncentiveApplicationId { get; set; }
        public long AccountId { get; set; }
        public long AccountLegalEntityId { get; set; }
        public IncentiveClaimApprenticeshipDto[] Apprenticeships { get; set; }
    }
}
