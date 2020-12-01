﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EpaoRegister.InnerApi.Requests;
using SFA.DAS.EpaoRegister.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Validation;

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

            var apiRequest = new GetEpaoRequest{EpaoId = request.EpaoId};
            var apiResult = await _assessorsApiClient.Get<InnerApi.Responses.GetEpaoResponse>(apiRequest);

            if (apiResult == default)
            {
                throw new EntityNotFoundException<InnerApi.Responses.GetEpaoResponse>();
            }

            return new GetEpaoResult {Epao = apiResult};
        }
    }
}
