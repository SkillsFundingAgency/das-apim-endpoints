using MediatR;
using SFA.DAS.Assessors.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Assessors.Application.Queries.GetStandardDetails
{
    public class GetStandardDetailsQueryHandler : IRequestHandler<GetStandardDetailsQuery, GetStandardDetailsResult>
    {
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;

        public GetStandardDetailsQueryHandler(ICoursesApiClient<CoursesApiConfiguration> coursesApiClient)
        {
            _coursesApiClient = coursesApiClient;
        }

        public async Task<GetStandardDetailsResult> Handle(GetStandardDetailsQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Id)) throw new ArgumentException("Standard Id is required", nameof(GetStandardDetailsQuery.Id));

            var standardDetails = await _coursesApiClient.Get<StandardDetailResponse>(new GetStandardDetailsByIdRequest(request.Id));

            return new GetStandardDetailsResult(standardDetails);
        }
    }
}
