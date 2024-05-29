﻿using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.RecruitApi.Requests
{
    public class GetClosedVacancyRequest(string vacancyReference) : IGetApiRequest
    {
        public string GetUrl => $"api/closedvacancies/{vacancyReference}";
    }
}
