using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EducationalOrganisations;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.PublicSectorOrganisations;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ReferenceDataJobs.Application.Commands;
public class StartDataLoadsCommandHandler : IRequestHandler<StartDataLoadsCommand>
{
    private readonly IPublicSectorOrganisationApiClient<PublicSectorOrganisationApiConfiguration> _psoApiClient;
    private readonly IEducationalOrganisationApiClient<EducationalOrganisationApiConfiguration> _educationalOrganisationApiClient;
    private readonly ILogger<StartDataLoadsCommandHandler> _logger;

    public StartDataLoadsCommandHandler(IPublicSectorOrganisationApiClient<PublicSectorOrganisationApiConfiguration> psoApiClient,
        IEducationalOrganisationApiClient<EducationalOrganisationApiConfiguration> educationalOrganisationApiClient,
        ILogger<StartDataLoadsCommandHandler> logger)
    {
        _psoApiClient = psoApiClient;
        _educationalOrganisationApiClient = educationalOrganisationApiClient;
        _logger = logger;
    }

    public async Task Handle(StartDataLoadsCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Calling Client to Starting ImportData");
        var psImportTask = _psoApiClient.PostWithResponseCode<object>(new PostPublicSectorOrganisationsDataLoadRequest(), false);
        var edImportTask = _educationalOrganisationApiClient.PostWithResponseCode<object>(new PostEducationOrganisationsDataLoadRequest(), false);
        Task.WhenAll(edImportTask, psImportTask);

        var psResult = await psImportTask;
        var edResult = await edImportTask;

        string errors = "";
        if (!string.IsNullOrWhiteSpace(psResult.ErrorContent))
        {
            errors = "Public Sector orgs Import \r\n" + psResult.ErrorContent + "\r\n";
        }
        if (!string.IsNullOrWhiteSpace(edResult.ErrorContent))
        {
            errors += "Education orgs Import \r\n" + edResult.ErrorContent;
        }

        if(!string.IsNullOrWhiteSpace(errors))
        {
            throw new ApplicationException(errors);
        }
        _logger.LogInformation("Completed ImportData Successfully");
    }
}