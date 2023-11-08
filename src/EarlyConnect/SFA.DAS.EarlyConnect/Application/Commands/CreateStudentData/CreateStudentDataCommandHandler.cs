using MediatR;
using SFA.DAS.EarlyConnect.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EarlyConnect.Application.Commands.CreateStudentData
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

           var response = await _apiClient.PostWithResponseCode<object>(new CreateStudentDataRequest(request.StudentDataList), false);

            response.EnsureSuccessStatusCode();

            return Unit.Value;
        }

        //public async Task<Unit> Handle(CreateStudentDataCommand request, CancellationToken cancellationToken)
        //{

        //    InnerApi.Requests.CreateLog createLog = new InnerApi.Requests.CreateLog
        //    {
        //        RequestType = request.RequestType,
        //        RequestSource = request.RequestSource,
        //        RequestIP = request.RequestIP,
        //        Payload = request.Payload,
        //        Status = CreateLogResponse.BulkUploadStatus.InProgress.ToString()
        //    };

        //    var createLogresponse = await _apiClient.PostWithResponseCode<InnerApi.Responses.CreateLogResponse>(new CreateLogRequest(createLog));

        //    createLogresponse.EnsureSuccessStatusCode();

        //    var response = await _apiClient.PostWithResponseCode<InnerApi.Requests.StudentData>(new CreateStudentDataRequest(request.StudentDataList), false);

        //    InnerApi.Requests.UpdateLog updateLog = new InnerApi.Requests.UpdateLog
        //    {
        //        LogId = createLogresponse.Body.Id,
        //        Status = response.StatusCode == HttpStatusCode.OK ? CreateLogResponse.BulkUploadStatus.Completed.ToString() : CreateLogResponse.BulkUploadStatus.Error.ToString(),
        //        Error = response.ErrorContent
        //    };

        //    await _apiClient.PostWithResponseCode<InnerApi.Requests.UpdateLog>(new UpdateLogRequest(updateLog), false);

        //    response.EnsureSuccessStatusCode();

        //    return Unit.Value;
        //}
    }
}
