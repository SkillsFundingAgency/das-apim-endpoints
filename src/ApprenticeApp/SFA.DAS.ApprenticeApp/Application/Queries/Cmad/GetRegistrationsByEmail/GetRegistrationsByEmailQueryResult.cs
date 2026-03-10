using SFA.DAS.ApprenticeApp.Models;
using System.Collections.Generic;

namespace SFA.DAS.ApprenticeApp.Application.Queries.Cmad.GetRegistrationsByEmail
{
    public class GetRegistrationsByEmailQueryResult
    {
        public List<Registration> Registrations { get; set; }
    }
}
