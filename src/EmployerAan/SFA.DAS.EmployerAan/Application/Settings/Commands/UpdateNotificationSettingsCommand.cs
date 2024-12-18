using MediatR;
using SFA.DAS.EmployerAan.Infrastructure;
using SFA.DAS.EmployerAan.InnerApi.Settings;
using SFA.DAS.EmployerAan.Models.ApiRequests.Settings;

namespace SFA.DAS.EmployerAan.Application.Settings.Commands
{
    public class UpdateNotificationSettingsCommand : IRequest
    {
        public Guid MemberId { get; set; }
        public List<Location> Locations { get; set; } = new List<Location>();

        public class Location
        {
            public string Name { get; set; }
            public int Radius { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }
    }

    public class UpdateNotificationSettingsCommandHandler(IAanHubRestApiClient aanHubApiClient)
        : IRequestHandler<UpdateNotificationSettingsCommand>
    {
        public async Task Handle(UpdateNotificationSettingsCommand request, CancellationToken cancellationToken)
        {
            var updateRequest = new NotificationLocationsPostApiRequest
            {
                Locations = request.Locations.Select(x => new NotificationLocationsPostApiRequest.Location{
                    Name = x.Name,
                    Radius = x.Radius,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude
                }).ToList(),
            };

            await aanHubApiClient.UpdateMemberNotificationLocations(request.MemberId, updateRequest, cancellationToken);
        }
    }
}