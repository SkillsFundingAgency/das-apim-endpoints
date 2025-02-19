using MediatR;
using SFA.DAS.AdminAan.Infrastructure;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EducationalOrganisations;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EducationalOrganisation;
using SFA.DAS.SharedOuterApi.Interfaces;
using static SFA.DAS.AdminAan.Application.CalendarEvents.Queries.GetCalendarEvent.GetCalendarEventQueryResult;

namespace SFA.DAS.AdminAan.Application.CalendarEvents.Queries.GetCalendarEvent;
public class GetCalendarEventQueryHandler(IAanHubRestApiClient apiClient, IEducationalOrganisationApiClient<EducationalOrganisationApiConfiguration> educationalOrganisationApiClient)
    : IRequestHandler<GetCalendarEventQuery, GetCalendarEventQueryResult>
{
    public async Task<GetCalendarEventQueryResult> Handle(GetCalendarEventQuery request, CancellationToken cancellationToken)
    {
        var apiResponse = await apiClient.GetCalendarEvent(request.RequestedByMemberId, request.CalendarEventId, cancellationToken);

        var result = new GetCalendarEventQueryResult
        {
            CalendarEventId = apiResponse.CalendarEventId,
            CalendarName = apiResponse.CalendarName,
            CalendarId = apiResponse.CalendarId,
            EventFormat = apiResponse.EventFormat,
            StartDate = apiResponse.StartDate,
            EndDate = apiResponse.EndDate,
            Title = apiResponse.Title,
            Description = apiResponse.Description,
            Summary = apiResponse.Summary,
            Location = apiResponse.Location,
            Postcode = apiResponse.Postcode,
            Latitude = apiResponse.Latitude,
            Longitude = apiResponse.Longitude,
            EventLink = apiResponse.EventLink,
            ContactName = apiResponse.ContactName,
            ContactEmail = apiResponse.ContactEmail,
            IsActive = apiResponse.IsActive,
            LastUpdatedDate = apiResponse.LastUpdatedDate,
            Attendees = apiResponse.Attendees.Select(a => new QueryAttendee(a.MemberId, a.UserType, a.MemberName, a.Surname, a.Email,a.AddedDate, a.CancelledDate)).ToList(),
            CancelledAttendees = apiResponse.CancelledAttendees.Select(ca => new QueryCancelledAttendee(ca.MemberId, ca.UserType, ca.MemberName, ca.Email, ca.AddedDate, ca.CancelledDate)).ToList(),
            EventGuests = apiResponse.EventGuests.Select(eg => new QueryEventGuest(eg.GuestName, eg.GuestJobTitle)).ToList(),
            PlannedAttendees = apiResponse.PlannedAttendees,
            CreatedDate = apiResponse.CreatedDate,
            RegionId = apiResponse.RegionId,
            Urn = apiResponse.Urn
        };

        if (result.RegionId != null)
        {
            var regionsResult = await apiClient.GetRegions(cancellationToken);
            result.RegionName = regionsResult.Regions.First(x => x.Id == result.RegionId).Area;
        }

        if (string.IsNullOrEmpty(result.Urn.ToString())) return result;

        var response =
            await educationalOrganisationApiClient.GetWithResponseCode<GetLatestDetailsForEducationalOrgResponse>(
                new GetLatestDetailsForEducationalOrgRequest(identifier: result.Urn.ToString()));

        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            result.SchoolName = response.Body.EducationalOrganisation.Name;
        }

        return result;
    }
}

