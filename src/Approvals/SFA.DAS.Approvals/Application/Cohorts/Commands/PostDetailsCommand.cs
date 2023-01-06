using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.Application.Shared.Enums;
using SFA.DAS.Approvals.InnerApi.Requests;

namespace SFA.DAS.Approvals.Application.Cohorts.Commands
{
    public class PostDetailsCommand : IRequest
    {
        public CohortSubmissionType SubmissionType { get; set; }
        public string Message { get; set; }
        public UserInfo UserInfo { get; set; }
    }

    public class PostDetailsCommandHandler : IRequestHandler<PostDetailsCommand>
    {
        public async Task<Unit> Handle(PostDetailsCommand request, CancellationToken cancellationToken)
        {



            return Unit.Value;

        }
    }
}
