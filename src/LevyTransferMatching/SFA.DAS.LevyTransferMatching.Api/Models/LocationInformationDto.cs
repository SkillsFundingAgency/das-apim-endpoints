using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Api.Models
{
    public class LocationInformationDto
    {
        public string Name { get; set; }
        public double[] GeoPoint { get; set; }
    }
}
