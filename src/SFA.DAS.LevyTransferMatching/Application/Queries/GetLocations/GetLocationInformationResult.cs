using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetLocations
{
    public class GetLocationInformationResult
    {
        public string Name { get; set; }
        public double[] GeoPoint { get; set; }
    }
}
