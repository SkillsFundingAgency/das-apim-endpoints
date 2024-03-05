using System;
using System.Collections.Generic;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.ProviderRegistrations
{
    public class GetInvitationResponse
    {
        public long Id { get; set; }

        public Guid Reference { get; set; }

        public long Ukprn { get; set; }

        public string EmployerOrganisation { get; set; }

        public string EmployerFirstName { get; set; }

        public string EmployerLastName { get; set; }

        public string EmployerEmail { get; set; }

        public int Status { get; set; }

        public DateTime SentDate { get; set; }

        public string ProviderOrganisationName { get; set; }

        public string ProviderUserFullName { get; set; }

        public IEnumerable<InvitationEventDto> Events { get; set; }

        public class InvitationEventDto
        {
            public long Id { get; set; }

            public int EventType { get; set; }

            public DateTime? Date { get; set; }
        }
    }
}
