using MediatR;
using SFA.DAS.ApprenticeFeedback.Models;
using SFA.DAS.SharedOuterApi.Infrastructure;
using System.Collections.Generic;

namespace SFA.DAS.ApprenticeFeedback.Application.Commands.ProcessFeedbackTargetVariants
{
    public class ProcessFeedbackTargetVariantsCommand : IRequest<NullResponse>
    {
        public List<FeedbackTargetVariant> FeedbackTargetVariants { get; set; }
    }
}
