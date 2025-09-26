using System;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.UpdateEmploymentLocations
{
    public record UpdateEmploymentLocationsCommandResult
    {
        public Domain.Models.Application Application { get; set; }
        public Guid Id { get; set; }
    }
}