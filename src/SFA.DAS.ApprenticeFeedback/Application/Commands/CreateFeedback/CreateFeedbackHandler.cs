using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.Application.Commands.CreateFeedback
{
    public class CreateFeedbackHandler : IRequestHandler<CreateFeedbackCommand, CreateFeedbackResult>
    {
        public Task<CreateFeedbackResult> Handle(CreateFeedbackCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
