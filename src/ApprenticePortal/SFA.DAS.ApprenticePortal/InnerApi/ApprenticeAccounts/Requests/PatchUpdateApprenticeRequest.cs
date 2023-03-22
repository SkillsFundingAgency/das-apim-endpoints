using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.ApprenticePortal.InnerApi.ApprenticeAccounts.Requests
{
    public class PatchUpdateApprenticeRequest : IPatchApiRequest<UpdateApprenticeRequest>
    {
        public string PatchUrl => $"apprentices/{Data.ApprenticeId}";
        public UpdateApprenticeRequest Data { get; set; }
    }

    public class UpdateApprenticeRequest
    {
        public Guid ApprenticeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}