﻿using System;
using System.Collections.Generic;
using SFA.DAS.EmployerDemand.Application.ProviderInterest.Commands.CreateProviderInterests;

namespace SFA.DAS.EmployerDemand.Api.ApiRequests
{
    public class CreateProviderInterestsRequest
    {
        public IEnumerable<Guid> EmployerDemandIds { get; set; }
        public int Ukprn { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }

        public static implicit operator CreateProviderInterestsCommand(CreateProviderInterestsRequest source)
        {
            return new CreateProviderInterestsCommand
            {
                EmployerDemandIds = source.EmployerDemandIds,
                Ukprn = source.Ukprn,
                Email = source.Email,
                Phone = source.Phone,
                Website = source.Website
            };
        }
    }
}