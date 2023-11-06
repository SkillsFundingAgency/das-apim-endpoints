using MediatR;
using SFA.DAS.EarlyConnect.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EarlyConnect.Application.Commands.StudentData
{
    public class CreateStudentDataCommandHandler : IRequestHandler<CreateStudentDataCommand, Unit>
    {

        private readonly IEarlyConnectApiClient<EarlyConnectApiConfiguration> _apiClient;

        public CreateStudentDataCommandHandler(IEarlyConnectApiClient<EarlyConnectApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<Unit> Handle(CreateStudentDataCommand request, CancellationToken cancellationToken)
        {
            var response = await _apiClient.PostWithResponseCode<InnerApi.Requests.StudentData>(new CreateStudentDataRequest(request.StudentDataList), false);

            response.EnsureSuccessStatusCode();

            return Unit.Value;
        }
    }
}
