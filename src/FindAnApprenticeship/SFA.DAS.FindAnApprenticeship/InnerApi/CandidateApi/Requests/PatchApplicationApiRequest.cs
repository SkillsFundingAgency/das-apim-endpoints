﻿using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests
{
    public class PatchApplicationApiRequest : IPatchApiRequest<>
    {
        private readonly string _vacancyReference;
        public object Data { get; set; }

        public PatchApplicationApiRequest(string vacancyReference, PutApplicationApiRequestData data)
        {
            _vacancyReference = vacancyReference;
            Data = data;
        }

        public string PutUrl => $"api/applications/{_vacancyReference}";
        
        public class PutApplicationApiRequestData
        {
            public string Email { get; init; }
        }
    }
}
