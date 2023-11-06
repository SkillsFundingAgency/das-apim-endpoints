using SFA.DAS.EarlyConnect.Api.Models;
using SFA.DAS.EarlyConnect.InnerApi.Requests;

namespace SFA.DAS.EarlyConnect.Api.Mappers
{
    public static class LogDataMapper
    {
        public static CreateLog MapFromLogCreateRequest(this CreateLogPostRequest request)
        {
            var log = new CreateLog
            {
                RequestSource = request.RequestSource,
                RequestType = request.RequestType,
                RequestIP = request.RequestIP,
                FileName = request.FileName,
                Payload = request.Payload,
                Status = request.Status,
                Error = request.Error
            };

            return log;
        }
        public static UpdateLog MapFromLogUpdateRequest(this UpdateLogPostRequest request)
        {
            var log = new UpdateLog
            {
                LogId = request.LogId,
                Error = request.Error,
                Status = request.Status,
            };

            return log;
        }
    }
}
