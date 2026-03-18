using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Courses;

namespace SFA.DAS.FindAnApprenticeship.Api.Models;

public class LevelApiResponse
{
    public static implicit operator LevelApiResponse(GetCourseLevelsListItem source)
    {
        return new LevelApiResponse
        {
            Name = source.Name,
            Code = source.Code
        };
    }
    public string Name { get; set; }
    public int Code { get; set; }
}