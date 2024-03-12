using MediatR;
using System;

namespace SFA.DAS.ApprenticeFeedback.Application.Queries.GetApprenticeTrainingProviders
{
    public class GetApprenticeTrainingProvidersQuery : IRequest<GetApprenticeTrainingProvidersResult>
    {
        public Guid ApprenticeId { get; set; }
    }
}
