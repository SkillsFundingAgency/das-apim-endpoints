using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.DeleteQualifications
{
    public class DeleteQualificationsCommand : IRequest
    {
        public Guid ApplicationId { get; set; }
        public Guid CandidateId { get; set; }
        public Guid QualificationReferenceId { get; set; }
    }

    public class DeleteQualificationsCommandHandler : IRequestHandler<DeleteQualificationsCommand>
    {
        public Task Handle(DeleteQualificationsCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
