using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Applications.GetLegacyApplications
{
    public record GetLegacyApplicationsQuery : IRequest<GetLegacyApplicationsQueryResult>
    {
        public string EmailAddress { get; set; }
    }
}
