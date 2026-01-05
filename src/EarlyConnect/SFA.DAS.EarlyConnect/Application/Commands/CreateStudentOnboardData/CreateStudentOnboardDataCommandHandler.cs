using MediatR;
using SFA.DAS.EarlyConnect.InnerApi.Requests;
using SFA.DAS.EarlyConnect.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EarlyConnect.Application.Commands.CreateStudentOnboardData
{
    public class CreateStudentOnboardDataCommandHandler : IRequestHandler<CreateStudentOnboardDataCommand, CreateStudentOnboardDataCommandResult>
    {
        private readonly IEarlyConnectApiClient<EarlyConnectApiConfiguration> _apiClient;

        public CreateStudentOnboardDataCommandHandler(IEarlyConnectApiClient<EarlyConnectApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<CreateStudentOnboardDataCommandResult> Handle(CreateStudentOnboardDataCommand request, CancellationToken cancellationToken)
        {
            var response = await _apiClient.PostWithResponseCode<CreateStudentOnboardDataResponse>(new CreateStudentOnboardDataRequest(request.Emails), true);

            response.EnsureSuccessStatusCode();

            return new CreateStudentOnboardDataCommandResult
            {
                Message = response.Body.Message
            };
        }
    }
}
