using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERSC.DomainObject
{
    // Represents the map boundaries for the application
    public class Map
    {
        // Gets the starting X coordinate of the map
        public static int StartX
        {
            get
            {
                return 0;
            }
        }

        // Gets the ending X coordinate of the map
        public static int EndX
        {
            get
            {
                return 673; // Map width boundary (was 688)
            }
        }

        // Gets the starting Y coordinate of the map
        public static int StartY
        {
            get
            {
                return 0;
            }
        }

        // Gets the ending Y coordinate of the map
        public static int EndY
        {
            get
            {
                return 433; // Map height boundary (was 448)
            }
        }
    }
}
