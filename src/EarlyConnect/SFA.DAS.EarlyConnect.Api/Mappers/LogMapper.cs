using SFA.DAS.EarlyConnect.Api.Models;
using SFA.DAS.EarlyConnect.InnerApi.Requests;

namespace SFA.DAS.EarlyConnect.Api.Mappers
{
    public static class LogMapper
    {
        public static LogCreate MapFromLogCreateRequest(this CreateLogPostRequest request)
        {
            var log = new LogCreate
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
        public static LogUpdate MapFromLogUpdateRequest(this UpdateLogPostRequest request)
        {
            var log = new LogUpdate
            {
                LogId = request.LogId,
                Error = request.Error,
                Status = request.Status,
            };

            return log;
        }
    }
}
