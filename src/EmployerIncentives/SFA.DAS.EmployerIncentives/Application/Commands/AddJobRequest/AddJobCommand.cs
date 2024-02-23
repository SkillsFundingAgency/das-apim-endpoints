using System.Collections.Generic;
using MediatR;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;

namespace SFA.DAS.EmployerIncentives.Application.Commands.AddJobRequest
{
    public class AddJobCommand : IRequest<Unit>
    {
        public JobType Type { get; }
        public Dictionary<string, string> Data { get; }

        public AddJobCommand(JobType type, Dictionary<string, string> data)
        {
            Type = type;
            Data = data;
        }
    }
}