using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests
{
    public class PutAccountUserRequest : IPutApiRequest
    {
        public PutAccountUserRequest(string userRef, string email, string firstName, string lastName)
        {
            Data = new
            {
                UserRef = userRef,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                CorrelationId = string.Empty
            };
        }

        public string PutUrl => $"api/user/upsert";
        public object Data { get; set; }
    }
}