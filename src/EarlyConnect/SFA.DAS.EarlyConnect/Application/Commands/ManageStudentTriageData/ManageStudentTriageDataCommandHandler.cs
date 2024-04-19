using MediatR;
using SFA.DAS.EarlyConnect.Configuration;
using SFA.DAS.EarlyConnect.Configuration.FeatureToggle;
using SFA.DAS.EarlyConnect.InnerApi.Requests;
using SFA.DAS.EarlyConnect.InnerApi.Responses;
using SFA.DAS.EarlyConnect.Services;
using SFA.DAS.EarlyConnect.Services.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EarlyConnect.Application.Commands.ManageStudentTriageData
{
    public class ManageStudentTriageDataCommandHandler : IRequestHandler<ManageStudentTriageDataCommand, ManageStudentTriageDataCommandResult>
    {
        private readonly IEarlyConnectApiClient<EarlyConnectApiConfiguration> _apiClient;
        private readonly IFeature _feature;
        private readonly ISendStudentDataToLepsService _sendStudentDataToLepsService;
        private ManageStudentTriageDataCommandResult _manageStudentTriageDataCommandResult;

        public ManageStudentTriageDataCommandHandler(IEarlyConnectApiClient<EarlyConnectApiConfiguration> apiClient,
            ISendStudentDataToLepsService sendStudentDataToLepsService,
            IFeature feature)
        {
            _apiClient = apiClient;
            _sendStudentDataToLepsService = sendStudentDataToLepsService;
            _feature = feature;
        }

        public async Task<ManageStudentTriageDataCommandResult> Handle(ManageStudentTriageDataCommand request, CancellationToken cancellationToken)
        {
            var manageStudentResponse = await _apiClient.PostWithResponseCode<ManageStudentTriageDataResponse>(new ManageStudentTriageDataRequest(request.StudentTriageData, request.SurveyGuid), true);
            manageStudentResponse.EnsureSuccessStatusCode();

            _manageStudentTriageDataCommandResult = new ManageStudentTriageDataCommandResult();

            if (request.StudentTriageData.StudentSurvey.DateCompleted == null)
                return CreateCommandResult($"{manageStudentResponse?.Body?.Message}");

            if (_feature.IsFeatureEnabled(FeatureNames.NorthEastDataSharing))
            {
                SendStudentDataToLepsServiceResponse response = await _sendStudentDataToLepsService.SendStudentDataToNe(request.SurveyGuid);
                CreateCommandResult($"{response?.Message}");
            }

            if (_feature.IsFeatureEnabled(FeatureNames.LancashireDataSharing))
            {
                SendStudentDataToLepsServiceResponse response = await _sendStudentDataToLepsService.SendStudentDataToLa(request.SurveyGuid);
                CreateCommandResult($"{response?.Message}");
            }

            return CreateCommandResult($"{manageStudentResponse?.Body?.Message}");
        }
        private ManageStudentTriageDataCommandResult CreateCommandResult(string message)
        {
            _manageStudentTriageDataCommandResult.Message = string.IsNullOrEmpty(_manageStudentTriageDataCommandResult.Message) ? message : $"{_manageStudentTriageDataCommandResult.Message}- {message}";
            return _manageStudentTriageDataCommandResult;
        }
    }
}


