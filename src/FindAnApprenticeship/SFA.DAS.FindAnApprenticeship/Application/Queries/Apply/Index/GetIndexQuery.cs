using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.Index
{
    public class GetIndexQuery : IRequest<GetIndexQueryResult>
    {
        public string ApplicantEmailAddress { get; set; }
        public string VacancyReference { get; set; }
    }

    public class GetIndexQueryResult
    {
        public string VacancyTitle { get; set; }
    }

    public class GetIndexQueryHandler : IRequestHandler<GetIndexQuery,GetIndexQueryResult>
    {
        public GetIndexQueryHandler(IMediator mediator, ILogger<GetIndexQueryHandler> logger)
        {
            
        }

        public Task<GetIndexQueryResult> Handle(GetIndexQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new GetIndexQueryResult());
        }
    }

}
