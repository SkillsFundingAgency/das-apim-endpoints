using System;
using MediatR;

namespace SFA.DAS.EmployerDemand.Application.Demand.Commands.RegisterDemand
{
    public class RegisterDemandCommand : IRequest<Guid?>
    {
        public Guid Id { get ; set ; }
        public string ContactEmailAddress { get ; set ; }
        public string OrganisationName { get ; set ; }
        public int NumberOfApprentices { get ; set ; }
        public double Lat { get ; set ; }
        public double Lon { get ; set ; }
        public string LocationName { get ; set ; }
        public string CourseTitle { get ; set ; }
        public int CourseLevel { get ; set ; }
        public int CourseId { get ; set ; }
        public string CourseRoute { get ; set ; }
        public string ConfirmationLink { get ; set ; }
        public string StopSharingUrl { get ; set ; }
        public string StartSharingUrl { get ; set ; }
        public Guid? ExpiredCourseDemandId { get ; set ; }
        public short? EntryPoint { get ; set ; }
    }
}
