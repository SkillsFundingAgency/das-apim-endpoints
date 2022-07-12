using System;
using System.Collections.Generic;
using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.CreateApplication
{
    public class CreateApplicationCommand : IRequest<CreateApplicationCommandResult>
    {
        public int PledgeId { get; set; }
        public long EmployerAccountId { get; set; }
        public string EncodedAccountId { get; set; }

        public string Details { get; set; }

        public string StandardId { get; set; }
        public int NumberOfApprentices { get; set; }
        public DateTime StartDate { get; set; }
        public bool HasTrainingProvider { get; set; }

        public IEnumerable<string> Sectors { get; set; }
        public List<int> Locations { get; set; }
        public string AdditionalLocation { get; set; }
        public string SpecificLocation { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IEnumerable<string> EmailAddresses { get; set; }
        public string BusinessWebsite { get; set; }
        public string UserId { get; set; }
        public string UserDisplayName { get; set; }

    }
}
