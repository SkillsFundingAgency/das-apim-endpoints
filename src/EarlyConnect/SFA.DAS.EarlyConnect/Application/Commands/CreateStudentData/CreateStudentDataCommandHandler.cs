using MediatR;
using SFA.DAS.EarlyConnect.Application.Commands.CreateStudentOnboardData;
using SFA.DAS.EarlyConnect.InnerApi.Requests;
using SFA.DAS.EarlyConnect.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EarlyConnect.Application.Commands.CreateStudentData
{
    public class CreateStudentDataCommandHandler : IRequestHandler<CreateStudentDataCommand, CreateStudentDataCommandResult>
    {
        private readonly IMediator _mediator;
        private readonly IEarlyConnectApiClient<EarlyConnectApiConfiguration> _apiClient;

        public CreateStudentDataCommandHandler(IEarlyConnectApiClient<EarlyConnectApiConfiguration> apiClient, IMediator mediator)
        {
            _apiClient = apiClient;
            _mediator = mediator;
        }

        public async Task<CreateStudentDataCommandResult> Handle(CreateStudentDataCommand request, CancellationToken cancellationToken)
        {
            var response = await _apiClient.PostWithResponseCode<CreateStudentDataResponse>(new CreateStudentDataRequest(request.StudentDataList), true);

            response.EnsureSuccessStatusCode();

            var emails = request.StudentDataList.ListOfStudentData.Select(studentData => studentData.Email).ToList();

            var result = await _mediator.Send(new CreateStudentOnboardDataCommand
            {
                Emails = new EmailData { Emails = emails }
            });

            return new CreateStudentDataCommandResult
            {
                Message = $"{response.Body.Message} - {result.Message}"
            };
        }

    }
}
