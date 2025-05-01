using System;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseProvider;

public sealed class GetCourseProviderRequest
{
    public string Location { get; set; }
    public int? Distance { get; set; }
    public Guid ShortlistUserId { get; set; }
}
