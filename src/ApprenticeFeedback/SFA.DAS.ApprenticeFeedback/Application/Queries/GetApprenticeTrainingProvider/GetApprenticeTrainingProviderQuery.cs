using MediatR;
using System;

namespace SFA.DAS.ApprenticeFeedback.Application.Queries.GetApprenticeTrainingProvider
{
    public class GetApprenticeTrainingProviderQuery : IRequest<GetApprenticeTrainingProviderResult>
    {
        public Guid ApprenticeId { get; set; }
        public long Ukprn { get; set; }
    }
}
