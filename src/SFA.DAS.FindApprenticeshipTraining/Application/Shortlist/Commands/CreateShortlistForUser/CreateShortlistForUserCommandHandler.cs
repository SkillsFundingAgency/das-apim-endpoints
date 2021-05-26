using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Commands.CreateShortlistForUser
{
    public class CreateShortlistForUserCommandHandler : IRequestHandler<CreateShortlistForUserCommand, Guid>
    {
        private readonly ICourseDeliveryApiClient<CourseDeliveryApiConfiguration> _courseDeliveryApiClient;
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;

        public CreateShortlistForUserCommandHandler (
            ICourseDeliveryApiClient<CourseDeliveryApiConfiguration> courseDeliveryApiClient, 
            ICoursesApiClient<CoursesApiConfiguration> coursesApiClient)
        {
            _courseDeliveryApiClient = courseDeliveryApiClient;
            _coursesApiClient = coursesApiClient;
        }
        public async Task<Guid> Handle(CreateShortlistForUserCommand request, CancellationToken cancellationToken)
        {
            var course = await _coursesApiClient.Get<GetStandardsListItem>(new GetStandardRequest(request.StandardId));
            
            var result = await _courseDeliveryApiClient.PostWithResponseCode<PostShortListResponse>(new PostShortlistForUserRequest
            {
                Data = new PostShortlistData
                {
                    Lat = request.Lat,
                    Lon = request.Lon,
                    Ukprn = request.Ukprn,
                    LocationDescription = request.LocationDescription,
                    StandardId = request.StandardId,
                    SectorSubjectArea = course.SectorSubjectAreaTier2Description,
                    ShortlistUserId = request.ShortlistUserId
                } 
            });
            
            return result.Body.Id;
        }
    }
}