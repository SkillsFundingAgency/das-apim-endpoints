using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests
{
    public class GetDeleteTrainingCourseRequest :IGetApiRequest
    {
        private readonly Guid _applicationId;
        private readonly Guid _candidateId;
        private readonly Guid _trainingCourseId;

        public GetDeleteTrainingCourseRequest(Guid applicationId, Guid candidateId, Guid trainingCourseId)
        {
            _applicationId = applicationId;
            _candidateId = candidateId;
            _trainingCourseId = trainingCourseId;
        }

        public string GetUrl =>
            $"candidates/{_candidateId}/applications/{_applicationId}/trainingcourses/{_trainingCourseId}";
    }
}
