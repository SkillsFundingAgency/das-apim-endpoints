using MediatR;
using SFA.DAS.RoatpProviderModeration.Application.Infrastructure;
using SFA.DAS.RoatpProviderModeration.Application.InnerApi.Requests;
using System.Web;


namespace SFA.DAS.RoatpProviderModeration.Application.Provider.Commands.UpdateProviderDescription;

public class UpdateProviderDescriptionCommandHandler : IRequestHandler<UpdateProviderDescriptionCommand, Unit>
{
    private readonly IRoatpV2ApiClient _innerApiClient;
    public UpdateProviderDescriptionCommandHandler(IRoatpV2ApiClient innerApiClient)
    {
        _innerApiClient = innerApiClient;
    }

    public async Task<Unit> Handle(UpdateProviderDescriptionCommand command, CancellationToken cancellationToken)
    {
        var patchOperations = BuildDataPatchFromModel(command.ProviderDescription);

        await _innerApiClient.UpdateProviderDescription(
                command.Ukprn,
                HttpUtility.UrlEncode(command.UserId),
                HttpUtility.UrlEncode(command.UserDisplayName),
                patchOperations,
                cancellationToken);

        return Unit.Value;
    }

    private List<PatchOperation> BuildDataPatchFromModel(string providerDescription)
    {
        var data = new List<PatchOperation>();

        if (providerDescription != null)
            data.Add(new PatchOperation { Path = "MarketingInfo", Value = providerDescription, Op = "replace" });

        return data;
    }
}
