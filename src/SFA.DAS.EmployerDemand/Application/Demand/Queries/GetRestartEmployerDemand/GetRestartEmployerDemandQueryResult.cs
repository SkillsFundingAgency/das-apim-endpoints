using System;
using SFA.DAS.EmployerDemand.InnerApi.Responses;

namespace SFA.DAS.EmployerDemand.Application.Demand.Queries.GetRestartEmployerDemand
{
    public class GetRestartEmployerDemandQueryResult
    {
        public GetEmployerDemandResponse EmployerDemand { get ; set ; }
        public bool RestartDemandExists { get ; set ; }
        public DateTime? LastStartDate { get; set; }
    }
}