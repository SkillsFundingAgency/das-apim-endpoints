using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetApplication
{
    public class GetApplicationQuery : IRequest<GetApplicationResult>
    {
        public int ApplicationId { get; set; }
    }
}