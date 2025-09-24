using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.EmployerFeedback.InnerApi.Requests
{
    public class UpdateSettingsRequest : IPutApiRequest<UpdateSettingsData>
    {
        public UpdateSettingsRequest(DateTime value)
        {
            Data = new UpdateSettingsData { Value = value };
        }
        public string PutUrl => "api/settings/RefreshALELastRunDate";
        public UpdateSettingsData Data { get; set; }
    }

    public class UpdateSettingsData
    {
        public DateTime Value { get; set; }
    }
}
