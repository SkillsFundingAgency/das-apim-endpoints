using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Shared;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests
{
    public class PutApplicationApiRequest : IPutApiRequest
    {
        private readonly string _vacancyReference;
        public object Data { get; set; }

        public PutApplicationApiRequest(string vacancyReference, PutApplicationApiRequestData data)
        {
            _vacancyReference = vacancyReference;
            Data = data;
        }

        public string PutUrl => $"api/applications/{_vacancyReference}";
        
        public class PutApplicationApiRequestData
        {
            public Guid CandidateId { get; set; }
            public List<KeyValuePair<int, string>> AdditionalQuestions { get; set; }
            public short IsAdditionalQuestion1Complete { get; set; }
            public short IsAdditionalQuestion2Complete { get; set; }
            public short IsDisabilityConfidenceComplete { get; set; }
            public required LocationDto EmploymentLocation { get; init; } = null!;
        }
    }
}
