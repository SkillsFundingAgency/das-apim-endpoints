using MediatR;
using SFA.DAS.ApprenticeApp.InnerApi.LearnerNotifications.Requests;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.Application.Commands.LearnerNotifications
{
    public class AddApprenticeshipProgressCommandHandler : IRequestHandler<AddApprenticeshipProgressCommand, Unit>
    {
        private readonly IApprenticeProgressApiClient<ApprenticeProgressApiConfiguration> _apprenticeProgressApiClient;

        public AddApprenticeshipProgressCommandHandler(IApprenticeProgressApiClient<ApprenticeProgressApiConfiguration> apprenticeProgressApiClient)
        {
            _apprenticeProgressApiClient = apprenticeProgressApiClient;
        }

        public async Task<Unit> Handle(AddApprenticeshipProgressCommand request, CancellationToken cancellationToken)
        {
            await _apprenticeProgressApiClient.PostWithResponseCode<object>(new AddApprenticeshipProgressRequest(request.ApprenticeId));
            return Unit.Value;
        }
    }
}
