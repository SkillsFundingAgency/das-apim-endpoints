using MediatR;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.FindApprenticeshipTraining.Configuration;
using SFA.DAS.FindApprenticeshipTraining.Services;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Commands.CreateShortlistForUser
{
    public class CreateShortlistForUserCommandHandler : IRequestHandler<CreateShortlistForUserCommand, Unit>
    {
        private readonly IShortlistApiClient<ShortlistApiConfiguration> _shortlistApiClient;

        public CreateShortlistForUserCommandHandler(
            IShortlistApiClient<ShortlistApiConfiguration> shortlistApiClient)
        {
            _shortlistApiClient = shortlistApiClient;
        }
        public async Task<Unit> Handle(CreateShortlistForUserCommand request, CancellationToken cancellationToken)
        {
            await _shortlistApiClient.PostWithResponseCode<PostShortListResponse>(new PostShortlistForUserRequest
            {
                Data = new PostShortlistData
                {
                    Latitude = request.Lat,
                    Longitude = request.Lon,
                    Ukprn = request.Ukprn,
                    LocationDescription = request.LocationDescription,
                    Larscode = request.StandardId,
                    ShortlistUserId = request.ShortlistUserId
                } 
            },false);
            
            return Unit.Value;
        }
    }
}