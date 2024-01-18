using MediatR;
using SFA.DAS.EarlyConnect.InnerApi.Requests;
using SFA.DAS.EarlyConnect.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EarlyConnect.Application.Commands.ManageStudentTriageData
{
    public class ManageStudentTriageDataCommandHandler : IRequestHandler<ManageStudentTriageDataCommand, ManageStudentTriageDataCommandResult>
    {
        private readonly IEarlyConnectApiClient<EarlyConnectApiConfiguration> _apiClient;

        public ManageStudentTriageDataCommandHandler(IEarlyConnectApiClient<EarlyConnectApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<ManageStudentTriageDataCommandResult> Handle(ManageStudentTriageDataCommand request, CancellationToken cancellationToken)
        {
            var response = await _apiClient.PostWithResponseCode<ManageStudentTriageDataResponse>(new ManageStudentTriageDataRequest(request.StudentTriageData, request.SurveyGuid), true);

            response.EnsureSuccessStatusCode();

            return new ManageStudentTriageDataCommandResult
            {
                Message = response.Body.Message
            };
        }
    }
}
