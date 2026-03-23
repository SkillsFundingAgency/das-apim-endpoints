using System;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.LearnerData;

public class CreateShortCoursePostResponse
{
    public Guid LearningKey { get; set; }
    public Guid EpisodeKey { get; set; }
}
