using SFA.DAS.FindAnApprenticeship.Models;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests
{
    public class UpdateSectionApiRequest : IPutApiRequest
    {
        private readonly string _vacancyReference;

        public UpdateSectionApiRequest(string vacancyReference, UpdateSectionRequest sectionRequest)
        {
            _vacancyReference = vacancyReference;
            Data = new
            {
                sectionRequest = sectionRequest
            };
        }

        public string PutUrl => $"/api/candidates/application/{_vacancyReference}/workHistory";
        public object Data { get; set; }
    }
}