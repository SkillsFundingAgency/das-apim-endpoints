using MediatR;
using System;

namespace SFA.DAS.ApprenticeApp.Application.Commands.Commitments
{
    public class ConfirmApprenticeshipPatchCommand : IRequest<Unit>
    {
        public object Patch { get; set; }
        public Guid ApprenticeId { get; set; }
        public long ApprenticeshipId { get; set; }
        public long RevisionId { get; set; }
    }
}
