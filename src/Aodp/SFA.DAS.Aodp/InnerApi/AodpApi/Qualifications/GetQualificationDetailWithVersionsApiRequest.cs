using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.AODP.Domain.Qualifications.Requests
{
    public class GetQualificationDetailWithVersionsApiRequest : IGetApiRequest
    {
        private readonly string _qualificationReference;

        public GetQualificationDetailWithVersionsApiRequest(string qualificationReference)
        {
            _qualificationReference = qualificationReference;
        }

        public string GetUrl => $"api/qualifications/{_qualificationReference}/detailwithversions";
    }
}
