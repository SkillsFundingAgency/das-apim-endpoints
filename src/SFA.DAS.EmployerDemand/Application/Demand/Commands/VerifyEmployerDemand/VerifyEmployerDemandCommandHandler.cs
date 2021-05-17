﻿using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerDemand.Domain.Models;
using SFA.DAS.EmployerDemand.InnerApi.Requests;
using SFA.DAS.EmployerDemand.InnerApi.Responses;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerDemand.Application.Demand.Commands.VerifyEmployerDemand
{
    public class VerifyEmployerDemandCommandHandler : IRequestHandler<VerifyEmployerDemandCommand, VerifyEmployerDemandCommandResult>
    {
        private readonly IEmployerDemandApiClient<EmployerDemandApiConfiguration> _apiClient;
        private readonly INotificationService _notificationService;

        public VerifyEmployerDemandCommandHandler (IEmployerDemandApiClient<EmployerDemandApiConfiguration> apiClient, INotificationService notificationService)
        {
            _apiClient = apiClient;
            _notificationService = notificationService;
        }
        public async Task<VerifyEmployerDemandCommandResult> Handle(VerifyEmployerDemandCommand request, CancellationToken cancellationToken)
        {
            var getEmployerDemandResponse =
                await _apiClient.Get<GetEmployerDemandResponse>(new GetEmployerDemandRequest(request.Id));

            if (getEmployerDemandResponse == null)
            {
                return new VerifyEmployerDemandCommandResult
                {
                    EmployerDemand = null
                };
            }
            
            if (!getEmployerDemandResponse.EmailVerified)
            {
                var verifyEmailResponse =
                    await _apiClient.PostWithResponseCode<PostEmployerCourseDemand>(new PostVerifyEmployerDemandEmailRequest(request.Id));

                if (verifyEmailResponse.StatusCode != HttpStatusCode.Accepted)
                {
                    throw new HttpRequestContentException($"Response status code does not indicate success: {(int)verifyEmailResponse.StatusCode} ({verifyEmailResponse.StatusCode})", verifyEmailResponse.StatusCode, verifyEmailResponse.ErrorContent);
                }
                
                if (verifyEmailResponse.StatusCode == HttpStatusCode.Accepted)
                {
                    var emailModel = new CreateDemandConfirmationEmail(
                        getEmployerDemandResponse.ContactEmailAddress,
                        getEmployerDemandResponse.OrganisationName,
                        getEmployerDemandResponse.Course.Title,
                        getEmployerDemandResponse.Course.Level,
                        getEmployerDemandResponse.Location.Name,
                        getEmployerDemandResponse.NumberOfApprentices
                    );
                    await _notificationService.Send(new SendEmailCommand(emailModel.TemplateId,emailModel.RecipientAddress, emailModel.Tokens));
                }    
            }

            return new VerifyEmployerDemandCommandResult
            {
                EmployerDemand = getEmployerDemandResponse
            };
        }
    }
}