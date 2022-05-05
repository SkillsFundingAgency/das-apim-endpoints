using SFA.DAS.ApprenticeFeedback.Application.Commands.CreateApprenticeFeedback;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using static SFA.DAS.ApprenticeFeedback.Models.Enums;

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
        public Guid ApprenticeFeedbackTargetId { get; set; }
        public OverallRating OverallRating { get; set; }
        public bool AllowContact { get; set; }
        public List<FeedbackAttribute> FeedbackAttributes { get; set; }
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
