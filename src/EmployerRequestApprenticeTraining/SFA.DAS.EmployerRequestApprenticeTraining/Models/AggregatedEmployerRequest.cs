using SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining;
using System;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Models
{
    public class AggregatedEmployerRequest
    {
        public Guid EmployerRequestId { get; set; }
        public string StandardReference { get; set; }
        public string StandardTitle { get; set; }
        public int StandardLevel { get; set; }
        public DateTime RequestedAt { get; set; }
        public DateTime? ExpiredAt { get; set; }
        public DateTime ExpiryAt { get; set; }
        public RequestStatus RequestStatus { get; set; }
        public int NumberOfResponses { get; set; }
        public int NewNumberOfResponses { get; set; }

        public static implicit operator AggregatedEmployerRequest(GetAggregatedEmployerRequestsResponse source)
        {
            return new AggregatedEmployerRequest
            {
                EmployerRequestId = source.EmployerRequestId,
                StandardReference = source.StandardReference,
                StandardTitle = source.StandardTitle,
                StandardLevel = source.StandardLevel,
                RequestedAt = source.RequestedAt,
                ExpiredAt = source.ExpiredAt,
                ExpiryAt = source.ExpiryAt,
                RequestStatus = source.RequestStatus,
                NumberOfResponses = source.NumberOfResponses,
                NewNumberOfResponses = source.NewNumberOfResponses
            };
        }
    }
}
