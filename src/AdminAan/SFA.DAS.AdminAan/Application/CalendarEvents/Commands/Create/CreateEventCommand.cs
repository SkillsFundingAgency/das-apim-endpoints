using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AdminAan.Infrastructure;
using SFA.DAS.AdminAan.Infrastructure.Configuration;

namespace SFA.DAS.AdminAan.Application.CalendarEvents.Commands.Create;
public class CreateEventCommand : IRequest<CreateEventCommandResult>
{
    [FromHeader(Name = Constants.ApiHeaders.RequestedByMemberIdHeader)]
    public Guid RequestedByMemberId { get; set; }

    public int? CalendarTypeId { get; set; }
    public EventFormat? EventFormat { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Summary { get; set; }
    public int? RegionId { get; set; }
    public string? Location { get; set; }
    public string? Postcode { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public long? Urn { get; set; }
    public string? EventLink { get; set; }
    public string? ContactName { get; set; }
    public string? ContactEmail { get; set; }
    public int? PlannedAttendees { get; set; }
    public List<GuestSpeaker> Guests { get; set; } = new List<GuestSpeaker>();

}
