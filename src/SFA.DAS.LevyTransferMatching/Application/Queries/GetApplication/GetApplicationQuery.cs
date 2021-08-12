using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetApplication
{
    public class GetApplicationQuery : IRequest<GetApplicationResult>
    {
        public int Id { get; set; }
    }
}