using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerDemand.Domain.Models;
using SFA.DAS.EmployerDemand.InnerApi.Requests;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using Location = SFA.DAS.EmployerDemand.InnerApi.Requests.Location;
using LocationPoint = SFA.DAS.EmployerDemand.InnerApi.Requests.LocationPoint;

namespace SFA.DAS.EmployerDemand.Application.Demand.Commands.RegisterDemand
{
    public class RegisterDemandCommandHandler : IRequestHandler<RegisterDemandCommand, Guid?>
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

        public async Task<Guid?> Handle(RegisterDemandCommand request, CancellationToken cancellationToken)
        {
            var result = await _apiClient.PostWithResponseCode<PostEmployerCourseDemand>(new PostCreateCourseDemandRequest(new CreateCourseDemandData
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
                    Level = request.CourseLevel,
                    Route = request.CourseRoute,
                },
                StopSharingUrl = request.StopSharingUrl,
                StartSharingUrl = request.StartSharingUrl,
                ExpiredCourseDemandId = request.ExpiredCourseDemandId,
                EntryPoint = request.EntryPoint
            }));
            
            if (result.StatusCode == HttpStatusCode.Created)
            {
                var emailModel = new VerifyEmployerDemandEmail(
                    request.ContactEmailAddress,
                    request.OrganisationName,
                    request.CourseTitle,
                    request.CourseLevel,
                    request.ConfirmationLink);
            
            
                await _notificationService.Send(new SendEmailCommand(emailModel.TemplateId,emailModel.RecipientAddress, emailModel.Tokens));                
            }

            if (result.StatusCode == HttpStatusCode.Conflict)
            {
                return null;
            }

            if(!((int)result.StatusCode >= 200 && (int)result.StatusCode <= 299))
            {
                throw new HttpRequestContentException($"Response status code does not indicate success: {(int)result.StatusCode} ({result.StatusCode})", result.StatusCode, result.ErrorContent);
            }
            
            return result.Body.Id;
        }
    }
}
