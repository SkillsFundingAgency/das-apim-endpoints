using System;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.CreateTrainingCourse;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications;

public class PostTrainingCourseApiResponse
{
    public Guid Id { get; set; }

    public static implicit operator PostTrainingCourseApiResponse(CreateTrainingCourseCommandResult source)
    {
        return new PostTrainingCourseApiResponse
        {
            Id = source.Id
        };
    }
}
