using System;
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.Recruit.Api.Models.Requests;

public class SortParams<T> where T : Enum
{
    [FromQuery] public SortOrder? SortOrder { get; init; }
    [FromQuery] public T? SortColumn { get; init; }
}