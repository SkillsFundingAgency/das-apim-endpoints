﻿using System;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeApp.InnerApi.ApprenticeProgress.Requests
{
    public class PostApprenticeTaskRequest : IPostApiRequest
    {
        private readonly Guid _apprenticeshipId;
        public string PostUrl => $"/apprenticeships/{_apprenticeshipId}/tasks";
        public object Data { get; set; }

        public PostApprenticeTaskRequest(Guid apprenticeshipId, ApprenticeTaskData data)
        {
            _apprenticeshipId = apprenticeshipId;
            Data = data;
        }
    }
}
