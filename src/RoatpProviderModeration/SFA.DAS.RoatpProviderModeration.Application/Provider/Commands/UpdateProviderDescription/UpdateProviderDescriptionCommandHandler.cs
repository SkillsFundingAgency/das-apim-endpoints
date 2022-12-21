using MediatR;
using SFA.DAS.RoatpProviderModeration.Application.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RoatpProviderModeration.Application.Provider.Commands.UpdateProviderDescription
{
    public class UpdateProviderDescriptionCommandHandler : IRequestHandler<UpdateProviderDescriptionCommand, Unit>
    {
        private readonly IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _innerApiClient;
        public UpdateProviderDescriptionCommandHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> innerApiClient)
        {
            _innerApiClient = innerApiClient;
        }

        public async Task<Unit> Handle(UpdateProviderDescriptionCommand command, CancellationToken cancellationToken)
        {
            var patchUpdateProvider = new ProviderUpdateModel
            {
                Ukprn = command.Ukprn,
                UserId = command.UserId,
                UserDisplayName = command.UserDisplayName,
                MarketingInfo = command.ProviderDescription
            };

            var patchRequest = new PatchProviderRequest(patchUpdateProvider);
            await _innerApiClient.PatchWithResponseCode(patchRequest);
            return Unit.Value;
        }
    }
}
