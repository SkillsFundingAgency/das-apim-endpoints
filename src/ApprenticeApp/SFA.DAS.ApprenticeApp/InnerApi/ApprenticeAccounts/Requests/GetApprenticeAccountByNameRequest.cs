using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.InnerApi.ApprenticeAccounts.Requests
{
    public class GetApprenticeAccountByNameRequest : IGetApiRequest
    {
        private readonly string _firstName;
        private readonly string _lastName;

        public GetApprenticeAccountByNameRequest(string firstName, string lastName)
        {
            _firstName = firstName;
            _lastName = lastName;
        }

        public string GetUrl => $"apprentices?firstName={_firstName}&lastName={_lastName}";
    }
}
