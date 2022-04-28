using MediatR;
using SFA.DAS.ApprenticeFeedback.Domain;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ApprenticeFeedback.Application.Commands.CreateApprenticeFeedback
{
    public class CreateApprenticeFeedbackCommand : IRequest<CreateApprenticeFeedbackResponse>
    {
        public Guid ApprenticeFeedbackTargetId { get; set; }
        public OverallRating OverallRating { get; set; }
        public bool AllowContact { get; set; }

        public List<FeedbackAttribute> FeedbackAttributes { get; set; }

        public class FeedbackAttribute
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public FeedbackAttributeStatus Status { get; set; }
        }
    }
}
