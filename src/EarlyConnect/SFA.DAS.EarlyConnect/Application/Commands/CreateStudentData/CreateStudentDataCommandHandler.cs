using MediatR;
using SFA.DAS.EarlyConnect.InnerApi.Requests;
using SFA.DAS.EarlyConnect.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EarlyConnect.Application.Commands.CreateStudentData
{
    public class CreateStudentDataCommandHandler : IRequestHandler<CreateStudentDataCommand, CreateStudentDataCommandResult>
    {
        private readonly IEarlyConnectApiClient<EarlyConnectApiConfiguration> _apiClient;

        public CreateStudentDataCommandHandler(IEarlyConnectApiClient<EarlyConnectApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<CreateStudentDataCommandResult> Handle(CreateStudentDataCommand request, CancellationToken cancellationToken)
        {
            var response = await _apiClient.PostWithResponseCode<CreateStudentDataResponse>(new CreateStudentDataRequest(request.StudentDataList), true);

            response.EnsureSuccessStatusCode();

            var emails = request.StudentDataList.ListOfStudentData.Select(studentData => studentData.Email).ToList();

            var result = await _apiClient.PostWithResponseCode<CreateStudentOnboardDataResponse>(new CreateStudentOnboardDataRequest(new EmailData { Emails = emails }), true);

            response.EnsureSuccessStatusCode();

            return new CreateStudentDataCommandResult
            {
                Message = $"{response.Body.Message} - {result.Body.Message}"
            };
        }
    }
}
