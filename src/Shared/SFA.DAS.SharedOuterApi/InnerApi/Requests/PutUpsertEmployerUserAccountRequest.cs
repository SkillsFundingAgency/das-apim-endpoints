using System.Web;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests
{
    public class PutUpsertEmployerUserAccountRequest : IPutApiRequest
    {
        public PutUpsertEmployerUserAccountRequest(string id, string email, string firstName, string lastName)
        {
            Data = new
            {
                GovUkIdentifier = id,
                FirstName = firstName,
                LastName = lastName,
                Email = email
            };
        }


        public string PutUrl => "api/users";
        public object Data { get; set; }
    }
}