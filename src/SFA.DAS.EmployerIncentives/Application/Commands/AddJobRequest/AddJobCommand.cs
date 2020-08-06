using System;
using System.Collections.Generic;
using MediatR;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;

namespace SFA.DAS.EmployerIncentives.Application.Commands.AddJobRequest
{
    public class AddJobCommand : IRequest
    {
        public JobType Type { get; }
        public Dictionary<string, object> Data { get; }

        public AddJobCommand(JobType type, Dictionary<string, object> data)
        {
            Type = type;
            Data = data;
        }
    }
}