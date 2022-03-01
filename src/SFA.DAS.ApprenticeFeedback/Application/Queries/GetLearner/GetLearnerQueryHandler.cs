using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.Application.Queries.GetLearner
{
    public class GetLearnerQueryHandler : IRequestHandler<GetLearnerQuery, GetLearnerResult>
    {
        public Task<GetLearnerResult> Handle(GetLearnerQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
