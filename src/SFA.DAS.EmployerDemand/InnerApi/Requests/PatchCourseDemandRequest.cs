using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerDemand.InnerApi.Requests
{
    public class PatchCourseDemandRequest : IPatchApiRequest<PatchCourseDemandData>
    {
        private readonly Guid _id;

        public PatchCourseDemandRequest(Guid id, PatchCourseDemandData data)
        {
            _id = id;
            Data = data;
        }

        public string PatchUrl => $"api/demand/{_id}";
        public PatchCourseDemandData Data { get; set; }
    }

    public class PatchCourseDemandData
    {
        public bool Stopped { get; set; }
        public string EmailAddress { get; set; }
        //more stuff here for verify
    }
}