using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerDemand.Domain.Models;
using SFA.DAS.EmployerDemand.InnerApi.Requests;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerDemand.Application.Demand.Commands.RegisterDemand
{
    public class RegisterDemandCommandHandler : IRequestHandler<RegisterDemandCommand, Guid>
    {
        private readonly IEmployerDemandApiClient<EmployerDemandApiConfiguration> _apiClient;
        private readonly INotificationService _notificationService;

        public RegisterDemandCommandHandler (
            IEmployerDemandApiClient<EmployerDemandApiConfiguration> apiClient,
            INotificationService notificationService)
        {
            _apiClient = apiClient;
            _notificationService = notificationService;
        }
        public async Task<Guid> Handle(RegisterDemandCommand request, CancellationToken cancellationToken)
        {
            var result = await _apiClient.Post<PostCreateCourseDemand>(new PostCreateCourseDemandRequest(new CreateCourseDemandData
            {
                Id = request.Id,
                ContactEmailAddress = request.ContactEmailAddress,
                OrganisationName = request.OrganisationName,
                NumberOfApprentices = request.NumberOfApprentices,
                Location = new Location
                {
                    Name = request.LocationName,
                    LocationPoint = new LocationPoint
                    {
                        GeoPoint = new List<double>{request.Lat, request.Lon}
                    }
                },
                Course = new Course
                {
                    Id = request.CourseId,
                    Title = request.CourseTitle,
                    Level = request.CourseLevel
                }
            }));
            var emailModel = new CreateDemandConfirmationEmail(
                request.ContactEmailAddress,
                request.OrganisationName,
                request.CourseTitle,
                request.CourseLevel,
                request.LocationName,
                request.NumberOfApprentices);
            
            
            await _notificationService.Send(new SendEmailCommand(emailModel.TemplateId,emailModel.RecipientAddress, emailModel.Tokens));

            return result.Id;
        }
    }
}
