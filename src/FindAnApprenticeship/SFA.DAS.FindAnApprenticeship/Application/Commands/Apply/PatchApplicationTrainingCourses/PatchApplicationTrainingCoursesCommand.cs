using System;
using MediatR;
using SFA.DAS.FindAnApprenticeship.Models;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.PatchApplicationTrainingCourses;
public class PatchApplicationTrainingCoursesCommand : IRequest<PatchApplicationTrainingCoursesCommandResponse>
{
    public Guid ApplicationId { set; get; }
    public Guid CandidateId { set; get; }
    public SectionStatus TrainingCoursesStatus { get; set; }
}
