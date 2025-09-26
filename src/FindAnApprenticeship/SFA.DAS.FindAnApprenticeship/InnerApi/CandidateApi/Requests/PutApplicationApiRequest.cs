using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Shared;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Domain;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests
{
    public class PutApplicationApiRequest(
        string vacancyReference,
        PutApplicationApiRequest.PutApplicationApiRequestData data)
        : IPutApiRequest
    {
        public object Data { get; set; } = data;

        public string PutUrl => $"api/applications/{vacancyReference}";
        
        public class PutApplicationApiRequestData
        {
            public Guid CandidateId { get; set; }
            public List<KeyValuePair<int, string>> AdditionalQuestions { get; set; }
            public short IsAdditionalQuestion1Complete { get; set; }
            public short IsAdditionalQuestion2Complete { get; set; }
            public short IsDisabilityConfidenceComplete { get; set; }
            public ApprenticeshipTypes ApprenticeshipType { get; set; }
            public short IsEmploymentLocationComplete { get; set; }
            public LocationDto? EmploymentLocation { get; init; }
        }
    }
}