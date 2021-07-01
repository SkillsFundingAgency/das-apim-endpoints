using System;

namespace SFA.DAS.ApprenticeCommitments.Apis.InnerApi
{
    public class Apprentice
    {
        public Guid Id { get; internal set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid UserIdentityId { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}