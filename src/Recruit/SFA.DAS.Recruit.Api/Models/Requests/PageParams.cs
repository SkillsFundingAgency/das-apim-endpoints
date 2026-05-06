using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.Recruit.Api.Models.Requests;

public class PageParams
{
    [FromQuery] public ushort? PageNumber { get; init; } = 1;
    [FromQuery] public ushort? PageSize { get; init; } = 10;
}