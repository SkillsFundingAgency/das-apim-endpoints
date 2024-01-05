using MediatR;
using SFA.DAS.EarlyConnect.InnerApi.Requests;
using SFA.DAS.EarlyConnect.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EarlyConnect.Application.Commands.CreateOtherStudentTriageData
{
    public class CreateOtherStudentTriageDataCommandHandler : IRequestHandler<CreateOtherStudentTriageDataCommand, CreateOtherStudentTriageDataCommandResult>
    {

        private readonly IEarlyConnectApiClient<EarlyConnectApiConfiguration> _apiClient;

        public CreateOtherStudentTriageDataCommandHandler(IEarlyConnectApiClient<EarlyConnectApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<CreateOtherStudentTriageDataCommandResult> Handle(CreateOtherStudentTriageDataCommand request, CancellationToken cancellationToken)
        {

           var response = await _apiClient.PostWithResponseCode<CreateOtherStudentTriageDataResponse>(new CreateOtherStudentTriageDataRequest(request.StudentTriageData),true);

            response.EnsureSuccessStatusCode();

            return new CreateOtherStudentTriageDataCommandResult
            {
                StudentSurveyId = response.Body.StudentSurveyId,
                AuthCode = response.Body.AuthCode,
                ExpiryDate = response.Body.ExpiryDate
            };
        }
    }
}
