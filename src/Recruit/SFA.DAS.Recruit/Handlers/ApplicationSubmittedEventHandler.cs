using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Recruit.Domain;
using SFA.DAS.Recruit.Events;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Handlers
{
    public class ApplicationSubmittedEventHandler(
        IMediator mediator,
        ILogger<ApplicationSubmittedEventHandler> logger,
        IRecruitApiClient<RecruitApiConfiguration> apiClient,
        EmailEnvironmentHelper emailHelper,
        INotificationService notificationService) : INotificationHandler<ApplicationSubmittedEvent>
    {
        public async Task Handle(ApplicationSubmittedEvent @event, CancellationToken cancellationToken)
        {
            // Need to submit this as a proper query. We don't need the full application details, just enough to get the employer details.
            var result = await mediator.Send(new GetVacancyByReferenceApiRequest(@event.VacancyReference), cancellationToken);

            // Get the vacancy from the vacancy reference
            // Get the employer's unique ref details off the vacancy
            // Get the users
            // Get the users' notification preferences
            // Send the email to the employer
            throw new NotImplementedException();
        }
    }
}
