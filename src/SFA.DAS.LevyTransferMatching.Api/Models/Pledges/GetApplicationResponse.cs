using System;
using System.Collections.Generic;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetApplication;

namespace SFA.DAS.LevyTransferMatching.Api.Models.Pledges
{
    public class GetApplicationResponse
    {
        public string Location { get; set; }
        public IEnumerable<string> Sector { get; set; }
        public string TypeOfJobRole { get; set; }
        public int Level { get; set; }
        public int NumberOfApprentices { get; set; }
        public int EstimatedDurationMonths { get; set; }
        public DateTime StartBy { get; set; }
        public bool HasTrainingProvider { get; set; }
        public string AboutOpportunity { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IEnumerable<string> EmailAddresses { get; set; }
        public string BusinessWebsite { get; set; }
        public string EmployerAccountName { get; set; }

        public static implicit operator GetApplicationResponse(GetApplicationResult result)
        {
            return new GetApplicationResponse
            {
                AboutOpportunity = result.AboutOpportunity,
                BusinessWebsite = result.BusinessWebsite,
                EmailAddresses = result.EmailAddresses,
                EstimatedDurationMonths = result.EstimatedDurationMonths,
                FirstName = result.FirstName,
                HasTrainingProvider = result.HasTrainingProvider,
                LastName = result.LastName,
                Level = result.Level,
                Location = result.Location,
                NumberOfApprentices = result.NumberOfApprentices,
                Sector = result.Sector,
                StartBy = result.StartBy,
                TypeOfJobRole = result.TypeOfJobRole,
                EmployerAccountName = result.EmployerAccountName,
            };
        }
    }
}
