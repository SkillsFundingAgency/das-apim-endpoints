using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Courses;

namespace SFA.DAS.FindAnApprenticeship.Api.Models;

public class RouteApiResponse
{
    public static implicit operator RouteApiResponse(GetRoutesListItem source)
    {
        return new RouteApiResponse
        {
            Name = source.Name,
            Id = source.Id
        };
    }
    public string Name { get; set; }
    public int Id { get; set; }
}