using System.Web;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests
{
    public class PutUpsertEmployerUserAccount : IPutApiRequest
    {
        private readonly string _id;
        private readonly string _email;

        public PutUpsertEmployerUserAccount(string id, string email, string firstName, string lastName)
        {
            _id = id;
            _email = email;
            Data = new
            {
                GovUkIdentifier = id,
                FirstName = firstName,
                LastName = lastName
            };
        }


        public string PutUrl => $"api/users/{HttpUtility.UrlEncode(_email)}";
        public object Data { get; set; }
    }
}