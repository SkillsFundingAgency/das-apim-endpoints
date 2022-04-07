using SFA.DAS.ApprenticeFeedback.Application.Commands.CreateApprenticeFeedback;
using SFA.DAS.ApprenticeFeedback.Domain;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ApprenticeFeedback.InnerApi.Requests
{
    public class CreateApprenticeFeedbackRequest : IPostApiRequest
    {
        public string PostUrl => "api/apprenticefeedback";

        public object Data { get; set; }

        public CreateApprenticeFeedbackRequest(CreateApprenticeFeedbackData data)
        {
            Data = data;
        }
    }

    public class CreateApprenticeFeedbackData
    {
        public Guid ApprenticeId { get; set; }
        public long Ukprn { get; set; }
        public OverallRating OverallRating { get; set; }
        public string ProviderName { get; set; }
        public int LarsCode { get; set; }
        public string StandardUId { get; set; }
        public string StandardReference { get; set; }
        public List<FeedbackAttribute> FeedbackAttributes { get; set; }
        public bool ContactConsent { get; set; }
    }

    public class FeedbackAttribute
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public FeedbackAttributeStatus Status { get; set; }

        public static implicit operator FeedbackAttribute(CreateApprenticeFeedbackCommand.FeedbackAttribute source)
        {
            return new FeedbackAttribute
            {
                Id = source.Id,
                Name = source.Name,
                Status = source.Status,
            };
        }
    }
}
