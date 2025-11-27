using SFA.DAS.EmployerFeedback.Application.Commands.SubmitEmployerFeedback;
using SFA.DAS.EmployerFeedback.Models;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.EmployerFeedback.InnerApi.Requests
{
    public class SubmitEmployerFeedbackRequest : IPostApiRequest<SubmitEmployerFeedbackRequestData>
    {
        public string PostUrl => "api/employerfeedbackresult";
        public SubmitEmployerFeedbackRequestData Data { get; set; }
        public SubmitEmployerFeedbackRequest(SubmitEmployerFeedbackRequestData data)
        {
            Data = data;
        }
    }
    public class SubmitEmployerFeedbackRequestData
    {
        public Guid UserRef { get; set; }
        public long Ukprn { get; set; }
        public long AccountId { get; set; }
        public string ProviderRating { get; set; }
        public int FeedbackSource { get; set; }
        public List<ProviderAttributeData> ProviderAttributes { get; set; }
        public static implicit operator SubmitEmployerFeedbackRequestData(SubmitEmployerFeedbackCommand source)
        {
            return new SubmitEmployerFeedbackRequestData
            {
                UserRef = source.UserRef,
                Ukprn = source.Ukprn,
                AccountId = source.AccountId,
                ProviderRating = source.ProviderRating.ToString(),
                FeedbackSource = source.FeedbackSource,
                ProviderAttributes = source.ProviderAttributes?
                    .Select(x => (ProviderAttributeData)x)
                    .ToList()
            };
        }
    }
    public class ProviderAttributeData
    {
        public long AttributeId { get; set; }
        public int AttributeValue { get; set; }
        public static implicit operator ProviderAttributeData(ProviderAttributeDto source)
        {
            return new ProviderAttributeData
            {
                AttributeId = source.AttributeId,
                AttributeValue = source.AttributeValue,
            };
        }
    }
}
