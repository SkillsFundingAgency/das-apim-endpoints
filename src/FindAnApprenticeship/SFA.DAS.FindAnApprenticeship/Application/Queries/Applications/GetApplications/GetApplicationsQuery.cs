using System;
using MediatR;
using SFA.DAS.FindAnApprenticeship.Models;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Applications.GetApplications
{
    public class GetApplicationsQuery : IRequest<GetApplicationsQueryResult>
    {
        public Guid CandidateId { get; set; }
        public ApplicationStatus Status { get; set; }
    }
}
