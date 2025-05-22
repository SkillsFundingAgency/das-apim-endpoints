using System;
using System.Collections.Generic;

namespace SFA.DAS.Recruit.Api.Models
{
    public record GetAllApplicationsByIdApiRequest
    {
        public required List<Guid> ApplicationIds { get; init; } = [];
        public bool IncludeDetails { get; set; } = false;
    }
}
