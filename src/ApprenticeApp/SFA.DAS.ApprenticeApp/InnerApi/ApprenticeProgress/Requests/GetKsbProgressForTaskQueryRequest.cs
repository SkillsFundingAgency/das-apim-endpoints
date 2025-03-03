using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.ApprenticeApp.InnerApi.ApprenticeProgress.Requests
{
    public class GetKsbProgressForTaskQueryRequest : IGetApiRequest
    {
        public long ApprenticeshipId;
        public int TaskId;

        public GetKsbProgressForTaskQueryRequest(long apprenticeshipId, int taskId)
        {
            ApprenticeshipId = apprenticeshipId;
            TaskId = taskId;
        }

        public string GetUrl => $"apprenticeships/{ApprenticeshipId}/ksbs/taskid/{TaskId}";
    }
}
