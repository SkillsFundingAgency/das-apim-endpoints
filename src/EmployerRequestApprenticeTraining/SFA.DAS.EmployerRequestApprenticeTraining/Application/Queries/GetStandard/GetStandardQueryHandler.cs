﻿using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Courses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetStandard
{
    public class GetStandardQueryHandler : IRequestHandler<GetStandardQuery, GetStandardResult>
    {
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;

        public GetStandardQueryHandler(ICoursesApiClient<CoursesApiConfiguration> coursesApiClient)
        {
            _coursesApiClient = coursesApiClient;
        }

        public async Task<GetStandardResult> Handle(GetStandardQuery request, CancellationToken cancellationToken)
        {
            var response = await _coursesApiClient.
                GetWithResponseCode<StandardDetailResponse>(new GetStandardDetailsByIdRequest(request.StandardId));

            if (response.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                response.EnsureSuccessStatusCode();
            }

            return new GetStandardResult { Standard = (Standard)response.Body };
        }
    }
}
