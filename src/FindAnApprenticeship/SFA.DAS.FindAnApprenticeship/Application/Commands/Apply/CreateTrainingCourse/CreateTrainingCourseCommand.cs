using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.CreateTrainingCourse;
public class CreateTrainingCourseCommand : IRequest<CreateTrainingCourseCommandResponse>
{
    public Guid CandidateId { get; set; }
    public Guid ApplicationId { get; set; }
    public string CourseName { get; set; }
    public string TrainingProviderName { get; set; }
    public int YearAchieved { get; set; }
}
