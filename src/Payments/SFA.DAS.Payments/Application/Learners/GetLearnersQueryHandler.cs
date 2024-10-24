using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Payments.Models.Requests;
using SFA.DAS.Payments.Models.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Payments.Application.Learners
{
    public class GetLearnersQuery : IRequest<IEnumerable<LearnerResponse>>
    {
        public string Ukprn { get; set; }
        public short AcademicYear { get; set; }
        public GetLearnersQuery(string ukprn, short academicYear)
        {
            Ukprn = ukprn;
            AcademicYear = academicYear;
        }
    }

    public class GetLearnersQueryHandler : IRequestHandler<GetLearnersQuery, IEnumerable<LearnerResponse>>
    {
        private readonly ILearnerDataApiClient<LearnerDataApiConfiguration> _apiClient;
        private readonly ILogger<GetLearnersQueryHandler> _logger;

        public GetLearnersQueryHandler(
            ILogger<GetLearnersQueryHandler> logger, ILearnerDataApiClient<LearnerDataApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
            _logger = logger;
        }

        public async Task<IEnumerable<LearnerResponse>> Handle(GetLearnersQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting learners for UKPRN: {ukprn} and Academic Year: {academicYear}", request.Ukprn, request.AcademicYear);
            List<LearnerResponse> learners = new List<LearnerResponse>();

            var initialResponse = await _apiClient.GetWithResponseCode<List<LearnerResponse>>(new GetLearnersRequest(request.Ukprn, request.AcademicYear, 1));
            if(initialResponse.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                _logger.LogWarning("No learners found for UKPRN: {ukprn} and Academic Year: {academicYear}", request.Ukprn, request.AcademicYear);
                return learners;
            }

            learners.AddRange(initialResponse.Body);

            var paginationHeader = GetPaginationHeader(initialResponse);

            for (var i = 1; i < paginationHeader.TotalPages; i++) // Start at 1 because we've already got the first page
            {
                var pageNumber = i + 1; // The API is 1-based
                var additionalPage = await _apiClient.GetWithResponseCode<List<LearnerResponse>>(new GetLearnersRequest(request.Ukprn, request.AcademicYear, pageNumber));
                learners.AddRange(additionalPage.Body);
            }

            _logger.LogInformation("Retrieved {learnerCount} learners", learners.Count);
            return learners;
        }

        private PaginationHeader GetPaginationHeader(ApiResponse<List<LearnerResponse>> apiResponse)
        {
            PaginationHeader? paginationHeader;

            try
            {
                var paginationHeaderString = apiResponse.Headers["X-Pagination"].Single();
                paginationHeader = Newtonsoft.Json.JsonConvert.DeserializeObject<PaginationHeader>(paginationHeaderString);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Failed to parse pagination header");
                throw;
            }

            if (paginationHeader == null)
            {
                throw new Exception("Failed to parse pagination header");
            }

            return paginationHeader;
        }

        private class PaginationHeader
        {
            public int TotalItems { get; set; }
            public int PageNumber { get; set; }
            public int PageSize { get; set; }
            public int TotalPages { get; set; }
        } 
    }
}
