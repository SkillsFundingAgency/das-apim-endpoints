using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Commands.CreateShortlistForUser
{
    public class CreateShortlistForUserCommandHandler : IRequestHandler<CreateShortlistForUserCommand, Guid>
    {
        private readonly ICourseDeliveryApiClient<CourseDeliveryApiConfiguration> _courseDeliveryApiClient;

        public CreateShortlistForUserCommandHandler (ICourseDeliveryApiClient<CourseDeliveryApiConfiguration> courseDeliveryApiClient)
        {
            _courseDeliveryApiClient = courseDeliveryApiClient;
        }
        public async Task<Guid> Handle(CreateShortlistForUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _courseDeliveryApiClient.Post<PostShortListResponse>(new PostShortlistForUserRequest
            {
                Data = new PostShortlistData
                {
                    Lat = request.Lat,
                    Lon = request.Lon,
                    Ukprn = request.Ukprn,
                    LocationDescription = request.LocationDescription,
                    StandardId = request.StandardId,
                    SectorSubjectArea = request.SectorSubjectArea,
                    ShortlistUserId = request.ShortlistUserId
                } 
            });
            
            return result.Id;
        }
    }
}