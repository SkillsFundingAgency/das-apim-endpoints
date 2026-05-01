using MediatR;
using System;
using System.Collections.Generic;
using SFA.DAS.EmployerFeedback.Models;

namespace SFA.DAS.EmployerFeedback.Application.Commands.SubmitEmployerFeedback
{
    public class SubmitEmployerFeedbackCommand : IRequest
    {
        public Guid UserRef { get; set; }
        public long Ukprn { get; set; }
        public long AccountId { get; set; }
        public OverallRating ProviderRating { get; set; }
        public int FeedbackSource { get; set; }
        public List<ProviderAttributeDto> ProviderAttributes { get; set; }

        public static implicit operator SubmitEmployerFeedbackCommand(SubmitEmployerFeedbackRequest source)
        {
            return new SubmitEmployerFeedbackCommand
            {
                UserRef = source.UserRef,
                Ukprn = source.Ukprn,
                AccountId = source.AccountId,
                ProviderRating = source.ProviderRating,
                FeedbackSource = source.FeedbackSource,
                ProviderAttributes = source.ProviderAttributes
            };
        }
    }
}
