using MediatR;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.RoatpCourseManagement.Application.Location.Queries.GetProviderLocationDetails
{
    public class GetProviderLocationDetailsQuery : IGetApiRequest, IRequest<GetProviderLocationDetailsQueryResult>
    {
        public string GetUrl => $"providers/{Ukprn}/locations/{Id}";
        public int Ukprn { get; }
        public Guid Id { get; }

        public GetProviderLocationDetailsQuery(int ukprn, Guid id)
        {
            Ukprn = ukprn;
            Id = id;
        }
    }
}
