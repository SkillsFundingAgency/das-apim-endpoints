using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.Models
{
    public class ApprenticeAccount
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Email Email { get; set; }
        public bool TermsOfUseAccepted { get; set; }
    }

    public class Email
    {
        public string DisplayName { get; set; }
        public string User { get; set; }
        public string Host { get; set; }
        public string Address { get; set; }
    }
}
