using MediatR;
using System;

namespace SFA.DAS.ApprenticeApp.Application.Queries.ApprenticeCommitments
{
    public class GetApprenticeApprenticeshipsQuery : IRequest<GetApprenticeApprenticeshipsQueryResult>
    {
        public Guid ApprenticeId { get; set; }
    }
}
