﻿using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
public class GetEmployerFeedbackDetailsAnnualRequest : IGetApiRequest
{
    public string GetUrl => $"api/EmployerFeedbackResult/{_ukprn}/annual";
    private long _ukprn { get; }
    public GetEmployerFeedbackDetailsAnnualRequest(long ukprn)
    {
        _ukprn = ukprn;
    }
}
