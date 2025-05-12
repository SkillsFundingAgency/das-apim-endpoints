using System;
using MediatR;
using SFA.DAS.FindAnApprenticeship.Domain.Models;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.UpdateEmploymentLocations
{
    public record UpdateEmploymentLocationsCommand : IRequest<UpdateEmploymentLocationsCommandResult>
    {
        public Guid ApplicationId { get; init; }
        public Guid CandidateId { get; init; }
        public InnerApi.CandidateApi.Shared.LocationDto EmployerLocation { get; init; }
        public SectionStatus EmploymentLocationSectionStatus { get; init; }
    }
}