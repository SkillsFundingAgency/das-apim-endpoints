using MediatR;
using SFA.DAS.ApprenticeFeedback.Domain;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ApprenticeFeedback.Application.Commands.CreateApprenticeFeedback
{
    public class CreateApprenticeFeedbackCommand : IRequest<CreateApprenticeFeedbackResponse>
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

        public class FeedbackAttribute
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public FeedbackAttributeStatus Status { get; set; }
        }
    }
}
