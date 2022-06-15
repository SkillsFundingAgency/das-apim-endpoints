<<<<<<< HEAD:src/RoatpCourseManagement/SFA.DAS.RoatpCourseManagement/InnerApi/Requests/UpdateProviderCourseRequest.cs
﻿using SFA.DAS.SharedOuterApi.Interfaces;
=======
﻿using SFA.DAS.RoatpCourseManagement.Application.Standards.Commands.UpdateContactDetails;
using SFA.DAS.SharedOuterApi.Interfaces;
>>>>>>> 06c8c044 (More separated pipelines, rename RoatpCourseManagement for alignment, clear up -old bits):src/RoatpCourseManagement/SFA.DAS.RoatpCourseManagement/InnerApi/Requests/UpdateContactDetailsRequest.cs

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Requests
{
    public class UpdateProviderCourseRequest : IPostApiRequest
    {
        public int Ukprn { get; }
        public int LarsCode { get; }
        public string UserId { get; set; }
        public string PostUrl => $"providers/{Ukprn}/courses/{LarsCode}/";

        public UpdateProviderCourseRequest(ProviderCourseUpdateModel data)
        {
            Ukprn = data.Ukprn;
            LarsCode = data.LarsCode;
            UserId = data.UserId;
            Data = data;
        }

        public object Data { get; set; }
    }
}
