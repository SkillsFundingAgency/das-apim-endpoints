using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EpaoRegister.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Validation;

namespace SFA.DAS.EpaoRegister.Application.Epaos.Queries.GetEpao
{
    public class GetEpaoQueryHandler : IRequestHandler<GetEpaoQuery, GetEpaoResult>
    {
        private readonly IValidator<GetEpaoQuery> _validator;
        private readonly IAssessorsApiClient<AssessorsApiConfiguration> _assessorsApiClient;

        public GetEpaoQueryHandler(IValidator<GetEpaoQuery> validator,
            IAssessorsApiClient<AssessorsApiConfiguration> assessorsApiClient)
        {
            _validator = validator;
            _assessorsApiClient = assessorsApiClient;
        }

        public async Task<GetEpaoResult> Handle(GetEpaoQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid())
            {
                throw new ValidationException(validationResult.DataAnnotationResult, null, null);
            }

            var apiRequest = new GetEpaoRequest(request.EpaoId);
            var apiResult = await _assessorsApiClient.Get<GetEpaoResponse>(apiRequest);

            if (apiResult == default)
            {
                throw new NotFoundException<GetEpaoResult>();
            }

            return new GetEpaoResult {Epao = apiResult};
        }
    }
}
