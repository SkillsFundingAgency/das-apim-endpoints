﻿using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Roatp.CourseManagement.InnerApi.Requests
{
    public class UpdateProviderLocationRequest : IPostApiRequest
    {
        public int Ukprn { get; }
        public int LarsCode { get; }
        public string UserId { get; set; }
        public string PostUrl => $"providers/{Ukprn}/locations/bulk";

        public UpdateProviderLocationRequest(ProviderLocationUpdateModel data)
        {
            Ukprn = data.Ukprn;
            LarsCode = data.LarsCode;
            UserId = data.UserId;
            Data = data;
        }

        public object Data { get; set; }
    }
}
