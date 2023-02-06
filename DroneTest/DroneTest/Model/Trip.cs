using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DroneTest.Model
{
    public class Trip
    {
        public string Dron { get; set; }
        public List<string> Locations { get; set; }
        public int NumberOfTrips { get; set; }
    }
}
