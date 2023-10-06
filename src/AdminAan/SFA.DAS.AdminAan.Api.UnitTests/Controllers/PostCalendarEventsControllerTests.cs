using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.AdminAan.Api.Controllers;
using SFA.DAS.AdminAan.Application.CalendarEvents.Commands.Create;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminAan.Api.UnitTests.Controllers;
public class PostCalendarEventsControllerTests
{
    [Test, MoqAutoData]
    public async Task CreateEvent_Post_InvokesRequest(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] CalendarEventsController sut,
        CreateEventRequestModel requestModel,
        PostEventCommandResult expected,
        CancellationToken cancellationToken,
        Guid requestedByMemberId)
    {
        mediatorMock.Setup(m => m.Send(It.Is<PostEventCommand>(
            x => x.RequestedByMemberId == requestedByMemberId
                 && x.CalendarId == requestModel.CalendarId
                 && x.EventFormat == requestModel.EventFormat
                 && x.StartDate == requestModel.StartDate
                 && x.EndDate == requestModel.EndDate
                 && x.Title == requestModel.Title
                 && x.Description == requestModel.Description
                 && x.Summary == requestModel.Summary
                 && x.RegionId == requestModel.RegionId
                 && x.Location == requestModel.Location
                 && x.Postcode == requestModel.Postcode
                 && x.Latitude == requestModel.Latitude
                 && x.Longitude == requestModel.Longitude
                 && x.Urn == requestModel.Urn
                 && x.EventLink == requestModel.EventLink
                 && x.ContactName == requestModel.ContactName
                 && x.ContactEmail == requestModel.ContactEmail
                 && x.PlannedAttendees == requestModel.PlannedAttendees
                 && x.Guests == requestModel.Guests
        ), cancellationToken)).ReturnsAsync(expected);

        var response = await sut.PostCalendarEvent(requestedByMemberId, requestModel, cancellationToken);

        mediatorMock.Verify(m => m.Send(It.Is<PostEventCommand>(
        x => x.RequestedByMemberId == requestedByMemberId
             && x.CalendarId == requestModel.CalendarId
             && x.EventFormat == requestModel.EventFormat
             && x.StartDate == requestModel.StartDate
             && x.EndDate == requestModel.EndDate
             && x.Title == requestModel.Title
             && x.Description == requestModel.Description
             && x.Summary == requestModel.Summary
             && x.RegionId == requestModel.RegionId
             && x.Location == requestModel.Location
             && x.Postcode == requestModel.Postcode
             && x.Latitude == requestModel.Latitude
             && x.Longitude == requestModel.Longitude
             && x.Urn == requestModel.Urn
             && x.EventLink == requestModel.EventLink
             && x.ContactName == requestModel.ContactName
             && x.ContactEmail == requestModel.ContactEmail
             && x.PlannedAttendees == requestModel.PlannedAttendees
             && x.Guests == requestModel.Guests), cancellationToken));
        response.As<OkObjectResult>().Value.Should().Be(expected);
    }
}

