using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Requests
{
    public class ProviderLocationUpdateRequest : IPutApiRequest<ProviderLocationUpdateModel>
    {
        public int Ukprn { get; }
        public Guid Id { get; }
        public string UserId { get; set; }
        public string PutUrl => $"providers/{Ukprn}/locations/{Id}/";

        public ProviderLocationUpdateRequest(ProviderLocationUpdateModel data)
        {
            Ukprn = data.Ukprn;
            Id = data.Id;
            UserId = data.UserId;
            Data = data;
        }

        public ProviderLocationUpdateModel Data { get; set; }
    }
}
