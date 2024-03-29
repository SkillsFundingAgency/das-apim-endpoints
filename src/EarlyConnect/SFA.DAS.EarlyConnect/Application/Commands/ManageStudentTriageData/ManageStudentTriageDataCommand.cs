﻿using MediatR;
using SFA.DAS.EarlyConnect.Models;

namespace SFA.DAS.EarlyConnect.Application.Commands.ManageStudentTriageData
{
    public class ManageStudentTriageDataCommand : IRequest<ManageStudentTriageDataCommandResult>
    {
        public Guid SurveyGuid { get; set; }
        public StudentTriageData StudentTriageData { get; set; }
    }
}