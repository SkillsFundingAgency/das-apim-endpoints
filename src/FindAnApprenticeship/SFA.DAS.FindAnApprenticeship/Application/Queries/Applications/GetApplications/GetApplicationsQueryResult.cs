using System;
using System.Collections.Generic;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Applications.GetApplications;

public class GetApplicationsQueryResult
{
    public List<Application> Applications { get; set; } = [];


    public class Application
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string EmployerName { get; set; }
    }
}