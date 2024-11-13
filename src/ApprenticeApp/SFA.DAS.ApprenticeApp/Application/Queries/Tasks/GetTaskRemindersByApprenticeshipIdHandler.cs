using MediatR;
using SFA.DAS.ApprenticeApp.InnerApi.ApprenticeProgress.Requests;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.Application.Queries.Tasks
{
    public class GetTaskRemindersByApprenticeshipIdHandler : IRequestHandler<GetTaskRemindersByApprenticeshipIdQuery, GetTaskRemindersByApprenticeshipIdQueryResult>
    {
        private readonly IApprenticeProgressApiClient<ApprenticeProgressApiConfiguration> _progressApiClient;

        public GetTaskRemindersByApprenticeshipIdHandler(
            IApprenticeProgressApiClient<ApprenticeProgressApiConfiguration> progressApiClient
            )
        {
            _progressApiClient = progressApiClient;

        }

        public async Task<GetTaskRemindersByApprenticeshipIdQueryResult> Handle(GetTaskRemindersByApprenticeshipIdQuery request, CancellationToken cancellationToken)
        {

            var taskReminders = await _progressApiClient.Get<ApprenticeTaskRemindersCollection>(new GetApprenticeTaskRemindersRequest(request.ApprenticeshipId));

            return new GetTaskRemindersByApprenticeshipIdQueryResult
            {
                TaskReminders = taskReminders
            };
        }
    }
}
