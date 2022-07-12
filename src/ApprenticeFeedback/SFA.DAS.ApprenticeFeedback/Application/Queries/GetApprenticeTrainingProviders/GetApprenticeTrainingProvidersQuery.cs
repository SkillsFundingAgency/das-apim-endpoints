using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.ApprenticeFeedback.Application.Queries.GetApprenticeTrainingProviders
{
    public class GetApprenticeTrainingProvidersQuery : IRequest<GetApprenticeTrainingProvidersResult>
    {
        public Guid ApprenticeId { get; set; }
    }
}
