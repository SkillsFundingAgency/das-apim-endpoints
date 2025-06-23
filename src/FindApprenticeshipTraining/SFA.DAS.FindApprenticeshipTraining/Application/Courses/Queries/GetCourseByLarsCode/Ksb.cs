using SFA.DAS.SharedOuterApi.InnerApi.Responses.Courses;
using System;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseByLarsCode;

public class Ksb
{
    public string Type { get; set; }
    public Guid Id { get; set; }
    public string Detail { get; set; }


    public static explicit operator Ksb(KsbResponse source)
    {
        return new Ksb
        {
            Type = source.Type,
            Id = source.Id,
            Detail = source.Description,
        };
    }
}