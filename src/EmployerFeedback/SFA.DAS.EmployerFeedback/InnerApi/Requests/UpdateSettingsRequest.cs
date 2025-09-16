using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.EmployerFeedback.InnerApi.Requests
{
    public class UpdateSettingsRequest : IPostApiRequest<UpdateSettingsData>
    {
        public UpdateSettingsRequest(string name, DateTime value)
        {
            Data = new UpdateSettingsData { Name = name, Value = value };
        }
        public string PostUrl => "api/settings";
        public UpdateSettingsData Data { get; set; }
    }

    public class UpdateSettingsData
    {
        public string Name { get; set; }
        public DateTime Value { get; set; }
    }
}
