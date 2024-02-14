using MediatR;
using SFA.DAS.FindAnApprenticeship.Models;
using System;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.PatchApplication
{
    public class PatchApplicationWorkHistoryCommand : IRequest<PatchApplicationWorkHistoryCommandResponse>
    {
        public Guid ApplicationId { set; get; }
        public Guid CandidateId { set; get; }
        public SectionStatus WorkExperienceStatus { get; set; }
        public SectionStatus JobsStatus { get; set; }
    }
}
