using MediatR;
using System;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.ApprenticeApp.Application.Commands.Commitments
{
    [ExcludeFromCodeCoverage]
    public class ConfirmApprenticeshipPatchCommand : IRequest<Unit>
    {
        public object Patch { get; set; }
        public Guid ApprenticeId { get; set; }
        public long ApprenticeshipId { get; set; }
        public long RevisionId { get; set; }
    }
}
