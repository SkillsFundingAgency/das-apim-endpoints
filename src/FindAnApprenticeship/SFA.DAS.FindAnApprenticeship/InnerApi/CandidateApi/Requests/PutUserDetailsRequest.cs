using SFA.DAS.SharedOuterApi.Interfaces;
using System;


namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests
{
    public class PutUserDetailsRequest : IPutApiRequest
    {
        private readonly Guid _candidateId;

        public PutUserDetailsRequest(string firstName, string lastName, Guid candidateId)
        {
            _candidateId = candidateId;
            Data = new
            {
                FirstName = firstName,
                LastName = lastName
            };
        }

        public object Data { get; set; }
        public string PutUrl => $"api/Candidates/{_candidateId}/add-details";

    }
}
