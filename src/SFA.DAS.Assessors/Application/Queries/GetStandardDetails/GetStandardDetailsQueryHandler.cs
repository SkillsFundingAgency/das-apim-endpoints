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
            if (string.IsNullOrEmpty(request.StandardUId)) throw new ArgumentException("StandardUId is required", nameof(GetStandardDetailsQuery.StandardUId));

            var standardDetails = await _coursesApiClient.Get<StandardDetailResponse>(new GetStandardDetailsByStandardUIdRequest(request.StandardUId));

            return new GetStandardDetailsResult(standardDetails);
        }
    }
}
