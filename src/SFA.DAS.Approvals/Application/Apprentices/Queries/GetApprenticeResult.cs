using System;

namespace SFA.DAS.Approvals.Application.Apprentices.Queries
{
    public class GetApprenticeResult
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}