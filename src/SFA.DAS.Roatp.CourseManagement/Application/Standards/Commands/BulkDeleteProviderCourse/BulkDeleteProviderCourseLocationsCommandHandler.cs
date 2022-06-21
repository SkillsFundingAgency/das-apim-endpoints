﻿using MediatR;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Roatp.CourseManagement.Application.Standards.Commands.BulkDeleteProviderCourse
{
    public class BulkDeleteProviderCourseLocationsCommandHandler : IRequestHandler<BulkDeleteProviderCourseLocationsCommand, Unit>
    {
        private readonly IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _innerApiClient;

        public BulkDeleteProviderCourseLocationsCommandHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> innerApiClient)
        {
            _innerApiClient = innerApiClient;
        }

        public async Task<Unit> Handle(BulkDeleteProviderCourseLocationsCommand request, CancellationToken cancellationToken)
        {
            await _innerApiClient.Delete((BulkDeleteProviderCourseLocationsRequest)request);
            return Unit.Value;
        }
    }
}
