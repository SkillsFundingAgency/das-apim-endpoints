using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerDemand.Domain.Models;
using SFA.DAS.EmployerDemand.InnerApi.Requests;
using SFA.DAS.EmployerDemand.InnerApi.Responses;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerDemand.Application.ProviderInterest.Commands.CreateProviderInterests
{
    public class CreateProviderInterestsCommandHandler: IRequestHandler<CreateProviderInterestsCommand, Guid>
    {
        private readonly IEmployerDemandApiClient<EmployerDemandApiConfiguration> _apiClient;
        private readonly INotificationService _notificationService;

        public CreateProviderInterestsCommandHandler(
            IEmployerDemandApiClient<EmployerDemandApiConfiguration> apiClient,
            INotificationService notificationService)
        {
            _apiClient = apiClient;
            _notificationService = notificationService;
        }

        public async Task<Guid> Handle(CreateProviderInterestsCommand request, CancellationToken cancellationToken)
        {
            var result = await _apiClient.PostWithResponseCode<PostCreateProviderInterestsResponse>(
                new PostCreateProviderInterestsRequest(request));

            if (result.StatusCode == System.Net.HttpStatusCode.Created)
            {
                foreach (var employerDemandId in request.EmployerDemandIds)
                {
                    var employerDemand = await _apiClient.Get<GetEmployerDemandResponse>(
                        new GetEmployerDemandRequest(employerDemandId));
                    var email = new ProviderIsInterestedEmail(
                        employerDemand.ContactEmailAddress, 
                        employerDemand.OrganisationName,
                        employerDemand.Course.Title, 
                        employerDemand.Course.Level, 
                        employerDemand.Location.Name, 
                        employerDemand.NumberOfApprentices,
                        request.ProviderName, 
                        request.Email, 
                        request.Phone, 
                        request.Website, 
                        request.FatUrl,
                        employerDemand.StopSharingUrl);
                    await _notificationService.Send(new SendEmailCommand(email.TemplateId,
                        email.RecipientAddress, email.Tokens));
                }
            }

            return result.Body.Id;
        }
    }
}