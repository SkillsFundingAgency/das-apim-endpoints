using System;
using System.Collections.Generic;
using SFA.DAS.EmployerDemand.Application.ProviderInterest.Commands.CreateProviderInterests;

namespace SFA.DAS.EmployerDemand.Api.ApiRequests
{
    public class CreateProviderInterestsRequest
    {
        public Guid Id { get ; set ; }
        public IEnumerable<Guid> EmployerDemandIds { get; set; }
        public int Ukprn { get; set; }
        public string ProviderName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public string FatUrl { get; set; }

        public static implicit operator CreateProviderInterestsCommand(CreateProviderInterestsRequest source)
        {
            return new CreateProviderInterestsCommand
            {
                Id = source.Id,
                EmployerDemandIds = source.EmployerDemandIds,
                Ukprn = source.Ukprn,
                ProviderName = source.ProviderName,
                Email = source.Email,
                Phone = source.Phone,
                Website = source.Website,
                FatUrl = source.FatUrl
            };
        }
    }
}