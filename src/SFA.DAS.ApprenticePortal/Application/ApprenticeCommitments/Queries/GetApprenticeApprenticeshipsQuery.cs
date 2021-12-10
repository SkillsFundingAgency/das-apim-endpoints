using MediatR;
using System;

namespace SFA.DAS.ApprenticePortal.Application.ApprenticeCommitments.Queries
{
    public class GetApprenticeApprenticeshipsQuery : IRequest<GetApprenticeApprenticeshipsQueryResult>
    {
        public Guid ApprenticeId { get; set; }
    }
}
