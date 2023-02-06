using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DroneTest.Model
{
    public class Drone
    {
        public string Name { get; set; }
        public int Weight { get; set; }
        public List<Location> Locations { get; set; }
    }
}
