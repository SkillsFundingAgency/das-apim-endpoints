﻿using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Roatp.CourseManagement.InnerApi.Requests
{
    public class DeleteProviderLocationRequest : IPostApiRequest
    {
        public int Ukprn { get; }
        public int LarsCode { get; }
        public string UserId { get; set; }
        public string PostUrl => $"providers/{Ukprn}/locations/cleanup";

        public DeleteProviderLocationRequest(ProviderLocationDeleteModel data)
        {
            Ukprn = data.Ukprn;
            LarsCode = data.LarsCode;
            UserId = data.UserId;
            Data = data;
        }

        public object Data { get; set; }
    }
}
