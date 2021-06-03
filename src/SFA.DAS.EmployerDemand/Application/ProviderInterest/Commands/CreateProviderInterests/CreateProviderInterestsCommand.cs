using System;
using System.Collections.Generic;
using MediatR;
using SFA.DAS.EmployerDemand.InnerApi.Requests;

namespace SFA.DAS.EmployerDemand.Application.ProviderInterest.Commands.CreateProviderInterests
{
    public class CreateProviderInterestsCommand: IRequest<Guid>
    {
        public Guid Id { get; set; }
        public IEnumerable<Guid> EmployerDemandIds { get; set; }
        public int Ukprn { get; set; }
        public string ProviderName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public string FatUrl { get; set; }

        public static implicit operator CreateProviderInterestsData(CreateProviderInterestsCommand source)
        {
            return new CreateProviderInterestsData
            {
                Id = source.Id,
                EmployerDemandIds = source.EmployerDemandIds,
                Ukprn = source.Ukprn,
                Email = source.Email,
                Phone = source.Phone,
                Website = source.Website
            };
        }
    }
}