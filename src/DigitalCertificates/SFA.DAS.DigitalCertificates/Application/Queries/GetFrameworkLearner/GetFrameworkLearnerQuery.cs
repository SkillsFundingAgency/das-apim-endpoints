using MediatR;
using System;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetFrameworkLearner
{
    public class GetFrameworkLearnerQuery : IRequest<GetFrameworkLearnerQueryResult>
    {
        public Guid FrameworkLearnerId { get; set; }

        public GetFrameworkLearnerQuery(Guid frameworkLearnerId)
        {
            FrameworkLearnerId = frameworkLearnerId;
        }
    }
}
