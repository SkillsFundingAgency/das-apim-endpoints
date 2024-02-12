using System;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.CreateTrainingCourse;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications;

public class PostTrainingCourseApiRequest
{
    public Guid CandidateId { get; set; }
    public string CourseName { get; set; }
    public string TrainingProviderName { get; set; }
    public int YearAchieved { get; set; }
}

public class PostTrainingCourseApiResponse
{
    public Guid Id { get; set; }

    public static implicit operator PostTrainingCourseApiResponse(CreateTrainingCourseCommandResponse source)
    {
        return new PostTrainingCourseApiResponse
        {
            Id = source.Id
        };
    }
}
