using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.Azure.Amqp.Serialization.SerializableType;

namespace SFA.DAS.ApprenticeCommitments.Apis.ApprenticeAccountsApi
{
    public class GetApprenticeByPersonalDetailsRequest : IGetApiRequest
    {        
        public string FirstName { get; }
        public string LastName { get; }
        public DateTime DateOfBirth { get; }

        public GetApprenticeByPersonalDetailsRequest(string firstName, string lastName, DateTime dateOfBirth)
        {
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
        }

        public string GetUrl => $"apprentices?firstName={FirstName}&lastName={LastName}&DateOfBirth={DateOfBirth}";
    }
}
