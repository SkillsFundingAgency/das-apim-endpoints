using System;

namespace SFA.DAS.Approvals.InnerApi.Responses
{
    public class GetApprenticeResponse
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}