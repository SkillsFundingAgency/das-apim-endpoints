using System;
using System.Collections.Generic;
using MediatR;
using SFA.DAS.EmployerDemand.InnerApi.Requests;

namespace SFA.DAS.EmployerDemand.Application.ProviderInterest.Commands.CreateProviderInterests
{
    public class CreateProviderInterestsCommand: IRequest<int>
    {
        public IEnumerable<Guid> EmployerDemandIds { get; set; }
        public int Ukprn { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }

        public static implicit operator CreateProviderInterestsData(CreateProviderInterestsCommand source)
        {
            return new CreateProviderInterestsData
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