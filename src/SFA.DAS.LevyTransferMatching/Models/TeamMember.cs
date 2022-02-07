using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.LevyTransferMatching.Models
{
    public class TeamMember
    {
        public string UserRef { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }

        public bool CanReceiveNotifications { get; set; }
    }
}
