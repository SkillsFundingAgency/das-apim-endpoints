using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.FindAnApprenticeship.Api.Models;

public class LevelApiResponse
{
    public static implicit operator LevelApiResponse(GetLevelsListItem source)
    {
        return new LevelApiResponse
        {
            Name = source.Name,
            Id = source.Code
        };
    }
    public string Name { get; set; }
    public int Id { get; set; }
}