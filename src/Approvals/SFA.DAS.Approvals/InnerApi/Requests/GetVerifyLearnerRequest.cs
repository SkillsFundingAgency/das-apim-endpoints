using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class GetVerifyLearnerRequest : IGetApiRequest
    {
        public readonly long AccountLegalEntityId;
        public string GetUrl => $"api/LearnerDetails/verify?uln={Uln}&firstName={FirstName}&lastName={LastName}";

        public GetVerifyLearnerRequest(string uln, string firstName, string lastName)
        {
            Uln = uln;
            FirstName = firstName;
            LastName = lastName;
        }
        public string Uln { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
