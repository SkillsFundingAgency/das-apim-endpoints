using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.EmployerFeedback.InnerApi.Requests
{
    public class UpsertRefreshALELastRunDateSettingRequest : IPutApiRequest<UpsertRefreshALELastRunDateSettingData>
    {
        public UpsertRefreshALELastRunDateSettingRequest(DateTime value)
        {
            Data = new UpsertRefreshALELastRunDateSettingData { Value = value };
        }
        public string PutUrl => "api/settings/RefreshALELastRunDate";
        public UpsertRefreshALELastRunDateSettingData Data { get; set; }
    }

    public class UpsertRefreshALELastRunDateSettingData
    {
        public DateTime Value { get; set; }
    }
}
