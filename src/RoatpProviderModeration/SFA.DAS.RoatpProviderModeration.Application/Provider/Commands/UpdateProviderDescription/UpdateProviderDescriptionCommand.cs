using MediatR;

namespace SFA.DAS.RoatpProviderModeration.Application.Provider.Commands.UpdateProviderDescription
{
    public class UpdateProviderDescriptionCommand : IRequest<Unit>
    {
        public int Ukprn { get; set; }
        public string UserId { get; set; }
        public string UserDisplayName { get; set; }
        public string ProviderDescription { get; set; }
    }
}
