using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeCommitments.Apis.InnerApi
{
    internal class CreateApprenticeshipRequest : IPostApiRequest
    {
        public string PostUrl => "/apprenticeships";
        public object Data { get; set; }

        public CreateApprenticeshipRequest(Guid registrationId, Guid apprenticeId, string lastName, DateTime dateOfBirth)
        {
            Data = new
            {
                RegistrationId = registrationId,
                ApprenticeId = apprenticeId,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
            };
        }

    }
}
